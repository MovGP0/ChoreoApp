#if !(ANDROID || IOS || MACCATALYST || WINDOWS)
namespace ChoreoApp.Styling;

public static partial class DialogBlurExtensions
{
    static partial void SetDialogBackgroundBlurInternal(VisualElement visualElement, bool isEnabled, double radius)
    {
    }
}
#endif
