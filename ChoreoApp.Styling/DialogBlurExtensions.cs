namespace ChoreoApp.Styling;

public static partial class DialogBlurExtensions
{
    public static void SetDialogBackgroundBlur(this VisualElement visualElement, bool isEnabled, double radius)
    {
        SetDialogBackgroundBlurInternal(visualElement, isEnabled, radius);
    }

    static partial void SetDialogBackgroundBlurInternal(VisualElement visualElement, bool isEnabled, double radius);
}
