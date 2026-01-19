using ChoreoApp.Floor.Behaviors;
using ChoreoApp.Models;
using ChoreoApp.Scenes;
using ChoreoApp.StateMachine.Triggers;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit.Abstractions;

namespace ChoreoApp.Components.Tests.Floor.Behaviors;

[FeatureDescription(
    @"In order to add dancers to a scene
As a user
I want a click to place a new position")]
public sealed class PlacePositionBehaviorTests(ITestOutputHelper testOutputHelper) : FeatureFixture
{
    private FloorBehaviorTestContext<PlacePositionBehavior>? _context;
    private SceneModel? _scene;
    private SceneViewModel? _sceneViewModel;

    [Scenario(DisplayName = "Should place a new position on click")]
    public void PlacePositionOnClick()
    {
        Runner.RunScenario(
            Given_a_place_position_context,
            Given_a_choreography_with_an_empty_scene,
            Given_place_mode_is_active,
            When_the_user_clicks_on_the_floor,
            Then_the_position_should_be_added,
            Then_cleanup_resources);
    }

    private void Given_a_place_position_context()
    {
        _context = FloorBehaviorTestContext<PlacePositionBehavior>.Create(services =>
        {
            services.AddLogging(logger =>
            {
                logger.SetMinimumLevel(LogLevel.Debug);
                logger.AddXUnit(testOutputHelper);
            });
        });
    }

    private void Given_a_choreography_with_an_empty_scene()
    {
        _context.ShouldNotBeNull();
        var choreography = BuildChoreography(out _scene);
        _sceneViewModel = _context.CreateSceneViewModel(_scene);
        _context.LoadChoreography(choreography, _sceneViewModel);
    }

    private void Given_place_mode_is_active()
    {
        _context.ShouldNotBeNull();
        _context.GlobalState.IsPlaceMode = true;
        _context.StateMachine.TryApply(new PlacePositionsStartedTrigger()).ShouldBeTrue();
    }

    private void When_the_user_clicks_on_the_floor()
    {
        _context.ShouldNotBeNull();
        _context.ClickFloorPoint(new Point(1, 1));
    }

    private void Then_the_position_should_be_added()
    {
        _scene.ShouldNotBeNull();
        _sceneViewModel.ShouldNotBeNull();

        _sceneViewModel.Positions.Count.ShouldBe(1);
        _scene.Positions.Count.ShouldBe(1);

        var position = _sceneViewModel.Positions[0];
        position.X.ShouldBe(1d, 0.0001);
        position.Y.ShouldBe(1d, 0.0001);
        _scene.Positions[0].ShouldBeSameAs(position);
    }

    private void Then_cleanup_resources()
    {
        _context?.Dispose();
        _context = null;
    }

    private static ChoreographyModel BuildChoreography(out SceneModel scene)
    {
        var dancer = new DancerModel
        {
            DancerId = 1,
            Name = "Alex",
            Shortcut = "A",
            Color = Colors.Red
        };

        scene = new SceneModel
        {
            SceneId = 1,
            Name = "Scene 1",
            FixedPositions = true
        };

        var choreography = new ChoreographyModel
        {
            Name = "Test",
            Floor = new FloorModel
            {
                SizeFront = 5,
                SizeBack = 5,
                SizeLeft = 5,
                SizeRight = 5
            },
            Settings = new SettingsModel
            {
                DancerSize = 1.0m,
                SnapToGrid = false,
                Resolution = 0
            }
        };

        choreography.Dancers.Add(dancer);
        choreography.Scenes.Add(scene);
        return choreography;
    }
}
