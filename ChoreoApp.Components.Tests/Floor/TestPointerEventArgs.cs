using System.Linq;
using System.Reflection;

namespace ChoreoApp.Components.Tests.Floor;

internal sealed class TestPointerEventArgs : PointerEventArgs
{
    private static readonly PropertyInfo? ButtonProperty = typeof(PointerEventArgs).GetProperty(
        "Button",
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    private static readonly FieldInfo? ButtonField = typeof(PointerEventArgs).GetField(
        "<Button>k__BackingField",
        BindingFlags.Instance | BindingFlags.NonPublic);

    private static readonly FieldInfo? ButtonFallbackField = typeof(PointerEventArgs)
        .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        .FirstOrDefault(field => field.FieldType == typeof(ButtonsMask));

    private readonly Point _point;

    public TestPointerEventArgs(Point point, ButtonsMask button = ButtonsMask.Primary)
    {
        _point = point;

        var setter = ButtonProperty?.GetSetMethod(true);
        setter?.Invoke(this, [button]);
        ButtonField?.SetValue(this, button);
        ButtonFallbackField?.SetValue(this, button);
    }

    public override Point? GetPosition(Element? relativeTo) => _point;
}
