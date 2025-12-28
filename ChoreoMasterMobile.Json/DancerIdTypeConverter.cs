using System.ComponentModel;

namespace ChoreoMasterMobile.Json;

public sealed class DancerIdTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(int) || sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object value)
    {
        return value switch
        {
            int intValue => new DancerId(intValue),
            string stringValue when !string.IsNullOrEmpty(stringValue) && int.TryParse(stringValue, out var result) => new DancerId(result),
            _ => base.ConvertFrom(context, culture, value),
        };
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? sourceType)
    {
        return sourceType == typeof(int) || sourceType == typeof(string) || base.CanConvertTo(context, sourceType);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, System.Globalization.CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is DancerId idValue)
        {
            if (destinationType == typeof(int))
            {
                return idValue.Value;
            }

            if (destinationType == typeof(string))
            {
                return idValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
