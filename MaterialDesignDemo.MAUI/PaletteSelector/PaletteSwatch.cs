namespace MaterialDesignDemo.Maui.PaletteSelector;

public sealed record PaletteSwatch
{
    public PaletteSwatch(string name, string primaryColorKey, string? secondaryColorKey)
    {
        Name = name;
        PrimaryColorKey = primaryColorKey;
        SecondaryColorKey = secondaryColorKey;
    }

    public string Name { get; }

    public string PrimaryColorKey { get; }

    public string? SecondaryColorKey { get; }

    public bool HasSecondary => SecondaryColorKey is not null;
}
