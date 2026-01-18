using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using Shouldly;

namespace ChoreoApp.Components.Tests.Floor.Behaviors;

[FeatureDescription(
    @"In order to navigate the floor view
As a user
I want pointer drags to pan the view")]
public sealed class GestureHandlingBehaviorTests : FeatureFixture
{
    private GestureHandlingBehaviorTestContext? _context;

    [Scenario(DisplayName = "Should apply pan translation on pointer drag")]
    public void ApplyPanTranslationOnPointerDrag()
    {
        Runner.RunScenario(
            Given_a_gesture_handling_context,
            When_the_user_drags_the_pointer,
            Then_the_view_should_translate,
            Then_cleanup_resources);
    }

    private void Given_a_gesture_handling_context()
    {
        _context = GestureHandlingBehaviorTestContext.Create();
    }

    private void When_the_user_drags_the_pointer()
    {
        _context.ShouldNotBeNull();
        _context.SendPointerPressed(new Point(10, 10));
        _context.SendPointerMoved(new Point(30, 25));
    }

    private void Then_the_view_should_translate()
    {
        _context.ShouldNotBeNull();

        var translated = SpinWait.SpinUntil(
            () =>
            {
                var matrix = _context.ViewModel.TransformationMatrix;
                return Math.Abs(matrix.TransX - 20f) < 0.001f
                       && Math.Abs(matrix.TransY - 15f) < 0.001f;
            },
            TimeSpan.FromSeconds(1));

        translated.ShouldBeTrue();
    }

    private void Then_cleanup_resources()
    {
        _context?.Dispose();
        _context = null;
    }
}
