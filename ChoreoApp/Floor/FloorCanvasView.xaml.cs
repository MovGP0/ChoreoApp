using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Floor.Messages;
using MaterialDesignThemes.Maui;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace ChoreoApp.Floor;

public partial class FloorCanvasView
{
    public FloorCanvasView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
        {
            var viewModelActivation = new SerialDisposable();
            viewModelActivation.DisposeWith(disposables);

            this
                .WhenAnyValue(view => view.ViewModel)
                .Subscribe(viewModel =>
                {
                    viewModelActivation.Disposable?.Dispose();

                    if (viewModel is null)
                    {
                        viewModelActivation.Disposable = null;
                        return;
                    }

                    var inner = new CompositeDisposable();

                    viewModel.CanvasView = CanvasView;
                    Disposable
                        .Create(() => viewModel.CanvasView = null)
                        .DisposeWith(inner);

                    viewModel.Activator.Activate().DisposeWith(inner);

                    CanvasView.InvalidateSurface();

                    viewModelActivation.Disposable = inner;
                })
                .DisposeWith(disposables);

            CanvasView.SizeChanged += OnCanvasViewSizeChanged;
            Disposable
                .Create(() => CanvasView.SizeChanged -= OnCanvasViewSizeChanged)
                .DisposeWith(disposables);

#if WINDOWS
            var wheelSubscription = new SerialDisposable();
            wheelSubscription.DisposeWith(disposables);

            CanvasView.HandlerChanged += OnCanvasViewHandlerChanged;
            Disposable
                .Create(() => CanvasView.HandlerChanged -= OnCanvasViewHandlerChanged)
                .DisposeWith(disposables);

            void OnCanvasViewHandlerChanged(object? sender, EventArgs args)
            {
                wheelSubscription.Disposable?.Dispose();

                if (CanvasView.Handler?.PlatformView is not Microsoft.UI.Xaml.FrameworkElement platformView)
                {
                    wheelSubscription.Disposable = null;
                    return;
                }

                void OnPointerWheelChanged(object? _, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs eventArgs)
                {
                    if (ViewModel is not { } viewModel)
                    {
                        return;
                    }

                    var point = eventArgs.GetCurrentPoint(platformView);
                    var position = new Point(point.Position.X, point.Position.Y);
                    viewModel
                        .PointerWheelChangedCommand
                        .Execute(new PointerWheelChangedCommand(CanvasView, point.Properties.MouseWheelDelta, position))
                        .Subscribe();
                }

                platformView.PointerWheelChanged += OnPointerWheelChanged;
                wheelSubscription.Disposable = Disposable.Create(() => platformView.PointerWheelChanged -= OnPointerWheelChanged);
            }
#endif

            CanvasView.InvalidateSurface();
        });
    }

    private static SKColor GetColor(string resourceKey)
    {
        var resources = Application.Current?.Resources;
        if (resources is null
            || !resources.TryGetValue(resourceKey, out var resource)
            || resource is not Color color)
        {
            return SKColors.Transparent;
        }

        return color.ToSKColor();
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        if (ViewModel is not { } viewModel)
        {
            SKColor surfaceColor = GetColor(MaterialDesignColorKey.Surface);
            var canvas = e.Surface.Canvas;
            canvas.Clear(surfaceColor);
            return;
        }

        viewModel.DrawFloorCommandPublisher.Publish(new DrawFloorCommand(e));
    }

    private void OnCanvasViewSizeChanged(object? sender, EventArgs e)
    {
        CanvasView.InvalidateSurface();
    }

    private void OnCanvasViewPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (ViewModel is not { } viewModel)
        {
            return;
        }

        viewModel
            .PanUpdatedCommand
            .Execute(new PanUpdatedCommand(CanvasView, e))
            .Subscribe();
    }

    private void OnCanvasViewPinchUpdated(object? sender, PinchGestureUpdatedEventArgs e)
    {
        if (ViewModel is not { } viewModel)
        {
            return;
        }

        viewModel
            .PinchUpdatedCommand
            .Execute(new PinchUpdatedCommand(CanvasView, e))
            .Subscribe();
    }

    private void OnCanvasViewPointerPressed(object? sender, PointerEventArgs e)
    {
        if (ViewModel is not { } viewModel)
        {
            return;
        }

        viewModel
            .PointerPressedCommand
            .Execute(new PointerPressedCommand(CanvasView, e))
            .Subscribe();
    }

    private void OnCanvasViewPointerMoved(object? sender, PointerEventArgs e)
    {
        if (ViewModel is not { } viewModel)
        {
            return;
        }

        viewModel
            .PointerMovedCommand
            .Execute(new PointerMovedCommand(CanvasView, e))
            .Subscribe();
    }

    private void OnCanvasViewPointerReleased(object? sender, PointerEventArgs e)
    {
        if (ViewModel is not { } viewModel)
        {
            return;
        }

        viewModel
            .PointerReleasedCommand
            .Execute(new PointerReleasedCommand(e))
            .Subscribe();
    }

    private void OnCanvasViewTouch(object? sender, SKTouchEventArgs e)
    {
        if (ViewModel is not { } viewModel)
        {
            return;
        }

        viewModel
            .TouchCommand
            .Execute(new TouchCommand(CanvasView, e))
            .Subscribe();
    }
}
