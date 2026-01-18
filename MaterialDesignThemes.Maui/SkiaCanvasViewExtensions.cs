using SkiaSharp.Views.Maui;

namespace MaterialDesignThemes.Maui;

public static class SkiaCanvasViewExtensions
{
    extension(ISKCanvasView canvasView)
    {
        public bool IsValid()
        {
            var width = canvasView.Width;
            var height = canvasView.Height;

            return width > 0
                   && height > 0
                   && !double.IsNaN(width)
                   && !double.IsNaN(height)
                   && !double.IsInfinity(width)
                   && !double.IsInfinity(height);
        }
    }
}
