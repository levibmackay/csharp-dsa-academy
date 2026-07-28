// Reference solution — only read this after a real attempt.
using System;
using System.Collections.Generic;
using System.Linq;

namespace Heaps;

/// <summary>
/// A binary min-heap of <see cref="int"/> values, backed by a <see cref="List{T}"/>.
/// </summary>
public class MinHeap
{
    private readonly List<int> _items = new();

    public int Count => _items.Count;

    public bool IsEmpty => _items.Count == 0;

    public void Insert(int value)
    {
        _items.Add(value);
        SiftUp(_items.Count - 1);
    }

    public int ExtractMin()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Cannot extract from an empty heap.");
        }

        int min = _items[0];
        int lastIndex = _items.Count - 1;

        // Move the last element to the root, then shrink and restore the heap.
        _items[0] = _items[lastIndex];
        _items.RemoveAt(lastIndex);

        if (!IsEmpty)
        {
            SiftDown(0);
        }

        return min;
    }

    public int Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Cannot peek an empty heap.");
        }

        return _items[0];
    }

    private void SiftUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (_items[index] >= _items[parent])
            {
                break;
            }

            (_items[index], _items[parent]) = (_items[parent], _items[index]);
            index = parent;
        }
    }

    private void SiftDown(int index)
    {
        int count = _items.Count;
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int smallest = index;

            if (left < count && _items[left] < _items[smallest])
            {
                smallest = left;
            }

            if (right < count && _items[right] < _items[smallest])
            {
                smallest = right;
            }

            if (smallest == index)
            {
                break;
            }

            (_items[index], _items[smallest]) = (_items[smallest], _items[index]);
            index = smallest;
        }
    }
}

/// <summary>
/// Heap-flavored interview-style algorithm problems.
/// </summary>
public static class Problems
{
    // Choice: use System.Collections.Generic.PriorityQueue<TElement, TPriority>
    // (the BCL min-heap) here rather than the hand-rolled MinHeap, since it's
    // generic over element type and priority, which is convenient for these
    // problems. Either choice is fine — the point is understanding the mechanics.
    public static int FindKthLargest(int[] nums, int k)
    {
        // Keep a min-heap of size k of the largest-k-so-far values.
        // The root of that heap is the kth largest overall once it's full.
        var minHeap = new PriorityQueue<int, int>();

        foreach (int num in nums)
        {
            minHeap.Enqueue(num, num);
            if (minHeap.Count > k)
            {
                minHeap.Dequeue();
            }
        }

        return minHeap.Peek();
    }

    public static List<int> TopKFrequent(int[] nums, int k)
    {
        var counts = new Dictionary<int, int>();
        foreach (int num in nums)
        {
            counts[num] = counts.GetValueOrDefault(num) + 1;
        }

        // Min-heap keyed by frequency, capped at size k, so the smallest
        // frequency among the "top k so far" is always evictable first.
        var minHeap = new PriorityQueue<int, int>();

        foreach (var (value, frequency) in counts)
        {
            minHeap.Enqueue(value, frequency);
            if (minHeap.Count > k)
            {
                minHeap.Dequeue();
            }
        }

        return minHeap.UnorderedItems.Select(entry => entry.Element).ToList();
    }

    public static List<int> MergeKSortedLists(List<List<int>> lists)
    {
        // Min-heap of (value, listIndex, elementIndex) so we always pull the
        // globally smallest "next" candidate and know which list to advance.
        var minHeap = new PriorityQueue<(int Value, int ListIndex, int ElementIndex), int>();

        for (int listIndex = 0; listIndex < lists.Count; listIndex++)
        {
            if (lists[listIndex].Count > 0)
            {
                int firstValue = lists[listIndex][0];
                minHeap.Enqueue((firstValue, listIndex, 0), firstValue);
            }
        }

        var result = new List<int>();

        while (minHeap.Count > 0)
        {
            var (value, listIndex, elementIndex) = minHeap.Dequeue();
            result.Add(value);

            int nextElementIndex = elementIndex + 1;
            if (nextElementIndex < lists[listIndex].Count)
            {
                int nextValue = lists[listIndex][nextElementIndex];
                minHeap.Enqueue((nextValue, listIndex, nextElementIndex), nextValue);
            }
        }

        return result;
    }
}
