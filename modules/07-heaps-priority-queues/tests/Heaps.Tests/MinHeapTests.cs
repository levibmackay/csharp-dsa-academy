using System;
using System.Collections.Generic;
using Heaps;

namespace Heaps.Tests;

public class MinHeapTests
{
    [Fact]
    public void NewHeap_IsEmpty()
    {
        var heap = new MinHeap();

        Assert.True(heap.IsEmpty);
        Assert.Equal(0, heap.Count);
    }

    [Fact]
    public void Peek_OnEmptyHeap_Throws()
    {
        var heap = new MinHeap();

        Assert.Throws<InvalidOperationException>(() => heap.Peek());
    }

    [Fact]
    public void ExtractMin_OnEmptyHeap_Throws()
    {
        var heap = new MinHeap();

        Assert.Throws<InvalidOperationException>(() => heap.ExtractMin());
    }

    [Fact]
    public void Insert_SingleElement_PeekReturnsIt()
    {
        var heap = new MinHeap();

        heap.Insert(42);

        Assert.False(heap.IsEmpty);
        Assert.Equal(1, heap.Count);
        Assert.Equal(42, heap.Peek());
    }

    [Fact]
    public void ExtractMin_SingleElement_ReturnsItAndEmptiesHeap()
    {
        var heap = new MinHeap();
        heap.Insert(7);

        int result = heap.ExtractMin();

        Assert.Equal(7, result);
        Assert.True(heap.IsEmpty);
        Assert.Equal(0, heap.Count);
    }

    [Fact]
    public void ExtractMin_ReturnsElementsInAscendingOrder()
    {
        var heap = new MinHeap();
        int[] values = { 5, 3, 8, 1, 9, 2, 7 };
        foreach (int value in values)
        {
            heap.Insert(value);
        }

        var extracted = new List<int>();
        while (!heap.IsEmpty)
        {
            extracted.Add(heap.ExtractMin());
        }

        var expected = new List<int>(values);
        expected.Sort();
        Assert.Equal(expected, extracted);
    }

    [Fact]
    public void Insert_HandlesDuplicates()
    {
        var heap = new MinHeap();
        heap.Insert(4);
        heap.Insert(4);
        heap.Insert(2);
        heap.Insert(2);
        heap.Insert(1);

        var extracted = new List<int>();
        while (!heap.IsEmpty)
        {
            extracted.Add(heap.ExtractMin());
        }

        Assert.Equal(new List<int> { 1, 2, 2, 4, 4 }, extracted);
    }

    [Fact]
    public void Peek_DoesNotRemoveElement()
    {
        var heap = new MinHeap();
        heap.Insert(10);
        heap.Insert(5);

        int peeked = heap.Peek();

        Assert.Equal(5, peeked);
        Assert.Equal(2, heap.Count);
        Assert.Equal(5, heap.Peek());
    }

    [Fact]
    public void Insert_NegativeAndPositiveValues_MaintainsMinHeapProperty()
    {
        var heap = new MinHeap();
        int[] values = { 0, -5, 3, -10, 100, -1 };
        foreach (int value in values)
        {
            heap.Insert(value);
        }

        Assert.Equal(-10, heap.ExtractMin());
        Assert.Equal(-5, heap.ExtractMin());
        Assert.Equal(-1, heap.ExtractMin());
    }
}
