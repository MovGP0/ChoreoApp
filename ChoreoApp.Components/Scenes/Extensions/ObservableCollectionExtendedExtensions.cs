using DynamicData.Binding;

namespace ChoreoApp.Scenes.Extensions;

public static class ObservableCollectionExtendedExtensions
{
    public static ObservableCollectionExtended<T> AsObservableCollectionExtended<T>(this IEnumerable<T> source)
    {
        return new ObservableCollectionExtended<T>(source);
    }
}
