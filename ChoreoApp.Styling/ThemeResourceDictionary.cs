using System.ComponentModel;
using Microsoft.Maui.Graphics;

namespace ChoreoApp;

public sealed class ThemeResourceDictionary : ResourceDictionary
{
    private Theme? _theme;

    public void Load(Theme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        if (_theme is not null)
        {
            _theme.PropertyChanged -= OnThemePropertyChanged;
        }

        _theme = theme;
        _theme.PropertyChanged += OnThemePropertyChanged;

        SetAllThemeColors(_theme);
    }

    private void OnThemePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_theme is null || string.IsNullOrWhiteSpace(e.PropertyName))
        {
            return;
        }

        if (TryGetThemeColor(_theme, e.PropertyName, out var color))
        {
            this.SetColor(e.PropertyName, color);
        }
    }

    private void SetAllThemeColors(Theme theme)
    {
        this.SetColor(ThemeKey.Primary, theme.Primary);
        this.SetColor(ThemeKey.SurfaceTint, theme.SurfaceTint);
        this.SetColor(ThemeKey.OnPrimary, theme.OnPrimary);
        this.SetColor(ThemeKey.PrimaryContainer, theme.PrimaryContainer);
        this.SetColor(ThemeKey.OnPrimaryContainer, theme.OnPrimaryContainer);
        this.SetColor(ThemeKey.Secondary, theme.Secondary);
        this.SetColor(ThemeKey.OnSecondary, theme.OnSecondary);
        this.SetColor(ThemeKey.SecondaryContainer, theme.SecondaryContainer);
        this.SetColor(ThemeKey.OnSecondaryContainer, theme.OnSecondaryContainer);
        this.SetColor(ThemeKey.Tertiary, theme.Tertiary);
        this.SetColor(ThemeKey.OnTertiary, theme.OnTertiary);
        this.SetColor(ThemeKey.TertiaryContainer, theme.TertiaryContainer);
        this.SetColor(ThemeKey.OnTertiaryContainer, theme.OnTertiaryContainer);
        this.SetColor(ThemeKey.Error, theme.Error);
        this.SetColor(ThemeKey.OnError, theme.OnError);
        this.SetColor(ThemeKey.ErrorContainer, theme.ErrorContainer);
        this.SetColor(ThemeKey.OnErrorContainer, theme.OnErrorContainer);
        this.SetColor(ThemeKey.Background, theme.Background);
        this.SetColor(ThemeKey.OnBackground, theme.OnBackground);
        this.SetColor(ThemeKey.Surface, theme.Surface);
        this.SetColor(ThemeKey.OnSurface, theme.OnSurface);
        this.SetColor(ThemeKey.SurfaceVariant, theme.SurfaceVariant);
        this.SetColor(ThemeKey.OnSurfaceVariant, theme.OnSurfaceVariant);
        this.SetColor(ThemeKey.Outline, theme.Outline);
        this.SetColor(ThemeKey.OutlineVariant, theme.OutlineVariant);
        this.SetColor(ThemeKey.Shadow, theme.Shadow);
        this.SetColor(ThemeKey.Scrim, theme.Scrim);
        this.SetColor(ThemeKey.InverseSurface, theme.InverseSurface);
        this.SetColor(ThemeKey.InverseOnSurface, theme.InverseOnSurface);
        this.SetColor(ThemeKey.InversePrimary, theme.InversePrimary);
        this.SetColor(ThemeKey.PrimaryFixed, theme.PrimaryFixed);
        this.SetColor(ThemeKey.OnPrimaryFixed, theme.OnPrimaryFixed);
        this.SetColor(ThemeKey.PrimaryFixedDim, theme.PrimaryFixedDim);
        this.SetColor(ThemeKey.OnPrimaryFixedVariant, theme.OnPrimaryFixedVariant);
        this.SetColor(ThemeKey.SecondaryFixed, theme.SecondaryFixed);
        this.SetColor(ThemeKey.OnSecondaryFixed, theme.OnSecondaryFixed);
        this.SetColor(ThemeKey.SecondaryFixedDim, theme.SecondaryFixedDim);
        this.SetColor(ThemeKey.OnSecondaryFixedVariant, theme.OnSecondaryFixedVariant);
        this.SetColor(ThemeKey.TertiaryFixed, theme.TertiaryFixed);
        this.SetColor(ThemeKey.OnTertiaryFixed, theme.OnTertiaryFixed);
        this.SetColor(ThemeKey.TertiaryFixedDim, theme.TertiaryFixedDim);
        this.SetColor(ThemeKey.OnTertiaryFixedVariant, theme.OnTertiaryFixedVariant);
        this.SetColor(ThemeKey.SurfaceDim, theme.SurfaceDim);
        this.SetColor(ThemeKey.SurfaceBright, theme.SurfaceBright);
        this.SetColor(ThemeKey.SurfaceContainerLowest, theme.SurfaceContainerLowest);
        this.SetColor(ThemeKey.SurfaceContainerLow, theme.SurfaceContainerLow);
        this.SetColor(ThemeKey.SurfaceContainer, theme.SurfaceContainer);
        this.SetColor(ThemeKey.SurfaceContainerHigh, theme.SurfaceContainerHigh);
        this.SetColor(ThemeKey.SurfaceContainerHighest, theme.SurfaceContainerHighest);
    }

    private static bool TryGetThemeColor(Theme theme, string propertyName, out Color color)
    {
        switch (propertyName)
        {
            case nameof(Theme.Primary):
                color = theme.Primary;
                return true;
            case nameof(Theme.SurfaceTint):
                color = theme.SurfaceTint;
                return true;
            case nameof(Theme.OnPrimary):
                color = theme.OnPrimary;
                return true;
            case nameof(Theme.PrimaryContainer):
                color = theme.PrimaryContainer;
                return true;
            case nameof(Theme.OnPrimaryContainer):
                color = theme.OnPrimaryContainer;
                return true;
            case nameof(Theme.Secondary):
                color = theme.Secondary;
                return true;
            case nameof(Theme.OnSecondary):
                color = theme.OnSecondary;
                return true;
            case nameof(Theme.SecondaryContainer):
                color = theme.SecondaryContainer;
                return true;
            case nameof(Theme.OnSecondaryContainer):
                color = theme.OnSecondaryContainer;
                return true;
            case nameof(Theme.Tertiary):
                color = theme.Tertiary;
                return true;
            case nameof(Theme.OnTertiary):
                color = theme.OnTertiary;
                return true;
            case nameof(Theme.TertiaryContainer):
                color = theme.TertiaryContainer;
                return true;
            case nameof(Theme.OnTertiaryContainer):
                color = theme.OnTertiaryContainer;
                return true;
            case nameof(Theme.Error):
                color = theme.Error;
                return true;
            case nameof(Theme.OnError):
                color = theme.OnError;
                return true;
            case nameof(Theme.ErrorContainer):
                color = theme.ErrorContainer;
                return true;
            case nameof(Theme.OnErrorContainer):
                color = theme.OnErrorContainer;
                return true;
            case nameof(Theme.Background):
                color = theme.Background;
                return true;
            case nameof(Theme.OnBackground):
                color = theme.OnBackground;
                return true;
            case nameof(Theme.Surface):
                color = theme.Surface;
                return true;
            case nameof(Theme.OnSurface):
                color = theme.OnSurface;
                return true;
            case nameof(Theme.SurfaceVariant):
                color = theme.SurfaceVariant;
                return true;
            case nameof(Theme.OnSurfaceVariant):
                color = theme.OnSurfaceVariant;
                return true;
            case nameof(Theme.Outline):
                color = theme.Outline;
                return true;
            case nameof(Theme.OutlineVariant):
                color = theme.OutlineVariant;
                return true;
            case nameof(Theme.Shadow):
                color = theme.Shadow;
                return true;
            case nameof(Theme.Scrim):
                color = theme.Scrim;
                return true;
            case nameof(Theme.InverseSurface):
                color = theme.InverseSurface;
                return true;
            case nameof(Theme.InverseOnSurface):
                color = theme.InverseOnSurface;
                return true;
            case nameof(Theme.InversePrimary):
                color = theme.InversePrimary;
                return true;
            case nameof(Theme.PrimaryFixed):
                color = theme.PrimaryFixed;
                return true;
            case nameof(Theme.OnPrimaryFixed):
                color = theme.OnPrimaryFixed;
                return true;
            case nameof(Theme.PrimaryFixedDim):
                color = theme.PrimaryFixedDim;
                return true;
            case nameof(Theme.OnPrimaryFixedVariant):
                color = theme.OnPrimaryFixedVariant;
                return true;
            case nameof(Theme.SecondaryFixed):
                color = theme.SecondaryFixed;
                return true;
            case nameof(Theme.OnSecondaryFixed):
                color = theme.OnSecondaryFixed;
                return true;
            case nameof(Theme.SecondaryFixedDim):
                color = theme.SecondaryFixedDim;
                return true;
            case nameof(Theme.OnSecondaryFixedVariant):
                color = theme.OnSecondaryFixedVariant;
                return true;
            case nameof(Theme.TertiaryFixed):
                color = theme.TertiaryFixed;
                return true;
            case nameof(Theme.OnTertiaryFixed):
                color = theme.OnTertiaryFixed;
                return true;
            case nameof(Theme.TertiaryFixedDim):
                color = theme.TertiaryFixedDim;
                return true;
            case nameof(Theme.OnTertiaryFixedVariant):
                color = theme.OnTertiaryFixedVariant;
                return true;
            case nameof(Theme.SurfaceDim):
                color = theme.SurfaceDim;
                return true;
            case nameof(Theme.SurfaceBright):
                color = theme.SurfaceBright;
                return true;
            case nameof(Theme.SurfaceContainerLowest):
                color = theme.SurfaceContainerLowest;
                return true;
            case nameof(Theme.SurfaceContainerLow):
                color = theme.SurfaceContainerLow;
                return true;
            case nameof(Theme.SurfaceContainer):
                color = theme.SurfaceContainer;
                return true;
            case nameof(Theme.SurfaceContainerHigh):
                color = theme.SurfaceContainerHigh;
                return true;
            case nameof(Theme.SurfaceContainerHighest):
                color = theme.SurfaceContainerHighest;
                return true;
            default:
                color = default;
                return false;
        }
    }
}
