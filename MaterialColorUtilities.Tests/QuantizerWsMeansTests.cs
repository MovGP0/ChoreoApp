namespace MaterialColorUtilities.Tests;

public sealed class QuantizerWsMeansTests
{
    private const int Red      = unchecked((int)0xFFFF0000);
    private const int Green    = unchecked((int)0xFF00FF00);
    private const int Blue     = unchecked((int)0xFF0000FF);
    private const int OneRando = unchecked((int)0xFF141216);
    private const int MaxColors = 256;

    private static int[] NoStartingClusters => [];

    [Fact(DisplayName = "1Rando")]
    public void OneRandomColor()
    {
        var result = QuantizerWsMeans.Quantize([OneRando], NoStartingClusters, MaxColors);
        var colors = result.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(OneRando);
    }

    [Fact(DisplayName = "1R (presence)")]
    public void OneRed_Presence()
    {
        var result = QuantizerWsMeans.Quantize([Red], NoStartingClusters, MaxColors);
        var colors = result.Keys.ToList();

        colors.Count.ShouldBe(1);
    }

    [Fact(DisplayName = "1R (exact)")]
    public void OneRed_Exact()
    {
        var result = QuantizerWsMeans.Quantize([Red], NoStartingClusters, MaxColors);
        var colors = result.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(Red);
    }

    [Fact(DisplayName = "1G")]
    public void OneGreen()
    {
        var result = QuantizerWsMeans.Quantize([Green], NoStartingClusters, MaxColors);
        var colors = result.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(Green);
    }

    [Fact(DisplayName = "1B")]
    public void OneBlue()
    {
        var result = QuantizerWsMeans.Quantize([Blue], NoStartingClusters, MaxColors);
        var colors = result.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(Blue);
    }

    [Fact(DisplayName = "5B")]
    public void FiveBlue()
    {
        var pixels = new[] { Blue, Blue, Blue, Blue, Blue };
        var result = QuantizerWsMeans.Quantize(pixels, NoStartingClusters, MaxColors);
        var colors = result.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(Blue);
        result[Blue].ShouldBe(5);
    }
}
