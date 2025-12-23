#if ANDROID
using Android.Graphics;
using Microsoft.Maui.Platform;

namespace ChoreoApp.Styling;

public static partial class DialogBlurExtensions
{
    static partial void SetDialogBackgroundBlurInternal(VisualElement visualElement, bool isEnabled, double radius)
    {
        if (!OperatingSystem.IsAndroidVersionAtLeast(31))
        {
            return;
        }

        var view = visualElement.Handler?.PlatformView as Android.Views.View;
        if (view is null)
        {
            return;
        }

        if (!isEnabled || radius <= 0)
        {
            view.SetRenderEffect(null);
            return;
        }

        float blurRadius = (float)radius;
        using var blurEffect = RenderEffect.CreateBlurEffect(blurRadius, blurRadius, Shader.TileMode.Clamp!);
        view.SetRenderEffect(blurEffect);
    }
}
#endif
