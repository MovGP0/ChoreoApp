using StronglyTypedIds;

namespace ChoreoMasterMobile.Json;

[StronglyTypedId(Template.Int)]
public partial struct SceneId
{
    public static explicit operator int(SceneId value) => value.Value;
    public static implicit operator SceneId(int value) => new(value);
}
