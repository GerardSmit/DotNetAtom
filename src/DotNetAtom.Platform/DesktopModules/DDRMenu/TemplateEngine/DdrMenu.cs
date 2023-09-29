using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFormsCore.UI;

namespace DotNetAtom.DesktopModules.DDRMenu.TemplateEngine;

public class DdrMenu(BodyExpression root)
{
    public Dictionary<string, BodyExpression> Templates { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    public BodyExpression Root { get; } = root;

    public ValueTask RenderAsync(IMenuItem item, HtmlTextWriter writer)
    {
        return Root.WriteAsync(this, item, writer);
    }

    public static DdrMenu Parse(ReadOnlySpan<char> remaining)
    {
        var start = 0;
        var body = new BodyExpression();
        var menu = new DdrMenu(body);

        AddExpressions(menu, ref remaining, start, body);

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
        int start,
        BodyExpression body,
        string? endTag = null,
        string? alternateEndTag = null)
    {
        while (true)
        {
            var index = remaining.IndexOf('[');
            ReadOnlySpan<char> raw;

            // No more expressions
            if (index == -1)
            {
                raw = remaining.Slice(start);

                if (raw.Length > 0)
                {
                    body.Expressions.Add(new RawExpression(raw.ToString()));
                }

                return EndReason.NoTokens;
            }

            // Add text before expression
            raw = remaining.Slice(start, index - start);

            if (raw.Length > 0)
            {
                body.Expressions.Add(new RawExpression(raw.ToString()));
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
            if (endTag is not null && expression.SequenceEqual(endTag))
            {
                return EndReason.EndTag;
            }

            if (alternateEndTag is not null && expression.SequenceEqual(alternateEndTag))
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

                case '?':
                {
                    var test = expression.Slice(1).ToString();
                    var choose = new BodyExpression();

                    if (AddExpressions(menu, ref remaining, 0, choose, "/?", "?ELSE") == EndReason.AlternateEndTag)
                    {
                        var otherwise = new BodyExpression();
                        AddExpressions(menu, ref remaining, 0, otherwise, "/?");
                    }

                    body.Expressions.Add(new TestExpression(test, choose));

                    break;
                }

                case '>':
                {
                    var name = expression.Slice(1).ToString();
                    var template = new BodyExpression();

                    AddExpressions(menu, ref remaining, 0, template, "/>");

                    menu.Templates[name] = template;

                    break;
                }

                default:
                    throw new Exception("Unknown expression");
            }

            start = 0;
        }
    }
}

public interface IMenuItem
{
    object? GetNode(string key);

    bool TestNode(string key);
}

public abstract record Expression
{
    public abstract ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer);
}

public sealed record BodyExpression : Expression
{
    public List<Expression> Expressions { get; } = new();

    public override async ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer)
    {
        foreach (var expression in Expressions)
        {
            await expression.WriteAsync(menu, item, writer);
        }
    }
}

public sealed record RawExpression(string Value) : Expression
{
    public override ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer)
    {
        return writer.WriteAsync(Value);
    }
}

public sealed record ApplyTemplatesExpression(string Node, string? Mode = null) : Expression
{
    private readonly string _templateName = Mode is null ? Node : $"{Node}-{Mode}";

    public override async ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer)
    {
        if (!menu.Templates.TryGetValue(_templateName, out var template))
        {
            await writer.WriteEncodedTextAsync($"<!-- Template for node '{Node}' with mode '{Mode}' not found -->");
            return;
        }

        var value = item.GetNode(Node);

        if (value is not IEnumerable<IMenuItem> children)
        {
            return;
        }

        foreach (var child in children)
        {
            foreach (var expression in template.Expressions)
            {
                await expression.WriteAsync(menu, child, writer);
            }
        }
    }
}

public sealed record NodeExpression(string Node) : Expression
{
    public override ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer)
    {
        var value = item.GetNode(Node)?.ToString();

        if (value is not null)
        {
            return writer.WriteEncodedTextAsync(value);
        }

        return default;
    }
}

public sealed record TestExpression(string Node, Expression Choose, Expression? Otherwise = null) : Expression
{
    public override ValueTask WriteAsync(DdrMenu menu, IMenuItem item, HtmlTextWriter writer)
    {
        if (item.TestNode(Node))
        {
            return Choose.WriteAsync(menu, item, writer);
        }

        if (Otherwise is not null)
        {
            return Otherwise.WriteAsync(menu, item, writer);
        }

        return default;
    }
}