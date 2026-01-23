namespace MaterialColorUtilities.Tests;

public sealed class ContrastTests
{
    [Fact(DisplayName = "ratioOfTones_outOfBoundsInput")]
    public void RatioOfTones_OutOfBounds_ReturnsMax()
    {
        var actual = Contrast.RatioOfTones(-10.0, 110.0);
        actual.ShouldBe(21.0, 0.001);
    }

    [Fact(DisplayName = "lighter_impossibleRatioErrors")]
    public void Lighter_ImpossibleRatio_ReturnsError()
    {
        var actual = Contrast.Lighter(tone: 90.0, ratio: 10.0);
        actual.ShouldBe(-1.0, 0.001);
    }

    [Fact(DisplayName = "lighter_outOfBoundsInputAboveErrors")]
    public void Lighter_ToneAboveBounds_ReturnsError()
    {
        var actual = Contrast.Lighter(tone: 110.0, ratio: 2.0);
        actual.ShouldBe(-1.0, 0.001);
    }

    [Fact(DisplayName = "lighter_outOfBoundsInputBelowErrors")]
    public void Lighter_ToneBelowBounds_ReturnsError()
    {
        var actual = Contrast.Lighter(tone: -10.0, ratio: 2.0);
        actual.ShouldBe(-1.0, 0.001);
    }

    [Fact(DisplayName = "lighterUnsafe_returnsMaxTone")]
    public void LighterUnsafe_ReturnsMaxToneOnFailure()
    {
        var actual = Contrast.LighterUnsafe(tone: 100.0, ratio: 2.0);
        actual.ShouldBe(100.0, 0.001);
    }

    [Fact(DisplayName = "darker_impossibleRatioErrors")]
    public void Darker_ImpossibleRatio_ReturnsError()
    {
        var actual = Contrast.Darker(tone: 10.0, ratio: 20.0);
        actual.ShouldBe(-1.0, 0.001);
    }

    [Fact(DisplayName = "darker_outOfBoundsInputAboveErrors")]
    public void Darker_ToneAboveBounds_ReturnsError()
    {
        var actual = Contrast.Darker(tone: 110.0, ratio: 2.0);
        actual.ShouldBe(-1.0, 0.001);
    }

    [Fact(DisplayName = "darker_outOfBoundsInputBelowErrors")]
    public void Darker_ToneBelowBounds_ReturnsError()
    {
        var actual = Contrast.Darker(tone: -10.0, ratio: 2.0);
        actual.ShouldBe(-1.0, 0.001);
    }

    [Fact(DisplayName = "darkerUnsafe_returnsMinTone")]
    public void DarkerUnsafe_ReturnsMinToneOnFailure()
    {
        var actual = Contrast.DarkerUnsafe(tone: 0.0, ratio: 2.0);
        actual.ShouldBe(0.0, 0.001);
    }
}
