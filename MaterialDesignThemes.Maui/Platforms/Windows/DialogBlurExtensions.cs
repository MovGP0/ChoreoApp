#if WINDOWS
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Brush = Microsoft.UI.Xaml.Media.Brush;

namespace MaterialDesignThemes.Maui;

public static partial class DialogBlurExtensions
{
    private sealed class BlurState
    {
        public Brush? OriginalBackground { get; init; }
    }

    private static readonly ConditionalWeakTable<FrameworkElement, BlurState> BlurStates = new();

    static partial void SetDialogBackgroundBlurInternal(VisualElement visualElement, bool isEnabled, double radius)
    {
        if (visualElement.Handler?.PlatformView is not FrameworkElement element)
        {
            return;
        }

        if (!TryGetBackgroundTarget(element, out var target))
        {
            return;
        }

        if (!isEnabled || radius <= 0)
        {
            if (BlurStates.TryGetValue(element, out var state))
            {
                target.Background = state.OriginalBackground;
                BlurStates.Remove(element);
            }
            return;
        }

        if (!BlurStates.TryGetValue(element, out _))
        {
            BlurStates.Add(element, new BlurState { OriginalBackground = target.Background });
        }

        target.Background = CreateAcrylicBrush(radius);
    }

    private static AcrylicBrush CreateAcrylicBrush(double radius)
    {
        double opacity = Math.Clamp(radius / 30d, 0.05, 0.9);
        return new AcrylicBrush
        {
            //BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
            TintOpacity = opacity,
            TintColor = Windows.UI.Color.FromArgb(255, 32, 32, 32),
            FallbackColor = Windows.UI.Color.FromArgb(200, 32, 32, 32)
        };
    }

    private static bool TryGetBackgroundTarget(FrameworkElement element, out IBackgroundTarget target)
    {
        if (element is Panel panel)
        {
            target = new PanelBackgroundTarget(panel);
            return true;
        }

        if (element is Control control)
        {
            target = new ControlBackgroundTarget(control);
            return true;
        }

        target = null!;
        return false;
    }

    private interface IBackgroundTarget
    {
        Brush? Background { get; set; }
    }

    private sealed class PanelBackgroundTarget(Panel panel) : IBackgroundTarget
    {
        public Brush? Background
        {
            get => panel.Background;
            set => panel.Background = value;
        }
    }

    private sealed class ControlBackgroundTarget(Control control) : IBackgroundTarget
    {
        public Brush? Background
        {
            get => control.Background;
            set => control.Background = value;
        }
    }
}
#endif
