namespace TreesBst;

/// <summary>
/// Classic algorithm problems built on top of plain <see cref="TreeNode{T}"/> trees
/// of ints (no BST wrapper required — these operate directly on node references).
/// </summary>
public static class TreeProblems
{
    /// <summary>
    /// Determines whether the tree rooted at <paramref name="root"/> is a valid
    /// binary search tree: for every node, ALL values in its left subtree must be
    /// strictly less than the node's value, and ALL values in its right subtree
    /// must be strictly greater — not just the node's immediate children.
    /// </summary>
    /// <param name="root">The root of the tree to validate, or null for an empty tree.</param>
    /// <returns>True if the tree satisfies the BST property everywhere; an empty
    /// tree (null root) is considered valid.</returns>
    public static bool IsValidBST(TreeNode<int>? root)
    {
        // TODO: Classic gotcha — checking only "node.Value > node.Left.Value" and
        // "node.Value < node.Right.Value" at each node is WRONG, because a deep
        // descendant could violate an ancestor's bound while still satisfying its
        // immediate parent's. Example: root 10 with left child 5, and 5 has a right
        // child 15 — 15 > 5 (fine locally) but 15 > 10 violates the whole-tree
        // property, since 15 lives in root's LEFT subtree and must be < 10.
        //
        // Fix: track a valid (min, max) RANGE that shrinks as you recurse, using a
        // private helper like
        //   bool IsValidBstHelper(TreeNode<int>? node, long? min, long? max)
        // where min/max are exclusive bounds (node.Value must be > min and < max).
        // Using long? (instead of int?) for the bounds avoids an edge case: if a
        // node's value is int.MinValue or int.MaxValue, tightening an int bound past
        // that would overflow; long has headroom on both sides so the comparisons
        // stay correct.
        //   - null node -> true (empty subtree is trivially valid)
        //   - if min.HasValue && node.Value <= min.Value -> false
        //   - if max.HasValue && node.Value >= max.Value -> false
        //   - return IsValidBstHelper(node.Left, min, node.Value)
        //          && IsValidBstHelper(node.Right, node.Value, max)
        throw new NotImplementedException();
    }

    /// <summary>
    /// Performs a breadth-first (level-order) traversal of the tree, visiting all
    /// nodes at depth 0, then depth 1, then depth 2, etc., left to right within
    /// each level.
    /// </summary>
    /// <param name="root">The root of the tree, or null for an empty tree.</param>
    /// <returns>The values in level order. Empty list for an empty tree.</returns>
    public static List<int> LevelOrderTraversal(TreeNode<int>? root)
    {
        // TODO: Use a System.Collections.Generic.Queue<TreeNode<int>> (the real BCL
        // queue is fine here). If root is null, return an empty list immediately.
        // Otherwise: Enqueue root, then loop while the queue is non-empty:
        //   - Dequeue a node, add its Value to the results list
        //   - if node.Left is not null, Enqueue it
        //   - if node.Right is not null, Enqueue it
        // Return the results list once the queue is empty.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Finds the lowest common ancestor (LCA) of the nodes with values
    /// <paramref name="p"/> and <paramref name="q"/> in a BINARY SEARCH TREE.
    /// This is simpler than the general-tree LCA problem: because of the BST
    /// ordering property, you can navigate directly toward the answer using
    /// comparisons instead of exploring both subtrees and merging results.
    /// </summary>
    /// <param name="root">The root of the BST. Assumed non-null for a valid call,
    /// but handled defensively.</param>
    /// <param name="p">The value of the first node. Assumed to exist in the tree.</param>
    /// <param name="q">The value of the second node. Assumed to exist in the tree.</param>
    /// <returns>The node whose value is the lowest common ancestor of p and q, or
    /// null if root is null.</returns>
    public static TreeNode<int>? LowestCommonAncestor(TreeNode<int>? root, int p, int q)
    {
        // TODO: Defensive: if root is null, return null.
        // Otherwise walk from root:
        //   - if BOTH p and q are less than root.Value, the LCA must be in the left
        //     subtree -> move root to root.Left and continue
        //   - if BOTH p and q are greater than root.Value, the LCA must be in the
        //     right subtree -> move root to root.Right and continue
        //   - otherwise (p and q are on opposite sides of root, or one of them
        //     equals root.Value), root itself is the split point -> return root
        // Recursive or iterative (a simple while loop) both work well here.
        throw new NotImplementedException();
    }
}
