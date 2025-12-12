using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ChoreoApp;

public sealed partial class MaterialDesignColorsDictionary : IDictionary<string, object>
{
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _baseDictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _baseDictionary.GetEnumerator();

    public void Add(KeyValuePair<string, object> item) => _baseDictionary.Add(item.Key, item.Value);

    public void Clear()
    {
        _baseDictionary.Clear();
        _brushesByColor.Clear();
        _colorKeyToColor.Clear();
    }

    bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
    {
        ICollection<KeyValuePair<string, object>> collection = _baseDictionary;
        return collection.Contains(item);
    }

    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
        ICollection<KeyValuePair<string, object>> collection = _baseDictionary;
        collection.CopyTo(array, arrayIndex);
    }

    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
    {
        ICollection<KeyValuePair<string, object>> collection = _baseDictionary;
        var removed = collection.Remove(item);
        if (removed)
        {
            RemoveCacheEntry(item.Key);
        }

        return removed;
    }

    public int Count => _baseDictionary.Count;

    public bool IsReadOnly => false;

    public void Add(string key, object value) => _baseDictionary.Add(key, value);

    public bool ContainsKey(string key)
    {
        if (key.EndsWith(BrushSuffix, StringComparison.Ordinal))
        {
            var colorKey = key[..^BrushSuffix.Length];
            return _baseDictionary.ContainsKey(colorKey);
        }

        return _baseDictionary.ContainsKey(key);
    }

    public bool Remove(string key)
    {
        if (key.EndsWith(BrushSuffix, StringComparison.Ordinal))
        {
            var colorKey = key[..^BrushSuffix.Length];
            var removedBrush = RemoveCacheEntry(colorKey);
            var removedColor = _baseDictionary.Remove(colorKey);
            return removedBrush || removedColor;
        }

        var removed = _baseDictionary.Remove(key);
        if (removed)
        {
            RemoveCacheEntry(key);
        }

        return removed;
    }

    [IndexerName("Item")]
    public object this[string key]
    {
        get => _baseDictionary[key];
        set => _baseDictionary[key] = value;
    }

    public ICollection<string> Keys => _baseDictionary.Keys;
    public ICollection<object> Values => _baseDictionary.Values;

    bool IDictionary<string, object>.TryGetValue(string key, out object value) => TryGetValue(key, out value);
}
