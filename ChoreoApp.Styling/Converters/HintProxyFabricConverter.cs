using System.Globalization;

namespace ChoreoApp.Styling.Converters;

/// <summary>
/// Converter for SmartHint to resolve an IHintProxy from a bound target.
/// </summary>
public sealed class HintProxyFabricConverter : IValueConverter
{
    private static readonly Lazy<HintProxyFabricConverter> InstanceValue = new();

    public static HintProxyFabricConverter Instance => InstanceValue.Value;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value as IHintProxy;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
