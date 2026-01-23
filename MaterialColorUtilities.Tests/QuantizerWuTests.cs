namespace MaterialColorUtilities.Tests;

public sealed class QuantizerWuTests
{
    private const int Red   = unchecked((int)0xFFFF0000);
    private const int Green = unchecked((int)0xFF00FF00);
    private const int Blue  = unchecked((int)0xFF0000FF);
    private const int MaxColors = 256;

    [Fact(DisplayName = "1R (presence)")]
    public void OneRed_Presence()
    {
        var wu = new QuantizerWu();
        var result = wu.Quantize([Red], MaxColors);
        var colors = result.ColorToCount.Keys.ToList();

        colors.Count.ShouldBe(1);
    }

    [Fact(DisplayName = "1Rando")]
    public void OneRandom()
    {
        var wu = new QuantizerWu();
        var argb = unchecked((int)0xFF141216);
        var result = wu.Quantize([argb], MaxColors);
        var colors = result.ColorToCount.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(argb);
    }

    [Fact(DisplayName = "1R (exact)")]
    public void OneRed_Exact()
    {
        var wu = new QuantizerWu();
        var result = wu.Quantize([Red], MaxColors);
        var colors = result.ColorToCount.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(Red);
    }

    [Fact(DisplayName = "1G")]
    public void OneGreen()
    {
        var wu = new QuantizerWu();
        var result = wu.Quantize([Green], MaxColors);
        var colors = result.ColorToCount.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(Green);
    }

    [Fact(DisplayName = "1B")]
    public void OneBlue()
    {
        var wu = new QuantizerWu();
        var result = wu.Quantize([Blue], MaxColors);
        var colors = result.ColorToCount.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(Blue);
    }

    [Fact(DisplayName = "5B")]
    public void FiveBlue()
    {
        var wu = new QuantizerWu();
        var pixels = new[] { Blue, Blue, Blue, Blue, Blue };
        var result = wu.Quantize(pixels, MaxColors);
        var colors = result.ColorToCount.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(Blue);
    }

    [Fact(DisplayName = "2R 3G")]
    public void TwoRed_ThreeGreen()
    {
        var wu = new QuantizerWu();
        var pixels = new[] { Red, Red, Green, Green, Green };
        var result = wu.Quantize(pixels, MaxColors);
        var colors = result.ColorToCount.Keys.ToList();

        // Set membership
        colors.ToHashSet().Count.ShouldBe(2);
        colors.ShouldContain(Green);
        colors.ShouldContain(Red);

        // Dart asserts order: [green, red]
        colors[0].ShouldBe(Green);
        colors[1].ShouldBe(Red);
    }

    [Fact(DisplayName = "1R 1G 1B")]
    public void OneRed_OneGreen_OneBlue()
    {
        var wu = new QuantizerWu();
        var result = wu.Quantize([Red, Green, Blue], MaxColors);
        var colors = result.ColorToCount.Keys.ToList();

        // Set membership
        colors.ToHashSet().Count.ShouldBe(3);
        colors.ShouldContain(Blue);
        colors.ShouldContain(Red);
        colors.ShouldContain(Green);

        // Dart asserts order: [blue, red, green]
        colors[0].ShouldBe(Blue);
        colors[1].ShouldBe(Red);
        colors[2].ShouldBe(Green);
    }
}
