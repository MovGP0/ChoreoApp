using ChoreoApp.Floor.Behaviors;
using ChoreoApp.Global;
using ChoreoApp.Models;
using ChoreoApp.StateMachine.States;
using ChoreoApp.StateMachine.Triggers;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.XUnit2;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace ChoreoApp.Components.Tests.Floor.Behaviors;

[FeatureDescription(
    @"In order to adjust dancer placements
As a user
I want dragging to move selected positions")]
public sealed class MovePositionsBehaviorTests : FeatureFixture
{
    private FloorBehaviorTestContext<MovePositionsBehavior>? _context;
    private PositionModel? _first;
    private PositionModel? _second;
    private PositionModel? _third;
    private Point _dragDelta;
    private Point _startFirst;
    private Point _startSecond;
    private Point _startThird;

    [Scenario(DisplayName = "Should move selected positions by drag delta")]
    public void MoveSelectedPositionsByDragDelta()
    {
        Runner.RunScenario(
            Given_a_move_positions_context,
            Given_a_choreography_with_positions_is_loaded,
            Given_move_mode_is_active,
            When_the_user_selects_positions_with_rectangle,
            When_the_user_drags_a_selected_position_by_delta,
            Then_selected_positions_should_move_by_delta,
            Then_cleanup_resources);
    }

    [Scenario(DisplayName = "Should move selected positions by drag delta with mouse")]
    public void MoveSelectedPositionsByDragDeltaWithMouse()
    {
        Runner.RunScenario(
            Given_a_move_positions_context,
            Given_a_choreography_with_positions_is_loaded,
            Given_move_mode_is_active,
            When_the_user_selects_positions_with_rectangle,
            When_the_user_drags_a_selected_position_by_delta_with_mouse,
            Then_selected_positions_should_move_by_delta,
            Then_cleanup_resources);
    }

    [Scenario(DisplayName = "Should clear selection when clicking outside")]
    public void ClearSelectionWhenClickingOutside()
    {
        Runner.RunScenario(
            Given_a_move_positions_context,
            Given_a_choreography_with_positions_is_loaded,
            Given_move_mode_is_active,
            When_the_user_selects_positions_with_rectangle,
            When_the_user_clicks_outside_of_positions,
            Then_the_selection_should_be_cleared,
            Then_cleanup_resources);
    }

    private void Given_a_move_positions_context()
    {
        _context = FloorBehaviorTestContext<MovePositionsBehavior>.Create(services =>
        {
            services.AddLogging(logger =>
            {
                logger.SetMinimumLevel(LogLevel.Debug);
                logger.AddXUnit(TestOutput);
            });
        });
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
        _startFirst = new Point(_first.X, _first.Y);
        _startSecond = new Point(_second.X, _second.Y);
        _startThird = new Point(_third.X, _third.Y);
    }

    private void Given_move_mode_is_active()
    {
        _context.ShouldNotBeNull();
        _context.GlobalState.InteractionMode = InteractionMode.Move;
        _context.StateMachine.TryApply(new MovePositionsStartedTrigger()).ShouldBeTrue();
    }

    private void When_the_user_selects_positions_with_rectangle()
    {
        _context.ShouldNotBeNull();
        _context.SelectByRectangle(new Point(-2, 2), new Point(2, 0));
    }

    private void When_the_user_drags_a_selected_position_by_delta()
    {
        _context.ShouldNotBeNull();
        _first.ShouldNotBeNull();
        _dragDelta = new Point(1.5, -1.0);
        _context.DragFromFloorToUsingTouch(
            _startFirst,
            new Point(_startFirst.X + _dragDelta.X, _startFirst.Y + _dragDelta.Y),
            state => state is MovePositionsDragState);
    }

    private void When_the_user_drags_a_selected_position_by_delta_with_mouse()
    {
        _context.ShouldNotBeNull();
        _first.ShouldNotBeNull();
        _dragDelta = new Point(1.5, -1.0);
        _context.DragFromFloorTo(
            _startFirst,
            new Point(_startFirst.X + _dragDelta.X, _startFirst.Y + _dragDelta.Y),
            state => state is MovePositionsDragState);
    }

    private void When_the_user_clicks_outside_of_positions()
    {
        _context.ShouldNotBeNull();
        _context.ClickFloorPoint(new Point(4, 4));
    }

    private void Then_selected_positions_should_move_by_delta()
    {
        _first.ShouldNotBeNull();
        _second.ShouldNotBeNull();
        _third.ShouldNotBeNull();

        var moved = SpinWait.SpinUntil(
            () =>
                Math.Abs(_first.X - (_startFirst.X + _dragDelta.X)) < 0.0001
                && Math.Abs(_first.Y - (_startFirst.Y + _dragDelta.Y)) < 0.0001
                && Math.Abs(_second.X - (_startSecond.X + _dragDelta.X)) < 0.0001
                && Math.Abs(_second.Y - (_startSecond.Y + _dragDelta.Y)) < 0.0001
                && Math.Abs(_third.X - _startThird.X) < 0.0001
                && Math.Abs(_third.Y - _startThird.Y) < 0.0001,
            TimeSpan.FromSeconds(1));

        moved.ShouldBeTrue();
    }

    private void Then_the_selection_should_be_cleared()
    {
        _context.ShouldNotBeNull();
        var cleared = SpinWait.SpinUntil(
            () => _context.GlobalState.SelectedPositions.Count == 0
                  && _context.GlobalState.SelectionRectangle is null,
            TimeSpan.FromSeconds(1));

        cleared.ShouldBeTrue();
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
