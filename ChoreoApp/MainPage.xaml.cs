using SkiaSharp.Views.Maui;
using ReactiveUI.Maui;
using SkiaSharp;

namespace ChoreoApp;

public partial class MainPage : ReactiveContentPage<MainViewModel>
{
    public MainPage()
    {
        InitializeComponent();
        ViewModel ??= new MainViewModel();

        this.WhenActivated(disposables =>
        {
            // Bindings and activation logic can go here.
        });
    }

    private void OnBurgerClicked(object sender, EventArgs e)
    {
        ViewModel?.ToggleNavigation();
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync(nameof(SettingsPage));
        }
    }

    private void OnNavItemDragStarting(object sender, DragStartingEventArgs e)
    {
        if (sender is BindableObject bindable && bindable.BindingContext is NavItemViewModel item)
        {
            e.Data.Properties["NavItem"] = item;
        }
    }

    private void OnNavItemDragOver(object sender, DragEventArgs e)
    {
        e.AcceptedOperation = DataPackageOperation.Copy;
    }

    private void OnNavItemDrop(object sender, DropEventArgs e)
    {
        if (ViewModel == null)
        {
            return;
        }

        if (e.Data.Properties.TryGetValue("NavItem", out var dragged) &&
            dragged is NavItemViewModel draggedItem &&
            sender is BindableObject bindable &&
            bindable.BindingContext is NavItemViewModel targetItem)
        {
            ViewModel.MoveNavItem(draggedItem, targetItem);
        }
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);

        using var backgroundPaint = new SKPaint
        {
            Shader = SKShader.CreateLinearGradient(
                new SKPoint(0, 0),
                new SKPoint(e.Info.Width, e.Info.Height),
                new[] { SKColors.DeepSkyBlue, SKColors.MediumPurple },
                null,
                SKShaderTileMode.Clamp)
        };

        canvas.DrawRect(e.Info.Rect, backgroundPaint);

        using var textPaint = new SKPaint
        {
            Color = SKColors.White,
            TextSize = 48,
            IsAntialias = true
        };

        const string message = "SkiaSharp Surface";
        var textWidth = textPaint.MeasureText(message);
        canvas.DrawText(message, (e.Info.Width - textWidth) / 2, e.Info.Height / 2f, textPaint);
    }
}
