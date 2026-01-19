using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using Microsoft.Extensions.Logging;
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

    [Scenario(DisplayName = "Should zoom in with mouse wheel")]
    public void ZoomInWithMouseWheel()
    {
        Runner.RunScenario(
            Given_a_gesture_handling_context,
            When_the_user_zooms_in_with_mouse_wheel,
            Then_the_view_should_zoom_in,
            Then_cleanup_resources);
    }

    [Scenario(DisplayName = "Should zoom out with mouse wheel")]
    public void ZoomOutWithMouseWheel()
    {
        Runner.RunScenario(
            Given_a_gesture_handling_context,
            When_the_user_zooms_out_with_mouse_wheel,
            Then_the_view_should_zoom_out,
            Then_cleanup_resources);
    }

    [Scenario(DisplayName = "Should zoom with two finger pinch")]
    public void ZoomWithTwoFingerPinch()
    {
        Runner.RunScenario(
            Given_a_gesture_handling_context,
            When_the_user_zooms_with_two_fingers,
            Then_the_view_should_zoom_in,
            Then_cleanup_resources);
    }

    private void Given_a_gesture_handling_context()
    {
        _context = GestureHandlingBehaviorTestContext.Create(services =>
        {
            services.AddLogging(logger =>
            {
                logger.SetMinimumLevel(LogLevel.Debug);
                logger.AddXUnit(TestOutput);
            });
        });
    }

    private void When_the_user_drags_the_pointer()
    {
        _context.ShouldNotBeNull();
        _context.SendPointerPressed(new Point(10, 10));
        _context.SendPointerMoved(new Point(30, 25));
    }

    private void When_the_user_zooms_in_with_mouse_wheel()
    {
        _context.ShouldNotBeNull();
        _context.SendPointerWheelChanged(120, new Point(50, 50));
    }

    private void When_the_user_zooms_out_with_mouse_wheel()
    {
        _context.ShouldNotBeNull();
        _context.SendPointerWheelChanged(-120, new Point(50, 50));
    }

    private void When_the_user_zooms_with_two_fingers()
    {
        _context.ShouldNotBeNull();
        _context.SendTouchPressed(1, new Point(40, 50));
        _context.SendTouchPressed(2, new Point(60, 50));
        _context.SendTouchMoved(1, new Point(30, 50));
        _context.SendTouchMoved(2, new Point(70, 50));
        _context.SendTouchReleased(1, new Point(30, 50));
        _context.SendTouchReleased(2, new Point(70, 50));
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

    private void Then_the_view_should_zoom_in()
    {
        _context.ShouldNotBeNull();
        _context.WaitForScaleChange(scale => scale > 1f);
    }

    private void Then_the_view_should_zoom_out()
    {
        _context.ShouldNotBeNull();
        _context.WaitForScaleChange(scale => scale < 1f);
    }

    private void Then_cleanup_resources()
    {
        _context?.Dispose();
        _context = null;
    }
}
