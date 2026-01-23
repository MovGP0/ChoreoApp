namespace MaterialColorUtilities.Tests;

public sealed class TonalPaletteTests
{
    private static readonly int[] CommonTones = [0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 95, 99, 100];

    private static int[] BuildArgbList(TonalPalette p) => CommonTones.Select(p.Tone).ToArray();

    [Fact(DisplayName = "[FromHueAndChroma] tones of blue")]
    public void FromHueAndChroma_Tones_Of_Blue()
    {
        var hctBlue = Hct.FromInt(unchecked((int)0xFF0000FF));
        var tones = TonalPalette.FromHueAndChroma(hctBlue.Hue, hctBlue.Chroma);

        tones.Tone(0).ShouldBe(unchecked((int)0xFF000000));
        tones.Tone(10).ShouldBe(unchecked((int)0xFF00006E));
        tones.Tone(20).ShouldBe(unchecked((int)0xFF0001AC));
        tones.Tone(30).ShouldBe(unchecked((int)0xFF0000EF));
        tones.Tone(40).ShouldBe(unchecked((int)0xFF343DFF));
        tones.Tone(50).ShouldBe(unchecked((int)0xFF5A64FF));
        tones.Tone(60).ShouldBe(unchecked((int)0xFF7C84FF));
        tones.Tone(70).ShouldBe(unchecked((int)0xFF9DA3FF));
        tones.Tone(80).ShouldBe(unchecked((int)0xFFBEC2FF));
        tones.Tone(90).ShouldBe(unchecked((int)0xFFE0E0FF));
        tones.Tone(95).ShouldBe(unchecked((int)0xFFF1EFFF));
        tones.Tone(99).ShouldBe(unchecked((int)0xFFFFFBFF));
        tones.Tone(100).ShouldBe(unchecked((int)0xFFFFFFFF));

        // Tone not in Dart's commonTones: 3
        tones.Tone(3).ShouldBe(unchecked((int)0xFF00003C));
    }

    [Fact(DisplayName = "[FromHueAndChroma] asList (common tones)")]
    public void FromHueAndChroma_AsList_CommonTones()
    {
        var hctBlue = Hct.FromInt(unchecked((int)0xFF0000FF));
        var tones = TonalPalette.FromHueAndChroma(hctBlue.Hue, hctBlue.Chroma);

        var expected = new[]
        {
            unchecked((int)0xFF000000),
            unchecked((int)0xFF00006E),
            unchecked((int)0xFF0001AC),
            unchecked((int)0xFF0000EF),
            unchecked((int)0xFF343DFF),
            unchecked((int)0xFF5A64FF),
            unchecked((int)0xFF7C84FF),
            unchecked((int)0xFF9DA3FF),
            unchecked((int)0xFFBEC2FF),
            unchecked((int)0xFFE0E0FF),
            unchecked((int)0xFFF1EFFF),
            unchecked((int)0xFFFFFBFF),
            unchecked((int)0xFFFFFFFF),
        };

        var actual = BuildArgbList(tones);

        // Compare element-wise
        actual.Length.ShouldBe(expected.Length);
        for (int i = 0; i < expected.Length; i++)
        {
            actual[i].ShouldBe(expected[i]);
        }
    }

    [Fact(DisplayName = "[FromHueAndChroma] equivalence by generated tones (==/hashCode analog)")]
    public void FromHueAndChroma_Equivalence_By_Tones()
    {
        var hctAB = Hct.FromInt(unchecked((int)0xFF0000FF));
        var tonesA = TonalPalette.FromHueAndChroma(hctAB.Hue, hctAB.Chroma);
        var tonesB = TonalPalette.FromHueAndChroma(hctAB.Hue, hctAB.Chroma);

        var hctC = Hct.FromInt(unchecked((int)0xFF123456));
        var tonesC = TonalPalette.FromHueAndChroma(hctC.Hue, hctC.Chroma);

        var listA = BuildArgbList(tonesA);
        var listB = BuildArgbList(tonesB);
        var listC = BuildArgbList(tonesC);

        // A equals B functionally
        for (int i = 0; i < listA.Length; i++)
        {
            listA[i].ShouldBe(listB[i]);
        }

        // B differs from C at least at one tone
        bool anyDiff = listB.Where((v, i) => v != listC[i]).Any();
        anyDiff.ShouldBeTrue();
    }

    [Fact(DisplayName = "KeyColor: exact chroma is available")]
    public void KeyColor_Exact_Chroma()
    {
        var palette = TonalPalette.FromHueAndChroma(50.0, 60.0);
        var result = palette.GetKeyColor();

        result.Hue.ShouldBe(50.0, 10.0);
        result.Chroma.ShouldBe(60.0, 0.5);
        result.Tone.ShouldBeGreaterThan(0.0);
        result.Tone.ShouldBeLessThan(100.0);
    }

    [Fact(DisplayName = "KeyColor: requesting unusually high chroma")]
    public void KeyColor_Unusually_High_Chroma()
    {
        // For Hue 149, chroma peak ~89.6 — result should approach peak if 200 requested.
        var palette = TonalPalette.FromHueAndChroma(149.0, 200.0);
        var result = palette.GetKeyColor();

        result.Hue.ShouldBe(149.0, 10.0);
        result.Chroma.ShouldBeGreaterThan(89.0);
        result.Tone.ShouldBeGreaterThan(0.0);
        result.Tone.ShouldBeLessThan(100.0);
    }

    [Fact(DisplayName = "KeyColor: requesting unusually low chroma")]
    public void KeyColor_Unusually_Low_Chroma()
    {
        var palette = TonalPalette.FromHueAndChroma(50.0, 3.0);
        var result = palette.GetKeyColor();

        result.Hue.ShouldBe(50.0, 10.0);
        result.Chroma.ShouldBe(3.0, 0.5);
        result.Tone.ShouldBe(50.0, 0.5);
    }
}
