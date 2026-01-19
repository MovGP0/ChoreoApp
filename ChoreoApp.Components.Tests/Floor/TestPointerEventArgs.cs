using System.Reflection;

namespace ChoreoApp.Components.Tests.Floor;

internal sealed class TestPointerEventArgs : PointerEventArgs
{
    private static readonly BindingFlags Flags =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private readonly Point _point;

    public TestPointerEventArgs(Point point, ButtonsMask button = ButtonsMask.Primary, bool isInContact = true)
    {
        _point = point;
        TrySetButton(this, button);
        TrySetIsInContact(this, isInContact);
    }

    public override Point? GetPosition(Element? relativeTo) => _point;

    private static void TrySetButton(PointerEventArgs args, ButtonsMask button)
    {
        for (var type = args.GetType(); type is not null; type = type.BaseType)
        {
            var setButton = TrySetButtonProperty(args, type, "Button", button);
            var setButtons = TrySetButtonProperty(args, type, "Buttons", button);
            if (setButton || setButtons)
            {
                return;
            }

            if (TrySetButtonField(args, type, button))
            {
                return;
            }
        }
    }

    private static bool TrySetButtonProperty(PointerEventArgs args, Type type, string propertyName, ButtonsMask button)
    {
        var property = type.GetProperty(propertyName, Flags);
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

    private static void TrySetIsInContact(PointerEventArgs args, bool isInContact)
    {
        for (var type = args.GetType(); type is not null; type = type.BaseType)
        {
            if (TrySetBooleanProperty(args, type, "IsInContact", isInContact)
                || TrySetBooleanProperty(args, type, "IsPressed", isInContact)
                || TrySetBooleanField(args, type, isInContact))
            {
                return;
            }
        }
    }

    private static bool TrySetBooleanProperty(PointerEventArgs args, Type type, string propertyName, bool value)
    {
        var property = type.GetProperty(propertyName, Flags);
        if (property is null)
        {
            return false;
        }

        if (property.PropertyType != typeof(bool) && property.PropertyType != typeof(bool?))
        {
            return false;
        }

        var setter = property.GetSetMethod(true);
        if (setter is null)
        {
            return false;
        }

        setter.Invoke(args, [value]);
        return true;
    }

    private static bool TrySetBooleanField(PointerEventArgs args, Type type, bool value)
    {
        var field = type.GetField("<IsInContact>k__BackingField", Flags)
            ?? type.GetField("<IsPressed>k__BackingField", Flags)
            ?? type.GetFields(Flags).FirstOrDefault(candidate => candidate.FieldType == typeof(bool))
            ?? type.GetFields(Flags).FirstOrDefault(candidate => candidate.FieldType == typeof(bool?));
        if (field is null)
        {
            return false;
        }

        field.SetValue(args, value);
        return true;
    }
}
