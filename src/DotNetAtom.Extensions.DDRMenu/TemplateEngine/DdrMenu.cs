using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetAtom.Entities.Portals;
using DotNetAtom.TemplateEngine.Expressions;
using WebFormsCore.UI;

namespace DotNetAtom.TemplateEngine;

public class DdrMenu(BodyExpression root)
{
    public Dictionary<string, BodyExpression> Templates { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public BodyExpression Root { get; } = root;

    public ValueTask RenderAsync(IMenuItem item, HtmlTextWriter writer, IPortalSettings settings)
    {
        return Root.WriteAsync(this, item, writer, settings);
    }

    public static DdrMenu Parse(ReadOnlySpan<char> remaining)
    {
        var body = new BodyExpression();
        var menu = new DdrMenu(body);

        AddExpressions(menu, ref remaining, body);

        return menu;
    }

    private enum EndReason
    {
        NoTokens,
        EndTag,
        AlternateEndTag
    }

    private static EndReason AddExpressions(
        DdrMenu menu,
        ref ReadOnlySpan<char> remaining,
        BodyExpression body,
        string? endTag = null,
        string? alternateEndTag = null)
    {
        while (true)
        {
            var index = remaining.IndexOf('[');

            // No more expressions
            if (index == -1)
            {
                if (remaining.Length > 0)
                {
                    body.Expressions.Add(new RawExpression(remaining.ToString()));
                }

                return EndReason.NoTokens;
            }

            // Add text before expression
            if (index > 0)
            {
                body.Expressions.Add(new RawExpression(remaining.Slice(0, index).ToString()));
            }

            // Get the expression
            remaining = remaining.Slice(index + 1);

            var end = remaining.IndexOf(']');

            if (end == -1)
            {
                throw new Exception("Missing closing bracket");
            }

            var expression = remaining.Slice(0, end);

            remaining = remaining.Slice(end + 1);

            // Check for the end tags
            if (endTag is not null && expression.SequenceEqual(endTag.AsSpan()))
            {
                return EndReason.EndTag;
            }

            if (alternateEndTag is not null && expression.SequenceEqual(alternateEndTag.AsSpan()))
            {
                return EndReason.AlternateEndTag;
            }

            // Add the expression
            switch (expression[0])
            {
                case '=':
                    body.Expressions.Add(new NodeExpression(expression.Slice(1).ToString()));
                    break;

                case '*' when expression[1] == '>':
                {
                    var subIndex = expression.IndexOf('-');

                    if (subIndex == -1)
                    {
                        var nodeName = expression.Slice(2).ToString();

                        body.Expressions.Add(new ApplyTemplatesExpression(nodeName));
                    }
                    else
                    {
                        var nodeName = expression.Slice(2, subIndex - 2).ToString();
                        var mode = expression.Slice(subIndex + 1).ToString();

                        body.Expressions.Add(new ApplyTemplatesExpression(nodeName, mode));
                    }

                    break;
                }

                case '*':
                {
                    var nodeName = expression.Slice(1).ToString();
                    var expressions = new BodyExpression();

                    AddExpressions(menu, ref remaining, expressions, endTag: "/*");

                    body.Expressions.Add(new ApplyExpression(nodeName, expressions));
                    break;
                }

                case '?':
                {
                    var test = expression.Slice(1).ToString();
                    var choose = new BodyExpression();
                    BodyExpression? otherwise = null;

                    if (AddExpressions(menu, ref remaining, choose, endTag: "/?", alternateEndTag: "?ELSE") == EndReason.AlternateEndTag)
                    {
                        otherwise = new BodyExpression();
                        AddExpressions(menu, ref remaining, otherwise, endTag: "/?");
                    }

                    body.Expressions.Add(new TestExpression(test, choose, otherwise));

                    break;
                }

                case '>':
                {
                    var name = expression.Slice(1).ToString();
                    var template = new BodyExpression();

                    AddExpressions(menu, ref remaining, template, endTag: "/>");

                    menu.Templates[name] = template;

                    break;
                }

                default:
                    throw new Exception("Unknown expression");
            }
        }
    }
}