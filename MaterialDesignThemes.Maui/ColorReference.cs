namespace MaterialDesignThemes.Maui;

public readonly record struct ColorReference(ThemeColorReference ThemeReference, Color? Color)
{
    public static ColorReference SecondaryLight { get; } = new(ThemeColorReference.SecondaryLight, null);
    public static ColorReference SecondaryMid { get; } = new(ThemeColorReference.SecondaryMid, null);
    public static ColorReference SecondaryDark { get; } = new(ThemeColorReference.SecondaryDark, null);
    public static ColorReference PrimaryLight { get; } = new(ThemeColorReference.PrimaryLight, null);
    public static ColorReference PrimaryMid { get; } = new(ThemeColorReference.PrimaryMid, null);
    public static ColorReference PrimaryDark { get; } = new(ThemeColorReference.PrimaryDark, null);

    public static implicit operator ColorReference(Color color) => new(ThemeColorReference.None, color);
    public static implicit operator ColorReference(ThemeColorReference @ref) => new(@ref, null);
    public static implicit operator Color(ColorReference color) => color.Color ??
        throw new InvalidOperationException($"{nameof(ColorReference)} does not contain any color");
}
