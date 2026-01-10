using System.Reflection;

namespace ChoreoApp.Models.Tests;

public static class TestDefaults
{
    static TestDefaults()
    {
        var method = typeof(Preferences)
            .GetMethod("SetDefault", BindingFlags.NonPublic | BindingFlags.Static);

        if (method is null)
        {
            throw new InvalidOperationException("Preferences.SetDefault(IPreferences) was not found.");
        }

        method.Invoke(null, [new InMemoryPreferences()]);
    }

    public static void Initialize()
    {
    }
}

public sealed class InMemoryPreferences : IPreferences
{
    private readonly Dictionary<string, Dictionary<string, object?>> _stores = new(StringComparer.Ordinal);

    public bool ContainsKey(string key)
    {
        return ContainsKey(key, null);
    }

    public void Remove(string key)
    {
        Remove(key, null);
    }

    public void Clear()
    {
        Clear(null);
    }

    public void Set<T>(string key, T value)
    {
        Set(key, value, null);
    }

    public T Get<T>(string key, T defaultValue)
    {
        return Get(key, defaultValue, null);
    }

    public bool ContainsKey(string key, string? sharedName = null)
    {
        return GetStore(sharedName).ContainsKey(key);
    }

    public void Remove(string key, string? sharedName = null)
    {
        GetStore(sharedName).Remove(key);
    }

    public void Clear(string? sharedName = null)
    {
        GetStore(sharedName).Clear();
    }

    public void Set<T>(string key, T value, string? sharedName = null)
    {
        GetStore(sharedName)[key] = value;
    }

    public T Get<T>(string key, T defaultValue, string? sharedName = null)
    {
        var store = GetStore(sharedName);
        if (store.TryGetValue(key, out var value) && value is T typed)
        {
            return typed;
        }

        return defaultValue;
    }

    private Dictionary<string, object?> GetStore(string? sharedName)
    {
        var name = sharedName ?? string.Empty;
        if (!_stores.TryGetValue(name, out var store))
        {
            store = new Dictionary<string, object?>(StringComparer.Ordinal);
            _stores[name] = store;
        }

        return store;
    }
}
