using System.Numerics;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Algorithms.Hungarian;
using ChoreoApp.Floor.Messages;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.States;
using MaterialDesignThemes.Maui;
using MessagePipe;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Choreography = ChoreoApp.Models.ChoreographyModel;
using Dancer = ChoreoApp.Models.DancerModel;
using Position = ChoreoApp.Models.PositionModel;
using Scene = ChoreoApp.Models.SceneModel;
using Microsoft.Extensions.Logging;
using ChoreoApp.Logging;

namespace ChoreoApp.Floor.Behaviors;

public sealed class PlacePositionBehavior(
    Global.GlobalStateModel globalState,
    ApplicationStateMachine stateMachine,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher,
    ILogger<FloorCanvasViewModel> logger)
    : IBehavior<FloorCanvasViewModel>
{
    private const float PointerMoveThreshold = 6f;

    private readonly Dictionary<long, SKPoint> _touchStartPositions = new();
    private readonly HashSet<long> _touchMoved = new();

    private bool _multiTouchActive;
    private Point? _pointerPressedPosition;
    private bool _pointerMoved;

    public void Activate(FloorCanvasViewModel viewModel, CompositeDisposable disposables)
    {
        BehaviorLog.BehaviorActivated(logger, nameof(PlacePositionBehavior), nameof(FloorCanvasViewModel));
        viewModel.PointerPressedCommand
            .Subscribe(command => HandlePointerPressed(command))
            .DisposeWith(disposables);

        viewModel.PointerMovedCommand
            .Subscribe(command => HandlePointerMoved(command))
            .DisposeWith(disposables);

        viewModel.PointerReleasedCommand
            .Subscribe(command => HandlePointerReleased(viewModel, command))
            .DisposeWith(disposables);

        viewModel.TouchCommand
            .Subscribe(command => HandleTouch(viewModel, command))
            .DisposeWith(disposables);
    }

    private void HandlePointerPressed(PointerPressedCommand command)
    {
        var position = GetPointerPosition(command);
        if (position is null || command.EventArgs.Button != ButtonsMask.Primary)
        {
            _pointerPressedPosition = null;
            _pointerMoved = false;
            return;
        }

        _pointerPressedPosition = position.Value;
        _pointerMoved = false;
    }

    private void HandlePointerMoved(PointerMovedCommand command)
    {
        if (_pointerPressedPosition is null)
        {
            return;
        }

        var position = GetPointerPosition(command);
        if (position is null)
        {
            return;
        }

        var deltaX = position.Value.X - _pointerPressedPosition.Value.X;
        var deltaY = position.Value.Y - _pointerPressedPosition.Value.Y;
        var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
        if (distance > PointerMoveThreshold)
        {
            _pointerMoved = true;
        }
    }

    private void HandlePointerReleased(FloorCanvasViewModel viewModel, PointerReleasedCommand command)
    {
        if (_pointerPressedPosition is null)
        {
            return;
        }

        if (viewModel.CanvasView is not { } canvasView)
        {
            _pointerPressedPosition = null;
            _pointerMoved = false;
            return;
        }

        var position = GetPointerPosition(command, canvasView);
        var shouldPlace = !_pointerMoved
            && position is not null;

        _pointerPressedPosition = null;
        _pointerMoved = false;

        if (shouldPlace)
        {
            TryPlacePosition(viewModel, position!.Value);
        }
    }

    private void HandleTouch(FloorCanvasViewModel viewModel, TouchCommand command)
    {
        var args = command.EventArgs;
        if (args.DeviceType != SKTouchDeviceType.Touch)
        {
            return;
        }

        switch (args.ActionType)
        {
            case SKTouchAction.Pressed:
                _touchStartPositions[args.Id] = args.Location;
                _touchMoved.Remove(args.Id);
                break;

            case SKTouchAction.Moved:
                MarkTouchMovedIfNeeded(args.Id, args.Location);
                break;

            case SKTouchAction.Released:
                TryHandleTouchRelease(viewModel, command.CanvasView, args.Id, args.Location);
                _touchStartPositions.Remove(args.Id);
                _touchMoved.Remove(args.Id);
                break;

            case SKTouchAction.Cancelled:
                _touchStartPositions.Remove(args.Id);
                _touchMoved.Remove(args.Id);
                if (_touchStartPositions.Count == 0)
                {
                    _multiTouchActive = false;
                }
                break;
        }

        if (_touchStartPositions.Count >= 2)
        {
            _multiTouchActive = true;
        }

        if (_touchStartPositions.Count == 0)
        {
            _multiTouchActive = false;
        }
    }

    private void MarkTouchMovedIfNeeded(long touchId, SKPoint location)
    {
        if (!_touchStartPositions.TryGetValue(touchId, out var start))
        {
            return;
        }

        var deltaX = location.X - start.X;
        var deltaY = location.Y - start.Y;
        var distance = MathF.Sqrt(deltaX * deltaX + deltaY * deltaY);
        if (distance > PointerMoveThreshold)
        {
            _touchMoved.Add(touchId);
        }
    }

    private void TryHandleTouchRelease(FloorCanvasViewModel viewModel, ISKCanvasView canvasView, long touchId, SKPoint location)
    {
        if (_multiTouchActive || _touchMoved.Contains(touchId))
        {
            return;
        }

        var (scaleX, scaleY) = GetCanvasScale(canvasView);
        var viewPoint = new Point(location.X / scaleX, location.Y / scaleY);
        TryPlacePosition(viewModel, viewPoint);
    }

    private void TryPlacePosition(FloorCanvasViewModel viewModel, Point viewPoint)
    {
        if (stateMachine.State is not PlacePositionsState)
        {
            return;
        }

        if (globalState.Choreography is not { } choreography)
        {
            return;
        }

        if (globalState.SelectedScene is not { } selectedScene)
        {
            return;
        }

        if (!globalState.IsPlaceMode)
        {
            return;
        }

        if (selectedScene.Positions.Count >= choreography.Dancers.Count)
        {
            return;
        }

        if (!TryGetFloorPosition(viewModel, choreography, viewPoint, out var positionX, out var positionY))
        {
            return;
        }

        SnapToGrid(choreography, ref positionX, ref positionY);
        var position = new Position
        {
            X = positionX,
            Y = positionY
        };

        selectedScene.Positions.Add(position);
        AddPositionToChoreographyScene(choreography, selectedScene, position);
        AssignDancersIfReady(choreography, selectedScene);
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private static void AssignDancersIfReady(Choreography choreography, Scenes.SceneViewModel selectedScene)
    {
        var dancers = choreography.Dancers;
        if (dancers.Count == 0 || selectedScene.Positions.Count != dancers.Count)
        {
            return;
        }

        var currentScene = FindScene(choreography, selectedScene);
        if (currentScene is null)
        {
            return;
        }

        var currentPositions = selectedScene.Positions.ToList();
        var currentPoints = currentPositions
            .Select(position => new Vector2((float)position.X, (float)position.Y))
            .ToList();

        var (previousScene, nextScene) = GetAdjacentScenes(choreography, currentScene);

        if (previousScene is not null && nextScene is not null
            && TryBuildDancerOrderedPoints(previousScene, dancers, out var previousPoints)
            && TryBuildDancerOrderedPoints(nextScene, dancers, out var nextPoints))
        {
            var assignment = ThreeSceneTransitionPlanner.ComputeMidSceneAssignment(previousPoints, currentPoints, nextPoints);
            ApplyAssignment(dancers, currentPositions, assignment);
            return;
        }

        if (previousScene is not null
            && TryBuildDancerOrderedPoints(previousScene, dancers, out var previousOnlyPoints))
        {
            var assignment = TransitionPlanner.ComputeAssignment(previousOnlyPoints, currentPoints);
            ApplyAssignment(dancers, currentPositions, assignment);
            return;
        }

        if (nextScene is not null
            && TryBuildDancerOrderedPoints(nextScene, dancers, out var nextOnlyPoints))
        {
            var assignment = TransitionPlanner.ComputeAssignment(nextOnlyPoints, currentPoints);
            ApplyAssignment(dancers, currentPositions, assignment);
            return;
        }

        AssignInOrder(dancers, currentPositions);
    }

    private static void ApplyAssignment(
        IReadOnlyList<Dancer> dancers,
        IReadOnlyList<Position> currentPositions,
        IReadOnlyList<int> assignment)
    {
        if (assignment.Count != dancers.Count || currentPositions.Count != dancers.Count)
        {
            return;
        }

        for (int dancerIndex = 0; dancerIndex < dancers.Count; dancerIndex++)
        {
            var targetIndex = assignment[dancerIndex];
            if (targetIndex < 0 || targetIndex >= currentPositions.Count)
            {
                continue;
            }

            currentPositions[targetIndex].Dancer = dancers[dancerIndex];
        }
    }

    private static void AssignInOrder(IReadOnlyList<Dancer> dancers, IReadOnlyList<Position> currentPositions)
    {
        for (int i = 0; i < dancers.Count && i < currentPositions.Count; i++)
        {
            currentPositions[i].Dancer = dancers[i];
        }
    }

    private static bool TryBuildDancerOrderedPoints(
        Scene scene,
        IReadOnlyList<Dancer> dancers,
        out List<Vector2> points)
    {
        points = new List<Vector2>(dancers.Count);
        if (scene.Positions.Count != dancers.Count)
        {
            return false;
        }

        var byId = new Dictionary<int, Position>();
        var byName = new Dictionary<string, Position>(StringComparer.Ordinal);
        foreach (var position in scene.Positions)
        {
            if (position.Dancer is null)
            {
                continue;
            }

            if (position.Dancer.DancerId.Value > 0)
            {
                byId[position.Dancer.DancerId.Value] = position;
            }
            else if (!string.IsNullOrWhiteSpace(position.Dancer.Name))
            {
                byName[position.Dancer.Name] = position;
            }
        }

        foreach (var dancer in dancers)
        {
            Position? position = null;
            if (dancer.DancerId.Value > 0)
            {
                byId.TryGetValue(dancer.DancerId.Value, out position);
            }
            else if (!string.IsNullOrWhiteSpace(dancer.Name))
            {
                byName.TryGetValue(dancer.Name, out position);
            }

            position ??= scene.Positions.FirstOrDefault(candidate => ReferenceEquals(candidate.Dancer, dancer));
            if (position is null)
            {
                return false;
            }

            points.Add(new Vector2((float)position.X, (float)position.Y));
        }

        return true;
    }

    private static (Scene? Previous, Scene? Next) GetAdjacentScenes(Choreography choreography, Scene currentScene)
    {
        var scenes = choreography.Scenes;
        var index = scenes.IndexOf(currentScene);
        if (index < 0)
        {
            return (null, null);
        }

        var previous = index > 0 ? scenes[index - 1] : null;
        var next = index + 1 < scenes.Count ? scenes[index + 1] : null;
        return (previous, next);
    }

    private static Scene? FindScene(Choreography choreography, Scenes.SceneViewModel selectedScene)
    {
        return choreography.Scenes.FirstOrDefault(scene => scene.SceneId == selectedScene.SceneId)
            ?? choreography.Scenes.FirstOrDefault(scene => string.Equals(scene.Name, selectedScene.Name, StringComparison.Ordinal));
    }

    private static void AddPositionToChoreographyScene(
        Choreography choreography,
        Scenes.SceneViewModel selectedScene,
        Position position)
    {
        var scene = choreography.Scenes.FirstOrDefault(s => s.SceneId == selectedScene.SceneId)
            ?? choreography.Scenes.FirstOrDefault(s => string.Equals(s.Name, selectedScene.Name, StringComparison.Ordinal));

        if (scene is null)
        {
            return;
        }

        scene.Positions.Add(position);
    }

    private static void SnapToGrid(Choreography choreography, ref double positionX, ref double positionY)
    {
        if (!choreography.Settings.SnapToGrid)
        {
            return;
        }

        var resolution = choreography.Settings.Resolution;
        if (resolution <= 0)
        {
            return;
        }

        var step = 1d / resolution;
        positionX = Math.Round(positionX / step) * step;
        positionY = Math.Round(positionY / step) * step;
    }

    private static bool TryGetFloorPosition(
        FloorCanvasViewModel viewModel,
        Choreography choreography,
        Point viewPoint,
        out double positionX,
        out double positionY)
    {
        positionX = 0d;
        positionY = 0d;

        if (viewModel.CanvasView is not { } canvasView)
        {
            return false;
        }

        if (!viewModel.HasFloorBounds)
        {
            return false;
        }

        var (dpiScaleX, dpiScaleY) = GetCanvasScale(canvasView);
        var canvasPoint = new SKPoint((float)(viewPoint.X * dpiScaleX), (float)(viewPoint.Y * dpiScaleY));

        var inverse = viewModel.TransformationMatrix.Invert();
        var transformedPoint = inverse.MapPoint(canvasPoint);
        var floorBounds = viewModel.FloorBounds;
        if (!floorBounds.Contains(transformedPoint))
        {
            return false;
        }

        var floor = choreography.Floor;
        float width = floorBounds.Width;
        float height = floorBounds.Height;
        float floorWidth = (float)(floor.SizeLeft + floor.SizeRight);
        float floorHeight = (float)(floor.SizeFront + floor.SizeBack);

        if (floorWidth <= 0f || floorHeight <= 0f || width <= 0f || height <= 0f)
        {
            return false;
        }

        float scaleX = width / floorWidth;
        float scaleY = height / floorHeight;
        float scale = Math.Min(scaleX, scaleY);
        if (scale <= 0f || float.IsNaN(scale) || float.IsInfinity(scale))
        {
            return false;
        }

        float centerX = floorBounds.Left + width / 2f;
        float centerY = floorBounds.Top + height / 2f;

        positionX = (transformedPoint.X - centerX) / scale;
        positionY = (centerY - transformedPoint.Y) / scale;
        return true;
    }

    private static (float ScaleX, float ScaleY) GetCanvasScale(ISKCanvasView canvasView)
    {
        if (!canvasView.IsValid())
        {
            return (1f, 1f);
        }

        var width = canvasView.Width;
        var height = canvasView.Height;
        var scaleX = canvasView.CanvasSize.Width / (float)width;
        var scaleY = canvasView.CanvasSize.Height / (float)height;
        return (scaleX, scaleY);
    }

    private static Point? GetPointerPosition(PointerPressedCommand command)
    {
        return command.EventArgs.GetPosition(command.CanvasView as Element);
    }

    private static Point? GetPointerPosition(PointerMovedCommand command)
    {
        return command.EventArgs.GetPosition(command.CanvasView as Element);
    }

    private static Point? GetPointerPosition(PointerReleasedCommand command, ISKCanvasView canvasView)
    {
        return command.EventArgs.GetPosition(canvasView as Element);
    }
}
