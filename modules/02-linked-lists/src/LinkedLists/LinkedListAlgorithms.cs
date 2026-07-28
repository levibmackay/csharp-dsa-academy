namespace LinkedLists;

/// <summary>
/// Classic singly linked list algorithms, operating directly on raw
/// <see cref="Node{T}"/> chains of <see cref="int"/> (not the
/// <see cref="SinglyLinkedList{T}"/> wrapper). See the module README.md
/// for full problem statements, examples, and hints.
/// </summary>
public static class LinkedListAlgorithms
{
    /// <summary>
    /// Reverse the list starting at <paramref name="head"/> and return the
    /// new head (what used to be the last node).
    /// Target complexity: O(n) time, O(1) extra space.
    /// </summary>
    public static Node<int>? Reverse(Node<int>? head)
    {
        // TODO: walk forward one node at a time, keeping a "previous"
        // pointer (starting at null). For each node, save its Next before
        // overwriting it to point backward at "previous", then advance
        // "previous" and the current pointer. The new head is the last
        // non-null node you visited.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return true if the list starting at <paramref name="head"/>
    /// contains a cycle (some node's Next eventually points back to an
    /// earlier node instead of ending in null).
    /// Target complexity: O(n) time, O(1) extra space (Floyd's
    /// tortoise-and-hare algorithm).
    /// </summary>
    public static bool HasCycle(Node<int>? head)
    {
        // TODO: use two pointers, "slow" (moves one step at a time) and
        // "fast" (moves two steps at a time), both starting at head. If
        // they ever point at the same node, there's a cycle. If "fast"
        // (or fast.Next) reaches null, there's no cycle.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the middle node of the list starting at <paramref name="head"/>.
    /// For an even-length list, return the SECOND of the two middle nodes.
    /// Target complexity: O(n) time, O(1) extra space (fast/slow pointers).
    /// </summary>
    public static Node<int>? FindMiddle(Node<int>? head)
    {
        // TODO: use two pointers, "slow" and "fast", both starting at
        // head. Advance slow by one node and fast by two nodes on each
        // step, until fast reaches the end (null) or has no next node.
        // slow will land on the correct middle node.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Merge two ascending sorted lists, <paramref name="a"/> and
    /// <paramref name="b"/>, into a single ascending sorted list and
    /// return its head. Reuse the existing nodes from both input lists
    /// (don't allocate any new Node&lt;int&gt; instances).
    /// Target complexity: O(n + m) time, O(1) extra space.
    /// </summary>
    public static Node<int>? MergeTwoSorted(Node<int>? a, Node<int>? b)
    {
        // TODO: use a dummy/sentinel node to simplify building the result
        // list, and a "tail" pointer that always points at the last node
        // appended so far. Repeatedly compare a.Value and b.Value, splice
        // the smaller node onto the tail, and advance that list's
        // pointer. When one list runs out, attach the remainder of the
        // other list directly (no need to walk it node by node).
        throw new NotImplementedException();
    }
}
