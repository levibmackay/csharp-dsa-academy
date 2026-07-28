// Reference solution — only read this after a real attempt.
//
// This file lives outside any .csproj on purpose: it is reference
// material only, not compiled as part of the module. If you want to
// run it, copy the method bodies into
// modules/05-hashing/src/Hashing/SimpleHashMap.cs and
// modules/05-hashing/src/Hashing/Problems.cs yourself.

namespace Hashing;

public class SimpleHashMap<TKey, TValue> where TKey : notnull
{
    private const int InitialCapacity = 8;
    private const double MaxLoadFactor = 0.75;

    private List<(TKey Key, TValue Value)>[] _buckets =
        new List<(TKey Key, TValue Value)>[InitialCapacity];

    private int _count;

    public int Count => _count;

    public void Put(TKey key, TValue value)
    {
        int index = BucketIndex(key, _buckets.Length);
        var bucket = _buckets[index] ??= new List<(TKey, TValue)>();

        for (int i = 0; i < bucket.Count; i++)
        {
            if (bucket[i].Key.Equals(key))
            {
                bucket[i] = (key, value);
                return;
            }
        }

        bucket.Add((key, value));
        _count++;

        if ((double)_count / _buckets.Length > MaxLoadFactor)
        {
            Resize();
        }
    }

    public bool TryGet(TKey key, out TValue value)
    {
        int index = BucketIndex(key, _buckets.Length);
        var bucket = _buckets[index];

        if (bucket is not null)
        {
            foreach (var entry in bucket)
            {
                if (entry.Key.Equals(key))
                {
                    value = entry.Value;
                    return true;
                }
            }
        }

        value = default!;
        return false;
    }

    public bool Remove(TKey key)
    {
        int index = BucketIndex(key, _buckets.Length);
        var bucket = _buckets[index];

        if (bucket is null)
        {
            return false;
        }

        for (int i = 0; i < bucket.Count; i++)
        {
            if (bucket[i].Key.Equals(key))
            {
                bucket.RemoveAt(i);
                _count--;
                return true;
            }
        }

        return false;
    }

    public bool ContainsKey(TKey key) => TryGet(key, out _);

    private static int BucketIndex(TKey key, int bucketCount)
    {
        // Mask off the sign bit so a negative hash code doesn't turn into a
        // negative array index.
        int hash = key.GetHashCode() & 0x7FFFFFFF;
        return hash % bucketCount;
    }

    private void Resize()
    {
        var oldBuckets = _buckets;
        _buckets = new List<(TKey Key, TValue Value)>[oldBuckets.Length * 2];
        _count = 0;

        foreach (var bucket in oldBuckets)
        {
            if (bucket is null)
            {
                continue;
            }

            foreach (var (key, value) in bucket)
            {
                Put(key, value);
            }
        }
    }
}

public static class Problems
{
    public static List<List<string>> GroupAnagrams(string[] strs)
    {
        var groups = new Dictionary<string, List<string>>();

        foreach (string s in strs)
        {
            char[] chars = s.ToCharArray();
            Array.Sort(chars);
            string key = new string(chars);

            if (!groups.TryGetValue(key, out List<string>? group))
            {
                group = new List<string>();
                groups[key] = group;
            }

            group.Add(s);
        }

        return groups.Values.ToList();
    }

    public static char? FirstNonRepeatingChar(string s)
    {
        var counts = new Dictionary<char, int>();

        foreach (char c in s)
        {
            counts[c] = counts.GetValueOrDefault(c) + 1;
        }

        foreach (char c in s)
        {
            if (counts[c] == 1)
            {
                return c;
            }
        }

        return null;
    }

    public static int[] TwoSumOptimal(int[] nums, int target)
    {
        var seen = new Dictionary<int, int>(); // value -> index

        for (int i = 0; i < nums.Length; i++)
        {
            int complement = target - nums[i];
            if (seen.TryGetValue(complement, out int complementIndex))
            {
                return new[] { complementIndex, i };
            }

            seen[nums[i]] = i;
        }

        throw new ArgumentException("No two sum solution exists for the given input.");
    }
}
