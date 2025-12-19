using Microsoft.Maui.Graphics;

namespace ChoreoApp.Styling;

internal sealed class SliderWithTicksTickDrawable : IDrawable
{
    private const float DefaultTickHeight = 4f;
    private IReadOnlyList<double> _ticks = Array.Empty<double>();

    public double Minimum { get; set; }
    public double Maximum { get; set; }
    public Color? TickColor { get; set; }

    public void SetTicks(IReadOnlyList<double> ticks)
    {
        _ticks = ticks;
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (_ticks.Count == 0)
        {
            return;
        }

        var range = Maximum - Minimum;

        if (range <= 0d)
        {
            return;
        }

        var color = TickColor ?? Colors.Gray;
        canvas.StrokeColor = color;
        canvas.StrokeSize = 1f;

        var height = dirtyRect.Height;
        var tickHeight = Math.Min(DefaultTickHeight, height);
        var y1 = height - tickHeight;
        var y2 = height;

        foreach (var value in _ticks)
        {
            if (value < Minimum || value > Maximum)
            {
                continue;
            }

            var ratio = (value - Minimum) / range;
            var x = (float)(dirtyRect.Width * ratio);

            canvas.DrawLine(x, y1, x, y2);
        }
    }
}
