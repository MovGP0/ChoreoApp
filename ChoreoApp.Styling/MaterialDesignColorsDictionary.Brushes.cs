using Microsoft.Maui.Graphics;

namespace ChoreoApp;

public sealed partial class MaterialDesignColorsDictionary : IDisposable
{
    private const string BrushSuffix = "Brush";

    private readonly Dictionary<Color, SolidColorBrush> _brushesByColor = new();
    private readonly Dictionary<string, Color> _colorKeyToColor = new(StringComparer.Ordinal);

    public new bool TryGetValue(string key, out object value)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (!key.EndsWith(BrushSuffix, StringComparison.Ordinal))
        {
            return base.TryGetValue(key, out value);
        }

        var colorKey = key[..^BrushSuffix.Length];

        if (base.TryGetValue(colorKey, out var colorValue) && colorValue is Color color)
        {
            _colorKeyToColor[colorKey] = color;

            if (!_brushesByColor.TryGetValue(color, out var brush))
            {
                brush = new SolidColorBrush
                {
                    Color = color
                };

                _brushesByColor[color] = brush;
            }

            value = brush;
            return true;
        }

        return base.TryGetValue(key, out value);
    }

    public void Dispose()
    {
        _brushesByColor.Clear();
        _colorKeyToColor.Clear();
        GC.SuppressFinalize(this);
    }
}
