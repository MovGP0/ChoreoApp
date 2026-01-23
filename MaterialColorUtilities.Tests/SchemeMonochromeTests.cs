namespace MaterialColorUtilities.Tests;

public sealed class SchemeMonochromeTests
{
    private static readonly int Blue = unchecked((int)0xFF0000FF);

    [Fact(DisplayName = "Monochrome spec (dark theme)")]
    public void DarkTheme_MonochromeSpec()
    {
        var mdc = new MaterialDynamicColors();
        var scheme = new SchemeMonochrome(
            sourceColorHct: Hct.FromInt(Blue),
            isDark: true,
            contrastLevel: 0.0
        );

        mdc.Primary.GetHct(scheme).Tone.ShouldBe(100, 1.0);
        mdc.OnPrimary.GetHct(scheme).Tone.ShouldBe(10, 1.0);
        mdc.PrimaryContainer.GetHct(scheme).Tone.ShouldBe(85, 1.0);
        mdc.OnPrimaryContainer.GetHct(scheme).Tone.ShouldBe(0, 1.0);

        mdc.Secondary.GetHct(scheme).Tone.ShouldBe(80, 1.0);
        mdc.OnSecondary.GetHct(scheme).Tone.ShouldBe(10, 1.0);
        mdc.SecondaryContainer.GetHct(scheme).Tone.ShouldBe(30, 1.0);
        mdc.OnSecondaryContainer.GetHct(scheme).Tone.ShouldBe(90, 1.0);

        mdc.Tertiary.GetHct(scheme).Tone.ShouldBe(90, 1.0);
        mdc.OnTertiary.GetHct(scheme).Tone.ShouldBe(10, 1.0);
        mdc.TertiaryContainer.GetHct(scheme).Tone.ShouldBe(60, 1.0);
        mdc.OnTertiaryContainer.GetHct(scheme).Tone.ShouldBe(0, 1.0);
    }

    [Fact(DisplayName = "Monochrome spec (light theme)")]
    public void LightTheme_MonochromeSpec()
    {
        var mdc = new MaterialDynamicColors();
        var scheme = new SchemeMonochrome(
            sourceColorHct: Hct.FromInt(Blue),
            isDark: false,
            contrastLevel: 0.0
        );

        mdc.Primary.GetHct(scheme).Tone.ShouldBe(0, 1.0);
        mdc.OnPrimary.GetHct(scheme).Tone.ShouldBe(90, 1.0);
        mdc.PrimaryContainer.GetHct(scheme).Tone.ShouldBe(25, 1.0);
        mdc.OnPrimaryContainer.GetHct(scheme).Tone.ShouldBe(100, 1.0);

        mdc.Secondary.GetHct(scheme).Tone.ShouldBe(40, 1.0);
        mdc.OnSecondary.GetHct(scheme).Tone.ShouldBe(100, 1.0);
        mdc.SecondaryContainer.GetHct(scheme).Tone.ShouldBe(85, 1.0);
        mdc.OnSecondaryContainer.GetHct(scheme).Tone.ShouldBe(10, 1.0);

        mdc.Tertiary.GetHct(scheme).Tone.ShouldBe(25, 1.0);
        mdc.OnTertiary.GetHct(scheme).Tone.ShouldBe(90, 1.0);
        mdc.TertiaryContainer.GetHct(scheme).Tone.ShouldBe(49, 1.0);
        mdc.OnTertiaryContainer.GetHct(scheme).Tone.ShouldBe(100, 1.0);
    }
}
