namespace MaterialDesignThemes.Maui;

internal static class ScaleHelper
{
    internal static double GetTotalTransformScaleX(VisualElement visual)
    {
        double totalTransform = 1.0d;
        VisualElement? current = visual;
        while (current is not null)
        {
            var scale = current.ScaleX;
            if (!double.IsNaN(scale) && !double.IsInfinity(scale))
            {
                totalTransform *= scale;
            }

            current = current.Parent as VisualElement;
        }

        return totalTransform;
    }

    internal static double GetTotalTransformScaleY(VisualElement visual)
    {
        double totalTransform = 1.0d;
        VisualElement? current = visual;
        while (current is not null)
        {
            var scale = current.ScaleY;
            if (!double.IsNaN(scale) && !double.IsInfinity(scale))
            {
                totalTransform *= scale;
            }

            current = current.Parent as VisualElement;
        }

        return totalTransform;
    }
}
