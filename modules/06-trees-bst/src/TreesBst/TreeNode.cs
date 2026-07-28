namespace TreesBst;

/// <summary>
/// A single node in a binary tree. Holds a value and references to an optional
/// left and right child. This type is intentionally unconstrained — the ordering
/// rules that make a tree of these nodes a *binary search* tree live in
/// <see cref="BinarySearchTree{T}"/>, not here.
/// </summary>
/// <typeparam name="T">The type of value stored in the node.</typeparam>
public class TreeNode<T>
{
    /// <summary>
    /// The value stored at this node.
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// The left child, or null if this node has no left child.
    /// </summary>
    public TreeNode<T>? Left { get; set; }

    /// <summary>
    /// The right child, or null if this node has no right child.
    /// </summary>
    public TreeNode<T>? Right { get; set; }

    /// <summary>
    /// Creates a new node holding <paramref name="value"/>, optionally wiring up
    /// its left and right children directly.
    /// </summary>
    /// <param name="value">The value to store at this node.</param>
    /// <param name="left">The left child, if any.</param>
    /// <param name="right">The right child, if any.</param>
    public TreeNode(T value, TreeNode<T>? left = null, TreeNode<T>? right = null)
    {
        Value = value;
        Left = left;
        Right = right;
    }
}
