using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Scenes;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.States;
using ChoreoApp.StateMachine.Triggers;
using MessagePipe;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using Position = ChoreoApp.Models.PositionModel;

namespace ChoreoApp.Floor.Behaviors;

public sealed class ScalePositionsBehavior(
    Global.GlobalStateModel globalState,
    ApplicationStateMachine stateMachine,
    IVibration vibration,
    IPublisher<RedrawFloorCommand> redrawFloorPublisher,
    ISubscriber<SelectedSceneChangedEvent> selectedSceneChangedSubscriber)
    : IBehavior<FloorCanvasViewModel>
{
    private const float PointerMoveThreshold = 6f;
    private static readonly TimeSpan DragVibrationDuration = TimeSpan.FromMilliseconds(20);

    private readonly Dictionary<long, Point> _touchStartPositions = new();
    private readonly HashSet<long> _touchMoved = new();

    private Point? _pointerPressedPosition;
    private bool _pointerMoved;
    private bool _selectionActive;
    private bool _scaleActive;
    private bool _clearSelectionOnRelease;

    private readonly Dictionary<Position, Point> _scaleStartPositions = new();
    private Point? _scaleCenter;
    private double? _scaleStartDistance;
    private Point? _lastScaleFloorPoint;

    public void Activate(FloorCanvasViewModel viewModel, CompositeDisposable disposables)
    {
        viewModel.PointerPressedCommand
            .Subscribe(command => HandlePointerPressed(viewModel, command))
            .DisposeWith(disposables);

        viewModel.PointerMovedCommand
            .Subscribe(command => HandlePointerMoved(viewModel, command))
            .DisposeWith(disposables);

        viewModel.PointerReleasedCommand
            .Subscribe(command => HandlePointerReleased(viewModel, command))
            .DisposeWith(disposables);

        viewModel.TouchCommand
            .Subscribe(command => HandleTouch(viewModel, command))
            .DisposeWith(disposables);

        selectedSceneChangedSubscriber
            .Subscribe(_ => ClearSelection())
            .DisposeWith(disposables);

        globalState
            .WhenAnyValue(state => state.InteractionMode)
            .Subscribe(mode =>
            {
                if (mode != Global.InteractionMode.Scale
                    && mode != Global.InteractionMode.Move
                    && mode != Global.InteractionMode.RotateAroundCenter
                    && mode != Global.InteractionMode.RotateAroundDancer)
                {
                    ClearSelection();
                }
            })
            .DisposeWith(disposables);
    }

    private void HandlePointerPressed(FloorCanvasViewModel viewModel, PointerPressedCommand command)
    {
        if (!IsScaleModeActive())
        {
            return;
        }

        var position = command.EventArgs.GetPosition(command.CanvasView);
        if (position is null || command.EventArgs.Button != ButtonsMask.Primary)
        {
            ResetPointerState();
            return;
        }

        _pointerPressedPosition = position.Value;
        _pointerMoved = false;

        if (!TryGetFloorPoint(viewModel, position.Value, out var floorPoint))
        {
            _clearSelectionOnRelease = true;
            return;
        }

        if (globalState.SelectedPositions.Count > 0 && !_selectionActive)
        {
            StartScale(floorPoint);
            return;
        }

        StartSelection(floorPoint);
    }

    private void HandlePointerMoved(FloorCanvasViewModel viewModel, PointerMovedCommand command)
    {
        if (!IsScaleModeActive())
        {
            return;
        }

        if (_pointerPressedPosition is null || command.EventArgs.Button != ButtonsMask.Primary)
        {
            return;
        }

        var position = command.EventArgs.GetPosition(command.CanvasView);
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

        if (!TryGetFloorPoint(viewModel, position.Value, out var floorPoint))
        {
            return;
        }

        if (_scaleActive && _pointerMoved)
        {
            UpdateScale(floorPoint);
            return;
        }

        if (_selectionActive)
        {
            UpdateSelection(floorPoint);
        }
    }

    private void HandlePointerReleased(FloorCanvasViewModel viewModel, PointerReleasedCommand command)
    {
        if (!IsScaleModeActive())
        {
            ResetPointerState();
            return;
        }

        if (_pointerPressedPosition is null)
        {
            return;
        }

        var position = command.EventArgs.GetPosition(viewModel.CanvasView);
        if (position is not null && TryGetFloorPoint(viewModel, position.Value, out var floorPoint))
        {
            if (_scaleActive)
            {
                if (_pointerMoved)
                {
                    CompleteScale();
                }
                else
                {
                    ClearSelection();
                }
            }
            else if (_selectionActive)
            {
                CompleteSelection();
            }
        }
        else
        {
            if (_clearSelectionOnRelease)
            {
                ClearSelection();
            }

            if (_selectionActive)
            {
                CompleteSelection();
            }
        }

        ResetPointerState();
    }

    private void HandleTouch(FloorCanvasViewModel viewModel, TouchCommand command)
    {
        if (!IsScaleModeActive())
        {
            return;
        }

        var args = command.EventArgs;
        if (args.DeviceType != SKTouchDeviceType.Touch)
        {
            return;
        }

        switch (args.ActionType)
        {
            case SKTouchAction.Pressed:
                _touchStartPositions[args.Id] = ToViewPoint(args.Location, command.CanvasView);
                _touchMoved.Remove(args.Id);
                HandleTouchPress(viewModel, args.Id);
                break;

            case SKTouchAction.Moved:
                HandleTouchMove(viewModel, args.Id, args.Location);
                break;

            case SKTouchAction.Released:
                HandleTouchRelease(viewModel, args.Id, args.Location);
                _touchStartPositions.Remove(args.Id);
                _touchMoved.Remove(args.Id);
                break;

            case SKTouchAction.Cancelled:
                _touchStartPositions.Remove(args.Id);
                _touchMoved.Remove(args.Id);
                ResetPointerState();
                break;
        }
    }

    private void HandleTouchPress(FloorCanvasViewModel viewModel, long touchId)
    {
        if (!_touchStartPositions.TryGetValue(touchId, out var viewPoint))
        {
            return;
        }

        _pointerPressedPosition = viewPoint;
        _pointerMoved = false;

        if (!TryGetFloorPoint(viewModel, viewPoint, out var floorPoint))
        {
            _clearSelectionOnRelease = true;
            return;
        }

        if (globalState.SelectedPositions.Count > 0 && !_selectionActive)
        {
            StartScale(floorPoint);
            return;
        }

        StartSelection(floorPoint);
    }

    private void HandleTouchMove(FloorCanvasViewModel viewModel, long touchId, SKPoint location)
    {
        if (!_touchStartPositions.TryGetValue(touchId, out var startViewPoint)
            || viewModel.CanvasView is not { } canvasView)
        {
            return;
        }

        var viewPoint = ToViewPoint(location, canvasView);
        var deltaX = viewPoint.X - startViewPoint.X;
        var deltaY = viewPoint.Y - startViewPoint.Y;
        var distance = MathF.Sqrt((float)(deltaX * deltaX + deltaY * deltaY));
        if (distance > PointerMoveThreshold)
        {
            _touchMoved.Add(touchId);
            _pointerMoved = true;
        }

        if (!TryGetFloorPoint(viewModel, viewPoint, out var floorPoint))
        {
            return;
        }

        if (_scaleActive && _pointerMoved)
        {
            UpdateScale(floorPoint);
            return;
        }

        if (_selectionActive)
        {
            UpdateSelection(floorPoint);
        }
    }

    private void HandleTouchRelease(FloorCanvasViewModel viewModel, long touchId, SKPoint location)
    {
        if (viewModel.CanvasView is not { } canvasView)
        {
            return;
        }

        var viewPoint = ToViewPoint(location, canvasView);
        if (!TryGetFloorPoint(viewModel, viewPoint, out var floorPoint))
        {
            if (_clearSelectionOnRelease)
            {
                ClearSelection();
            }

            CompleteSelection();
            ResetPointerState();
            return;
        }

        if (_scaleActive)
        {
            if (_pointerMoved)
            {
                CompleteScale();
            }
            else
            {
                ClearSelection();
            }

            ResetPointerState();
            return;
        }

        if (_selectionActive)
        {
            CompleteSelection();
        }

        ResetPointerState();
    }

    private void StartSelection(Point floorPoint)
    {
        _selectionActive = true;
        _scaleActive = false;
        _clearSelectionOnRelease = false;
        globalState.SelectedPositions.Clear();
        globalState.SelectionRectangle = new Global.SelectionRectangle(floorPoint, floorPoint);
        stateMachine.TryApply(new ScalePositionsSelectionStartedTrigger());
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private void UpdateSelection(Point floorPoint)
    {
        if (globalState.SelectionRectangle is not { } rectangle)
        {
            rectangle = new Global.SelectionRectangle(floorPoint, floorPoint);
        }

        rectangle = rectangle with { End = floorPoint };
        globalState.SelectionRectangle = rectangle;

        var positions = GetPositionsInRectangle(globalState.SelectedScene, rectangle);
        SyncSelection(positions);
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private void CompleteSelection()
    {
        if (!_pointerMoved)
        {
            globalState.SelectedPositions.Clear();
        }

        globalState.SelectionRectangle = null;
        _selectionActive = false;
        stateMachine.TryApply(new ScalePositionsSelectionCompletedTrigger());
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private void StartScale(Point floorPoint)
    {
        if (globalState.SelectedPositions.Count == 0)
        {
            return;
        }

        _scaleStartPositions.Clear();
        foreach (var selected in globalState.SelectedPositions)
        {
            _scaleStartPositions[selected] = new Point(selected.X, selected.Y);
        }

        _scaleCenter = CalculateCenter(globalState.SelectedPositions);
        _scaleStartDistance = CalculateDistance(_scaleCenter.Value, floorPoint);
        if (_scaleStartDistance <= 0d)
        {
            _scaleStartDistance = null;
            return;
        }

        _lastScaleFloorPoint = floorPoint;
        _scaleActive = true;
        _selectionActive = false;
        _clearSelectionOnRelease = false;
        globalState.SelectionRectangle = null;
        stateMachine.TryApply(new ScalePositionsDragStartedTrigger());
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
        vibration.Vibrate(DragVibrationDuration);
    }

    private void UpdateScale(Point floorPoint)
    {
        if (_scaleCenter is null || _scaleStartDistance is null)
        {
            return;
        }

        var currentDistance = CalculateDistance(_scaleCenter.Value, floorPoint);
        if (currentDistance <= 0d)
        {
            return;
        }

        var factor = currentDistance / _scaleStartDistance.Value;
        foreach (var (position, startPoint) in _scaleStartPositions)
        {
            var relativeX = startPoint.X - _scaleCenter.Value.X;
            var relativeY = startPoint.Y - _scaleCenter.Value.Y;
            position.X = _scaleCenter.Value.X + relativeX * factor;
            position.Y = _scaleCenter.Value.Y + relativeY * factor;
        }

        _lastScaleFloorPoint = floorPoint;
        SnapSelectedPositionsToGrid();
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private void CompleteScale()
    {
        var floorPoint = _lastScaleFloorPoint;
        if (floorPoint is null)
        {
            return;
        }

        UpdateScale(floorPoint.Value);
        SnapSelectedPositionsToGrid();
        _scaleActive = false;
        _scaleStartPositions.Clear();
        _scaleCenter = null;
        _scaleStartDistance = null;
        _lastScaleFloorPoint = null;
        stateMachine.TryApply(new ScalePositionsDragCompletedTrigger());
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
        vibration.Cancel();
    }

    private void ClearSelection()
    {
        var wasScaleActive = _scaleActive;

        globalState.SelectedPositions.Clear();
        globalState.SelectionRectangle = null;
        _selectionActive = false;
        _scaleActive = false;
        _scaleStartPositions.Clear();
        _scaleCenter = null;
        _scaleStartDistance = null;
        _lastScaleFloorPoint = null;
        if (wasScaleActive)
        {
            vibration.Cancel();
        }

        ResetPointerState();
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private void ResetPointerState()
    {
        _pointerPressedPosition = null;
        _pointerMoved = false;
        _clearSelectionOnRelease = false;
    }

    private bool IsScaleModeActive()
    {
        if (globalState.InteractionMode != Global.InteractionMode.Scale)
        {
            return false;
        }

        return stateMachine.State is ScalePositionsState;
    }

    private static Point CalculateCenter(IReadOnlyCollection<Position> positions)
    {
        double sumX = 0d;
        double sumY = 0d;
        foreach (var position in positions)
        {
            sumX += position.X;
            sumY += position.Y;
        }

        var count = positions.Count;
        if (count == 0)
        {
            return new Point(0d, 0d);
        }

        return new Point(sumX / count, sumY / count);
    }

    private static double CalculateDistance(Point center, Point point)
    {
        var dx = point.X - center.X;
        var dy = point.Y - center.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private IReadOnlyList<Position> GetPositionsInRectangle(SceneViewModel? scene, Global.SelectionRectangle rectangle)
    {
        if (scene is null || globalState.Choreography is not { } choreography)
        {
            return [];
        }

        var minX = Math.Min(rectangle.Start.X, rectangle.End.X);
        var maxX = Math.Max(rectangle.Start.X, rectangle.End.X);
        var minY = Math.Min(rectangle.Start.Y, rectangle.End.Y);
        var maxY = Math.Max(rectangle.Start.Y, rectangle.End.Y);

        var size = (double)choreography.Settings.DancerSize;
        var halfSize = size / 2d;

        var selected = new List<Position>();
        foreach (var candidate in scene.Positions)
        {
            var candidateMinX = candidate.X - halfSize;
            var candidateMaxX = candidate.X + halfSize;
            var candidateMinY = candidate.Y - halfSize;
            var candidateMaxY = candidate.Y + halfSize;

            var intersects = candidateMaxX >= minX
                && candidateMinX <= maxX
                && candidateMaxY >= minY
                && candidateMinY <= maxY;

            if (intersects)
            {
                selected.Add(candidate);
            }
        }

        return selected;
    }

    private void SyncSelection(IReadOnlyList<Position> selectedPositions)
    {
        var selectedSet = new HashSet<Position>(selectedPositions);

        for (int i = globalState.SelectedPositions.Count - 1; i >= 0; i--)
        {
            var existing = globalState.SelectedPositions[i];
            if (!selectedSet.Contains(existing))
            {
                globalState.SelectedPositions.RemoveAt(i);
            }
        }

        foreach (var candidate in selectedPositions)
        {
            if (!globalState.SelectedPositions.Contains(candidate))
            {
                globalState.SelectedPositions.Add(candidate);
            }
        }
    }

    private void SnapSelectedPositionsToGrid()
    {
        if (globalState.Choreography is not { } choreography)
        {
            return;
        }

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
        foreach (var position in globalState.SelectedPositions)
        {
            position.X = Math.Round(position.X / step) * step;
            position.Y = Math.Round(position.Y / step) * step;
        }
    }

    private bool TryGetFloorPoint(FloorCanvasViewModel viewModel, Point viewPoint, out Point floorPoint)
    {
        floorPoint = default;

        if (viewModel.CanvasView is not { } canvasView)
        {
            return false;
        }

        if (!viewModel.HasFloorBounds)
        {
            return false;
        }

        if (globalState.Choreography is not { } choreography)
        {
            return false;
        }

        var (scaleX, scaleY) = GetCanvasScale(canvasView);
        var canvasPoint = new SKPoint((float)(viewPoint.X * scaleX), (float)(viewPoint.Y * scaleY));

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

        float scale = Math.Min(width / floorWidth, height / floorHeight);
        if (scale <= 0f || float.IsNaN(scale) || float.IsInfinity(scale))
        {
            return false;
        }

        float centerX = floorBounds.Left + width / 2f;
        float centerY = floorBounds.Top + height / 2f;

        var positionX = (transformedPoint.X - centerX) / scale;
        var positionY = (centerY - transformedPoint.Y) / scale;
        floorPoint = new Point(positionX, positionY);
        return true;
    }

    private static (float ScaleX, float ScaleY) GetCanvasScale(SKCanvasView canvasView)
    {
        var width = canvasView.Width;
        var height = canvasView.Height;

        if (width <= 0 || height <= 0)
        {
            return (1f, 1f);
        }

        var scaleX = canvasView.CanvasSize.Width / (float)width;
        var scaleY = canvasView.CanvasSize.Height / (float)height;
        return (scaleX, scaleY);
    }

    private static Point ToViewPoint(SKPoint point, SKCanvasView canvasView)
    {
        var (scaleX, scaleY) = GetCanvasScale(canvasView);
        var x = point.X / scaleX;
        var y = point.Y / scaleY;
        return new Point(x, y);
    }
}
