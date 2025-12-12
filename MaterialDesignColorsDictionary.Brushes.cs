using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Microsoft.Maui.Graphics;

namespace ChoreoApp;

public sealed partial class MaterialDesignColorsDictionary : IDisposable
{
    public event EventHandler<ResourcesChangedEventArgs>? ValuesChanged;

    private const string BrushSuffix = "Brush";

    private readonly Dictionary<string, Color> _colorKeyToColor = new(StringComparer.Ordinal);
    private readonly Dictionary<Color, SolidColorBrush> _brushesByColor = new();

    public bool TryGetValue(string key, out object value)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (key.EndsWith(BrushSuffix, StringComparison.Ordinal))
        {
            var colorKey = key[..^BrushSuffix.Length];

            if (_baseDictionary.TryGetValue(colorKey, out var colorValue) && colorValue is Color color)
            {
                CacheColor(colorKey, color);
                value = GetOrCreateBrush(color);
                return true;
            }
        }

        return _baseDictionary.TryGetValue(key, out value);
    }

    internal void TrackColor(string key, Color color)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        if (_colorKeyToColor.TryGetValue(key, out var existingColor) && existingColor != color)
        {
            _colorKeyToColor[key] = color;
            RemoveBrushIfUnused(existingColor);
            return;
        }

        _colorKeyToColor[key] = color;
    }

    private void OnValuesChanged(object sender, ResourcesChangedEventArgs args)
    {
        foreach (var change in args.Values)
        {
            if (change.Value is Color newColor)
            {
                if (_colorKeyToColor.TryGetValue(change.Key, out var previousColor) && previousColor != newColor)
                {
                    _colorKeyToColor[change.Key] = newColor;
                    RemoveBrushIfUnused(previousColor);
                }
                else
                {
                    _colorKeyToColor[change.Key] = newColor;
                }
            }
            else
            {
                RemoveCacheEntry(change.Key);
            }
        }

        ValuesChanged?.Invoke(this, args);
    }

    public void Dispose()
    {
        var resourceDictionary = (IResourceDictionary)_baseDictionary;
        resourceDictionary.ValuesChanged -= OnValuesChanged;

        _brushesByColor.Clear();
        _colorKeyToColor.Clear();
        GC.SuppressFinalize(this);
    }

    private SolidColorBrush GetOrCreateBrush(Color color)
    {
        if (!_brushesByColor.TryGetValue(color, out var brush))
        {
            brush = new SolidColorBrush
            {
                Color = color
            };

            _brushesByColor[color] = brush;
        }

        return brush;
    }

    private void RemoveBrushIfUnused(Color color)
    {
        foreach (var mappedColor in _colorKeyToColor.Values)
        {
            if (mappedColor == color)
            {
                return;
            }
        }

        _brushesByColor.Remove(color);
    }

    private bool RemoveCacheEntry(string key)
    {
        if (!_colorKeyToColor.TryGetValue(key, out var color))
        {
            return false;
        }

        _colorKeyToColor.Remove(key);
        RemoveBrushIfUnused(color);
        return true;
    }
}
