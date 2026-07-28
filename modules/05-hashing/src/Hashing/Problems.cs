namespace Hashing;

/// <summary>
/// Module 5: Hashing.
/// Implement each method below. Replace the TODO and the
/// <see cref="NotImplementedException"/> with your own working code.
///
/// These problems use the built-in <see cref="Dictionary{TKey,TValue}"/>
/// rather than <see cref="SimpleHashMap{TKey,TValue}"/> — in real code you
/// should always reach for the framework's Dictionary, which is more
/// battle-tested and faster than anything we'd hand-roll here.
/// SimpleHashMap above exists purely so you understand what Dictionary is
/// doing under the hood; these problems are where you put that
/// understanding to use.
/// See the module README.md for full problem statements, examples, and
/// hints.
/// </summary>
public static class Problems
{
    /// <summary>
    /// Group the strings in <paramref name="strs"/> so that all anagrams of
    /// each other end up in the same inner list. The order of the groups in
    /// the outer list, and the order of strings within each group, does not
    /// matter.
    /// Target complexity: O(n * k log k) time (n strings, average length k,
    /// from sorting each string as its grouping key), O(n * k) space.
    /// </summary>
    public static List<List<string>> GroupAnagrams(string[] strs)
    {
        // TODO: use a Dictionary<string, List<string>> keyed by each
        // string's sorted-character form (e.g. "eat" -> "aet"). Strings
        // that are anagrams of each other produce the same sorted key, so
        // append each string to the list stored under its key. Return the
        // dictionary's values as a List<List<string>>.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the first character in <paramref name="s"/> that appears
    /// exactly once (reading left to right), or <c>null</c> if every
    /// character repeats (or <paramref name="s"/> is empty).
    /// Target complexity: O(n) time, O(1) space (fixed alphabet) or O(n)
    /// space for a general-purpose dictionary.
    /// </summary>
    public static char? FirstNonRepeatingChar(string s)
    {
        // TODO: first pass — build a Dictionary<char, int> (or fixed-size
        // count array) of character counts. Second pass — walk the string
        // in order and return the first character whose count is 1. If
        // none qualifies, return null.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Given an array of integers <paramref name="nums"/> and an integer
    /// <paramref name="target"/>, return the indices of the two numbers
    /// that add up to <paramref name="target"/>, using a single hash-map
    /// pass (as opposed to module 1's TwoSum, which is the same problem —
    /// this is the hashing-focused revisit). You may assume you may not
    /// use the same element twice.
    /// If no two numbers sum to <paramref name="target"/>, throw an
    /// <see cref="ArgumentException"/> — do not return a sentinel array.
    /// Target complexity: O(n) time, O(n) space.
    /// </summary>
    public static int[] TwoSumOptimal(int[] nums, int target)
    {
        // TODO: use a Dictionary<int,int> mapping value -> index. Scan
        // once; for each nums[i], check whether (target - nums[i]) is
        // already a key in the map. If so, return the two indices. If not,
        // record nums[i] -> i and keep going. If the loop finishes with no
        // match, throw an ArgumentException.
        throw new NotImplementedException();
    }
}
