using System;
using System.Collections.Generic;

namespace Heaps;

/// <summary>
/// A binary min-heap of <see cref="int"/> values, backed by a <see cref="List{T}"/>.
/// The smallest value is always at the root (index 0) and can be read/removed in
/// O(log n) time. See the README's "C# syntax you'll need" section for the index
/// math (children of i are 2i+1 and 2i+2; parent of i is (i-1)/2) before you start.
/// </summary>
public class MinHeap
{
    private readonly List<int> _items = new();

    /// <summary>Number of elements currently in the heap.</summary>
    public int Count => _items.Count;

    /// <summary>True when the heap has no elements.</summary>
    public bool IsEmpty => _items.Count == 0;

    /// <summary>
    /// Inserts a value into the heap, maintaining the min-heap property.
    /// TODO: append the value to the backing list, then "sift up" (a.k.a.
    /// "bubble up") by repeatedly swapping with its parent while it is
    /// smaller than its parent.
    /// </summary>
    public void Insert(int value)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes and returns the smallest value in the heap.
    /// TODO: save the root, move the last element to the root, shrink the
    /// list by one, then "sift down" (a.k.a. "bubble down" / "heapify down")
    /// by repeatedly swapping with the smaller child until the heap property
    /// is restored. Throw InvalidOperationException if the heap is empty.
    /// </summary>
    public int ExtractMin()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns (without removing) the smallest value in the heap.
    /// TODO: throw InvalidOperationException if the heap is empty, otherwise
    /// return the root element.
    /// </summary>
    public int Peek()
    {
        throw new NotImplementedException();
    }
}
