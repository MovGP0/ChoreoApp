using ChoreoApp.Floor.Messages;
using ChoreoApp.Models;
using ChoreoApp.Scenes;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Shouldly;
using SkiaSharp.Views.Maui;

namespace ChoreoApp.Components.Tests.Floor.Behaviors;

[FeatureDescription(
    @"In order to keep the floor up to date
As a user
I want redraws to happen when data changes")]
public sealed class RedrawFloorBehaviorTests : FeatureFixture
{
    private RedrawFloorBehaviorTestContext? _context;

    [Scenario(DisplayName = "Should redraw when choreography changes")]
    public void RedrawWhenChoreographyChanges()
    {
        Runner.RunScenario(
            Given_a_redraw_context,
            When_the_choreography_changes,
            Then_the_canvas_should_redraw,
            Then_cleanup_resources);
    }

    [Scenario(DisplayName = "Should redraw when selected scene changes")]
    public void RedrawWhenSelectedSceneChanges()
    {
        Runner.RunScenario(
            Given_a_redraw_context,
            When_the_selected_scene_changes,
            Then_the_canvas_should_redraw,
            Then_cleanup_resources);
    }

    [Scenario(DisplayName = "Should redraw when redraw command is published")]
    public void RedrawWhenRedrawCommandPublished()
    {
        Runner.RunScenario(
            Given_a_redraw_context,
            When_a_redraw_command_is_published,
            Then_the_canvas_should_redraw,
            Then_cleanup_resources);
    }

    private void Given_a_redraw_context()
    {
        _context = RedrawFloorBehaviorTestContext.Create(services =>
        {
            services.AddLogging(logger =>
            {
                logger.SetMinimumLevel(LogLevel.Debug);
                logger.AddXUnit(TestOutput);
            });
        });
    }

    private void When_the_choreography_changes()
    {
        _context.ShouldNotBeNull();
        _context.GlobalState.Choreography = new ChoreographyModel
        {
            Name = "Updated"
        };
    }

    private void When_the_selected_scene_changes()
    {
        _context.ShouldNotBeNull();
        _context.SelectedSceneChangedPublisher.Publish(new SelectedSceneChangedEvent(null));
    }

    private void When_a_redraw_command_is_published()
    {
        _context.ShouldNotBeNull();
        _context.RedrawPublisher.Publish(new RedrawFloorCommand());
    }

    private void Then_the_canvas_should_redraw()
    {
        _context.ShouldNotBeNull();
        var redrawn = SpinWait.SpinUntil(
            () => CountInvalidateCalls(_context.CanvasView) > 0,
            TimeSpan.FromSeconds(1));

        redrawn.ShouldBeTrue();
    }

    private void Then_cleanup_resources()
    {
        _context?.Dispose();
        _context = null;
    }

    private static int CountInvalidateCalls(ISKCanvasView canvasView)
    {
        return canvasView.ReceivedCalls().Count(call => call.GetMethodInfo().Name == "InvalidateSurface");
    }
}
