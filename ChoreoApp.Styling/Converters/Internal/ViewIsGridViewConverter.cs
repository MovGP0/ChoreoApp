using System.Globalization;
using System.Reflection;

namespace ChoreoApp.Styling.Converters.Internal;

internal sealed class ViewIsGridViewConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is CollectionView collectionView)
        {
            return collectionView.ItemsLayout is GridItemsLayout;
        }

        if (value is ItemsView itemsView)
        {
            var layout = GetItemsLayout(itemsView);
            return layout is GridItemsLayout;
        }

        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    private static IItemsLayout? GetItemsLayout(ItemsView itemsView)
    {
        var property = itemsView.GetType().GetProperty("ItemsLayout");
        if (property?.GetValue(itemsView) is IItemsLayout layout)
        {
            return layout;
        }

        var internalProperty = itemsView.GetType().GetProperty(
            "InternalItemsLayout",
            BindingFlags.Instance | BindingFlags.NonPublic);
        if (internalProperty?.GetValue(itemsView) is IItemsLayout internalLayout)
        {
            return internalLayout;
        }

        return null;
    }
}
