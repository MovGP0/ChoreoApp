using ChoreoApp.Floor.Behaviors;
using ChoreoApp.Global;
using ChoreoApp.Models;
using ChoreoApp.StateMachine.Triggers;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using Shouldly;

namespace ChoreoApp.Components.Tests.Floor.Behaviors;

[FeatureDescription(
    @"In order to resize formations
As a user
I want to scale selected positions around their center")]
public sealed class ScalePositionsBehaviorTests : FeatureFixture
{
    private FloorBehaviorTestContext<ScalePositionsBehavior>? _context;
    private PositionModel? _first;
    private PositionModel? _second;
    private PositionModel? _third;

    [Scenario(DisplayName = "Should scale selected positions")]
    public void ScaleSelectedPositions()
    {
        Runner.RunScenario(
            Given_a_scale_positions_context,
            Given_a_choreography_with_positions_is_loaded,
            Given_scale_mode_is_active,
            When_the_user_selects_positions_with_rectangle,
            When_the_user_scales_the_selection,
            Then_selected_positions_should_scale,
            Then_cleanup_resources);
    }

    private void Given_a_scale_positions_context()
    {
        _context = FloorBehaviorTestContext<ScalePositionsBehavior>.Create();
    }

    private void Given_a_choreography_with_positions_is_loaded()
    {
        _context.ShouldNotBeNull();
        var choreography = BuildChoreography(out var scene);
        var sceneViewModel = _context.CreateSceneViewModel(scene);
        _context.LoadChoreography(choreography, sceneViewModel);

        _first = sceneViewModel.Positions[0];
        _second = sceneViewModel.Positions[1];
        _third = sceneViewModel.Positions[2];
    }

    private void Given_scale_mode_is_active()
    {
        _context.ShouldNotBeNull();
        _context.GlobalState.InteractionMode = InteractionMode.Scale;
        _context.StateMachine.TryApply(new ScalePositionsStartedTrigger()).ShouldBeTrue();
    }

    private void When_the_user_selects_positions_with_rectangle()
    {
        _context.ShouldNotBeNull();
        _context.SelectByRectangle(new Point(-2, 2), new Point(2, 0));
    }

    private void When_the_user_scales_the_selection()
    {
        _context.ShouldNotBeNull();
        _context.DragFromFloorTo(new Point(2, 1), new Point(4, 1));
    }

    private void Then_selected_positions_should_scale()
    {
        _first.ShouldNotBeNull();
        _second.ShouldNotBeNull();
        _third.ShouldNotBeNull();

        _first.X.ShouldBe(-2d, 0.0001);
        _first.Y.ShouldBe(1d, 0.0001);
        _second.X.ShouldBe(2d, 0.0001);
        _second.Y.ShouldBe(1d, 0.0001);
        _third.X.ShouldBe(3d, 0.0001);
        _third.Y.ShouldBe(-2d, 0.0001);
    }

    private void Then_cleanup_resources()
    {
        _context?.Dispose();
        _context = null;
    }

    private static ChoreographyModel BuildChoreography(out SceneModel scene)
    {
        var roleLead = new RoleModel
        {
            Name = "Lead",
            Color = Colors.Red
        };

        var roleFollow = new RoleModel
        {
            Name = "Follow",
            Color = Colors.Blue
        };

        var dancerA = new DancerModel
        {
            DancerId = 1,
            Name = "Alice",
            Shortcut = "A",
            Role = roleLead,
            Color = Colors.Red
        };

        var dancerB = new DancerModel
        {
            DancerId = 2,
            Name = "Bob",
            Shortcut = "B",
            Role = roleFollow,
            Color = Colors.Blue
        };

        var dancerC = new DancerModel
        {
            DancerId = 3,
            Name = "Cory",
            Shortcut = "C",
            Role = roleLead,
            Color = Colors.Green
        };

        scene = new SceneModel
        {
            SceneId = 1,
            Name = "Scene 1",
            FixedPositions = true
        };

        scene.Positions.Add(new PositionModel
        {
            Dancer = dancerA,
            X = -1,
            Y = 1
        });

        scene.Positions.Add(new PositionModel
        {
            Dancer = dancerB,
            X = 1,
            Y = 1
        });

        scene.Positions.Add(new PositionModel
        {
            Dancer = dancerC,
            X = 3,
            Y = -2
        });

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

        choreography.Roles.Add(roleLead);
        choreography.Roles.Add(roleFollow);
        choreography.Dancers.Add(dancerA);
        choreography.Dancers.Add(dancerB);
        choreography.Dancers.Add(dancerC);
        choreography.Scenes.Add(scene);
        return choreography;
    }
}
