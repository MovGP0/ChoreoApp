using System.Reactive.Concurrency;
using System.Reactive.Linq;

using ChoreoApp.Floor;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Global;
using ChoreoApp.Scenes;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.States;
using MaterialDesignThemes.Maui;
using NSubstitute;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace ChoreoApp.Components.Tests.Floor.Behaviors;

internal sealed class FloorBehaviorTestContext<TBehavior> : IDisposable
    where TBehavior : class, IBehavior<FloorCanvasViewModel>
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDisposable _activation;
    private readonly SKRect _floorBounds;
    private long _touchIdCounter;

    private FloorBehaviorTestContext(
        ServiceProvider serviceProvider,
        GlobalStateModel globalState,
        ApplicationStateMachine stateMachine,
        FloorCanvasViewModel viewModel,
        TBehavior behavior,
        ISKCanvasView canvasView,
        IDisposable activation,
        SKRect floorBounds)
    {
        _serviceProvider = serviceProvider;
        GlobalState = globalState;
        StateMachine = stateMachine;
        ViewModel = viewModel;
        Behavior = behavior;
        CanvasView = canvasView;
        _activation = activation;
        _floorBounds = floorBounds;
    }

    public GlobalStateModel GlobalState { get; }
    public ApplicationStateMachine StateMachine { get; }
    public FloorCanvasViewModel ViewModel { get; }
    public TBehavior Behavior { get; }
    public ISKCanvasView CanvasView { get; }

    public static FloorBehaviorTestContext<TBehavior> Create(Action<ServiceCollection>? configureServices = null)
    {
        ReactiveUITestInitialization.Initialize();

        var services = new ServiceCollection();
        services.AddMessagePipe();

        var preferences = Substitute.For<IPreferences>();
        preferences.Get(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);

        var haptic = Substitute.For<IHapticFeedback>();
        haptic.IsSupported.Returns(false);

        var vibration = Substitute.For<IVibration>();
        vibration.IsSupported.Returns(false);

        services.AddSingleton(preferences);
        services.AddSingleton(haptic);
        services.AddSingleton(vibration);
        services.AddSingleton<Settings.SettingsViewModel>();
        services.AddSingleton<GlobalStateModel>();
        services.AddSingleton<IGlobalStateModel>(sp => sp.GetRequiredService<GlobalStateModel>());
        services.AddApplicationStateMachine();

        services.AddSingleton<TBehavior>();
        services.AddSingleton<IBehavior<FloorCanvasViewModel>>(sp => sp.GetRequiredService<TBehavior>());
        services.AddSingleton<FloorCanvasViewModel>();
        services.AddTransient<SceneViewModel>();

        configureServices?.Invoke(services);

        var serviceProvider = services.BuildServiceProvider();
        var globalState = serviceProvider.GetRequiredService<GlobalStateModel>();
        var stateMachine = serviceProvider.GetRequiredService<ApplicationStateMachine>();
        var viewModel = serviceProvider.GetRequiredService<FloorCanvasViewModel>();
        var behavior = serviceProvider.GetRequiredService<TBehavior>();

        var canvasView = Substitute.For<ISKCanvasView>();
        canvasView.Width.Returns(100d);
        canvasView.Height.Returns(100d);
        canvasView.CanvasSize.Returns(new SKSize(100, 100));

        viewModel.CanvasView = canvasView;
        var floorBounds = new SKRect(0, 0, 100, 100);
        viewModel.UpdateFloorBounds(floorBounds, new SKSize(100, 100));

        var activation = viewModel.Activator.Activate();

        return new FloorBehaviorTestContext<TBehavior>(
            serviceProvider,
            globalState,
            stateMachine,
            viewModel,
            behavior,
            canvasView,
            activation,
            floorBounds);
    }

    public SceneViewModel CreateSceneViewModel(Models.SceneModel scene)
    {
        var viewModel = _serviceProvider.GetRequiredService<SceneViewModel>();
        viewModel.SceneId = scene.SceneId;
        viewModel.Name = scene.Name;
        viewModel.FixedPositions = scene.FixedPositions;

        foreach (var position in scene.Positions)
        {
            viewModel.Positions.Add(position);
        }

        return viewModel;
    }

    public void LoadChoreography(Models.ChoreographyModel choreography, SceneViewModel selectedScene)
    {
        GlobalState.Choreography = choreography;
        GlobalState.SelectedScene = selectedScene;
        GlobalState.Scenes.Clear();
        GlobalState.Scenes.Add(selectedScene);
    }

    public void SelectByRectangle(Point startFloorPoint, Point endFloorPoint)
    {
        DragFromFloorTo(startFloorPoint, endFloorPoint);
        SpinWait.SpinUntil(
            () => GlobalState.SelectionRectangle is null
                  && GlobalState.SelectedPositions.Count > 0,
            TimeSpan.FromSeconds(1));
    }

    public void DragFromFloorTo(Point startFloorPoint, Point endFloorPoint)
    {
        var startView = ToViewPoint(startFloorPoint);
        var endView = ToViewPoint(endFloorPoint);
        SendPointer(startView, vm => vm.PointerPressedCommand, command => new PointerPressedCommand(CanvasView, command), isInContact: true);
        SpinWait.SpinUntil(() => false, TimeSpan.FromMilliseconds(10));
        SendPointer(endView, vm => vm.PointerMovedCommand, command => new PointerMovedCommand(CanvasView, command), isInContact: true);
        SpinWait.SpinUntil(() => false, TimeSpan.FromMilliseconds(10));
        SendPointer(endView, vm => vm.PointerReleasedCommand, command => new PointerReleasedCommand(command), isInContact: false);
    }

    public void DragFromFloorTo(Point startFloorPoint, Point endFloorPoint, Func<ApplicationState, bool> waitForState)
    {
        var startView = ToViewPoint(startFloorPoint);
        var endView = ToViewPoint(endFloorPoint);
        SendPointer(startView, vm => vm.PointerPressedCommand, command => new PointerPressedCommand(CanvasView, command), isInContact: true);
        SpinWait.SpinUntil(() => waitForState(StateMachine.State), TimeSpan.FromSeconds(1));
        SendPointer(endView, vm => vm.PointerMovedCommand, command => new PointerMovedCommand(CanvasView, command), isInContact: true);
        SpinWait.SpinUntil(() => false, TimeSpan.FromMilliseconds(10));
        SendPointer(endView, vm => vm.PointerReleasedCommand, command => new PointerReleasedCommand(command), isInContact: false);
    }

    public void DragFromFloorToUsingTouch(Point startFloorPoint, Point endFloorPoint)
    {
        DragFromFloorToUsingTouch(startFloorPoint, endFloorPoint, _ => true);
    }

    public void DragFromFloorToUsingTouch(Point startFloorPoint, Point endFloorPoint, Func<ApplicationState, bool> waitForState)
    {
        var startView = ToViewPoint(startFloorPoint);
        var endView = ToViewPoint(endFloorPoint);
        var touchId = ++_touchIdCounter;
        SendTouch(touchId, startView, SKTouchAction.Pressed, true);
        SpinWait.SpinUntil(() => waitForState(StateMachine.State), TimeSpan.FromSeconds(1));
        SendTouch(touchId, endView, SKTouchAction.Moved, true);
        SpinWait.SpinUntil(() => false, TimeSpan.FromMilliseconds(10));
        SendTouch(touchId, endView, SKTouchAction.Released, false);
    }

    public void ClickFloorPoint(Point floorPoint)
    {
        var viewPoint = ToViewPoint(floorPoint);
        ClickViewPoint(viewPoint);
    }

    public void TapFloorPoint(Point floorPoint)
    {
        var viewPoint = ToViewPoint(floorPoint);
        var touchId = ++_touchIdCounter;
        SendTouch(touchId, viewPoint, SKTouchAction.Pressed, true);
        SendTouch(touchId, viewPoint, SKTouchAction.Released, false);
    }

    public void ClickViewPoint(Point viewPoint)
    {
        SendPointer(viewPoint, vm => vm.PointerPressedCommand, command => new PointerPressedCommand(CanvasView, command), isInContact: true);
        SpinWait.SpinUntil(() => false, TimeSpan.FromMilliseconds(10));
        SendPointer(viewPoint, vm => vm.PointerReleasedCommand, command => new PointerReleasedCommand(command), isInContact: false);
    }

    public void Dispose()
    {
        ViewModel.CanvasView = null;
        _activation.Dispose();
        _serviceProvider.Dispose();
    }

    private void SendPointer<TCommand>(
        Point viewPoint,
        Func<FloorCanvasViewModel, IReactiveCommand<TCommand, TCommand>> commandSelector,
        Func<PointerEventArgs, TCommand> commandFactory,
        bool isInContact = true)
    {
        var args = new TestPointerEventArgs(viewPoint, isInContact: isInContact);
        commandSelector(ViewModel)
            .Execute(commandFactory(args))
            .FirstAsync()
            .Wait();
    }

    private void SendTouch(long touchId, Point viewPoint, SKTouchAction actionType, bool inContact)
    {
        var args = new SKTouchEventArgs(
            touchId,
            actionType,
            SKMouseButton.Left,
            SKTouchDeviceType.Touch,
            new SKPoint((float)viewPoint.X, (float)viewPoint.Y),
            inContact);

        ViewModel.TouchCommand
            .Execute(new TouchCommand(CanvasView, args))
            .FirstAsync()
            .Wait();
    }

    private Point ToViewPoint(Point floorPoint)
    {
        var floor = GlobalState.Choreography.Floor;
        var width = (double)_floorBounds.Width;
        var height = (double)_floorBounds.Height;
        var floorWidth = (double)(floor.SizeLeft + floor.SizeRight);
        var floorHeight = (double)(floor.SizeFront + floor.SizeBack);
        var scale = Math.Min(width / floorWidth, height / floorHeight);
        var centerX = _floorBounds.Left + (float)(width / 2d);
        var centerY = _floorBounds.Top + (float)(height / 2d);
        var x = centerX + floorPoint.X * scale;
        var y = centerY - floorPoint.Y * scale;
        var canvasPoint = new SKPoint((float)x, (float)y);
        var transformed = ViewModel.TransformationMatrix.MapPoint(canvasPoint);

        var (scaleX, scaleY) = GetCanvasScale();
        var viewX = transformed.X / scaleX;
        var viewY = transformed.Y / scaleY;
        return new Point(viewX, viewY);
    }

    private (float ScaleX, float ScaleY) GetCanvasScale()
    {
        if (!CanvasView.IsValid())
        {
            return (1f, 1f);
        }

        var width = CanvasView.Width;
        var height = CanvasView.Height;
        var scaleX = CanvasView.CanvasSize.Width / (float)width;
        var scaleY = CanvasView.CanvasSize.Height / (float)height;
        return (scaleX, scaleY);
    }
}
