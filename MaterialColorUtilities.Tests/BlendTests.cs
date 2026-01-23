namespace MaterialColorUtilities.Tests;

public sealed class BlendTests
{
    // Same ARGB constants as in the Dart tests (note the unchecked casts for >0x7FFFFFFF).
    private const int Red = unchecked((int)0xFFFF0000);
    private const int Blue = unchecked((int)0xFF0000FF);
    private const int Green = unchecked((int)0xFF00FF00);
    private const int Yellow = unchecked((int)0xFFFFFF00);

    [Theory(DisplayName = "Blend.Harmonize matches MaterialColorUtilities vectors")]
    [InlineData(Red, Blue, unchecked((int)0xFFFB0057))] // redToBlue
    [InlineData(Red, Green, unchecked((int)0xFFD85600))] // redToGreen
    [InlineData(Red, Yellow, unchecked((int)0xFFD85600))] // redToYellow
    [InlineData(Blue, Green, unchecked((int)0xFF0047A3))] // blueToGreen
    [InlineData(Blue, Red, unchecked((int)0xFF5700DC))] // blueToRed
    [InlineData(Blue, Yellow, unchecked((int)0xFF0047A3))] // blueToYellow
    [InlineData(Green, Blue, unchecked((int)0xFF00FC94))] // greenToBlue
    [InlineData(Green, Red, unchecked((int)0xFFB1F000))] // greenToRed
    [InlineData(Green, Yellow, unchecked((int)0xFFB1F000))] // greenToYellow
    [InlineData(Yellow,Blue, unchecked((int)0xFFEBFFBA))] // yellowToBlue
    [InlineData(Yellow,Green, unchecked((int)0xFFEBFFBA))] // yellowToGreen
    [InlineData(Yellow,Red, unchecked((int)0xFFFFF6E3))] // yellowToRed
    public void Harmonize_Matches_MaterialColorUtilities_Vectors(int designColor, int sourceColor, int expectedArgb)
    {
        // act
        var actual = Blend.Harmonize(designColor, sourceColor);

        actual.ShouldBe(expectedArgb);
    }

    [Theory(DisplayName = "Blend.HctHue blends hue correctly")]
    [InlineData(Red, Blue, 0.5, unchecked((int)0xffe700c9))]
    [InlineData(Green, Yellow, 1.0, unchecked((int)0xffe3e300))]
    public void HctHue_BlendsCorrectly(int from, int to, double amount, int expectedArgb)
    {
        var actual = Blend.HctHue(from, to, amount);
        actual.ShouldBe(expectedArgb);
    }
}
