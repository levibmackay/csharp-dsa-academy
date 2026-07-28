using TreesBst;

namespace TreesBst.Tests;

public class TreeProblemsTests
{
    [Fact]
    public void IsValidBST_NullTree_ReturnsTrue()
    {
        Assert.True(TreeProblems.IsValidBST(null));
    }

    [Fact]
    public void IsValidBST_ValidTree_ReturnsTrue()
    {
        var root = new TreeNode<int>(10,
            left: new TreeNode<int>(5,
                left: new TreeNode<int>(2),
                right: new TreeNode<int>(7)),
            right: new TreeNode<int>(15,
                left: new TreeNode<int>(12),
                right: new TreeNode<int>(20)));

        Assert.True(TreeProblems.IsValidBST(root));
    }

    [Fact]
    public void IsValidBST_DeepViolationOfAncestorBound_ReturnsFalse()
    {
        // root 10, left child 5, and 5 has a right child of 15.
        // 15 > 5 satisfies the immediate parent check, but 15 lives in root's LEFT
        // subtree and must be < 10 — a naive immediate-parent-only check would
        // wrongly say this tree is valid.
        var root = new TreeNode<int>(10,
            left: new TreeNode<int>(5,
                right: new TreeNode<int>(15)),
            right: new TreeNode<int>(20));

        Assert.False(TreeProblems.IsValidBST(root));
    }

    [Fact]
    public void IsValidBST_SingleNode_ReturnsTrue()
    {
        var root = new TreeNode<int>(1);

        Assert.True(TreeProblems.IsValidBST(root));
    }

    [Fact]
    public void LevelOrderTraversal_NullTree_ReturnsEmptyList()
    {
        List<int> result = TreeProblems.LevelOrderTraversal(null);

        Assert.Empty(result);
    }

    [Fact]
    public void LevelOrderTraversal_SingleNode_ReturnsThatValue()
    {
        var root = new TreeNode<int>(42);

        List<int> result = TreeProblems.LevelOrderTraversal(root);

        Assert.Equal(new List<int> { 42 }, result);
    }

    [Fact]
    public void LevelOrderTraversal_MultipleLevels_VisitsLeftToRightPerLevel()
    {
        var root = new TreeNode<int>(1,
            left: new TreeNode<int>(2,
                left: new TreeNode<int>(4),
                right: new TreeNode<int>(5)),
            right: new TreeNode<int>(3,
                left: new TreeNode<int>(6),
                right: new TreeNode<int>(7)));

        List<int> result = TreeProblems.LevelOrderTraversal(root);

        Assert.Equal(new List<int> { 1, 2, 3, 4, 5, 6, 7 }, result);
    }

    [Fact]
    public void LevelOrderTraversal_UnevenTree_MissingChildrenHandledCorrectly()
    {
        var root = new TreeNode<int>(1,
            left: new TreeNode<int>(2,
                left: new TreeNode<int>(4)),
            right: new TreeNode<int>(3));

        List<int> result = TreeProblems.LevelOrderTraversal(root);

        Assert.Equal(new List<int> { 1, 2, 3, 4 }, result);
    }

    [Fact]
    public void LowestCommonAncestor_NormalCase_ReturnsInternalNode()
    {
        var root = new TreeNode<int>(6,
            left: new TreeNode<int>(2,
                left: new TreeNode<int>(0),
                right: new TreeNode<int>(4,
                    left: new TreeNode<int>(3),
                    right: new TreeNode<int>(5))),
            right: new TreeNode<int>(8,
                left: new TreeNode<int>(7),
                right: new TreeNode<int>(9)));

        TreeNode<int>? lca = TreeProblems.LowestCommonAncestor(root, 3, 5);

        Assert.NotNull(lca);
        Assert.Equal(4, lca!.Value);
    }

    [Fact]
    public void LowestCommonAncestor_OneNodeIsAncestorOfOther_ReturnsTheAncestor()
    {
        var root = new TreeNode<int>(6,
            left: new TreeNode<int>(2,
                left: new TreeNode<int>(0),
                right: new TreeNode<int>(4,
                    left: new TreeNode<int>(3),
                    right: new TreeNode<int>(5))),
            right: new TreeNode<int>(8,
                left: new TreeNode<int>(7),
                right: new TreeNode<int>(9)));

        TreeNode<int>? lca = TreeProblems.LowestCommonAncestor(root, 2, 4);

        Assert.NotNull(lca);
        Assert.Equal(2, lca!.Value);
    }

    [Fact]
    public void LowestCommonAncestor_OppositeSidesOfRoot_ReturnsRoot()
    {
        var root = new TreeNode<int>(6,
            left: new TreeNode<int>(2),
            right: new TreeNode<int>(8));

        TreeNode<int>? lca = TreeProblems.LowestCommonAncestor(root, 2, 8);

        Assert.NotNull(lca);
        Assert.Equal(6, lca!.Value);
    }

    [Fact]
    public void LowestCommonAncestor_SingleNodeTree_PAndQEqualRoot_ReturnsRoot()
    {
        var root = new TreeNode<int>(1);

        TreeNode<int>? lca = TreeProblems.LowestCommonAncestor(root, 1, 1);

        Assert.NotNull(lca);
        Assert.Equal(1, lca!.Value);
    }
}
