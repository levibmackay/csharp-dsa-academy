using TreesBst;

namespace TreesBst.Tests;

public class BinarySearchTreeTests
{
    [Fact]
    public void NewTree_RootIsNull()
    {
        var tree = new BinarySearchTree<int>();

        Assert.Null(tree.Root);
    }

    [Fact]
    public void Insert_SingleValue_RootHoldsThatValue()
    {
        var tree = new BinarySearchTree<int>();

        tree.Insert(5);

        Assert.NotNull(tree.Root);
        Assert.Equal(5, tree.Root!.Value);
        Assert.Null(tree.Root.Left);
        Assert.Null(tree.Root.Right);
    }

    [Fact]
    public void Insert_MultipleValues_InOrderTraversalIsSorted()
    {
        var tree = new BinarySearchTree<int>();
        int[] values = { 5, 3, 8, 1, 4, 7, 9 };

        foreach (int v in values)
        {
            tree.Insert(v);
        }

        List<int> result = tree.InOrderTraversal();

        Assert.Equal(new List<int> { 1, 3, 4, 5, 7, 8, 9 }, result);
    }

    [Fact]
    public void InOrderTraversal_OnEmptyTree_ReturnsEmptyList()
    {
        var tree = new BinarySearchTree<int>();

        List<int> result = tree.InOrderTraversal();

        Assert.Empty(result);
    }

    [Fact]
    public void Insert_DuplicateValue_GoesToRightSubtreeAndIsKeptInTraversal()
    {
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(5);

        Assert.NotNull(tree.Root);
        Assert.NotNull(tree.Root!.Right);
        Assert.Equal(5, tree.Root.Right!.Value);
        Assert.Null(tree.Root.Left);

        List<int> result = tree.InOrderTraversal();
        Assert.Equal(new List<int> { 5, 5 }, result);
    }

    [Fact]
    public void Contains_FindsInsertedValue_IncludingDuplicates()
    {
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(8);
        tree.Insert(5);

        Assert.True(tree.Contains(5));
        Assert.True(tree.Contains(3));
        Assert.True(tree.Contains(8));
    }

    [Fact]
    public void Contains_ValueNotInTree_ReturnsFalse()
    {
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(8);

        Assert.False(tree.Contains(100));
    }

    [Fact]
    public void Contains_OnEmptyTree_ReturnsFalse()
    {
        var tree = new BinarySearchTree<int>();

        Assert.False(tree.Contains(1));
    }

    [Fact]
    public void Delete_OnEmptyTree_ReturnsFalse_NoException()
    {
        var tree = new BinarySearchTree<int>();

        bool removed = tree.Delete(5);

        Assert.False(removed);
        Assert.Null(tree.Root);
    }

    [Fact]
    public void Delete_ValueNotPresent_ReturnsFalse_TreeUnchanged()
    {
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(8);

        bool removed = tree.Delete(100);

        Assert.False(removed);
        Assert.Equal(new List<int> { 3, 5, 8 }, tree.InOrderTraversal());
    }

    [Fact]
    public void Delete_LeafNode_RemovesItAndReturnsTrue()
    {
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(8);

        bool removed = tree.Delete(3);

        Assert.True(removed);
        Assert.False(tree.Contains(3));
        Assert.Equal(new List<int> { 5, 8 }, tree.InOrderTraversal());
    }

    [Fact]
    public void Delete_NodeWithOnlyLeftChild_SplicesChildUp()
    {
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(2);

        bool removed = tree.Delete(3);

        Assert.True(removed);
        Assert.False(tree.Contains(3));
        Assert.Equal(new List<int> { 2, 5 }, tree.InOrderTraversal());
    }

    [Fact]
    public void Delete_NodeWithOnlyRightChild_SplicesChildUp()
    {
        var tree = new BinarySearchTree<int>();
        tree.Insert(5);
        tree.Insert(3);
        tree.Insert(4);

        bool removed = tree.Delete(3);

        Assert.True(removed);
        Assert.False(tree.Contains(3));
        Assert.Equal(new List<int> { 4, 5 }, tree.InOrderTraversal());
    }

    [Fact]
    public void Delete_NodeWithTwoChildren_UsesInOrderSuccessor_TraversalStaysSorted()
    {
        var tree = new BinarySearchTree<int>();
        int[] values = { 5, 3, 8, 1, 4, 7, 9 };
        foreach (int v in values)
        {
            tree.Insert(v);
        }

        bool removed = tree.Delete(5);

        Assert.True(removed);
        Assert.False(tree.Contains(5));

        List<int> result = tree.InOrderTraversal();
        Assert.Equal(new List<int> { 1, 3, 4, 7, 8, 9 }, result);

        var sorted = new List<int>(result);
        sorted.Sort();
        Assert.Equal(sorted, result);
    }

    [Fact]
    public void Delete_Root_TreeStillValidAfterward()
    {
        var tree = new BinarySearchTree<int>();
        tree.Insert(10);
        tree.Insert(5);
        tree.Insert(15);

        bool removed = tree.Delete(10);

        Assert.True(removed);
        Assert.False(tree.Contains(10));
        Assert.Equal(new List<int> { 5, 15 }, tree.InOrderTraversal());
    }

    [Fact]
    public void Root_ExposesActualRootNode()
    {
        var tree = new BinarySearchTree<int>();
        tree.Insert(42);

        Assert.NotNull(tree.Root);
        Assert.Equal(42, tree.Root!.Value);
    }
}
