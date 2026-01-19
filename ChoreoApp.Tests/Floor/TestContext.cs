using ChoreoApp.Floor;
using ChoreoApp.Floor.Behaviors;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Global;
using ChoreoApp.Models;
using ChoreoApp.Scenes;
using ChoreoApp.Scenes.Behaviors;
using ChoreoApp.StateMachine;
using ChoreoApp.StateMachine.Triggers;
using NSubstitute;
using Shouldly;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ChoreoApp.Tests.Floor;

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

    public static TestContext Create()
    {
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
        services.AddSingleton<GlobalStateModel>();
        services.AddSingleton<IGlobalStateModel>(sp => sp.GetRequiredService<GlobalStateModel>());
        services.AddApplicationStateMachine();

        services.AddTransient<IBehavior<FloorCanvasViewModel>, MovePositionsBehavior>();
        services.AddTransient<FloorCanvasViewModel>();

        services.AddTransient<IBehavior<ScenesPaneViewModel>, LoadScenesBehavior>();
        services.AddTransient<ScenesPaneViewModel>();
        services.AddTransient<SceneViewModel>();

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
        GlobalState.SelectedScene.ShouldNotBeNull();
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
        SendTouch(1, SKTouchAction.Pressed, startView, true);
        SendTouch(1, SKTouchAction.Moved, endView, true);
        SendTouch(1, SKTouchAction.Released, endView, false);
    }

    public void DragFromTo(Point startFloorPoint, Point endFloorPoint)
    {
        var startView = ToViewPoint(startFloorPoint);
        var endView = ToViewPoint(endFloorPoint);
        SendTouch(2, SKTouchAction.Pressed, startView, true);
        SendTouch(2, SKTouchAction.Moved, endView, true);
        SendTouch(2, SKTouchAction.Released, endView, false);
    }

    public void ClickInView(Point viewPoint)
    {
        SendTouch(3, SKTouchAction.Pressed, viewPoint, true);
        SendTouch(3, SKTouchAction.Released, viewPoint, false);
    }

    public void Dispose()
    {
        _floorActivation.Dispose();
        _scenesActivation.Dispose();
        FloorViewModel.CanvasView = null;
        _serviceProvider.Dispose();
    }

    private void SendTouch(long id, SKTouchAction action, Point viewPoint, bool inContact)
    {
        var args = new SKTouchEventArgs(
            id,
            action,
            SKMouseButton.Left,
            SKTouchDeviceType.Touch,
            new SKPoint((float)viewPoint.X, (float)viewPoint.Y),
            inContact);

        FloorViewModel
            .TouchCommand
            .Execute(new TouchCommand(CanvasView, args))
            .Subscribe();
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
        return new Point(x, y);
    }
}
