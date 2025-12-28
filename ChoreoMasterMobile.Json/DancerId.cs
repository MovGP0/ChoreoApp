using StronglyTypedIds;

namespace ChoreoMasterMobile.Json;

[StronglyTypedId(Template.Int)]
public readonly partial struct DancerId
{
    public static explicit operator int(DancerId value) => value.Value;
    public static implicit operator DancerId(int value) => new(value);
}
