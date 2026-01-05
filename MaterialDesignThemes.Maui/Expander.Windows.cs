#if WINDOWS
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace MaterialDesignThemes.Maui;

public partial class Expander
{
    private static void ForceUpdateCellSize(CollectionView collectionView, Size size, Point point)
    {
        if (collectionView.Handler?.PlatformView is not ItemsControl listView)
        {
            return;
        }

        if (listView is not ListViewBase listViewBase)
        {
            return;
        }

        var element = listViewBase.ContainerFromIndex(0) as FrameworkElement;
        if (element is null)
        {
            return;
        }

        element.Measure(new Windows.Foundation.Size(size.Width, size.Height));
    }
}
#endif
