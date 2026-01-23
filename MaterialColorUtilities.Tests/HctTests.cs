namespace MaterialColorUtilities.Tests;

public sealed class HctTests
{
    [Fact(DisplayName = "HCT preserves original color for all opaque ARGB values", Skip = "Takes a long time to run")]
    public void Hct_Preserves_Original_Color_For_All_Opaque_ARGB()
    {
        // Iterate from 0xFF000000 to 0xFFFFFFFF inclusive (all opaque colors).
        uint argb = 0xFF000000u;
        while (true)
        {
            int argbInt = unchecked((int)argb);

            var hct = Hct.FromInt(argbInt);
            var reconstructedArgb = Hct.From(hct.Hue, hct.Chroma, hct.Tone).Argb;

            reconstructedArgb.ShouldBe(argbInt);

            if (argb == 0xFFFFFFFFu)
                break;

            argb++;
        }
    }

    private const int Black   = unchecked((int)0xFF000000);
    private const int White   = unchecked((int)0xFFFFFFFF);
    private const int Red     = unchecked((int)0xFFFF0000);
    private const int Green   = unchecked((int)0xFF00FF00);
    private const int Blue    = unchecked((int)0xFF0000FF);
    private const int MidGray = unchecked((int)0xFF777777);

    private static bool ColorIsOnBoundary(int argb) =>
        ColorUtils.RedFromArgb(argb) == 0   || ColorUtils.RedFromArgb(argb) == 255 ||
        ColorUtils.GreenFromArgb(argb) == 0 || ColorUtils.GreenFromArgb(argb) == 255 ||
        ColorUtils.BlueFromArgb(argb) == 0  || ColorUtils.BlueFromArgb(argb) == 255;

    [Fact(DisplayName = "Hct equality/hash basics (by ARGB)")]
    public void Hct_Equality_Hash_Basics_ByArgb()
    {
        var a = Hct.FromInt(123);
        var b = Hct.FromInt(123);

        // Equality by ARGB value
        a.Argb.ShouldBe(b.Argb);

        // Hash by ARGB value
        a.Argb.GetHashCode().ShouldBe(b.Argb.GetHashCode());
    }

    [Fact(DisplayName = "CAM16 conversions are reflexive via viewed(sRGB)")]
    public void Conversions_Are_Reflexive()
    {
        var cam = Cam16.FromInt(Red);

        // pass an empty buffer again (API requires the arg)
        var xyz = cam.XyzInViewingConditions(ViewingConditions.DEFAULT);
        var color = ColorUtils.ArgbFromXyz(xyz[0], xyz[1], xyz[2]);

        color.ShouldBe(Red);
    }

    [Fact] public void Y_Midgray() => ColorUtils.YFromLstar(50.0).ShouldBe(18.418, 0.001);
    [Fact] public void Y_Black()   => ColorUtils.YFromLstar(0.0).ShouldBe(0.0, 0.001);
    [Fact] public void Y_White()   => ColorUtils.YFromLstar(100).ShouldBe(100.0, 0.001);

    [Fact(DisplayName = "CAM16 red metrics")]
    public void Cam_Red()
    {
        var cam = Cam16.FromInt(Red);
        cam.GetJ().ShouldBe(46.445, 0.001);
        cam.GetChroma().ShouldBe(113.357, 0.001);
        cam.GetHue().ShouldBe(27.408, 0.001);
        cam.GetM().ShouldBe(89.494, 0.001);
        cam.GetS().ShouldBe(91.889, 0.001);
        cam.GetQ().ShouldBe(105.988, 0.001);
    }

    [Fact(DisplayName = "CAM16 green metrics")]
    public void Cam_Green()
    {
        var cam = Cam16.FromInt(Green);
        cam.GetJ().ShouldBe(79.331, 0.001);
        cam.GetChroma().ShouldBe(108.410, 0.001);
        cam.GetHue().ShouldBe(142.139, 0.001);
        cam.GetM().ShouldBe(85.587, 0.001);
        cam.GetS().ShouldBe(78.604, 0.001);
        cam.GetQ().ShouldBe(138.520, 0.001);
    }

    [Fact(DisplayName = "CAM16 blue metrics")]
    public void Cam_Blue()
    {
        var cam = Cam16.FromInt(Blue);
        cam.GetJ().ShouldBe(25.465, 0.001);
        cam.GetChroma().ShouldBe(87.230, 0.001);
        cam.GetHue().ShouldBe(282.788, 0.001);
        cam.GetM().ShouldBe(68.867, 0.001);
        cam.GetS().ShouldBe(93.674, 0.001);
        cam.GetQ().ShouldBe(78.481, 0.001);
    }

    [Fact(DisplayName = "CAM16 black metrics")]
    public void Cam_Black()
    {
        var cam = Cam16.FromInt(Black);
        cam.GetJ().ShouldBe(0.0, 0.001);
        cam.GetChroma().ShouldBe(0.0, 0.001);
        cam.GetHue().ShouldBe(0.0, 0.001);
        cam.GetM().ShouldBe(0.0, 0.001);
        cam.GetS().ShouldBe(0.0, 0.001);
        cam.GetQ().ShouldBe(0.0, 0.001);
    }

    [Fact(DisplayName = "CAM16 white metrics")]
    public void Cam_White()
    {
        var cam = Cam16.FromInt(White);
        cam.GetJ().ShouldBe(100.0, 0.001);
        cam.GetChroma().ShouldBe(2.869, 0.001);
        cam.GetHue().ShouldBe(209.492, 0.001);
        cam.GetM().ShouldBe(2.265, 0.001);
        cam.GetS().ShouldBe(12.068, 0.001);
        cam.GetQ().ShouldBe(155.521, 0.001);
    }

    [Fact] public void GamutMap_Red() => AssertGamutMapIdentity(Red);
    [Fact] public void GamutMap_Green() => AssertGamutMapIdentity(Green);
    [Fact] public void GamutMap_Blue() => AssertGamutMapIdentity(Blue);
    [Fact] public void GamutMap_White() => AssertGamutMapIdentity(White);
    // NOTE: The Dart “midgray” test body actually used `green`; keep parity by testing `green`.
    [Fact] public void GamutMap_Midgray_ParityWithDart() => AssertGamutMapIdentity(Green);

    private static void AssertGamutMapIdentity(int colorToTest)
    {
        var cam = Cam16.FromInt(colorToTest);
        var color = Hct.From(cam.GetHue(), cam.GetChroma(), ColorUtils.LstarFromArgb(colorToTest)).Argb;
        color.ShouldBe(colorToTest);
    }

    [Fact(DisplayName = "HCT returns a sufficiently close color")]
    public void Hct_Returns_Sufficiently_Close_Color()
    {
        for (int hue = 15; hue < 360; hue += 30)
        for (int chroma = 0; chroma <= 100; chroma += 10)
        for (int tone = 20; tone <= 80; tone += 10)
        {
            var desc = $"H{hue} C{chroma} T{tone}";
            var hct = Hct.From(hue, chroma, tone);

            if (chroma > 0)
            {
                hct.Hue.ShouldBe(hue, 4.0, $"Hue should be close for {desc}");
            }

            // chroma ∈ [0, chroma + 2.5]
            hct.Chroma.ShouldBeGreaterThanOrEqualTo(0.0, $"Chroma lower bound for {desc}");
            hct.Chroma.ShouldBeLessThanOrEqualTo(chroma + 2.5, $"Chroma should be close or less for {desc}");

            if (hct.Chroma < chroma - 2.5)
            {
                // Non-sRGB request should land on sRGB cube boundary
                ColorIsOnBoundary(hct.Argb)
                    .ShouldBeTrue($"Out-of-gamut {desc} should be on sRGB boundary, got 0x{hct.Argb:X8}");
            }

            hct.Tone.ShouldBe(tone, 0.5, $"Tone should be close for {desc}");
        }
    }

    [Fact(DisplayName = "CAM16 to XYZ (without preallocated array)")]
    public void Cam16_To_Xyz_NoArray()
    {
        var cam = Cam16.FromInt(Red);

        // pass an empty buffer to indicate "no preallocated array"
        var xyz = cam.XyzInViewingConditions(ViewingConditions.DEFAULT);

        xyz[0].ShouldBe(41.23, 0.01);
        xyz[1].ShouldBe(21.26, 0.01);
        xyz[2].ShouldBe(1.93, 0.01);
    }

    [Fact(DisplayName = "CAM16 to XYZ (with preallocated array)")]
    public void Cam16_To_Xyz_WithArray()
    {
        var cam = Cam16.FromInt(Red);

        // Provide a length-3 buffer
        var buffer = new double[3];
        var xyz = cam.XyzInViewingConditions(ViewingConditions.DEFAULT, buffer);

        xyz[0].ShouldBe(41.23, 0.01);
        xyz[1].ShouldBe(21.26, 0.01);
        xyz[2].ShouldBe(1.93, 0.01);

        // Optional: verify the implementation reused the provided array
        // await Assert.That(ReferenceEquals(xyz, buffer)).IsTrue();
    }

    [Fact(DisplayName = "Color Relativity — red in black/white")]
    public void ColorRelativity_Red()
    {
        var hct = Hct.FromInt(Red);
        var inBlack = hct.InViewingConditions(ViewingConditions.DefaultWithBackgroundLstar(0.0)).Argb;
        var inWhite = hct.InViewingConditions(ViewingConditions.DefaultWithBackgroundLstar(100.0)).Argb;

        inBlack.ShouldBe(unchecked((int)0xFF9F5C51));
        inWhite.ShouldBe(unchecked((int)0xFFFF5D48));
    }

    [Fact(DisplayName = "Color Relativity — green in black/white")]
    public void ColorRelativity_Green()
    {
        var hct = Hct.FromInt(Green);
        var inBlack = hct.InViewingConditions(ViewingConditions.DefaultWithBackgroundLstar(0.0)).Argb;
        var inWhite = hct.InViewingConditions(ViewingConditions.DefaultWithBackgroundLstar(100.0)).Argb;

        inBlack.ShouldBe(unchecked((int)0xFFACD69D));
        inWhite.ShouldBe(unchecked((int)0xFF8EFF77));
    }

    [Fact(DisplayName = "Color Relativity — blue in black/white")]
    public void ColorRelativity_Blue()
    {
        var hct = Hct.FromInt(Blue);
        var inBlack = hct.InViewingConditions(ViewingConditions.DefaultWithBackgroundLstar(0.0)).Argb;
        var inWhite = hct.InViewingConditions(ViewingConditions.DefaultWithBackgroundLstar(100.0)).Argb;

        inBlack.ShouldBe(unchecked((int)0xFF343654));
        inWhite.ShouldBe(unchecked((int)0xFF3F49FF));
    }

    [Fact(DisplayName = "Color Relativity — white in black/white")]
    public void ColorRelativity_White()
    {
        var hct = Hct.FromInt(White);
        var inBlack = hct.InViewingConditions(ViewingConditions.DefaultWithBackgroundLstar(0.0)).Argb;
        var inWhite = hct.InViewingConditions(ViewingConditions.DefaultWithBackgroundLstar(100.0)).Argb;

        inBlack.ShouldBe(unchecked((int)0xFFFFFFFF));
        inWhite.ShouldBe(unchecked((int)0xFFFFFFFF));
    }

    [Fact(DisplayName = "Color Relativity — midgray in black/white")]
    public void ColorRelativity_MidGray()
    {
        var hct = Hct.FromInt(MidGray);
        var inBlack = hct.InViewingConditions(ViewingConditions.DefaultWithBackgroundLstar(0.0)).Argb;
        var inWhite = hct.InViewingConditions(ViewingConditions.DefaultWithBackgroundLstar(100.0)).Argb;

        inBlack.ShouldBe(unchecked((int)0xFF605F5F));
        inWhite.ShouldBe(unchecked((int)0xFF8E8E8E));
    }

    [Fact(DisplayName = "Color Relativity — black in black/white")]
    public void ColorRelativity_Black()
    {
        var hct = Hct.FromInt(Black);
        var inBlack = hct.InViewingConditions(ViewingConditions.DefaultWithBackgroundLstar(0.0)).Argb;
        var inWhite = hct.InViewingConditions(ViewingConditions.DefaultWithBackgroundLstar(100.0)).Argb;

        inBlack.ShouldBe(unchecked((int)0xFF000000));
        inWhite.ShouldBe(unchecked((int)0xFF000000));
    }
}
