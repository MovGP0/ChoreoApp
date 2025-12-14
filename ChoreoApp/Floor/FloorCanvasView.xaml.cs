using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using ChoreoApp.Styling;
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

            CanvasView.InvalidateSurface();
        });
    }

    /// <summary>
    /// Transformation matrix for zooming/panning/rotating the floor view.
    /// </summary>
    /// <remarks>
    /// Do not use this at the moment, as zooming/panning/rotating will be implemented later.
    /// </remarks>
    private SKMatrix TransformationMatrix { get; set; } = SKMatrix.Identity;

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
}
