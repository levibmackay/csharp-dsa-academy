using System;
using System.Collections.Generic;

namespace Heaps;

/// <summary>
/// Heap-flavored interview-style algorithm problems. Each method is static and
/// self-contained; see the README's "Problems" section for full descriptions,
/// examples, and complexity targets.
/// </summary>
public static class Problems
{
    /// <summary>
    /// Returns the kth largest element in an unsorted array (k=1 is the largest).
    /// TODO: use a heap of size k to avoid a full O(n log n) sort. You may use
    /// your own MinHeap or System.Collections.Generic.PriorityQueue&lt;TElement,TPriority&gt;
    /// (note your choice in a comment above your implementation).
    /// </summary>
    public static int FindKthLargest(int[] nums, int k)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the k most frequent elements in nums. Order of the returned list
    /// does not matter.
    /// TODO: count occurrences with a Dictionary&lt;int, int&gt;, then use a heap
    /// keyed on frequency to select the top k keys.
    /// </summary>
    public static List<int> TopKFrequent(int[] nums, int k)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Merges k already-sorted (ascending) lists of ints into a single sorted list.
    /// TODO: use a heap to repeatedly pull the smallest "next" value across all
    /// lists. Track, for each list, which index you've consumed up to.
    /// </summary>
    public static List<int> MergeKSortedLists(List<List<int>> lists)
    {
        throw new NotImplementedException();
    }
}
