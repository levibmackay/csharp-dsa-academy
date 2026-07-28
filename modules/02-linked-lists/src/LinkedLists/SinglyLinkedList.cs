namespace LinkedLists;

/// <summary>
/// A generic singly linked list wrapper. Keeps track of the head node and
/// the number of elements, and exposes the usual add/remove/search
/// operations. See the module README.md for full method descriptions,
/// examples, and hints.
/// </summary>
public class SinglyLinkedList<T>
{
    /// <summary>The first node in the list, or null if the list is empty.</summary>
    public Node<T>? Head { get; private set; }

    /// <summary>The number of elements currently in the list.</summary>
    public int Count { get; private set; }

    /// <summary>
    /// Add <paramref name="value"/> to the end of the list.
    /// Target complexity: O(n) time (no tail pointer), O(1) space.
    /// </summary>
    public void AddLast(T value)
    {
        // TODO: create a new Node<T>. If the list is empty, it becomes
        // Head. Otherwise walk from Head to the last node (the one whose
        // Next is null) and attach the new node there. Don't forget to
        // increment Count.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Add <paramref name="value"/> to the front of the list.
    /// Target complexity: O(1) time, O(1) space.
    /// </summary>
    public void AddFirst(T value)
    {
        // TODO: create a new Node<T> whose Next points at the current
        // Head, then make it the new Head. Don't forget to increment
        // Count.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Remove the first node whose Value equals <paramref name="value"/>.
    /// Returns true if a node was found and removed, false otherwise.
    /// Target complexity: O(n) time, O(1) space.
    /// </summary>
    public bool Remove(T value)
    {
        // TODO: walk the list keeping a "previous" pointer. If the match
        // is Head, just move Head to Head.Next. Otherwise splice it out
        // by pointing previous.Next at the matched node's Next. Remember
        // to decrement Count and return true only if something was
        // actually removed.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return true if any node's Value equals <paramref name="value"/>.
    /// Target complexity: O(n) time, O(1) space.
    /// </summary>
    public bool Contains(T value)
    {
        // TODO: walk from Head, comparing each node's Value using
        // EqualityComparer<T>.Default.Equals(...) (works even when T is
        // a reference type or could be null).
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return a new List&lt;T&gt; containing the values in this linked
    /// list, in order from Head to the end.
    /// Target complexity: O(n) time, O(n) space.
    /// </summary>
    public List<T> ToList()
    {
        // TODO: walk from Head, appending each node's Value to a new
        // List<T>.
        throw new NotImplementedException();
    }
}
