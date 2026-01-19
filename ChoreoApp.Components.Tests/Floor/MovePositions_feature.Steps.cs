using ChoreoApp.Models;
using ChoreoApp.StateMachine.States;
using LightBDD.XUnit2;
using Microsoft.Extensions.Logging;
using Shouldly;

namespace ChoreoApp.Components.Tests.Floor;

public partial class MovePositions_feature : FeatureFixture
{
    private TestContext? _context;
    private PositionModel? _first;
    private PositionModel? _second;
    private PositionModel? _third;
    private Point _dragDelta;
    private Point _startFirst;
    private Point _startSecond;
    private Point _startThird;

    private void Given_dependency_injection_is_configured()
    {
        _context = TestContext.Create(services =>
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
        _context.LoadChoreography(BuildChoreography());

        var scene = _context.GlobalState.SelectedScene.ShouldNotBeNull();
        _first = scene.Positions[0];
        _second = scene.Positions[1];
        _third = scene.Positions[2];
        _startFirst = new Point(_first.X, _first.Y);
        _startSecond = new Point(_second.X, _second.Y);
        _startThird = new Point(_third.X, _third.Y);
    }

    private void Given_move_mode_is_active()
    {
        _context.ShouldNotBeNull();
        _context.EnableMoveMode();
    }

    private void Then_move_state_should_be_active()
    {
        _context.ShouldNotBeNull();
        _context.StateMachine.State.ShouldBeOfType<MovePositionsState>();
    }

    private void When_the_user_selects_positions_with_rectangle()
    {
        _context.ShouldNotBeNull();
        _context.SelectByRectangle(new Point(-2, 2), new Point(2, 0));
    }

    private void When_the_user_selects_positions_with_mouse_rectangle()
    {
        _context.ShouldNotBeNull();
        _context.SelectByRectangle(new Point(-2, 2), new Point(2, 0));
    }

    private void Given_the_view_is_translated()
    {
        _context.ShouldNotBeNull();
        _context.TranslateView(10f, -12f);
    }

    private void When_the_user_drags_a_selected_position_by_delta()
    {
        _context.ShouldNotBeNull();
        _first.ShouldNotBeNull();
        _dragDelta = new Point(1.5, -1.0);
        _context.DragFromTo(_startFirst, new Point(_startFirst.X + _dragDelta.X, _startFirst.Y + _dragDelta.Y));
    }

    private void When_the_user_clicks_outside_of_positions()
    {
        _context.ShouldNotBeNull();
        _context.ClickInView(new Point(-10, -10));
    }

    private void When_the_user_drags_a_single_position_by_delta()
    {
        _context.ShouldNotBeNull();
        _first.ShouldNotBeNull();
        _dragDelta = new Point(-1.0, 2.0);
        _context.DragFromTo(_startFirst, new Point(_startFirst.X + _dragDelta.X, _startFirst.Y + _dragDelta.Y));
    }

    private void Then_all_selected_positions_should_move_by_delta()
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

    private void Then_only_that_position_should_move()
    {
        _first.ShouldNotBeNull();
        _second.ShouldNotBeNull();
        _third.ShouldNotBeNull();

        _first.X.ShouldBe(_startFirst.X + _dragDelta.X, 0.0001);
        _first.Y.ShouldBe(_startFirst.Y + _dragDelta.Y, 0.0001);
        _second.X.ShouldBe(_startSecond.X, 0.0001);
        _second.Y.ShouldBe(_startSecond.Y, 0.0001);
        _third.X.ShouldBe(_startThird.X, 0.0001);
        _third.Y.ShouldBe(_startThird.Y, 0.0001);

        _context.ShouldNotBeNull();
        _context.GlobalState.SelectedPositions.ShouldContain(_first);
        _context.GlobalState.SelectedPositions.Count.ShouldBe(1);
    }

    private void Then_the_expected_positions_should_be_selected()
    {
        _context.ShouldNotBeNull();
        _first.ShouldNotBeNull();
        _second.ShouldNotBeNull();
        _third.ShouldNotBeNull();

        var selected = _context.GlobalState.SelectedPositions;
        "result".ShouldSatisfyAllConditions(
            () => SpinWait.SpinUntil(() => selected.Count == 2, TimeSpan.FromSeconds(1)).ShouldBeTrue(),
            () => selected.Any(position =>
                Math.Abs(position.X - _startFirst.X) < 0.0001
                && Math.Abs(position.Y - _startFirst.Y) < 0.0001).ShouldBeTrue(),
            () => selected.Any(position =>
                Math.Abs(position.X - _startSecond.X) < 0.0001
                && Math.Abs(position.Y - _startSecond.Y) < 0.0001).ShouldBeTrue(),
            () => selected.Any(position =>
                Math.Abs(position.X - _startThird.X) < 0.0001
                && Math.Abs(position.Y - _startThird.Y) < 0.0001).ShouldBeFalse());
    }

    private void Then_cleanup_resources()
    {
        _context?.Dispose();
        _context = null;
    }

    private static ChoreographyModel BuildChoreography()
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

        var scene = new SceneModel
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
