namespace MaterialColorUtilities.Tests;

public sealed class TemperatureCacheTests
{
    [Fact(DisplayName = "TemperatureCache.rawTemperature")]
    public void Raw_Temperature()
    {
        var blueTemp  = TemperatureCache.RawTemperature(Hct.FromInt(unchecked((int)0xFF0000FF)));
        var redTemp   = TemperatureCache.RawTemperature(Hct.FromInt(unchecked((int)0xFFFF0000)));
        var greenTemp = TemperatureCache.RawTemperature(Hct.FromInt(unchecked((int)0xFF00FF00)));
        var whiteTemp = TemperatureCache.RawTemperature(Hct.FromInt(unchecked((int)0xFFFFFFFF)));
        var blackTemp = TemperatureCache.RawTemperature(Hct.FromInt(unchecked((int)0xFF000000)));

        blueTemp.ShouldBe(-1.393, 0.001);
        redTemp.ShouldBe(2.351, 0.001);
        greenTemp.ShouldBe(-0.267, 0.001);
        whiteTemp.ShouldBe(-0.5, 0.001);
        blackTemp.ShouldBe(-0.5, 0.001);
    }

    [Fact(DisplayName = "TemperatureCache.inputRelativeTemperature")]
    public void Relative_Temperature()
    {
        var blue  = Hct.FromInt(unchecked((int)0xFF0000FF));
        var red   = Hct.FromInt(unchecked((int)0xFFFF0000));
        var green = Hct.FromInt(unchecked((int)0xFF00FF00));
        var white = Hct.FromInt(unchecked((int)0xFFFFFFFF));
        var black = Hct.FromInt(unchecked((int)0xFF000000));

        var blueTemp  = new TemperatureCache(blue ).GetRelativeTemperature(blue);
        var redTemp   = new TemperatureCache(red  ).GetRelativeTemperature(red);
        var greenTemp = new TemperatureCache(green).GetRelativeTemperature(green);
        var whiteTemp = new TemperatureCache(white).GetRelativeTemperature(white);
        var blackTemp = new TemperatureCache(black).GetRelativeTemperature(black);

        blueTemp.ShouldBe(0.000, 0.001);
        redTemp.ShouldBe(1.000, 0.001);
        greenTemp.ShouldBe(0.467, 0.001);
        whiteTemp.ShouldBe(0.500, 0.001);
        blackTemp.ShouldBe(0.500, 0.001);
    }

    [Fact(DisplayName = "TemperatureCache.complement")]
    public void Complement()
    {
        var blueComp  = new TemperatureCache(Hct.FromInt(unchecked((int)0xFF0000FF))).GetComplement().Argb;
        var redComp   = new TemperatureCache(Hct.FromInt(unchecked((int)0xFFFF0000))).GetComplement().Argb;
        var greenComp = new TemperatureCache(Hct.FromInt(unchecked((int)0xFF00FF00))).GetComplement().Argb;
        var whiteComp = new TemperatureCache(Hct.FromInt(unchecked((int)0xFFFFFFFF))).GetComplement().Argb;
        var blackComp = new TemperatureCache(Hct.FromInt(unchecked((int)0xFF000000))).GetComplement().Argb;

        blueComp.ShouldBe(unchecked((int)0xFF9D0002));
        redComp.ShouldBe(unchecked((int)0xFF007BFC));
        greenComp.ShouldBe(unchecked((int)0xFFFFD2C9));
        whiteComp.ShouldBe(unchecked((int)0xFFFFFFFF));
        blackComp.ShouldBe(unchecked((int)0xFF000000));
    }

    [Fact(DisplayName = "TemperatureCache.analogous")]
    public void Analogous()
    {
        // Blue
        var blueAnalogous = new TemperatureCache(Hct.FromInt(unchecked((int)0xFF0000FF)))
            .GetAnalogousColors()
            .Select(h => h.Argb)
            .ToList();

        blueAnalogous[0].ShouldBe(unchecked((int)0xFF00590C));
        blueAnalogous[1].ShouldBe(unchecked((int)0xFF00564E));
        blueAnalogous[2].ShouldBe(unchecked((int)0xFF0000FF));
        blueAnalogous[3].ShouldBe(unchecked((int)0xFF6700CC));
        blueAnalogous[4].ShouldBe(unchecked((int)0xFF81009F));

        // Red
        var redAnalogous = new TemperatureCache(Hct.FromInt(unchecked((int)0xFFFF0000)))
            .GetAnalogousColors()
            .Select(h => h.Argb)
            .ToList();

        redAnalogous[0].ShouldBe(unchecked((int)0xFFF60082));
        redAnalogous[1].ShouldBe(unchecked((int)0xFFFC004C));
        redAnalogous[2].ShouldBe(unchecked((int)0xFFFF0000));
        redAnalogous[3].ShouldBe(unchecked((int)0xFFD95500));
        redAnalogous[4].ShouldBe(unchecked((int)0xFFAF7200));

        // Green
        var greenAnalogous = new TemperatureCache(Hct.FromInt(unchecked((int)0xFF00FF00)))
            .GetAnalogousColors()
            .Select(h => h.Argb)
            .ToList();

        greenAnalogous[0].ShouldBe(unchecked((int)0xFFCEE900));
        greenAnalogous[1].ShouldBe(unchecked((int)0xFF92F500));
        greenAnalogous[2].ShouldBe(unchecked((int)0xFF00FF00));
        greenAnalogous[3].ShouldBe(unchecked((int)0xFF00FD6F));
        greenAnalogous[4].ShouldBe(unchecked((int)0xFF00FAB3));

        // Black → all black
        var blackAnalogous = new TemperatureCache(Hct.FromInt(unchecked((int)0xFF000000)))
            .GetAnalogousColors()
            .Select(h => h.Argb)
            .ToList();

        blackAnalogous[0].ShouldBe(unchecked((int)0xFF000000));
        blackAnalogous[1].ShouldBe(unchecked((int)0xFF000000));
        blackAnalogous[2].ShouldBe(unchecked((int)0xFF000000));
        blackAnalogous[3].ShouldBe(unchecked((int)0xFF000000));
        blackAnalogous[4].ShouldBe(unchecked((int)0xFF000000));

        // White → all white
        var whiteAnalogous = new TemperatureCache(Hct.FromInt(unchecked((int)0xFFFFFFFF)))
            .GetAnalogousColors()
            .Select(h => h.Argb)
            .ToList();

        whiteAnalogous[0].ShouldBe(unchecked((int)0xFFFFFFFF));
        whiteAnalogous[1].ShouldBe(unchecked((int)0xFFFFFFFF));
        whiteAnalogous[2].ShouldBe(unchecked((int)0xFFFFFFFF));
        whiteAnalogous[3].ShouldBe(unchecked((int)0xFFFFFFFF));
        whiteAnalogous[4].ShouldBe(unchecked((int)0xFFFFFFFF));
    }
}
