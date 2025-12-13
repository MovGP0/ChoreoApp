namespace MaterialColorUtilities;

public class SchemeContent: DynamicScheme
{
    public SchemeContent(Hct primaryColorHct, bool isDark, double contrastLevel)
        : this(primaryColorHct, isDark, contrastLevel, DEFAULT_SPEC_VERSION, DEFAULT_PLATFORM)
    {
    }

    public SchemeContent(Hct primaryColorHct, Hct secondaryColorHct, bool isDark, double contrastLevel)
        : this(primaryColorHct, secondaryColorHct, isDark, contrastLevel, DEFAULT_SPEC_VERSION, DEFAULT_PLATFORM)
    {
    }

    public SchemeContent(Hct primaryColorHct, Hct secondaryColorHct, Hct tertiaryColorHct, bool isDark, double contrastLevel)
        : this(primaryColorHct, secondaryColorHct, tertiaryColorHct, isDark, contrastLevel, DEFAULT_SPEC_VERSION, DEFAULT_PLATFORM)
    {
    }

    public SchemeContent(
        Hct sourceColorHct,
        bool isDark,
        double contrastLevel,
        SpecVersion specVersion,
        Platform platform)
        : base(
            sourceColorHct,
            Variant.Content,
            isDark,
            contrastLevel,
            platform,
            specVersion,
            ColorSpecs.Get(specVersion).GetPrimaryPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetSecondaryPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetTertiaryPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetNeutralPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetNeutralVariantPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetErrorPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel))
    {
    }

    public SchemeContent(
        Hct sourceColorHct,
        Hct secondaryColorHct,
        bool isDark,
        double contrastLevel,
        SpecVersion specVersion,
        Platform platform)
        : base(
            sourceColorHct,
            Variant.Content,
            isDark,
            contrastLevel,
            platform,
            specVersion,
            ColorSpecs.Get(specVersion).GetPrimaryPalette(Variant.Content, secondaryColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetSecondaryPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetTertiaryPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetNeutralPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetNeutralVariantPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetErrorPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel))
    {
    }

    public SchemeContent(
        Hct sourceColorHct,
        Hct secondaryColorHct,
        Hct tertiaryColorHct,
        bool isDark,
        double contrastLevel,
        SpecVersion specVersion,
        Platform platform)
        : base(
            sourceColorHct,
            Variant.Content,
            isDark,
            contrastLevel,
            platform,
            specVersion,
            ColorSpecs.Get(specVersion).GetPrimaryPalette(Variant.Content, secondaryColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetSecondaryPalette(Variant.Content, tertiaryColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetTertiaryPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetNeutralPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetNeutralVariantPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel),
            ColorSpecs.Get(specVersion).GetErrorPalette(Variant.Content, sourceColorHct, isDark, platform, contrastLevel))
    {
    }
}
