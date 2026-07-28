// Reference solution — only read this after a real attempt.
//
// This file lives outside any .csproj (it is plain reference material, not
// compiled as part of the project), so it reuses the same namespace/class/method
// names as the src stubs without causing a build conflict.

namespace TreesBst;

public class TreeNode<T>
{
    public T Value { get; set; }
    public TreeNode<T>? Left { get; set; }
    public TreeNode<T>? Right { get; set; }

    public TreeNode(T value, TreeNode<T>? left = null, TreeNode<T>? right = null)
    {
        Value = value;
        Left = left;
        Right = right;
    }
}

public class BinarySearchTree<T> where T : IComparable<T>
{
    public TreeNode<T>? Root { get; private set; }

    public void Insert(T value)
    {
        Root = InsertHelper(Root, value);
    }

    private static TreeNode<T> InsertHelper(TreeNode<T>? node, T value)
    {
        if (node is null)
        {
            return new TreeNode<T>(value);
        }

        if (value.CompareTo(node.Value) < 0)
        {
            node.Left = InsertHelper(node.Left, value);
        }
        else
        {
            // Equal-or-greater goes right — this is the documented duplicate policy.
            node.Right = InsertHelper(node.Right, value);
        }

        return node;
    }

    public bool Contains(T value)
    {
        TreeNode<T>? current = Root;

        while (current is not null)
        {
            int cmp = value.CompareTo(current.Value);
            if (cmp == 0) return true;
            current = cmp < 0 ? current.Left : current.Right;
        }

        return false;
    }

    public bool Delete(T value)
    {
        bool removed = false;
        Root = DeleteHelper(Root, value, ref removed);
        return removed;
    }

    private static TreeNode<T>? DeleteHelper(TreeNode<T>? node, T value, ref bool removed)
    {
        if (node is null)
        {
            return null; // value not present anywhere down this path
        }

        int cmp = value.CompareTo(node.Value);

        if (cmp < 0)
        {
            node.Left = DeleteHelper(node.Left, value, ref removed);
            return node;
        }

        if (cmp > 0)
        {
            node.Right = DeleteHelper(node.Right, value, ref removed);
            return node;
        }

        // Found the node to delete.
        removed = true;

        // Case 1: leaf.
        if (node.Left is null && node.Right is null)
        {
            return null;
        }

        // Case 2: one child.
        if (node.Left is null)
        {
            return node.Right;
        }

        if (node.Right is null)
        {
            return node.Left;
        }

        // Case 3: two children — replace this node's value with its in-order
        // successor (smallest value in the right subtree), then delete that
        // successor from the right subtree (it's now a leaf-or-one-child case).
        TreeNode<T> successor = node.Right;
        while (successor.Left is not null)
        {
            successor = successor.Left;
        }

        node.Value = successor.Value;

        bool successorRemoved = false;
        node.Right = DeleteHelper(node.Right, successor.Value, ref successorRemoved);

        return node;
    }

    public List<T> InOrderTraversal()
    {
        var results = new List<T>();
        InOrderHelper(Root, results);
        return results;
    }

    private static void InOrderHelper(TreeNode<T>? node, List<T> results)
    {
        if (node is null) return;

        InOrderHelper(node.Left, results);
        results.Add(node.Value);
        InOrderHelper(node.Right, results);
    }
}

public static class TreeProblems
{
    public static bool IsValidBST(TreeNode<int>? root)
    {
        return IsValidBstHelper(root, null, null);
    }

    private static bool IsValidBstHelper(TreeNode<int>? node, long? min, long? max)
    {
        if (node is null) return true;

        if (min.HasValue && node.Value <= min.Value) return false;
        if (max.HasValue && node.Value >= max.Value) return false;

        return IsValidBstHelper(node.Left, min, node.Value)
            && IsValidBstHelper(node.Right, node.Value, max);
    }

    public static List<int> LevelOrderTraversal(TreeNode<int>? root)
    {
        var results = new List<int>();
        if (root is null) return results;

        var queue = new Queue<TreeNode<int>>();
        queue.Enqueue(root);

        while (queue.Count > 0)
        {
            TreeNode<int> node = queue.Dequeue();
            results.Add(node.Value);

            if (node.Left is not null) queue.Enqueue(node.Left);
            if (node.Right is not null) queue.Enqueue(node.Right);
        }

        return results;
    }

    public static TreeNode<int>? LowestCommonAncestor(TreeNode<int>? root, int p, int q)
    {
        TreeNode<int>? current = root;

        while (current is not null)
        {
            if (p < current.Value && q < current.Value)
            {
                current = current.Left;
            }
            else if (p > current.Value && q > current.Value)
            {
                current = current.Right;
            }
            else
            {
                return current;
            }
        }

        return null;
    }
}
