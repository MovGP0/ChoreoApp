using System.ComponentModel;
using Microsoft.Maui.Graphics;

namespace ChoreoApp;

/// <summary>
/// Resource dictionary that exposes colors via strongly-typed keys and keeps
/// itself in sync with a <see cref="ColorPalette"/> or <see cref="Theme"/> by
/// listening to their change notifications.
/// </summary>
public sealed class ColorPaletteResourceDictionary : ResourceDictionary
{
    private ColorPalette? _palette;

    public void Load(ColorPalette palette)
    {
        ArgumentNullException.ThrowIfNull(palette);

        if (_palette is not null)
        {
            _palette.PropertyChanged -= OnPalettePropertyChanged;
        }

        _palette = palette;
        _palette.PropertyChanged += OnPalettePropertyChanged;

        SetAllPaletteColors(_palette);
    }

    private void OnPalettePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_palette is null || string.IsNullOrWhiteSpace(e.PropertyName))
        {
            return;
        }

        if (TryGetPaletteColor(_palette, e.PropertyName, out var color))
        {
            this.SetColor(e.PropertyName, color);
        }
    }

    private void SetAllPaletteColors(ColorPalette palette)
    {
        this.SetColor(ColorPaletteKey.Primary, palette.Primary);
        this.SetColor(ColorPaletteKey.Secondary, palette.Secondary);
        this.SetColor(ColorPaletteKey.Tertiary, palette.Tertiary);
        this.SetColor(ColorPaletteKey.Error, palette.Error);
        this.SetColor(ColorPaletteKey.Neutral, palette.Neutral);
        this.SetColor(ColorPaletteKey.NeutralVariant, palette.NeutralVariant);

        this.SetColor(ColorPaletteKey.Primary0, palette.Primary0);
        this.SetColor(ColorPaletteKey.Primary5, palette.Primary5);
        this.SetColor(ColorPaletteKey.Primary10, palette.Primary10);
        this.SetColor(ColorPaletteKey.Primary15, palette.Primary15);
        this.SetColor(ColorPaletteKey.Primary20, palette.Primary20);
        this.SetColor(ColorPaletteKey.Primary25, palette.Primary25);
        this.SetColor(ColorPaletteKey.Primary30, palette.Primary30);
        this.SetColor(ColorPaletteKey.Primary35, palette.Primary35);
        this.SetColor(ColorPaletteKey.Primary40, palette.Primary40);
        this.SetColor(ColorPaletteKey.Primary50, palette.Primary50);
        this.SetColor(ColorPaletteKey.Primary60, palette.Primary60);
        this.SetColor(ColorPaletteKey.Primary70, palette.Primary70);
        this.SetColor(ColorPaletteKey.Primary80, palette.Primary80);
        this.SetColor(ColorPaletteKey.Primary90, palette.Primary90);
        this.SetColor(ColorPaletteKey.Primary95, palette.Primary95);
        this.SetColor(ColorPaletteKey.Primary98, palette.Primary98);
        this.SetColor(ColorPaletteKey.Primary99, palette.Primary99);
        this.SetColor(ColorPaletteKey.Primary100, palette.Primary100);

        this.SetColor(ColorPaletteKey.Secondary0, palette.Secondary0);
        this.SetColor(ColorPaletteKey.Secondary5, palette.Secondary5);
        this.SetColor(ColorPaletteKey.Secondary10, palette.Secondary10);
        this.SetColor(ColorPaletteKey.Secondary15, palette.Secondary15);
        this.SetColor(ColorPaletteKey.Secondary20, palette.Secondary20);
        this.SetColor(ColorPaletteKey.Secondary25, palette.Secondary25);
        this.SetColor(ColorPaletteKey.Secondary30, palette.Secondary30);
        this.SetColor(ColorPaletteKey.Secondary35, palette.Secondary35);
        this.SetColor(ColorPaletteKey.Secondary40, palette.Secondary40);
        this.SetColor(ColorPaletteKey.Secondary50, palette.Secondary50);
        this.SetColor(ColorPaletteKey.Secondary60, palette.Secondary60);
        this.SetColor(ColorPaletteKey.Secondary70, palette.Secondary70);
        this.SetColor(ColorPaletteKey.Secondary80, palette.Secondary80);
        this.SetColor(ColorPaletteKey.Secondary90, palette.Secondary90);
        this.SetColor(ColorPaletteKey.Secondary95, palette.Secondary95);
        this.SetColor(ColorPaletteKey.Secondary98, palette.Secondary98);
        this.SetColor(ColorPaletteKey.Secondary99, palette.Secondary99);
        this.SetColor(ColorPaletteKey.Secondary100, palette.Secondary100);

        this.SetColor(ColorPaletteKey.Tertiary0, palette.Tertiary0);
        this.SetColor(ColorPaletteKey.Tertiary5, palette.Tertiary5);
        this.SetColor(ColorPaletteKey.Tertiary10, palette.Tertiary10);
        this.SetColor(ColorPaletteKey.Tertiary15, palette.Tertiary15);
        this.SetColor(ColorPaletteKey.Tertiary20, palette.Tertiary20);
        this.SetColor(ColorPaletteKey.Tertiary25, palette.Tertiary25);
        this.SetColor(ColorPaletteKey.Tertiary30, palette.Tertiary30);
        this.SetColor(ColorPaletteKey.Tertiary35, palette.Tertiary35);
        this.SetColor(ColorPaletteKey.Tertiary40, palette.Tertiary40);
        this.SetColor(ColorPaletteKey.Tertiary50, palette.Tertiary50);
        this.SetColor(ColorPaletteKey.Tertiary60, palette.Tertiary60);
        this.SetColor(ColorPaletteKey.Tertiary70, palette.Tertiary70);
        this.SetColor(ColorPaletteKey.Tertiary80, palette.Tertiary80);
        this.SetColor(ColorPaletteKey.Tertiary90, palette.Tertiary90);
        this.SetColor(ColorPaletteKey.Tertiary95, palette.Tertiary95);
        this.SetColor(ColorPaletteKey.Tertiary98, palette.Tertiary98);
        this.SetColor(ColorPaletteKey.Tertiary99, palette.Tertiary99);
        this.SetColor(ColorPaletteKey.Tertiary100, palette.Tertiary100);

        this.SetColor(ColorPaletteKey.Neutral0, palette.Neutral0);
        this.SetColor(ColorPaletteKey.Neutral5, palette.Neutral5);
        this.SetColor(ColorPaletteKey.Neutral10, palette.Neutral10);
        this.SetColor(ColorPaletteKey.Neutral15, palette.Neutral15);
        this.SetColor(ColorPaletteKey.Neutral20, palette.Neutral20);
        this.SetColor(ColorPaletteKey.Neutral25, palette.Neutral25);
        this.SetColor(ColorPaletteKey.Neutral30, palette.Neutral30);
        this.SetColor(ColorPaletteKey.Neutral35, palette.Neutral35);
        this.SetColor(ColorPaletteKey.Neutral40, palette.Neutral40);
        this.SetColor(ColorPaletteKey.Neutral50, palette.Neutral50);
        this.SetColor(ColorPaletteKey.Neutral60, palette.Neutral60);
        this.SetColor(ColorPaletteKey.Neutral70, palette.Neutral70);
        this.SetColor(ColorPaletteKey.Neutral80, palette.Neutral80);
        this.SetColor(ColorPaletteKey.Neutral90, palette.Neutral90);
        this.SetColor(ColorPaletteKey.Neutral95, palette.Neutral95);
        this.SetColor(ColorPaletteKey.Neutral98, palette.Neutral98);
        this.SetColor(ColorPaletteKey.Neutral99, palette.Neutral99);
        this.SetColor(ColorPaletteKey.Neutral100, palette.Neutral100);

        this.SetColor(ColorPaletteKey.NeutralVariant0, palette.NeutralVariant0);
        this.SetColor(ColorPaletteKey.NeutralVariant5, palette.NeutralVariant5);
        this.SetColor(ColorPaletteKey.NeutralVariant10, palette.NeutralVariant10);
        this.SetColor(ColorPaletteKey.NeutralVariant15, palette.NeutralVariant15);
        this.SetColor(ColorPaletteKey.NeutralVariant20, palette.NeutralVariant20);
        this.SetColor(ColorPaletteKey.NeutralVariant25, palette.NeutralVariant25);
        this.SetColor(ColorPaletteKey.NeutralVariant30, palette.NeutralVariant30);
        this.SetColor(ColorPaletteKey.NeutralVariant35, palette.NeutralVariant35);
        this.SetColor(ColorPaletteKey.NeutralVariant40, palette.NeutralVariant40);
        this.SetColor(ColorPaletteKey.NeutralVariant50, palette.NeutralVariant50);
        this.SetColor(ColorPaletteKey.NeutralVariant60, palette.NeutralVariant60);
        this.SetColor(ColorPaletteKey.NeutralVariant70, palette.NeutralVariant70);
        this.SetColor(ColorPaletteKey.NeutralVariant80, palette.NeutralVariant80);
        this.SetColor(ColorPaletteKey.NeutralVariant90, palette.NeutralVariant90);
        this.SetColor(ColorPaletteKey.NeutralVariant95, palette.NeutralVariant95);
        this.SetColor(ColorPaletteKey.NeutralVariant98, palette.NeutralVariant98);
        this.SetColor(ColorPaletteKey.NeutralVariant99, palette.NeutralVariant99);
        this.SetColor(ColorPaletteKey.NeutralVariant100, palette.NeutralVariant100);
    }

    private static bool TryGetPaletteColor(ColorPalette palette, string propertyName, out Color color)
    {
        switch (propertyName)
        {
            case nameof(ColorPalette.Primary):
                color = palette.Primary;
                return true;
            case nameof(ColorPalette.Secondary):
                color = palette.Secondary;
                return true;
            case nameof(ColorPalette.Tertiary):
                color = palette.Tertiary;
                return true;
            case nameof(ColorPalette.Error):
                color = palette.Error;
                return true;
            case nameof(ColorPalette.Neutral):
                color = palette.Neutral;
                return true;
            case nameof(ColorPalette.NeutralVariant):
                color = palette.NeutralVariant;
                return true;
            case nameof(ColorPalette.Primary0):
                color = palette.Primary0;
                return true;
            case nameof(ColorPalette.Primary5):
                color = palette.Primary5;
                return true;
            case nameof(ColorPalette.Primary10):
                color = palette.Primary10;
                return true;
            case nameof(ColorPalette.Primary15):
                color = palette.Primary15;
                return true;
            case nameof(ColorPalette.Primary20):
                color = palette.Primary20;
                return true;
            case nameof(ColorPalette.Primary25):
                color = palette.Primary25;
                return true;
            case nameof(ColorPalette.Primary30):
                color = palette.Primary30;
                return true;
            case nameof(ColorPalette.Primary35):
                color = palette.Primary35;
                return true;
            case nameof(ColorPalette.Primary40):
                color = palette.Primary40;
                return true;
            case nameof(ColorPalette.Primary50):
                color = palette.Primary50;
                return true;
            case nameof(ColorPalette.Primary60):
                color = palette.Primary60;
                return true;
            case nameof(ColorPalette.Primary70):
                color = palette.Primary70;
                return true;
            case nameof(ColorPalette.Primary80):
                color = palette.Primary80;
                return true;
            case nameof(ColorPalette.Primary90):
                color = palette.Primary90;
                return true;
            case nameof(ColorPalette.Primary95):
                color = palette.Primary95;
                return true;
            case nameof(ColorPalette.Primary98):
                color = palette.Primary98;
                return true;
            case nameof(ColorPalette.Primary99):
                color = palette.Primary99;
                return true;
            case nameof(ColorPalette.Primary100):
                color = palette.Primary100;
                return true;
            case nameof(ColorPalette.Secondary0):
                color = palette.Secondary0;
                return true;
            case nameof(ColorPalette.Secondary5):
                color = palette.Secondary5;
                return true;
            case nameof(ColorPalette.Secondary10):
                color = palette.Secondary10;
                return true;
            case nameof(ColorPalette.Secondary15):
                color = palette.Secondary15;
                return true;
            case nameof(ColorPalette.Secondary20):
                color = palette.Secondary20;
                return true;
            case nameof(ColorPalette.Secondary25):
                color = palette.Secondary25;
                return true;
            case nameof(ColorPalette.Secondary30):
                color = palette.Secondary30;
                return true;
            case nameof(ColorPalette.Secondary35):
                color = palette.Secondary35;
                return true;
            case nameof(ColorPalette.Secondary40):
                color = palette.Secondary40;
                return true;
            case nameof(ColorPalette.Secondary50):
                color = palette.Secondary50;
                return true;
            case nameof(ColorPalette.Secondary60):
                color = palette.Secondary60;
                return true;
            case nameof(ColorPalette.Secondary70):
                color = palette.Secondary70;
                return true;
            case nameof(ColorPalette.Secondary80):
                color = palette.Secondary80;
                return true;
            case nameof(ColorPalette.Secondary90):
                color = palette.Secondary90;
                return true;
            case nameof(ColorPalette.Secondary95):
                color = palette.Secondary95;
                return true;
            case nameof(ColorPalette.Secondary98):
                color = palette.Secondary98;
                return true;
            case nameof(ColorPalette.Secondary99):
                color = palette.Secondary99;
                return true;
            case nameof(ColorPalette.Secondary100):
                color = palette.Secondary100;
                return true;
            case nameof(ColorPalette.Tertiary0):
                color = palette.Tertiary0;
                return true;
            case nameof(ColorPalette.Tertiary5):
                color = palette.Tertiary5;
                return true;
            case nameof(ColorPalette.Tertiary10):
                color = palette.Tertiary10;
                return true;
            case nameof(ColorPalette.Tertiary15):
                color = palette.Tertiary15;
                return true;
            case nameof(ColorPalette.Tertiary20):
                color = palette.Tertiary20;
                return true;
            case nameof(ColorPalette.Tertiary25):
                color = palette.Tertiary25;
                return true;
            case nameof(ColorPalette.Tertiary30):
                color = palette.Tertiary30;
                return true;
            case nameof(ColorPalette.Tertiary35):
                color = palette.Tertiary35;
                return true;
            case nameof(ColorPalette.Tertiary40):
                color = palette.Tertiary40;
                return true;
            case nameof(ColorPalette.Tertiary50):
                color = palette.Tertiary50;
                return true;
            case nameof(ColorPalette.Tertiary60):
                color = palette.Tertiary60;
                return true;
            case nameof(ColorPalette.Tertiary70):
                color = palette.Tertiary70;
                return true;
            case nameof(ColorPalette.Tertiary80):
                color = palette.Tertiary80;
                return true;
            case nameof(ColorPalette.Tertiary90):
                color = palette.Tertiary90;
                return true;
            case nameof(ColorPalette.Tertiary95):
                color = palette.Tertiary95;
                return true;
            case nameof(ColorPalette.Tertiary98):
                color = palette.Tertiary98;
                return true;
            case nameof(ColorPalette.Tertiary99):
                color = palette.Tertiary99;
                return true;
            case nameof(ColorPalette.Tertiary100):
                color = palette.Tertiary100;
                return true;
            case nameof(ColorPalette.Neutral0):
                color = palette.Neutral0;
                return true;
            case nameof(ColorPalette.Neutral5):
                color = palette.Neutral5;
                return true;
            case nameof(ColorPalette.Neutral10):
                color = palette.Neutral10;
                return true;
            case nameof(ColorPalette.Neutral15):
                color = palette.Neutral15;
                return true;
            case nameof(ColorPalette.Neutral20):
                color = palette.Neutral20;
                return true;
            case nameof(ColorPalette.Neutral25):
                color = palette.Neutral25;
                return true;
            case nameof(ColorPalette.Neutral30):
                color = palette.Neutral30;
                return true;
            case nameof(ColorPalette.Neutral35):
                color = palette.Neutral35;
                return true;
            case nameof(ColorPalette.Neutral40):
                color = palette.Neutral40;
                return true;
            case nameof(ColorPalette.Neutral50):
                color = palette.Neutral50;
                return true;
            case nameof(ColorPalette.Neutral60):
                color = palette.Neutral60;
                return true;
            case nameof(ColorPalette.Neutral70):
                color = palette.Neutral70;
                return true;
            case nameof(ColorPalette.Neutral80):
                color = palette.Neutral80;
                return true;
            case nameof(ColorPalette.Neutral90):
                color = palette.Neutral90;
                return true;
            case nameof(ColorPalette.Neutral95):
                color = palette.Neutral95;
                return true;
            case nameof(ColorPalette.Neutral98):
                color = palette.Neutral98;
                return true;
            case nameof(ColorPalette.Neutral99):
                color = palette.Neutral99;
                return true;
            case nameof(ColorPalette.Neutral100):
                color = palette.Neutral100;
                return true;
            case nameof(ColorPalette.NeutralVariant0):
                color = palette.NeutralVariant0;
                return true;
            case nameof(ColorPalette.NeutralVariant5):
                color = palette.NeutralVariant5;
                return true;
            case nameof(ColorPalette.NeutralVariant10):
                color = palette.NeutralVariant10;
                return true;
            case nameof(ColorPalette.NeutralVariant15):
                color = palette.NeutralVariant15;
                return true;
            case nameof(ColorPalette.NeutralVariant20):
                color = palette.NeutralVariant20;
                return true;
            case nameof(ColorPalette.NeutralVariant25):
                color = palette.NeutralVariant25;
                return true;
            case nameof(ColorPalette.NeutralVariant30):
                color = palette.NeutralVariant30;
                return true;
            case nameof(ColorPalette.NeutralVariant35):
                color = palette.NeutralVariant35;
                return true;
            case nameof(ColorPalette.NeutralVariant40):
                color = palette.NeutralVariant40;
                return true;
            case nameof(ColorPalette.NeutralVariant50):
                color = palette.NeutralVariant50;
                return true;
            case nameof(ColorPalette.NeutralVariant60):
                color = palette.NeutralVariant60;
                return true;
            case nameof(ColorPalette.NeutralVariant70):
                color = palette.NeutralVariant70;
                return true;
            case nameof(ColorPalette.NeutralVariant80):
                color = palette.NeutralVariant80;
                return true;
            case nameof(ColorPalette.NeutralVariant90):
                color = palette.NeutralVariant90;
                return true;
            case nameof(ColorPalette.NeutralVariant95):
                color = palette.NeutralVariant95;
                return true;
            case nameof(ColorPalette.NeutralVariant98):
                color = palette.NeutralVariant98;
                return true;
            case nameof(ColorPalette.NeutralVariant99):
                color = palette.NeutralVariant99;
                return true;
            case nameof(ColorPalette.NeutralVariant100):
                color = palette.NeutralVariant100;
                return true;
            default:
                color = default;
                return false;
        }
    }
}
