using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ChoreoApp;

public sealed partial class MaterialDesignColorsDictionary : IDisposable
{
    private const string BrushSuffix = "Brush";

    private readonly Dictionary<string, Color> colorKeyToColor = new(StringComparer.Ordinal);
    private readonly Dictionary<Color, BrushEntry> brushesByColor = new();

    public new bool TryGetValue(string key, out object value)
    {
        ArgumentNullException.ThrowIfNull(key);

        if (key.EndsWith(BrushSuffix, StringComparison.Ordinal))
        {
            var colorKey = key[..^BrushSuffix.Length];

            var hasColor = colorKeyToColor.TryGetValue(colorKey, out var color);

            if (!hasColor && base.TryGetValue(colorKey, out var baseValue) && baseValue is Color discoveredColor)
            {
                TrackColor(colorKey, discoveredColor);
                color = discoveredColor;
                hasColor = true;
            }

            if (hasColor)
            {
                value = GetOrCreateBrush(color);
                return true;
            }
        }

        return base.TryGetValue(key, out value);
    }

    internal void TrackColor(string key, Color color)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        if (colorKeyToColor.TryGetValue(key, out var existingColor))
        {
            if (existingColor == color)
            {
                return;
            }

            DecrementUsage(existingColor);
        }

        colorKeyToColor[key] = color;
        IncrementUsage(color);
    }

    public void Dispose()
    {
        foreach (var entry in brushesByColor.Values)
        {
            entry.Dispose();
        }

        brushesByColor.Clear();
        colorKeyToColor.Clear();
        GC.SuppressFinalize(this);
    }

    private SolidColorBrush GetOrCreateBrush(Color color)
    {
        if (!brushesByColor.TryGetValue(color, out var entry))
        {
            entry = new BrushEntry(new SolidColorBrush
            {
                Color = color
            });

            brushesByColor[color] = entry;
        }

        return entry.Brush;
    }

    private void IncrementUsage(Color color)
    {
        if (brushesByColor.TryGetValue(color, out var existing))
        {
            existing.AddRef();
            return;
        }

        brushesByColor[color] = new BrushEntry(new SolidColorBrush
        {
            Color = color
        });
    }

    private void DecrementUsage(Color color)
    {
        if (!brushesByColor.TryGetValue(color, out var entry))
        {
            return;
        }

        if (entry.Release())
        {
            brushesByColor.Remove(color);
        }
    }

    private sealed class BrushEntry : IDisposable
    {
        public BrushEntry(SolidColorBrush brush)
        {
            Brush = brush;
        }

        public SolidColorBrush Brush { get; }

        private int RefCount { get; set; } = 1;

        public void AddRef()
        {
            RefCount++;
        }

        public bool Release()
        {
            RefCount--;

            if (RefCount > 0)
            {
                return false;
            }

            (Brush as IDisposable)?.Dispose();
            return true;
        }

        public void Dispose()
        {
            (Brush as IDisposable)?.Dispose();
        }
    }
}
