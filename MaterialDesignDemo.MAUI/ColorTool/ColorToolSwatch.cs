namespace MaterialDesignDemo.Maui.ColorTool;

public sealed record ColorToolSwatch
{
    public ColorToolSwatch(string name, IReadOnlyList<string> colorKeys)
    {
        Name = name;
        ColorKeys = colorKeys;
    }

    public string Name { get; }

    public IReadOnlyList<string> ColorKeys { get; }
}
