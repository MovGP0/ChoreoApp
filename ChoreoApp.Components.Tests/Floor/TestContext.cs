using ChoreoApp.Floor;
using ChoreoApp.Floor.Behaviors;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Global;
using ChoreoApp.Models;
using ChoreoApp.Scenes;
using ChoreoApp.Scenes.Behaviors;
using ChoreoApp.Settings;
using ChoreoApp.Settings.Behaviors;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.Triggers;
using MaterialDesignThemes.Maui;
using NSubstitute;
using Shouldly;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Reactive.Linq;

namespace ChoreoApp.Components.Tests.Floor;

internal sealed class TestContext : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDisposable _floorActivation;
    private readonly IDisposable _scenesActivation;
    private readonly SKRect _floorBounds;

    private TestContext(
        ServiceProvider serviceProvider,
        GlobalStateModel globalState,
        ApplicationStateMachine stateMachine,
        FloorCanvasViewModel floorViewModel,
        ScenesPaneViewModel scenesPaneViewModel,
        ISKCanvasView canvasView,
        IDisposable floorActivation,
        IDisposable scenesActivation,
        SKRect floorBounds)
    {
        _serviceProvider = serviceProvider;
        GlobalState = globalState;
        StateMachine = stateMachine;
        FloorViewModel = floorViewModel;
        ScenesPaneViewModel = scenesPaneViewModel;
        CanvasView = canvasView;
        _floorActivation = floorActivation;
        _scenesActivation = scenesActivation;
        _floorBounds = floorBounds;
    }

    public GlobalStateModel GlobalState { get; }
    public ApplicationStateMachine StateMachine { get; }
    public FloorCanvasViewModel FloorViewModel { get; }
    public ScenesPaneViewModel ScenesPaneViewModel { get; }
    public ISKCanvasView CanvasView { get; }

    public static TestContext Create(Action<ServiceCollection>? configureServices = null)
    {
        var services = new ServiceCollection();
        services.AddMessagePipe();
        services.AddLogging();

        var preferences = Substitute.For<IPreferences>();
        preferences.Get(Arg.Any<string>(), Arg.Any<string>()).Returns(string.Empty);

        var haptic = Substitute.For<IHapticFeedback>();
        haptic.IsSupported.Returns(false);

        var vibration = Substitute.For<IVibration>();
        vibration.IsSupported.Returns(false);

        services.AddSingleton(preferences);
        services.AddSingleton(haptic);
        services.AddSingleton(vibration);
        services.AddSingleton<SettingsViewModel>();
        services.AddTransient<IBehavior<SettingsViewModel>, LoadSettingsPreferencesBehavior>();
        services.AddTransient<IBehavior<SettingsViewModel>, SwitchDarkLightModeBehavior>();
        services.AddTransient<IBehavior<SettingsViewModel>, ColorPreferencesBehavior>();
        services.AddSingleton<GlobalStateModel>();
        services.AddSingleton<IGlobalStateModel>(sp => sp.GetRequiredService<GlobalStateModel>());
        services.AddApplicationStateMachine();

        services.AddTransient<IBehavior<FloorCanvasViewModel>, MovePositionsBehavior>();
        services.AddTransient<FloorCanvasViewModel>();

        services.AddTransient<IBehavior<ScenesPaneViewModel>, LoadScenesBehavior>();
        services.AddTransient<ScenesPaneViewModel>();
        services.AddTransient<SceneViewModel>();

        configureServices?.Invoke(services);

        var serviceProvider = services.BuildServiceProvider();
        var globalState = serviceProvider.GetRequiredService<GlobalStateModel>();
        var stateMachine = serviceProvider.GetRequiredService<ApplicationStateMachine>();
        var floorViewModel = serviceProvider.GetRequiredService<FloorCanvasViewModel>();
        var scenesPaneViewModel = serviceProvider.GetRequiredService<ScenesPaneViewModel>();
        var canvasView = Substitute.For<ISKCanvasView>();
        canvasView.Width.Returns(100);
        canvasView.Height.Returns(100);
        canvasView.CanvasSize.Returns(new SKSize(100, 100));

        var floorBounds = new SKRect(0, 0, 100, 100);
        floorViewModel.CanvasView = canvasView;
        floorViewModel.UpdateFloorBounds(floorBounds, new SKSize(100, 100));

        var floorActivation = floorViewModel.Activator.Activate();
        var scenesActivation = scenesPaneViewModel.Activator.Activate();

        return new TestContext(
            serviceProvider,
            globalState,
            stateMachine,
            floorViewModel,
            scenesPaneViewModel,
            canvasView,
            floorActivation,
            scenesActivation,
            floorBounds);
    }

    public void LoadChoreography(ChoreographyModel choreography)
    {
        GlobalState.Choreography = choreography;

        SpinWait.SpinUntil(
            () => GlobalState.SelectedScene is { Positions.Count: > 0 },
            TimeSpan.FromSeconds(1)).ShouldBeTrue();
    }

    public void EnableMoveMode()
    {
        GlobalState.InteractionMode = InteractionMode.Move;
        StateMachine.TryApply(new MovePositionsStartedTrigger()).ShouldBeTrue();
    }

    public void SelectByRectangle(Point startFloorPoint, Point endFloorPoint)
    {
        var startView = ToViewPoint(startFloorPoint);
        var endView = ToViewPoint(endFloorPoint);
        SendPointer(startView, vm => vm.PointerPressedCommand, command => new PointerPressedCommand(CanvasView, command), isInContact: true);
        SpinWait.SpinUntil(() => false, TimeSpan.FromMilliseconds(10));
        SendPointer(endView, vm => vm.PointerMovedCommand, command => new PointerMovedCommand(CanvasView, command), isInContact: true);
        SpinWait.SpinUntil(() => false, TimeSpan.FromMilliseconds(10));
        SendPointer(endView, vm => vm.PointerReleasedCommand, command => new PointerReleasedCommand(command), isInContact: false);
        SpinWait.SpinUntil(
            () => GlobalState.SelectionRectangle is null
                  && GlobalState.SelectedPositions.Count > 0,
            TimeSpan.FromSeconds(1));
    }

    public void DragFromTo(Point startFloorPoint, Point endFloorPoint)
    {
        var startView = ToViewPoint(startFloorPoint);
        var endView = ToViewPoint(endFloorPoint);
        SendPointer(startView, vm => vm.PointerPressedCommand, command => new PointerPressedCommand(CanvasView, command), isInContact: true);
        SpinWait.SpinUntil(() => false, TimeSpan.FromMilliseconds(10));
        SendPointer(endView, vm => vm.PointerMovedCommand, command => new PointerMovedCommand(CanvasView, command), isInContact: true);
        SpinWait.SpinUntil(() => false, TimeSpan.FromMilliseconds(10));
        SendPointer(endView, vm => vm.PointerReleasedCommand, command => new PointerReleasedCommand(command), isInContact: false);
    }

    public void ClickInView(Point viewPoint)
    {
        SendPointer(viewPoint, vm => vm.PointerPressedCommand, command => new PointerPressedCommand(CanvasView, command), isInContact: true);
        SpinWait.SpinUntil(() => false, TimeSpan.FromMilliseconds(10));
        SendPointer(viewPoint, vm => vm.PointerReleasedCommand, command => new PointerReleasedCommand(command), isInContact: false);
    }

    public void Dispose()
    {
        _floorActivation.Dispose();
        _scenesActivation.Dispose();
        FloorViewModel.CanvasView = null;
        _serviceProvider.Dispose();
    }

    private void SendPointer<TCommand>(
        Point viewPoint,
        Func<FloorCanvasViewModel, IReactiveCommand<TCommand, TCommand>> commandSelector,
        Func<PointerEventArgs, TCommand> commandFactory,
        bool isInContact = true)
    {
        var args = new TestPointerEventArgs(viewPoint, isInContact: isInContact);
        commandSelector(FloorViewModel)
            .Execute(commandFactory(args))
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
        var transformed = FloorViewModel.TransformationMatrix.MapPoint(canvasPoint);

        var (scaleX, scaleY) = GetCanvasScale();
        var viewX = transformed.X / scaleX;
        var viewY = transformed.Y / scaleY;
        return new Point(viewX, viewY);
    }

    public void TranslateView(float deltaX, float deltaY)
    {
        FloorViewModel.TransformationMatrix = SKMatrix.CreateTranslation(deltaX, deltaY);
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
