namespace DotNetAtom.TemplateEngine;

public interface IMenuItem
{
    object? GetNode(string key);

    bool TestNode(string key);
}