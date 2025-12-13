using Splat;

namespace ChoreoApp;

public static class ReadonlyDependencyResolverExtension
{
    extension(IReadonlyDependencyResolver resolver)
    {
        public T GetRequiredService<T>()
        {
            var obj = resolver.GetService<T>();
            ArgumentNullException.ThrowIfNull(obj);
            return obj;
        }
    }
}
