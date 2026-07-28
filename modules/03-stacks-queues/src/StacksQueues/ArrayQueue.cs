namespace StacksQueues;

/// <summary>
/// A generic, resizable, array-backed First-In-First-Out (FIFO) queue implemented
/// as a circular buffer. This is a from-scratch re-implementation of the ideas
/// behind System.Collections.Generic.Queue&lt;T&gt; — you will not use that BCL type in here.
/// </summary>
/// <typeparam name="T">The type of element stored in the queue.</typeparam>
public class ArrayQueue<T>
{
    private T[] _items;
    private int _head; // index of the current front element
    private int _count;

    /// <summary>
    /// Creates an empty queue with a small initial backing array.
    /// </summary>
    public ArrayQueue()
    {
        _items = new T[4];
        _head = 0;
        _count = 0;
    }

    /// <summary>
    /// The number of elements currently stored in the queue.
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// True when the queue contains no elements.
    /// </summary>
    public bool IsEmpty => _count == 0;

    /// <summary>
    /// Adds <paramref name="item"/> to the back of the queue.
    /// If the backing array is full, it must be resized (doubled) first, and the
    /// existing elements must be copied out in logical (front-to-back) order,
    /// starting fresh at index 0 with _head reset to 0.
    /// </summary>
    /// <param name="item">The item to enqueue.</param>
    public void Enqueue(T item)
    {
        // TODO: If _count == _items.Length, grow the backing array (e.g. double its
        // capacity) and copy the existing elements into the new array in logical
        // order starting at index 0 (see the README's circular buffer refresher),
        // resetting _head to 0.
        //
        // Then compute the tail index using circular (modulo) arithmetic:
        //   tail index = (_head + _count) % _items.Length
        // store the item there, and increment _count.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes and returns the item at the front of the queue.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
    public T Dequeue()
    {
        // TODO: If the queue is empty, throw new InvalidOperationException("Queue is empty.").
        // Otherwise grab the item at _head, advance _head using
        //   _head = (_head + 1) % _items.Length
        // decrement _count, and return the item.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the item at the front of the queue without removing it.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the queue is empty.</exception>
    public T Peek()
    {
        // TODO: If the queue is empty, throw new InvalidOperationException("Queue is empty.").
        // Otherwise return the item at _head, without modifying _head or _count.
        throw new NotImplementedException();
    }
}
