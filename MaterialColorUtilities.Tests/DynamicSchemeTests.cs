namespace MaterialColorUtilities.Tests;

public sealed class DynamicSchemeTests
{
    private static Color FromArgb(int argb) => Color.FromArgb(argb);

    private static int Argb(Color color) => Color.ArgbFromColor(color);

    private static Color RequireColor(DynamicColor? dynamicColor, DynamicScheme scheme)
    {
        if (dynamicColor is null)
        {
            throw new InvalidOperationException("Expected a dynamic color instance.");
        }

        return dynamicColor.GetColor(scheme);
    }

    [Fact(DisplayName = "0 length input")]
    public void ZeroLengthInput_NoRotation()
    {
        var hue = DynamicScheme.GetRotatedHue(
            Hct.From(43, 16, 16),
            [],
            []);

        hue.ShouldBe(43.0, 1.0);
    }

    [Fact(DisplayName = "1 length input no rotation")]
    public void OneLengthInput_NoRotation()
    {
        var hue = DynamicScheme.GetRotatedHue(
            Hct.From(43, 16, 16),
            [0.0],
            [0.0]);

        hue.ShouldBe(43.0, 1.0);
    }

    [Fact(DisplayName = "input length mismatch asserts")]
    public void InputLengthMismatch_Throws()
    {
        Should.Throw<ArgumentException>(() =>
            DynamicScheme.GetRotatedHue(
                Hct.From(43, 16, 16),
                [0.0, 1.0],   // 2 breakpoints
                [0.0] // 1 rotation
            ));
    }

    [Fact(DisplayName = "on boundary rotation correct")]
    public void OnBoundary_RotationApplied()
    {
        var hue = DynamicScheme.GetRotatedHue(
            Hct.From(43, 16, 16),
            [0.0, 42.0, 360.0],
            [0.0, 15.0, 0.0]);

        hue.ShouldBe(43.0 + 15.0, 1.0);
    }

    [Fact(DisplayName = "rotation > 360 wraps")]
    public void RotationGreaterThan360_Wraps()
    {
        var hue = DynamicScheme.GetRotatedHue(
            Hct.From(43, 16, 16),
            [0.0, 42.0, 360.0],
            [0.0, 480.0, 0.0]);

        // 43 + 480 = 523 -> 163 after sanitize/wrap
        hue.ShouldBe(163.0, 1.0);
    }

    /// <summary>
    /// Shows how te crate a theme from a primary color.
    /// </summary>
    [Fact]
    public void CreateThemeFromColor()
    {
        var mdc = new MaterialDynamicColors();
        var primaryColorHct = Hct.FromInt(Color.ArgbFromColor(FromArgb(unchecked((int)0xFF6A9C59))));
        var scheme = new SchemeContent(
            primaryColorHct,
            isDark: true,
            contrastLevel : 0.5,
            SpecVersion.Spec2025,
            Platform.Phone);

        scheme.ShouldSatisfyAllConditions(
            // Main Palettes
            () => Argb(mdc.PrimaryPaletteKeyColor.GetColor(scheme)).ShouldBe(unchecked((int)0xFF528343)),
            () => Argb(mdc.SecondaryPaletteKeyColor.GetColor(scheme)).ShouldBe(unchecked((int)0xFF687D5E)),
            () => Argb(mdc.TertiaryPaletteKeyColor.GetColor(scheme)).ShouldBe(unchecked((int)0xFF2B9F94)),
            () => Argb(mdc.NeutralPaletteKeyColor.GetColor(scheme)).ShouldBe(unchecked((int)0xFF757871)),
            () => Argb(mdc.NeutralVariantPaletteKeyColor.GetColor(scheme)).ShouldBe(unchecked((int)0xFF72796C)),
            () => Argb(mdc.ErrorPaletteKeyColor.GetColor(scheme)).ShouldBe(unchecked((int)0xFFDE3730)),

            // Surfaces [S]
            () => Argb(mdc.Background.GetColor(scheme)).ShouldBe(unchecked((int)0xFF0C0F0A)),
            () => Argb(mdc.OnBackground.GetColor(scheme)).ShouldBe(unchecked((int)0xFFFFFFFF)),
            () => Argb(mdc.Surface.GetColor(scheme)).ShouldBe(unchecked((int)0xFF0C0F0A)),
            () => Argb(mdc.SurfaceDim.GetColor(scheme)).ShouldBe(unchecked((int)0xFF0C0F0A)),
            () => Argb(mdc.SurfaceBright.GetColor(scheme)).ShouldBe(unchecked((int)0xFF2A2D27)),
            () => Argb(mdc.SurfaceContainerLowest.GetColor(scheme)).ShouldBe(unchecked((int)0xFF000000)),
            () => Argb(mdc.SurfaceContainerLow.GetColor(scheme)).ShouldBe(unchecked((int)0xFF11140F)),
            () => Argb(mdc.SurfaceContainer.GetColor(scheme)).ShouldBe(unchecked((int)0xFF171A15)),
            () => Argb(mdc.SurfaceContainerHigh.GetColor(scheme)).ShouldBe(unchecked((int)0xFF1D201B)),
            () => Argb(mdc.SurfaceContainerHighest.GetColor(scheme)).ShouldBe(unchecked((int)0xFF242721)),
            () => Argb(mdc.OnSurface.GetColor(scheme)).ShouldBe(unchecked((int)0xFFFFFFFF)),
            () => Argb(mdc.SurfaceVariant.GetColor(scheme)).ShouldBe(unchecked((int)0xFF242721)),
            () => Argb(mdc.OnSurfaceVariant.GetColor(scheme)).ShouldBe(unchecked((int)0xFFB7BAB2)),
            () => Argb(mdc.Outline.GetColor(scheme)).ShouldBe(unchecked((int)0xFF92948D)),
            () => Argb(mdc.OutlineVariant.GetColor(scheme)).ShouldBe(unchecked((int)0xFF74766F)),
            () => Argb(mdc.InverseSurface.GetColor(scheme)).ShouldBe(unchecked((int)0xFFF9FAF1)),
            () => Argb(mdc.InverseOnSurface.GetColor(scheme)).ShouldBe(unchecked((int)0xFF363933)),
            () => Argb(mdc.Shadow.GetColor(scheme)).ShouldBe(unchecked((int)0xFF000000)),
            () => Argb(mdc.Scrim.GetColor(scheme)).ShouldBe(unchecked((int)0xFF000000)),
            () => Argb(mdc.SurfaceTint.GetColor(scheme)).ShouldBe(unchecked((int)0xFFE2FFD3)),

            // Primaries [P]
            () => Argb(mdc.Primary.GetColor(scheme)).ShouldBe(unchecked((int)0xFFC0F7AA)),
            () => Argb(RequireColor(mdc.PrimaryDim, scheme)).ShouldBe(unchecked((int)0xFFE2FFD3)),
            () => Argb(mdc.OnPrimary.GetColor(scheme)).ShouldBe(unchecked((int)0xFF27551B)),
            () => Argb(mdc.PrimaryContainer.GetColor(scheme)).ShouldBe(unchecked((int)0xFF7BAE69)),
            () => Argb(mdc.OnPrimaryContainer.GetColor(scheme)).ShouldBe(unchecked((int)0xFF011900)),
            () => Argb(mdc.PrimaryFixed.GetColor(scheme)).ShouldBe(unchecked((int)0xFFC0F7AA)),
            () => Argb(mdc.PrimaryFixedDim.GetColor(scheme)).ShouldBe(unchecked((int)0xFFE2FFD3)),
            () => Argb(mdc.OnPrimaryFixed.GetColor(scheme)).ShouldBe(unchecked((int)0xFF113F07)),
            () => Argb(mdc.OnPrimaryFixedVariant.GetColor(scheme)).ShouldBe(unchecked((int)0xFF2F5E23)),
            () => Argb(mdc.InversePrimary.GetColor(scheme)).ShouldBe(unchecked((int)0xFF316024)),

            // Secondaries [Q]
            () => Argb(mdc.Secondary.GetColor(scheme)).ShouldBe(unchecked((int)0xFFB7CDAA)),
            () => Argb(RequireColor(mdc.SecondaryDim, scheme)).ShouldBe(unchecked((int)0xFFD2E9C5)),
            () => Argb(mdc.OnSecondary.GetColor(scheme)).ShouldBe(unchecked((int)0xFF283B22)),
            () => Argb(mdc.SecondaryContainer.GetColor(scheme)).ShouldBe(unchecked((int)0xFF677B5D)),
            () => Argb(mdc.OnSecondaryContainer.GetColor(scheme)).ShouldBe(unchecked((int)0xFFFFFFFF)),

            // Secondary Fixed [QF]
            () => Argb(mdc.SecondaryFixed.GetColor(scheme)).ShouldBe(unchecked((int)0xFFD2E9C5)),
            () => Argb(mdc.SecondaryFixedDim.GetColor(scheme)).ShouldBe(unchecked((int)0xFFE0F8D3)),
            () => Argb(mdc.OnSecondaryFixed.GetColor(scheme)).ShouldBe(unchecked((int)0xFF25381F)),
            () => Argb(mdc.OnSecondaryFixedVariant.GetColor(scheme)).ShouldBe(unchecked((int)0xFF42563A)),

            // Tertiaries [T]
            () => Argb(mdc.Tertiary.GetColor(scheme)).ShouldBe(unchecked((int)0xFF8BF5E8)),
            () => Argb(RequireColor(mdc.TertiaryDim, scheme)).ShouldBe(unchecked((int)0xFFB3FFF4)),
            () => Argb(mdc.OnTertiary.GetColor(scheme)).ShouldBe(unchecked((int)0xFF00514B)),
            () => Argb(mdc.TertiaryContainer.GetColor(scheme)).ShouldBe(unchecked((int)0xFF8BF5E8)),
            () => Argb(mdc.OnTertiaryContainer.GetColor(scheme)).ShouldBe(unchecked((int)0xFF00514B)),

            // Tertiary Fixed [TF]
            () => Argb(mdc.TertiaryFixed.GetColor(scheme)).ShouldBe(unchecked((int)0xFF8BF5E8)),
            () => Argb(mdc.TertiaryFixedDim.GetColor(scheme)).ShouldBe(unchecked((int)0xFFB3FFF4)),
            () => Argb(mdc.OnTertiaryFixed.GetColor(scheme)).ShouldBe(unchecked((int)0xFF003A35)),
            () => Argb(mdc.OnTertiaryFixedVariant.GetColor(scheme)).ShouldBe(unchecked((int)0xFF005B53)),

            // Errors [E]
            () => Argb(mdc.Error.GetColor(scheme)).ShouldBe(unchecked((int)0xFFFF9F94)),
            () => Argb(RequireColor(mdc.ErrorDim, scheme)).ShouldBe(unchecked((int)0xFFFF9E93)),
            () => Argb(mdc.OnError.GetColor(scheme)).ShouldBe(unchecked((int)0xFF600004)),
            () => Argb(mdc.ErrorContainer.GetColor(scheme)).ShouldBe(unchecked((int)0xFFDB352F)),
            () => Argb(mdc.OnErrorContainer.GetColor(scheme)).ShouldBe(unchecked((int)0xFFFFFFFF)),

            // Android-only
            () => Argb(mdc.ControlActivated.GetColor(scheme)).ShouldBe(unchecked((int)0xFF7BAE69)),
            () => Argb(mdc.ControlNormal.GetColor(scheme)).ShouldBe(unchecked((int)0xFFB7BAB2)),
            () => Argb(mdc.ControlHighlight.GetColor(scheme)).ShouldBe(unchecked((int)0x33FFFFFF)),
            () => Argb(mdc.TextPrimaryInverse.GetColor(scheme)).ShouldBe(unchecked((int)0xFF363933)),
            () => Argb(mdc.TextSecondaryAndTertiaryInverse.GetColor(scheme)).ShouldBe(unchecked((int)0xFF42493E)),
            () => Argb(mdc.TextPrimaryInverseDisableOnly.GetColor(scheme)).ShouldBe(unchecked((int)0xFF191C17)),
            () => Argb(mdc.TextSecondaryAndTertiaryInverseDisabled.GetColor(scheme)).ShouldBe(unchecked((int)0xFF191C17)),
            () => Argb(mdc.TextHintInverse.GetColor(scheme)).ShouldBe(unchecked((int)0xFF191C17)));
    }
}
