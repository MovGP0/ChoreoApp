namespace MaterialDesignThemes.Maui;

internal static class VisualTreeExtensions
{
    public static IEnumerable<Element> VisualDepthFirstTraversal(this Element node)
    {
        ArgumentNullException.ThrowIfNull(node);

        yield return node;

        foreach (var child in GetLogicalChildren(node))
        {
            foreach (var descendant in child.VisualDepthFirstTraversal())
            {
                yield return descendant;
            }
        }
    }

    public static IEnumerable<Element> VisualBreadthFirstTraversal(this Element node)
    {
        ArgumentNullException.ThrowIfNull(node);

        foreach (var child in GetLogicalChildren(node))
        {
            yield return child;
        }

        foreach (var child in GetLogicalChildren(node))
        {
            foreach (var descendant in child.VisualDepthFirstTraversal())
            {
                yield return descendant;
            }
        }
    }

    public static bool IsAncestorOf(this Element parent, Element? node)
        => node is not null && parent.VisualDepthFirstTraversal().Contains(node);

    public static IEnumerable<Element> GetVisualAncestry(this Element? leaf)
    {
        while (leaf is not null)
        {
            yield return leaf;
            leaf = leaf.Parent;
        }
    }

    public static IEnumerable<Element> GetLogicalAncestry(this Element? leaf)
    {
        while (leaf is not null)
        {
            yield return leaf;
            leaf = leaf.Parent;
        }
    }

    public static bool IsDescendantOf(this Element? leaf, Element? ancestor)
    {
        if (leaf is null || ancestor is null)
        {
            return false;
        }

        foreach (var node in leaf.GetVisualAncestry())
        {
            if (ReferenceEquals(node, ancestor))
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<Element> GetLogicalChildren(Element element)
    {
        if (element is IElementController controller)
        {
            foreach (var child in controller.LogicalChildren)
            {
                if (child is not null)
                {
                    yield return child;
                }
            }

            yield break;
        }

        if (element is IVisualTreeElement visualTreeElement)
        {
            foreach (var child in visualTreeElement.GetVisualChildren().OfType<Element>())
            {
                yield return child;
            }
        }
    }
}
