namespace MaterialColorUtilities.Tests;

public sealed class QuantizerCelebiTests
{
    private const int Red   = unchecked((int)0xFFFF0000);
    private const int Green = unchecked((int)0xFF00FF00);
    private const int Blue  = unchecked((int)0xFF0000FF);
    private const int MaxColors = 256;

    [Fact(DisplayName = "1R")]
    public void OneRed()
    {
        var result = QuantizerCelebi.Quantize([Red], MaxColors);
        var colors = result.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(Red);
    }

    [Fact(DisplayName = "1G")]
    public void OneGreen()
    {
        var result = QuantizerCelebi.Quantize([Green], MaxColors);
        var colors = result.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(Green);
    }

    [Fact(DisplayName = "1B")]
    public void OneBlue()
    {
        var result = QuantizerCelebi.Quantize([Blue], MaxColors);
        var colors = result.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(Blue);
    }

    [Fact(DisplayName = "5B")]
    public void FiveBlue()
    {
        var pixels = new[] { Blue, Blue, Blue, Blue, Blue };
        var result = QuantizerCelebi.Quantize(pixels, MaxColors);
        var colors = result.Keys.ToList();

        colors.Count.ShouldBe(1);
        colors[0].ShouldBe(Blue);
        result[Blue].ShouldBe(5);
    }

    [Fact(DisplayName = "1R 1G 1B")]
    public void OneRedOneGreenOneBlue()
    {
        var result = QuantizerCelebi.Quantize([Red, Green, Blue], MaxColors);

        // Content (set) must contain exactly the three colors
        var set = result.Keys.ToHashSet();
        set.Count.ShouldBe(3);
        set.ShouldContain(Blue);
        set.ShouldContain(Red);
        set.ShouldContain(Green);

        // Each should have count 1
        result[Blue].ShouldBe(1);
        result[Red].ShouldBe(1);
        result[Green].ShouldBe(1);

        // If you want to mimic the Dart list-order assertion, sort by population desc, then by ARGB for ties:
        var ordered = result.OrderByDescending(kv => kv.Value).ThenBy(kv => kv.Key).Select(kv => kv.Key).ToList();
        ordered.ToHashSet().SetEquals([Blue, Red, Green]).ShouldBeTrue();
    }

    [Fact(DisplayName = "2R 3G")]
    public void TwoRedThreeGreen()
    {
        var pixels = new[] { Red, Red, Green, Green, Green };
        var result = QuantizerCelebi.Quantize(pixels, MaxColors);

        // Exactly two colors present
        var set = result.Keys.ToHashSet();
        set.Count.ShouldBe(2);
        set.ShouldContain(Green);
        set.ShouldContain(Red);

        // Counts must reflect populations
        result[Green].ShouldBe(3);
        result[Red].ShouldBe(2);

        // Verify population ordering (green first)
        var ordered = result.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).ToList();
        ordered[0].ShouldBe(Green);
        ordered[1].ShouldBe(Red);
    }
}
