using System.Collections;

namespace DotNetAtom.Collections;

public class HashtableDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    private readonly Hashtable _hashtable;
    private readonly TValue _defaultValue;

    public HashtableDictionary(Hashtable hashtable, TValue defaultValue)
    {
        _hashtable = hashtable;
        _defaultValue = defaultValue;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        foreach (DictionaryEntry entry in _hashtable)
        {
            if (entry is { Key: TKey key, Value: TValue value })
            {
                yield return new KeyValuePair<TKey, TValue>(key, value);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => _hashtable.Cast<DictionaryEntry>().Count(entry => entry is { Key: TKey, Value: TValue });

    public bool ContainsKey(TKey key)
    {
        return _hashtable.ContainsKey(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        if (_hashtable[key] is TValue result)
        {
            value = result;
            return true;
        }

        value = _defaultValue;
        return false;
    }

    public TValue this[TKey key] => _hashtable[key] is TValue value ? value : _defaultValue;

    public IEnumerable<TKey> Keys => _hashtable.Keys.OfType<TKey>();
    public IEnumerable<TValue> Values => _hashtable.Values.OfType<TValue>();
}
