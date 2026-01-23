namespace MaterialColorUtilities.Tests;

public sealed class MathUtilsTests
{
    private static double ReferenceRotationDirection(double from, double to)
    {
        var a = to - from;
        var b = to - from + 360.0;
        var c = to - from - 360.0;
        var aAbs = Math.Abs(a);
        var bAbs = Math.Abs(b);
        var cAbs = Math.Abs(c);

        if (aAbs <= bAbs && aAbs <= cAbs)
        {
            return a >= 0.0 ? 1.0 : -1.0;
        }

        if (bAbs <= aAbs && bAbs <= cAbs)
        {
            return b >= 0.0 ? 1.0 : -1.0;
        }

        return c >= 0.0 ? 1.0 : -1.0;
    }

    [Fact(DisplayName = "rotationDirection behaves correctly")]
    public void RotationDirection_Behaves_Correctly()
    {
        for (double from = 0.0; from < 360.0; from += 15.0)
        {
            for (double to = 7.5; to < 360.0; to += 15.0)
            {
                var expected = ReferenceRotationDirection(from, to);
                var actual = MathUtils.RotationDirection(from, to);

                actual.ShouldBe(expected, $"should be {expected} from {from} to {to}");
                Math.Abs(actual).ShouldBe(1.0, $"should be either +1.0 or -1.0 from {from} to {to} (got {actual})");
            }
        }
    }
}
