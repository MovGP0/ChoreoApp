using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace ChoreoApp;

public sealed partial class MaterialDesignColorsDictionary : IDisposable
{
    public event EventHandler<ResourcesChangedEventArgs>? ValuesChanged;

    private const string BrushSuffix = "Brush";

    private readonly Dictionary<Color, SolidColorBrush> _brushesByColor = new();
    private readonly Dictionary<string, Color> _colorKeyToColor = new(StringComparer.Ordinal);

    public bool TryGetValue(string key, out object value)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (key.EndsWith(BrushSuffix, StringComparison.Ordinal))
        {
            var colorKey = key[..^BrushSuffix.Length];

            if (_baseDictionary.TryGetValue(colorKey, out var colorValue) && colorValue is Color color)
            {
                if (!_colorKeyToColor.TryGetValue(colorKey, out var currentColor) || currentColor != color)
                {
                    _colorKeyToColor[colorKey] = color;
                }

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
        }

        return _baseDictionary.TryGetValue(key, out value);
    }

    private void OnValuesChanged(object sender, ResourcesChangedEventArgs args)
    {
        ValuesChanged?.Invoke(this, args);
    }

    public void Dispose()
    {
        foreach (var brush in _brushesByColor.Values)
        {
            (brush as IDisposable)?.Dispose();
        }

        _brushesByColor.Clear();
        _colorKeyToColor.Clear();
        GC.SuppressFinalize(this);
    }
}
