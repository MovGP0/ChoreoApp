namespace MaterialColorUtilities.Tests;

public class DislikeAnalyzerTests
{
    [Fact(DisplayName = "Monk Skin Tone Scale colors liked")]
    public void MonkSkinToneScale_Colors_NotDisliked()
    {
        // From https://skintone.google#/get-started (ported)
        var monkSkinToneScaleColors = new[]
        {
            unchecked((int)0xFFF6EDE4),
            unchecked((int)0xFFF3E7DB),
            unchecked((int)0xFFF7EAD0),
            unchecked((int)0xFFEADABA),
            unchecked((int)0xFFD7BD96),
            unchecked((int)0xFFA07E56),
            unchecked((int)0xFF825C43),
            unchecked((int)0xFF604134),
            unchecked((int)0xFF3A312A),
            unchecked((int)0xFF292420),
        };

        foreach (var color in monkSkinToneScaleColors)
        {
            var hct = Hct.FromInt(color);
            DislikeAnalyzer.IsDisliked(hct).ShouldBeFalse();
        }
    }

    [Fact(DisplayName = "bile colors disliked")]
    public void Bile_Colors_AreDisliked()
    {
        var unlikable = new[]
        {
            unchecked((int)0xFF95884B),
            unchecked((int)0xFF716B40),
            unchecked((int)0xFFB08E00),
            unchecked((int)0xFF4C4308),
            unchecked((int)0xFF464521),
        };

        foreach (var color in unlikable)
        {
            var hct = Hct.FromInt(color);
            DislikeAnalyzer.IsDisliked(hct).ShouldBeTrue();
        }
    }

    [Fact(DisplayName = "bile colors became likable after fix")]
    public void Bile_Colors_BecomeLikable_AfterFix()
    {
        var unlikable = new[]
        {
            unchecked((int)0xFF95884B),
            unchecked((int)0xFF716B40),
            unchecked((int)0xFFB08E00),
            unchecked((int)0xFF4C4308),
            unchecked((int)0xFF464521),
        };

        foreach (var color in unlikable)
        {
            var hct = Hct.FromInt(color);
            DislikeAnalyzer.IsDisliked(hct).ShouldBeTrue();

            var likable = DislikeAnalyzer.FixIfDisliked(hct);
            DislikeAnalyzer.IsDisliked(likable).ShouldBeFalse();
        }
    }

    [Fact(DisplayName = "tone 67 not disliked and remains unchanged by fix")]
    public void Tone67_NotDisliked_And_Unchanged()
    {
        var color = Hct.From(100.0, 50.0, 67.0);

        DislikeAnalyzer.IsDisliked(color).ShouldBeFalse();

        var fixedColor = DislikeAnalyzer.FixIfDisliked(color);
        // In the C# implementation, Hct exposes ARGB via `.Argb`
        fixedColor.Argb.ShouldBe(color.Argb);
    }
}
