namespace LinkedLists.Tests;

public class SinglyLinkedListTests
{
    [Fact]
    public void NewList_IsEmpty()
    {
        var list = new SinglyLinkedList<int>();

        Assert.Equal(0, list.Count);
        Assert.Null(list.Head);
        Assert.Empty(list.ToList());
    }

    [Fact]
    public void AddLast_SingleElement_SetsHeadAndCount()
    {
        var list = new SinglyLinkedList<int>();

        list.AddLast(1);

        Assert.Equal(1, list.Count);
        Assert.NotNull(list.Head);
        Assert.Equal(1, list.Head!.Value);
        Assert.Equal(new List<int> { 1 }, list.ToList());
    }

    [Fact]
    public void AddLast_MultipleElements_PreservesOrder()
    {
        var list = new SinglyLinkedList<int>();

        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);

        Assert.Equal(3, list.Count);
        Assert.Equal(new List<int> { 1, 2, 3 }, list.ToList());
    }

    [Fact]
    public void AddFirst_MultipleElements_PrependsInReverseOrder()
    {
        var list = new SinglyLinkedList<int>();

        list.AddFirst(1);
        list.AddFirst(2);
        list.AddFirst(3);

        Assert.Equal(3, list.Count);
        Assert.Equal(new List<int> { 3, 2, 1 }, list.ToList());
    }

    [Fact]
    public void AddFirst_ThenAddLast_MixesCorrectly()
    {
        var list = new SinglyLinkedList<int>();

        list.AddFirst(2);
        list.AddFirst(1);
        list.AddLast(3);

        Assert.Equal(new List<int> { 1, 2, 3 }, list.ToList());
    }

    [Fact]
    public void Contains_AllowsDuplicates_AndFindsExistingValue()
    {
        var list = new SinglyLinkedList<int>();
        list.AddLast(5);
        list.AddLast(5);
        list.AddLast(7);

        Assert.True(list.Contains(5));
        Assert.True(list.Contains(7));
        Assert.Equal(3, list.Count);
        Assert.Equal(new List<int> { 5, 5, 7 }, list.ToList());
    }

    [Fact]
    public void Contains_MissingValue_ReturnsFalse()
    {
        var list = new SinglyLinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);

        Assert.False(list.Contains(99));
    }

    [Fact]
    public void Contains_EmptyList_ReturnsFalse()
    {
        var list = new SinglyLinkedList<int>();

        Assert.False(list.Contains(1));
    }

    [Fact]
    public void Remove_HeadElement_UpdatesHeadAndCount()
    {
        var list = new SinglyLinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);

        var removed = list.Remove(1);

        Assert.True(removed);
        Assert.Equal(2, list.Count);
        Assert.Equal(2, list.Head!.Value);
        Assert.Equal(new List<int> { 2, 3 }, list.ToList());
    }

    [Fact]
    public void Remove_MiddleElement_SplicesCorrectly()
    {
        var list = new SinglyLinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);

        var removed = list.Remove(2);

        Assert.True(removed);
        Assert.Equal(new List<int> { 1, 3 }, list.ToList());
    }

    [Fact]
    public void Remove_TailElement_RemovesLast()
    {
        var list = new SinglyLinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);
        list.AddLast(3);

        var removed = list.Remove(3);

        Assert.True(removed);
        Assert.Equal(new List<int> { 1, 2 }, list.ToList());
    }

    [Fact]
    public void Remove_OnlyFirstMatchingDuplicate()
    {
        var list = new SinglyLinkedList<int>();
        list.AddLast(5);
        list.AddLast(5);
        list.AddLast(7);

        var removed = list.Remove(5);

        Assert.True(removed);
        Assert.Equal(2, list.Count);
        Assert.Equal(new List<int> { 5, 7 }, list.ToList());
    }

    [Fact]
    public void Remove_MissingValue_ReturnsFalseAndLeavesListUnchanged()
    {
        var list = new SinglyLinkedList<int>();
        list.AddLast(1);
        list.AddLast(2);

        var removed = list.Remove(99);

        Assert.False(removed);
        Assert.Equal(2, list.Count);
        Assert.Equal(new List<int> { 1, 2 }, list.ToList());
    }

    [Fact]
    public void Remove_FromEmptyList_ReturnsFalse()
    {
        var list = new SinglyLinkedList<int>();

        Assert.False(list.Remove(1));
        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void Remove_OnlyElement_EmptiesList()
    {
        var list = new SinglyLinkedList<int>();
        list.AddLast(42);

        var removed = list.Remove(42);

        Assert.True(removed);
        Assert.Equal(0, list.Count);
        Assert.Null(list.Head);
        Assert.Empty(list.ToList());
    }

    [Fact]
    public void ToList_WorksWithStringType()
    {
        var list = new SinglyLinkedList<string>();
        list.AddLast("a");
        list.AddLast("b");

        Assert.Equal(new List<string> { "a", "b" }, list.ToList());
    }
}
