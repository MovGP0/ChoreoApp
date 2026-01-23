namespace MaterialColorUtilities.Tests;

public sealed class ColorUtilsTests
{
    private static IEnumerable<double> Range(double start, double stop, int caseCount)
    {
        if (caseCount <= 1)
        {
            yield return start;
            yield break;
        }

        var stepSize = (stop - start) / (caseCount - 1);
        for (var i = 0; i < caseCount; i++)
        {
            yield return start + stepSize * i;
        }
    }

    private static IReadOnlyList<int> RgbRange() => Range(0.0, 255.0, 8).Select(d => (int)Math.Round(d)).ToArray();

    private static IReadOnlyList<int> FullRgbRange() => Enumerable.Range(0, 256).ToArray();

    [Fact(DisplayName = "range_integrity")]
    public void Range_Integrity()
    {
        // Dart: final range = _range(3.0, 9999.0, 1234);
        var range = Range(3.0, 9999.0, 1234).ToArray();

        // Dart: expect(range[i], closeTo(3 + 8.1070559611 * i, 1e-5));
        for (var i = 0; i < 1234; i++)
        {
            var expected = 3 + 8.1070559611 * i;
            range[i].ShouldBe(expected, 1e-5);
        }
    }

    [Fact(DisplayName = "argbFromRgb returns correct values")]
    public void ArgbFromRgb_KnownVectors()
    {
        // black
        ColorUtils.ArgbFromRgb(0, 0, 0).ShouldBe(unchecked((int)0xFF000000));
        ColorUtils.ArgbFromRgb(0, 0, 0).ShouldBe(unchecked((int)4278190080));

        // white
        ColorUtils.ArgbFromRgb(255, 255, 255).ShouldBe(unchecked((int)0xFFFFFFFF));
        ColorUtils.ArgbFromRgb(255, 255, 255).ShouldBe(unchecked((int)4294967295));

        // random color
        ColorUtils.ArgbFromRgb(50, 150, 250).ShouldBe(unchecked((int)0xFF3296FA));
        ColorUtils.ArgbFromRgb(50, 150, 250).ShouldBe(unchecked((int)4281505530));
    }

    [Fact(DisplayName = "y_to_lstar_to_y")]
    public void Y_To_Lstar_To_Y_RoundTrip()
    {
        foreach (var y in Range(0, 100, 1001))
        {
            var l = ColorUtils.LstarFromY(y);
            var y2 = ColorUtils.YFromLstar(l);
            y2.ShouldBe(y, 1e-5);
        }
    }

    [Fact(DisplayName = "lstar_to_y_to_lstar")]
    public void Lstar_To_Y_To_Lstar_RoundTrip()
    {
        foreach (var lstar in Range(0, 100, 1001))
        {
            var y = ColorUtils.YFromLstar(lstar);
            var l2 = ColorUtils.LstarFromY(y);
            l2.ShouldBe(lstar, 1e-5);
        }
    }

    [Fact(DisplayName = "yFromLstar numeric vectors")]
    public void YFromLstar_Vectors()
    {
        void Check(double lstar, double expected) =>
            ColorUtils.YFromLstar(lstar).ShouldBe(expected, 1e-5);

        Check(0.0, 0.0);
        Check(0.1, 0.0110705);
        Check(0.2, 0.0221411);
        Check(0.3, 0.0332116);
        Check(0.4, 0.0442822);
        Check(0.5, 0.0553528);
        Check(1.0, 0.1107056);
        Check(2.0, 0.2214112);
        Check(3.0, 0.3321169);
        Check(4.0, 0.4428225);
        Check(5.0, 0.5535282);
        Check(8.0, 0.8856451);
        Check(10.0, 1.1260199);
        Check(15.0, 1.9085832);
        Check(20.0, 2.9890524);
        Check(25.0, 4.4154767);
        Check(30.0, 6.2359055);
        Check(40.0, 11.2509737);
        Check(50.0, 18.4186518);
        Check(60.0, 28.1233342);
        Check(70.0, 40.7494157);
        Check(80.0, 56.6812907);
        Check(90.0, 76.3033539);
        Check(95.0, 87.6183294);
        Check(99.0, 97.4360239);
        Check(100.0, 100.0);
    }

    [Fact(DisplayName = "lstarFromY numeric vectors")]
    public void LstarFromY_Vectors()
    {
        void Check(double y, double expected) =>
            ColorUtils.LstarFromY(y).ShouldBe(expected, 1e-5);

        Check(0.0, 0.0);
        Check(0.1, 0.9032962);
        Check(0.2, 1.8065925);
        Check(0.3, 2.7098888);
        Check(0.4, 3.6131851);
        Check(0.5, 4.5164814);
        Check(0.8856451, 8.0);
        Check(1.0, 8.9914424);
        Check(2.0, 15.4872443);
        Check(3.0, 20.0438970);
        Check(4.0, 23.6714419);
        Check(5.0, 26.7347653);
        Check(10.0, 37.8424304);
        Check(15.0, 45.6341970);
        Check(20.0, 51.8372115);
        Check(25.0, 57.0754208);
        Check(30.0, 61.6542222);
        Check(40.0, 69.4695307);
        Check(50.0, 76.0692610);
        Check(60.0, 81.8381891);
        Check(70.0, 86.9968642);
        Check(80.0, 91.6848609);
        Check(90.0, 95.9967686);
        Check(95.0, 98.0335184);
        Check(99.0, 99.6120372);
        Check(100.0, 100.0);
    }

    [Fact(DisplayName = "y continuity at l* = 8")]
    public void Y_Continuity()
    {
        const double epsilon = 1e-6;
        const double delta = 1e-8;
        var left = 8.0 - delta;
        var mid = 8.0;
        var right = 8.0 + delta;

        ColorUtils.YFromLstar(left)
            .ShouldBe(ColorUtils.YFromLstar(mid), epsilon);

        ColorUtils.YFromLstar(right)
            .ShouldBe(ColorUtils.YFromLstar(mid), epsilon);
    }

    [Fact(DisplayName = "rgb -> xyz -> rgb roundtrip (approx)")]
    public void Rgb_To_Xyz_To_Rgb_Roundtrip()
    {
        var range = RgbRange();
        foreach (var r in range)
        foreach (var g in range)
        foreach (var b in range)
        {
            var argb = ColorUtils.ArgbFromRgb(r, g, b);
            var xyz = ColorUtils.XyzFromArgb(argb);
            var converted = ColorUtils.ArgbFromXyz(xyz[0], xyz[1], xyz[2]);

            ((double)ColorUtils.RedFromArgb(converted)).ShouldBe(r, 1.5);
            ((double)ColorUtils.GreenFromArgb(converted)).ShouldBe(g, 1.5);
            ((double)ColorUtils.BlueFromArgb(converted)).ShouldBe(b, 1.5);
        }
    }

    [Fact(DisplayName = "rgb -> lab -> rgb roundtrip (approx)")]
    public void Rgb_To_Lab_To_Rgb_Roundtrip()
    {
        var range = RgbRange();
        foreach (var r in range)
        foreach (var g in range)
        foreach (var b in range)
        {
            var argb = ColorUtils.ArgbFromRgb(r, g, b);
            var lab = ColorUtils.LabFromArgb(argb);
            var converted = ColorUtils.ArgbFromLab(lab[0], lab[1], lab[2]);

            ((double)ColorUtils.RedFromArgb(converted)).ShouldBe(r, 1.5);
            ((double)ColorUtils.GreenFromArgb(converted)).ShouldBe(g, 1.5);
            ((double)ColorUtils.BlueFromArgb(converted)).ShouldBe(b, 1.5);
        }
    }

    [Fact(DisplayName = "rgb -> l* -> rgb (exact, via argbFromLstar)")]
    public void Rgb_To_Lstar_To_Rgb()
    {
        foreach (var component in FullRgbRange())
        {
            var argb = ColorUtils.ArgbFromRgb(component, component, component);
            var lstar = ColorUtils.LstarFromArgb(argb);
            var converted = ColorUtils.ArgbFromLstar(lstar);
            converted.ShouldBe(argb);
        }
    }

    [Fact(DisplayName = "rgb -> l* -> y commutes with Y from XYZ")]
    public void Rgb_To_Lstar_To_Y_Commutes()
    {
        var range = RgbRange();
        foreach (var r in range)
        foreach (var g in range)
        foreach (var b in range)
        {
            var argb = ColorUtils.ArgbFromRgb(r, g, b);
            var lstar = ColorUtils.LstarFromArgb(argb);
            var y1 = ColorUtils.YFromLstar(lstar);
            var y2 = ColorUtils.XyzFromArgb(argb)[1];

            y1.ShouldBe(y2, 1e-5);
        }
    }

    [Fact(DisplayName = "l* -> rgb -> y commutes (looser tol)")]
    public void Lstar_To_Rgb_To_Y_Commutes()
    {
        foreach (var lstar in Range(0, 100, 1001))
        {
            var argb = ColorUtils.ArgbFromLstar(lstar);
            var yFromRgb = ColorUtils.XyzFromArgb(argb)[1];
            var yFromL = ColorUtils.YFromLstar(lstar);

            yFromRgb.ShouldBe(yFromL, 1.0);
        }
    }

    [Fact(DisplayName = "linearize -> delinearize is identity on 0..255")]
    public void Linearize_Delinearize_RoundTrip()
    {
        foreach (var c in FullRgbRange())
        {
            var lin = ColorUtils.Linearized(c);
            var converted = ColorUtils.Delinearized(lin);
            converted.ShouldBe(c);
        }
    }

    public static IEnumerable<object[]> TestColors()
    {
        yield return [unchecked((int)0xFFFF0000), FromArgbBytes(0xFF, 0xFF, 0x00, 0x00)];
        yield return [unchecked((int)0xFF00FF00), FromArgbBytes(0xFF, 0x00, 0xFF, 0x00)];
        yield return [unchecked((int)0xFF0000FF), FromArgbBytes(0xFF, 0x00, 0x00, 0xFF)];
        yield return [unchecked((int)0xFFFF00FF), FromArgbBytes(0xFF, 0xFF, 0x00, 0xFF)];
        yield return [unchecked((int)0xFFFFFF00), FromArgbBytes(0xFF, 0xFF, 0xFF, 0x00)];
        yield return [unchecked((int)0xFF00FFFF), FromArgbBytes(0xFF, 0x00, 0xFF, 0xFF)];
        yield return [unchecked((int)0xFFFFFFFF), FromArgbBytes(0xFF, 0xFF, 0xFF, 0xFF)];
        yield return [unchecked((int)0xFF000000), FromArgbBytes(0xFF, 0x00, 0x00, 0x00)];
        yield return [unchecked((int)0x00FFFFFF), FromArgbBytes(0x00, 0xFF, 0xFF, 0xFF)];
    }

    private static Color FromArgbBytes(byte a, byte r, byte g, byte b) =>
        new(r / 255f, g / 255f, b / 255f, a / 255f);

    [Theory(DisplayName = "colorFromArgb returns known Colors")]
    [MemberData(nameof(TestColors))]
    public void ColorFromArgb_KnownColors(int argb, Color color)
    {
        var converted = Color.FromArgb(argb);

        string result = Color.ArgbFromColor(converted).ToString("X");
        string expected = Color.ArgbFromColor(color).ToString("X");

        result.ShouldBe(expected);
    }

    [Theory(DisplayName = "argbFromColor returns known ints")]
    [MemberData(nameof(TestColors))]
    public void ArgbFromColor_KnownColors(int argb, Color color)
    {
        string result = Color.ArgbFromColor(color).ToString("X");
        string expected = argb.ToString("X");

        result.ShouldBe(expected);
    }
}
