namespace ChoreoApp.Models;

public interface ICloneable<out T> : ICloneable
{
    T Clone(CloneMode mode);
}
