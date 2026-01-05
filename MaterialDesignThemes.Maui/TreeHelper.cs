namespace MaterialDesignThemes.Maui;

internal static class TreeHelper
{
    public static double GetVisibleWidth(VisualElement element, VisualElement parent, FlowDirection flowDirection)
    {
        ArgumentNullException.ThrowIfNull(element);
        ArgumentNullException.ThrowIfNull(parent);

        if (!TryGetLocationRelativeTo(element, parent, out var location))
        {
            return element.Width;
        }

        if (flowDirection != parent.FlowDirection)
        {
            location = new Point(location.X - element.Width, location.Y);
        }

        var elementRect = new Rect(location.X, location.Y, element.Width, element.Height);
        var parentRect = new Rect(0, 0, parent.Width, parent.Height);
        var visible = Rect.Intersect(elementRect, parentRect);
        return visible.Width > 0 ? visible.Width : 0;
    }

    private static bool TryGetLocationRelativeTo(VisualElement element, VisualElement parent, out Point location)
    {
        var x = 0d;
        var y = 0d;
        var current = element;

        while (current is not null && current != parent)
        {
            x += current.X;
            y += current.Y;
            current = current.Parent as VisualElement;
        }

        if (current != parent)
        {
            location = default;
            return false;
        }

        location = new Point(x, y);
        return true;
    }

    public static VisualElement? FindMainTreeVisual(VisualElement? visual)
    {
        Element? root = null;
        Element? current = visual;

        while (current is not null)
        {
            root = current;
            current = current.Parent;
        }

        return root as VisualElement;
    }

    public static T? FindChild<T>(this Element? parent, string childName)
        where T : Element
    {
        if (parent is null)
        {
            return null;
        }

        foreach (var child in parent.VisualDepthFirstTraversal().Skip(1))
        {
            if (child is not T candidate)
            {
                continue;
            }

            if (string.IsNullOrEmpty(childName))
            {
                return candidate;
            }

            if (candidate is VisualElement visual)
            {
                if (string.Equals(visual.StyleId, childName, StringComparison.Ordinal)
                    || string.Equals(visual.AutomationId, childName, StringComparison.Ordinal))
                {
                    return candidate;
                }
            }
        }

        return null;
    }
}
