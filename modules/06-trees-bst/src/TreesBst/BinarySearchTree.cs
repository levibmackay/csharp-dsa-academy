namespace TreesBst;

/// <summary>
/// A binary search tree (BST): every node's value is greater than every value in
/// its left subtree and less than every value in its right subtree.
/// </summary>
/// <remarks>
/// Duplicate policy: inserting a value that already exists in the tree places the
/// duplicate into the RIGHT subtree of the equal node (i.e. "equal goes right").
/// This means <see cref="InOrderTraversal"/> keeps duplicates (sorted, adjacent),
/// and <see cref="Contains"/> still finds them.
/// </remarks>
/// <typeparam name="T">
/// The type of value stored in the tree. Must implement <see cref="IComparable{T}"/>
/// so nodes can be ordered relative to one another.
/// </typeparam>
public class BinarySearchTree<T> where T : IComparable<T>
{
    /// <summary>
    /// The root node of the tree, or null if the tree is empty.
    /// </summary>
    public TreeNode<T>? Root { get; private set; }

    /// <summary>
    /// Inserts <paramref name="value"/> into the tree, preserving the BST ordering
    /// property. Duplicates are inserted into the right subtree of the equal node
    /// (see the duplicate policy documented on this class).
    /// </summary>
    /// <param name="value">The value to insert.</param>
    public void Insert(T value)
    {
        // TODO: Call a private recursive helper like
        //   TreeNode<T>? InsertHelper(TreeNode<T>? node, T value)
        // that returns the (possibly new) subtree root:
        //   - if node is null, create and return a new TreeNode<T>(value)
        //   - compare value.CompareTo(node.Value):
        //       - negative -> recurse into node.Left, reassign node.Left to the result
        //       - zero or positive -> recurse into node.Right (this is the "equal goes
        //         right" duplicate policy), reassign node.Right to the result
        //   - return node
        // Assign Root = InsertHelper(Root, value).
        throw new NotImplementedException();
    }

    /// <summary>
    /// Determines whether <paramref name="value"/> exists anywhere in the tree.
    /// </summary>
    /// <param name="value">The value to search for.</param>
    /// <returns>True if the value is present; otherwise false.</returns>
    public bool Contains(T value)
    {
        // TODO: Walk from Root, using value.CompareTo(current.Value) at each step to
        // decide whether to go left (negative), right (positive), or stop (zero ->
        // found it, return true). If you fall off the tree (current becomes null),
        // the value isn't present, return false. Recursive or iterative both work.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes <paramref name="value"/> from the tree if present.
    /// </summary>
    /// <param name="value">The value to remove.</param>
    /// <returns>True if the value was found and removed; false if it was not present
    /// (including when the tree is empty), in which case the tree is left unchanged.</returns>
    public bool Delete(T value)
    {
        // TODO: This is the trickiest BST operation. Use a private recursive helper like
        //   TreeNode<T>? DeleteHelper(TreeNode<T>? node, T value, ref bool removed)
        // (or track "was it found" some other way, e.g. check Contains(value) first)
        // that returns the (possibly new) subtree root after deleting value from it:
        //   - if node is null, value isn't here -> return null (nothing to do)
        //   - compare value.CompareTo(node.Value) and recurse left/right as in Insert
        //     until you find the node to delete (comparison == 0)
        //   - once found, handle three cases:
        //       1. Leaf (no children): just return null, detaching it from its parent.
        //       2. One child: return whichever child is non-null, splicing it up into
        //          the deleted node's place.
        //       3. Two children: find the in-order successor (the smallest value in
        //          the right subtree — keep going Left from node.Right until Left is
        //          null), copy that successor's value into the current node, then
        //          recursively delete the successor's value from node.Right (it's
        //          guaranteed to be a leaf-or-one-child case now, so this recursion
        //          terminates cleanly).
        // Assign Root = DeleteHelper(Root, value, ...) and return whether it was found.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns every value in the tree, in ascending sorted order.
    /// </summary>
    /// <returns>A list of values produced by an in-order (left, node, right) walk.</returns>
    public List<T> InOrderTraversal()
    {
        // TODO: Create an empty List<T>, then call a private recursive helper like
        //   void InOrderHelper(TreeNode<T>? node, List<T> results)
        // that (if node is not null) recurses left, adds node.Value, then recurses
        // right. Return the populated list.
        throw new NotImplementedException();
    }
}
