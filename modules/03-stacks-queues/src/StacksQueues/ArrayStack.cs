namespace StacksQueues;

/// <summary>
/// A generic, resizable, array-backed Last-In-First-Out (LIFO) stack.
/// This is a from-scratch re-implementation of the ideas behind
/// System.Collections.Generic.Stack&lt;T&gt; — you will not use that BCL type in here.
/// </summary>
/// <typeparam name="T">The type of element stored in the stack.</typeparam>
public class ArrayStack<T>
{
    private T[] _items;
    private int _count;

    /// <summary>
    /// Creates an empty stack with a small initial backing array.
    /// </summary>
    public ArrayStack()
    {
        _items = new T[4];
        _count = 0;
    }

    /// <summary>
    /// The number of elements currently stored in the stack.
    /// </summary>
    public int Count => _count;

    /// <summary>
    /// True when the stack contains no elements.
    /// </summary>
    public bool IsEmpty => _count == 0;

    /// <summary>
    /// Pushes <paramref name="item"/> onto the top of the stack.
    /// If the backing array is full, it must be resized (doubled) first.
    /// </summary>
    /// <param name="item">The item to push.</param>
    public void Push(T item)
    {
        // TODO: If _count == _items.Length, grow the backing array (e.g. double its capacity)
        // before storing the new item at index _count, then increment _count.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes and returns the item at the top of the stack.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
    public T Pop()
    {
        // TODO: If the stack is empty, throw new InvalidOperationException("Stack is empty.").
        // Otherwise decrement _count and return the item that was at the new top index.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the item at the top of the stack without removing it.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the stack is empty.</exception>
    public T Peek()
    {
        // TODO: If the stack is empty, throw new InvalidOperationException("Stack is empty.").
        // Otherwise return the item at the top, without modifying _count.
        throw new NotImplementedException();
    }
}
