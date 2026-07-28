namespace LinkedLists;

/// <summary>
/// A single node in a singly linked list, holding one value of type
/// <typeparamref name="T"/> and a reference to the next node (or
/// <see langword="null"/> if this is the last node).
/// </summary>
public class Node<T>
{
    /// <summary>The value stored in this node.</summary>
    public T Value { get; set; }

    /// <summary>The next node in the list, or null if this is the last node.</summary>
    public Node<T>? Next { get; set; }

    public Node(T value)
    {
        Value = value;
        Next = null;
    }
}
