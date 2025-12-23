#if IOS
using Microsoft.Maui.Platform;
using UIKit;

namespace ChoreoApp.Styling;

public static partial class DialogBlurExtensions
{
    private const int BlurViewTag = 0x4D445442;

    static partial void SetDialogBackgroundBlurInternal(VisualElement visualElement, bool isEnabled, double radius)
    {
        if (!OperatingSystem.IsIOSVersionAtLeast(13))
        {
            return;
        }

        var view = visualElement.ToPlatform(visualElement.Handler?.MauiContext
            ?? Application.Current?.Windows.LastOrDefault()?.Page?.Handler?.MauiContext);
        if (view is null)
        {
            return;
        }

        var existing = view.ViewWithTag(BlurViewTag) as UIVisualEffectView;
        if (!isEnabled || radius <= 0)
        {
            existing?.RemoveFromSuperview();
            return;
        }

        if (existing is null)
        {
            var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.SystemMaterial);
            existing = new UIVisualEffectView(blurEffect)
            {
                Tag = BlurViewTag,
                Frame = view.Bounds,
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight
            };
            view.InsertSubview(existing, 0);
        }
        else
        {
            existing.Frame = view.Bounds;
        }
    }
}
#endif
