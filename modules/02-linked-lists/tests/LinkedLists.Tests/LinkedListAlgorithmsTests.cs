namespace LinkedLists.Tests;

public class LinkedListAlgorithmsTests
{
    // --- helpers -----------------------------------------------------

    private static Node<int>? BuildList(params int[] values)
    {
        Node<int>? head = null;
        Node<int>? tail = null;

        foreach (var value in values)
        {
            var node = new Node<int>(value);
            if (head is null)
            {
                head = node;
                tail = node;
            }
            else
            {
                tail!.Next = node;
                tail = node;
            }
        }

        return head;
    }

    private static List<int> ToValues(Node<int>? head)
    {
        var result = new List<int>();
        var current = head;
        while (current is not null)
        {
            result.Add(current.Value);
            current = current.Next;
        }
        return result;
    }

    // --- Reverse -------------------------------------------------------

    [Fact]
    public void Reverse_NullHead_ReturnsNull()
    {
        Assert.Null(LinkedListAlgorithms.Reverse(null));
    }

    [Fact]
    public void Reverse_SingleNode_ReturnsSameValue()
    {
        var head = BuildList(1);

        var result = LinkedListAlgorithms.Reverse(head);

        Assert.Equal(new List<int> { 1 }, ToValues(result));
    }

    [Fact]
    public void Reverse_MultipleNodes_ReversesOrder()
    {
        var head = BuildList(1, 2, 3, 4);

        var result = LinkedListAlgorithms.Reverse(head);

        Assert.Equal(new List<int> { 4, 3, 2, 1 }, ToValues(result));
    }

    // --- HasCycle --------------------------------------------------------

    [Fact]
    public void HasCycle_NullHead_ReturnsFalse()
    {
        Assert.False(LinkedListAlgorithms.HasCycle(null));
    }

    [Fact]
    public void HasCycle_SingleNodeNoCycle_ReturnsFalse()
    {
        var head = BuildList(1);

        Assert.False(LinkedListAlgorithms.HasCycle(head));
    }

    [Fact]
    public void HasCycle_SingleNodeSelfCycle_ReturnsTrue()
    {
        var head = new Node<int>(1);
        head.Next = head;

        Assert.True(LinkedListAlgorithms.HasCycle(head));
    }

    [Fact]
    public void HasCycle_NoCycle_ReturnsFalse()
    {
        var head = BuildList(1, 2, 3, 4);

        Assert.False(LinkedListAlgorithms.HasCycle(head));
    }

    [Fact]
    public void HasCycle_CycleBackToHead_ReturnsTrue()
    {
        var head = BuildList(1, 2, 3, 4)!;
        var current = head;
        while (current.Next is not null)
        {
            current = current.Next;
        }
        current.Next = head; // tail points back to head

        Assert.True(LinkedListAlgorithms.HasCycle(head));
    }

    [Fact]
    public void HasCycle_CycleIntoMiddle_ReturnsTrue()
    {
        var n1 = new Node<int>(1);
        var n2 = new Node<int>(2);
        var n3 = new Node<int>(3);
        var n4 = new Node<int>(4);
        n1.Next = n2;
        n2.Next = n3;
        n3.Next = n4;
        n4.Next = n2; // cycle back into the middle (not the head)

        Assert.True(LinkedListAlgorithms.HasCycle(n1));
    }

    // --- FindMiddle ------------------------------------------------------

    [Fact]
    public void FindMiddle_NullHead_ReturnsNull()
    {
        Assert.Null(LinkedListAlgorithms.FindMiddle(null));
    }

    [Fact]
    public void FindMiddle_SingleNode_ReturnsThatNode()
    {
        var head = BuildList(1);

        var result = LinkedListAlgorithms.FindMiddle(head);

        Assert.NotNull(result);
        Assert.Equal(1, result!.Value);
    }

    [Fact]
    public void FindMiddle_OddLength_ReturnsExactMiddle()
    {
        var head = BuildList(1, 2, 3, 4, 5);

        var result = LinkedListAlgorithms.FindMiddle(head);

        Assert.NotNull(result);
        Assert.Equal(3, result!.Value);
    }

    [Fact]
    public void FindMiddle_EvenLength_ReturnsSecondMiddleNode()
    {
        var head = BuildList(1, 2, 3, 4);

        var result = LinkedListAlgorithms.FindMiddle(head);

        Assert.NotNull(result);
        Assert.Equal(3, result!.Value);
    }

    [Fact]
    public void FindMiddle_TwoNodes_ReturnsSecondNode()
    {
        var head = BuildList(1, 2);

        var result = LinkedListAlgorithms.FindMiddle(head);

        Assert.NotNull(result);
        Assert.Equal(2, result!.Value);
    }

    // --- MergeTwoSorted ----------------------------------------------------

    [Fact]
    public void MergeTwoSorted_BothNull_ReturnsNull()
    {
        Assert.Null(LinkedListAlgorithms.MergeTwoSorted(null, null));
    }

    [Fact]
    public void MergeTwoSorted_FirstEmpty_ReturnsSecond()
    {
        var b = BuildList(1, 3, 5);

        var result = LinkedListAlgorithms.MergeTwoSorted(null, b);

        Assert.Equal(new List<int> { 1, 3, 5 }, ToValues(result));
    }

    [Fact]
    public void MergeTwoSorted_SecondEmpty_ReturnsFirst()
    {
        var a = BuildList(2, 4, 6);

        var result = LinkedListAlgorithms.MergeTwoSorted(a, null);

        Assert.Equal(new List<int> { 2, 4, 6 }, ToValues(result));
    }

    [Fact]
    public void MergeTwoSorted_InterleavedValues_MergesInOrder()
    {
        var a = BuildList(1, 3, 5);
        var b = BuildList(2, 4, 6);

        var result = LinkedListAlgorithms.MergeTwoSorted(a, b);

        Assert.Equal(new List<int> { 1, 2, 3, 4, 5, 6 }, ToValues(result));
    }

    [Fact]
    public void MergeTwoSorted_WithNegativesAndDuplicates_MergesInOrder()
    {
        var a = BuildList(-5, -1, 0, 4);
        var b = BuildList(-3, -1, 2, 4);

        var result = LinkedListAlgorithms.MergeTwoSorted(a, b);

        Assert.Equal(
            new List<int> { -5, -3, -1, -1, 0, 2, 4, 4 },
            ToValues(result));
    }

    [Fact]
    public void MergeTwoSorted_DifferentLengths_AppendsRemainder()
    {
        var a = BuildList(1);
        var b = BuildList(0, 2, 3, 4);

        var result = LinkedListAlgorithms.MergeTwoSorted(a, b);

        Assert.Equal(new List<int> { 0, 1, 2, 3, 4 }, ToValues(result));
    }
}
