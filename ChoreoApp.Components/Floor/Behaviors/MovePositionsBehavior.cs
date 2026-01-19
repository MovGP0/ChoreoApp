using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Scenes;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.States;
using ChoreoApp.StateMachine.Triggers;
using MaterialDesignThemes.Maui;
using MessagePipe;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using Position = ChoreoApp.Models.PositionModel;

namespace ChoreoApp.Floor.Behaviors;

public sealed class MovePositionsBehavior(
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
    private Point? _dragStartFloorPoint;
    private bool _pointerMoved;
    private bool _selectionActive;
    private bool _dragActive;
    private bool _clearSelectionOnRelease;

    private readonly Dictionary<Position, Point> _dragStartPositions = new();
    private Point? _lastDragFloorPoint;

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
                if (mode != Global.InteractionMode.Move
                    && mode != Global.InteractionMode.RotateAroundCenter
                    && mode != Global.InteractionMode.Scale
                    && mode != Global.InteractionMode.RotateAroundDancer)
                {
                    ClearSelection();
                }
            })
            .DisposeWith(disposables);
    }

    private void HandlePointerPressed(FloorCanvasViewModel viewModel, PointerPressedCommand command)
    {
        if (!IsMoveModeActive())
        {
            return;
        }

        var position = command.EventArgs.GetPosition(command.CanvasView as Element);
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

        if (TryGetPositionAtPoint(globalState.SelectedScene, floorPoint, out var hitPosition))
        {
            StartDrag(floorPoint, hitPosition);
            return;
        }

        StartSelection(floorPoint);
    }

    private void HandlePointerMoved(FloorCanvasViewModel viewModel, PointerMovedCommand command)
    {
        if (!IsMoveModeActive())
        {
            return;
        }

        if (_pointerPressedPosition is null || command.EventArgs.Button != ButtonsMask.Primary)
        {
            return;
        }

        var position = command.EventArgs.GetPosition(command.CanvasView as Element);
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

        if (_dragActive)
        {
            UpdateDrag(floorPoint);
            return;
        }

        if (_selectionActive)
        {
            UpdateSelection(floorPoint);
        }
    }

    private void HandlePointerReleased(FloorCanvasViewModel viewModel, PointerReleasedCommand command)
    {
        if (!IsMoveModeActive())
        {
            ResetPointerState();
            return;
        }

        if (_pointerPressedPosition is null)
        {
            return;
        }

        var position =  command.EventArgs.GetPosition(viewModel.CanvasView as Element);
        if (position is not null && TryGetFloorPoint(viewModel, position.Value, out var floorPoint))
        {
            if (_dragActive)
            {
                CompleteDrag();
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
        if (!IsMoveModeActive())
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

        if (TryGetPositionAtPoint(globalState.SelectedScene, floorPoint, out var hitPosition))
        {
            StartDrag(floorPoint, hitPosition);
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

        if (_dragActive)
        {
            UpdateDrag(floorPoint);
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

        if (_dragActive)
        {
            CompleteDrag();
            ResetPointerState();
            return;
        }

        if (_selectionActive)
        {
            CompleteSelection();
        }

        ResetPointerState();
    }

    private void StartDrag(Point floorPoint, Position hitPosition)
    {
        if (!globalState.SelectedPositions.Contains(hitPosition))
        {
            globalState.SelectedPositions.Clear();
            globalState.SelectedPositions.Add(hitPosition);
        }

        _dragStartPositions.Clear();
        var selectedPositions = globalState.SelectedPositions.ToArray();
        foreach (var selected in selectedPositions)
        {
            _dragStartPositions[selected] = new Point(selected.X, selected.Y);
        }

        _dragStartFloorPoint = floorPoint;
        _lastDragFloorPoint = floorPoint;
        _dragActive = true;
        _selectionActive = false;
        _clearSelectionOnRelease = false;
        globalState.SelectionRectangle = null;
        stateMachine.TryApply(new MovePositionsDragStartedTrigger());
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
        if (vibration.IsSupported)
        {
            vibration.Vibrate(DragVibrationDuration);
        }
    }

    private void UpdateDrag(Point floorPoint)
    {
        if (_dragStartFloorPoint is null)
        {
            return;
        }

        var deltaX = floorPoint.X - _dragStartFloorPoint.Value.X;
        var deltaY = floorPoint.Y - _dragStartFloorPoint.Value.Y;

        foreach (var (position, startPoint) in _dragStartPositions)
        {
            position.X = startPoint.X + deltaX;
            position.Y = startPoint.Y + deltaY;
        }

        _lastDragFloorPoint = floorPoint;
        SnapSelectedPositionsToGrid();
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private void CompleteDrag()
    {
        var floorPoint = _lastDragFloorPoint ?? _dragStartFloorPoint;
        if (floorPoint is null)
        {
            return;
        }

        UpdateDrag(floorPoint.Value);
        SnapSelectedPositionsToGrid();
        _dragActive = false;
        _dragStartPositions.Clear();
        _dragStartFloorPoint = null;
        _lastDragFloorPoint = null;
        stateMachine.TryApply(new MovePositionsDragCompletedTrigger());
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
        if (vibration.IsSupported)
        {
            vibration.Cancel();
        }
    }

    private void StartSelection(Point floorPoint)
    {
        _selectionActive = true;
        _dragActive = false;
        _clearSelectionOnRelease = false;
        globalState.SelectedPositions.Clear();
        globalState.SelectionRectangle = new Global.SelectionRectangle(floorPoint, floorPoint);
        stateMachine.TryApply(new MovePositionsSelectionStartedTrigger());
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
        stateMachine.TryApply(new MovePositionsSelectionCompletedTrigger());
        redrawFloorPublisher.Publish(new RedrawFloorCommand());
    }

    private void ClearSelection()
    {
        var wasDragActive = _dragActive;

        globalState.SelectedPositions.Clear();
        globalState.SelectionRectangle = null;
        _selectionActive = false;
        _dragActive = false;
        _dragStartPositions.Clear();
        _dragStartFloorPoint = null;
        _lastDragFloorPoint = null;
        if (wasDragActive && vibration.IsSupported)
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

    private bool IsMoveModeActive()
    {
        if (globalState.InteractionMode != Global.InteractionMode.Move)
        {
            return false;
        }

        return stateMachine.State is MovePositionsState
            || stateMachine.State is MovePositionsSelectionState
            || stateMachine.State is MovePositionsDragState;
    }

    private bool TryGetPositionAtPoint(SceneViewModel? scene, Point floorPoint, out Position position)
    {
        position = null!;

        if (scene is null || globalState.Choreography is not { } choreography)
        {
            return false;
        }

        var size = (double)choreography.Settings.DancerSize;
        var halfSize = size / 2d;

        foreach (var candidate in scene.Positions)
        {
            if (Math.Abs(candidate.X - floorPoint.X) <= halfSize
                && Math.Abs(candidate.Y - floorPoint.Y) <= halfSize)
            {
                position = candidate;
                return true;
            }
        }

        return false;
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

    private static Point ToViewPoint(SKPoint point, ISKCanvasView canvasView)
    {
        var (scaleX, scaleY) = GetCanvasScale(canvasView);
        var x = point.X / scaleX;
        var y = point.Y / scaleY;
        return new Point(x, y);
    }
}
