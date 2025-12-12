using System.Collections;
using System.Runtime.CompilerServices;

namespace ChoreoApp;

public sealed partial class MaterialDesignColorsDictionary: IDictionary<string, object>
{
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _baseDictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _baseDictionary.GetEnumerator();

    public void Add(KeyValuePair<string, object> item) => _baseDictionary.Add(item.Key, item.Value);

    public void Clear() => _baseDictionary.Clear();

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
        return collection.Remove(item);
    }

    public int Count => _baseDictionary.Count;

    public bool IsReadOnly => false;

    public void Add(string key, object value) => _baseDictionary.Add(key, value);

    public bool ContainsKey(string key)
    {
        if (key.EndsWith(BrushSuffix, StringComparison.InvariantCulture))
        {
            var k = key.TrimEnd(BrushSuffix).ToString();
            return _baseDictionary.ContainsKey(k);
        }

        return _baseDictionary.ContainsKey(key);
    }

    public bool Remove(string key) => _baseDictionary.Remove(key);

    [IndexerName("Item")]
    public object this[string key]
    {
        get => _baseDictionary[key];
        set => _baseDictionary[key] = value;
    }

    public ICollection<string> Keys => _baseDictionary.Keys;
    public ICollection<object> Values => _baseDictionary.Values;
}
