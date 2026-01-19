using System.Linq;
using System.Reflection;

namespace ChoreoApp.Components.Tests.Floor;

internal sealed class TestPointerEventArgs : PointerEventArgs
{
    private static readonly BindingFlags Flags =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private readonly Point _point;

    public TestPointerEventArgs(Point point, ButtonsMask button = ButtonsMask.Primary)
    {
        _point = point;
        TrySetButton(this, button);
    }

    public override Point? GetPosition(Element? relativeTo) => _point;

    private static void TrySetButton(PointerEventArgs args, ButtonsMask button)
    {
        for (var type = args.GetType(); type is not null; type = type.BaseType)
        {
            if (TrySetButtonProperty(args, type, button))
            {
                return;
            }

            if (TrySetButtonField(args, type, button))
            {
                return;
            }
        }
    }

    private static bool TrySetButtonProperty(PointerEventArgs args, Type type, ButtonsMask button)
    {
        var property = type.GetProperty("Button", Flags)
            ?? type.GetProperty("Buttons", Flags);
        if (property is null)
        {
            return false;
        }

        var propertyType = property.PropertyType;
        if (propertyType != typeof(ButtonsMask) && propertyType != typeof(ButtonsMask?))
        {
            return false;
        }

        var setter = property.GetSetMethod(true);
        if (setter is null)
        {
            return false;
        }

        setter.Invoke(args, [button]);
        return true;
    }

    private static bool TrySetButtonField(PointerEventArgs args, Type type, ButtonsMask button)
    {
        var field = type.GetField("<Button>k__BackingField", Flags)
            ?? type.GetField("<Buttons>k__BackingField", Flags)
            ?? type.GetFields(Flags).FirstOrDefault(candidate => candidate.FieldType == typeof(ButtonsMask))
            ?? type.GetFields(Flags).FirstOrDefault(candidate => candidate.FieldType == typeof(ButtonsMask?));
        if (field is null)
        {
            return false;
        }

        field.SetValue(args, button);
        return true;
    }
}
