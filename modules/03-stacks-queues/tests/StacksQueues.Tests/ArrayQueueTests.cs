using StacksQueues;

namespace StacksQueues.Tests;

public class ArrayQueueTests
{
    [Fact]
    public void NewQueue_IsEmpty()
    {
        var queue = new ArrayQueue<int>();

        Assert.True(queue.IsEmpty);
        Assert.Equal(0, queue.Count);
    }

    [Fact]
    public void Enqueue_IncreasesCount_AndIsNotEmpty()
    {
        var queue = new ArrayQueue<int>();

        queue.Enqueue(1);

        Assert.False(queue.IsEmpty);
        Assert.Equal(1, queue.Count);
    }

    [Fact]
    public void Peek_ReturnsFrontItem_WithoutRemovingIt()
    {
        var queue = new ArrayQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);

        int front = queue.Peek();

        Assert.Equal(1, front);
        Assert.Equal(2, queue.Count);
    }

    [Fact]
    public void Dequeue_ReturnsItemsInFifoOrder()
    {
        var queue = new ArrayQueue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
        Assert.True(queue.IsEmpty);
    }

    [Fact]
    public void Dequeue_OnEmptyQueue_Throws()
    {
        var queue = new ArrayQueue<int>();

        Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
    }

    [Fact]
    public void Peek_OnEmptyQueue_Throws()
    {
        var queue = new ArrayQueue<int>();

        Assert.Throws<InvalidOperationException>(() => queue.Peek());
    }

    [Fact]
    public void Enqueue_BeyondInitialCapacity_ResizesAndPreservesOrder()
    {
        var queue = new ArrayQueue<int>();

        for (int i = 0; i < 100; i++)
        {
            queue.Enqueue(i);
        }

        Assert.Equal(100, queue.Count);

        for (int i = 0; i < 100; i++)
        {
            Assert.Equal(i, queue.Dequeue());
        }

        Assert.True(queue.IsEmpty);
    }

    [Fact]
    public void WrapAround_AfterPartialDequeue_KeepsCorrectOrder()
    {
        // This specifically exercises the circular-buffer index math: fill the
        // queue, drain some from the front, then enqueue more so the tail wraps
        // past the end of the backing array before a resize would occur.
        var queue = new ArrayQueue<int>();

        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        queue.Enqueue(4); // backing array (capacity 4) is now full

        Assert.Equal(1, queue.Dequeue());
        Assert.Equal(2, queue.Dequeue()); // _head is now 2, 2 free slots at the "front"

        queue.Enqueue(5); // tail wraps around to index 0
        queue.Enqueue(6); // tail wraps around to index 1

        Assert.Equal(4, queue.Count);
        Assert.Equal(3, queue.Dequeue());
        Assert.Equal(4, queue.Dequeue());
        Assert.Equal(5, queue.Dequeue());
        Assert.Equal(6, queue.Dequeue());
        Assert.True(queue.IsEmpty);
    }

    [Fact]
    public void EnqueueDequeueInterleaved_ThenResize_PreservesOrder()
    {
        var queue = new ArrayQueue<int>();

        queue.Enqueue(1);
        queue.Enqueue(2);
        Assert.Equal(1, queue.Dequeue());
        queue.Enqueue(3);
        queue.Enqueue(4);
        queue.Enqueue(5); // forces a resize while _head != 0

        Assert.Equal(2, queue.Dequeue());
        Assert.Equal(3, queue.Dequeue());
        Assert.Equal(4, queue.Dequeue());
        Assert.Equal(5, queue.Dequeue());
        Assert.True(queue.IsEmpty);
    }

    [Fact]
    public void WorksWithReferenceTypes()
    {
        var queue = new ArrayQueue<string>();
        queue.Enqueue("hello");

        Assert.Equal("hello", queue.Dequeue());
    }
}
