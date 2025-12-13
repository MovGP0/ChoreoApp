using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace ChoreoApp.Floor;

public partial class FloorCanvasView
{
    public FloorCanvasView()
    {
        InitializeComponent();
    }

    private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);

        using var backgroundPaint = new SKPaint();
        backgroundPaint.Shader = SKShader.CreateLinearGradient(
            new SKPoint(0, 0),
            new SKPoint(e.Info.Width, e.Info.Height),
            [SKColors.DeepSkyBlue, SKColors.MediumPurple],
            null,
            SKShaderTileMode.Clamp);

        canvas.DrawRect(e.Info.Rect, backgroundPaint);

        using var textPaint = new SKPaint();
        textPaint.Color = SKColors.White;
        textPaint.IsAntialias = true;

        using var font = new SKFont();
        font.Size = 48;

        const string message = "SkiaSharp Surface";
        var textWidth = font.MeasureText(message);
        canvas.DrawText(message, (e.Info.Width - textWidth) / 2, e.Info.Height / 2f, font, textPaint);
    }
}
