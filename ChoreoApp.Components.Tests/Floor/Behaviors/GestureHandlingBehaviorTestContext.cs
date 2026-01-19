using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reflection;
using ChoreoApp.Floor;
using ChoreoApp.Floor.Behaviors;
using ChoreoApp.Floor.Messages;
using ChoreoApp.Global;
using ChoreoApp.Settings;
using ChoreoApp.StateMachine;
using NSubstitute;
using Shouldly;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace ChoreoApp.Components.Tests.Floor.Behaviors;

internal sealed class GestureHandlingBehaviorTestContext : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly IDisposable _activation;
    private readonly GestureHandlingBehavior _gestureHandlingBehavior;

    private GestureHandlingBehaviorTestContext(
        ServiceProvider serviceProvider,
        FloorCanvasViewModel viewModel,
        ISKCanvasView canvasView,
        GestureHandlingBehavior gestureHandlingBehavior)
    {
        _serviceProvider = serviceProvider;
        ViewModel = viewModel;
        CanvasView = canvasView;
        _gestureHandlingBehavior = gestureHandlingBehavior;
        ViewModel.CanvasView = canvasView;
        _activation = ViewModel.Activator.Activate();
    }

    public FloorCanvasViewModel ViewModel { get; }
    public ISKCanvasView CanvasView { get; }

    public static GestureHandlingBehaviorTestContext Create(Action<ServiceCollection>? configureServices = null)
    {
        RxApp.MainThreadScheduler = ImmediateScheduler.Instance;
        RxApp.TaskpoolScheduler = ImmediateScheduler.Instance;

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
        services.AddSingleton<GlobalStateModel>();
        services.AddSingleton<IGlobalStateModel>(sp => sp.GetRequiredService<GlobalStateModel>());
        services.AddApplicationStateMachine();

        services.AddSingleton<GestureHandlingBehavior>();
        services.AddSingleton<IBehavior<FloorCanvasViewModel>>(sp => sp.GetRequiredService<GestureHandlingBehavior>());
        services.AddSingleton<FloorCanvasViewModel>();

        configureServices?.Invoke(services);

        var canvasView = Substitute.For<ISKCanvasView>();
        canvasView.Width.Returns(100d);
        canvasView.Height.Returns(100d);
        canvasView.CanvasSize.Returns(new SKSize(100, 100));

        var provider = services.BuildServiceProvider();
        var viewModel = provider.GetRequiredService<FloorCanvasViewModel>();
        var behavior = provider.GetRequiredService<GestureHandlingBehavior>();
        return new GestureHandlingBehaviorTestContext(provider, viewModel, canvasView, behavior);
    }

    public void SendPointerPressed(Point viewPoint)
    {
        var args = new TestPointerEventArgs(viewPoint);
        ViewModel.PointerPressedCommand
            .Execute(new PointerPressedCommand(CanvasView, args))
            .FirstAsync()
            .Wait();
    }

    public void SendPointerMoved(Point viewPoint)
    {
        var args = new TestPointerEventArgs(viewPoint);
        ViewModel.PointerMovedCommand
            .Execute(new PointerMovedCommand(CanvasView, args))
            .FirstAsync()
            .Wait();
    }

    public void SendPointerWheelChanged(int delta, Point? position)
    {
        ViewModel.PointerWheelChangedCommand
            .Execute(new PointerWheelChangedCommand(CanvasView, delta, position))
            .FirstAsync()
            .Wait();
    }

    public void SendTouchPressed(long id, Point viewPoint)
    {
        SendTouch(id, viewPoint, SKTouchAction.Pressed, true);
    }

    public void SendTouchMoved(long id, Point viewPoint)
    {
        SendTouch(id, viewPoint, SKTouchAction.Moved, true);
    }

    public void SendTouchReleased(long id, Point viewPoint)
    {
        SendTouch(id, viewPoint, SKTouchAction.Released, false);
    }

    public void WaitForScaleChange(Func<float, bool> predicate)
    {
        SpinWait.SpinUntil(
            () => predicate(ViewModel.TransformationMatrix.ScaleX),
            TimeSpan.FromSeconds(1)).ShouldBeTrue();
    }

    public void Dispose()
    {
        ViewModel.CanvasView = null;
        _activation.Dispose();
        _serviceProvider.Dispose();
    }

    private void SendTouch(long id, Point viewPoint, SKTouchAction actionType, bool inContact)
    {
        var args = new SKTouchEventArgs(
            id,
            actionType,
            SKMouseButton.Left,
            SKTouchDeviceType.Touch,
            new SKPoint((float)viewPoint.X, (float)viewPoint.Y),
            inContact);

        InvokeHandleTouch(new TouchCommand(CanvasView, args));
    }

    private void InvokeHandleTouch(TouchCommand command)
    {
        var method = typeof(GestureHandlingBehavior).GetMethod(
            "HandleTouch",
            BindingFlags.Instance | BindingFlags.NonPublic);

        method.ShouldNotBeNull();
        method.Invoke(_gestureHandlingBehavior, [ViewModel, command]);
    }
}
