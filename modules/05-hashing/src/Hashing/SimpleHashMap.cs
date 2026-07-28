namespace Hashing;

/// <summary>
/// A simplified hash map built from scratch using separate chaining, so you
/// can see what <see cref="System.Collections.Generic.Dictionary{TKey,TValue}"/>
/// is doing internally. Keys are bucketed by <c>key.GetHashCode()</c> modulo
/// the current bucket-array length; each bucket is a
/// <see cref="List{T}"/> of <c>(TKey Key, TValue Value)</c> tuples holding
/// every entry that hashed into that bucket ("chaining" the collisions
/// together). When the load factor (Count / bucket count) exceeds ~0.75,
/// the bucket array doubles in size and every entry is rehashed into its
/// new bucket ("resizing"/"rehashing").
/// See the module README.md for full explanation, examples, and hints.
/// </summary>
/// <typeparam name="TKey">
/// Key type. Constrained to <c>notnull</c> because null keys can't be
/// hashed meaningfully here (the real Dictionary&lt;TKey,TValue&gt; has the
/// same constraint).
/// </typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
public class SimpleHashMap<TKey, TValue> where TKey : notnull
{
    // TODO: back this with an array of buckets, e.g.
    // private List<(TKey Key, TValue Value)>[] _buckets = new List<(TKey, TValue)>[initialCapacity];
    // and track _count separately (don't recompute it by walking every bucket).

    /// <summary>
    /// The number of key/value pairs currently stored.
    /// Target complexity: O(1) time, O(1) space.
    /// </summary>
    public int Count
    {
        get
        {
            // TODO: return a tracked count field, updated by Put/Remove.
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Insert <paramref name="key"/>/<paramref name="value"/>, or update the
    /// value if <paramref name="key"/> is already present ("upsert").
    /// After inserting, if the load factor (Count / bucket count) exceeds
    /// 0.75, double the bucket array and rehash every existing entry into
    /// its new bucket.
    /// Target complexity: O(1) average time (amortized, including
    /// occasional resizes), O(n) space overall.
    /// </summary>
    public void Put(TKey key, TValue value)
    {
        // TODO: compute the bucket index from key.GetHashCode() (mask off
        // the sign bit, then mod by the bucket array length). Scan that
        // bucket's list for an existing entry with an equal key (use
        // key.Equals(...) or EqualityComparer<TKey>.Default) and overwrite
        // its value if found; otherwise append a new (key, value) tuple.
        // Then check the load factor and resize if needed.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Try to fetch the value stored for <paramref name="key"/>.
    /// Returns true and sets <paramref name="value"/> if found; otherwise
    /// returns false and sets <paramref name="value"/> to <c>default</c>.
    /// Target complexity: O(1) average time, O(1) space.
    /// </summary>
    public bool TryGet(TKey key, out TValue value)
    {
        // TODO: compute the bucket index, scan the bucket's list for a
        // matching key, and return the associated value if found.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Remove <paramref name="key"/> and its value if present.
    /// Returns true if an entry was removed, false if the key was not
    /// found.
    /// Target complexity: O(1) average time, O(1) space.
    /// </summary>
    public bool Remove(TKey key)
    {
        // TODO: compute the bucket index, find the matching entry in the
        // bucket's list, remove it, and decrement the tracked count.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return true if <paramref name="key"/> is present.
    /// Target complexity: O(1) average time, O(1) space.
    /// </summary>
    public bool ContainsKey(TKey key)
    {
        // TODO: this can delegate to TryGet and discard the value.
        throw new NotImplementedException();
    }
}
