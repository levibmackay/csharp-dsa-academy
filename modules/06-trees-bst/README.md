# Module 06 — Trees & Binary Search Trees

Learn how hierarchical data is represented with linked nodes, and how the binary
search tree's ordering invariant turns search, insert, and delete into O(h)
operations instead of O(n). You'll implement a generic BST from scratch and solve
several classic interview-style tree problems (BFS traversal, validation, lowest
common ancestor) that show up constantly in real codebases and technical interviews.

## C# syntax you'll need

### Generic constraints — `where T : IComparable<T>`

A generic class like `BinarySearchTree<T>` needs to compare values of type `T` to
decide whether they go left or right. But the compiler doesn't know `T` supports
comparison unless you tell it. A **generic constraint** does exactly that:

```csharp
public class BinarySearchTree<T> where T : IComparable<T>
{
    // Inside this class, every T is guaranteed to have a CompareTo method.
}
```

`IComparable<T>` is a built-in .NET interface with one method:

```csharp
public interface IComparable<T>
{
    int CompareTo(T? other);
}
```

`CompareTo` returns:
- a **negative** number if `this` comes before `other`
- **zero** if they're equal
- a **positive** number if `this` comes after `other`

Built-in types like `int`, `string`, and `double` already implement
`IComparable<T>`, so you can call `CompareTo` directly:

```csharp
int a = 3;
int b = 7;
int result = a.CompareTo(b); // negative, because 3 < 7

string x = "apple";
string y = "banana";
int cmp = x.CompareTo(y); // negative, because "apple" < "banana" alphabetically
```

If you wrote your own type and wanted it usable in a `BinarySearchTree<T>`, you'd
implement the interface yourself:

```csharp
public class Money : IComparable<Money>
{
    public int Cents { get; set; }

    public int CompareTo(Money? other)
    {
        if (other is null) return 1;
        return Cents.CompareTo(other.Cents);
    }
}
```

You won't need to write your own `IComparable` type for this module — `int` is
enough — but understanding what the constraint buys you (a guaranteed `CompareTo`)
is essential for reading and writing `BinarySearchTree<T>`.

### Nullable reference types — the `?`

This project has `<Nullable>enable</Nullable>` turned on (check the `.csproj` files
if curious). That means the compiler tracks, for every reference type, whether a
variable is allowed to be `null` — and warns you if you might be dereferencing
`null` without checking first.

```csharp
TreeNode<int>? maybeNode = null;   // the ? means "this CAN be null"
TreeNode<int> definiteNode = new TreeNode<int>(5); // no ?, compiler assumes non-null
```

In this module, `TreeNode<T>.Left` and `TreeNode<T>.Right` are typed
`TreeNode<T>?` because a node might not have a child — that's just normal tree
shape, not an error state. The compiler will nag (with a warning, not necessarily
an error) if you write code that could dereference a `TreeNode<T>?` without first
confirming it isn't null:

```csharp
TreeNode<int>? node = root.Left;
Console.WriteLine(node.Value); // compiler warning: node might be null

if (node is not null)
{
    Console.WriteLine(node.Value); // fine — compiler knows node isn't null here
}
```

Two operators make working with nullable values less verbose:

- **Null-conditional `?.`** — "call this member only if the thing on the left
  isn't null; otherwise the whole expression evaluates to null."
  ```csharp
  int? leftValue = node?.Value; // if node is null, leftValue is null; no crash
  ```
- **Null-coalescing `??`** — "use the left side, unless it's null, in which case
  use the right side instead."
  ```csharp
  int safeValue = node?.Value ?? -1; // -1 if node is null, otherwise node.Value
  ```

You'll see `TreeNode<T>?` throughout this module's types. Get comfortable reading
it as "a reference to a node, or nothing at all."

### Recursion in C#

A recursive method calls itself with a smaller version of the problem, and always
has a **base case** that stops the recursion. The classic non-tree example is
factorial:

```csharp
int Factorial(int n)
{
    if (n <= 1) return 1;           // base case: stop recursing
    return n * Factorial(n - 1);    // recursive case: smaller subproblem
}
```

Trees are naturally recursive: a tree is a node with two children, and each child
is itself the root of a smaller tree. So tree algorithms almost always follow the
same shape — base case is "the node is null" (an empty subtree), and the recursive
case does something with the node's value and recurses into `Left` and `Right`:

```csharp
int CountNodes(TreeNode<int>? node)
{
    if (node is null) return 0;                              // base case
    return 1 + CountNodes(node.Left) + CountNodes(node.Right); // recursive case
}
```

A common pattern you'll use repeatedly in this module: a recursive helper that
takes a `TreeNode<T>?` and **returns** a `TreeNode<T>?` — the (possibly changed,
possibly newly-created) subtree root — so the caller can reassign
`node.Left = Helper(node.Left, ...)` and thread structural changes back up the
call stack. Insert, Delete, and validation all lean on this shape.

### `Queue<T>` for breadth-first search

Depth-first (recursive) traversal visits a whole branch before backtracking.
Breadth-first traversal visits level by level, and the standard tool for that is a
FIFO queue: enqueue a node's children, and by the time you dequeue them, you've
already visited everything at the current level.

`System.Collections.Generic.Queue<T>` is a built-in .NET FIFO collection:

```csharp
var queue = new Queue<int>();
queue.Enqueue(1);
queue.Enqueue(2);
queue.Enqueue(3);

Console.WriteLine(queue.Count); // 3

int first = queue.Dequeue(); // 1 (removes and returns the front item)
Console.WriteLine(queue.Count); // 2
```

For `LevelOrderTraversal`, you'll enqueue a node, then loop while the queue has
items: dequeue a node, record its value, and enqueue its non-null children.

### `List<T>` basics

`List<T>` is the go-to resizable collection for "collect results as I go":

```csharp
var results = new List<int>();
results.Add(10);
results.Add(20);

Console.WriteLine(results[0]);   // 10 — indexing works like an array
Console.WriteLine(results.Count); // 2
```

Every traversal method in this module returns a `List<T>` (or `List<int>`) built up
by appending values as the traversal visits each node.

## Problems

### Insert

Insert a value into the BST, preserving the ordering property (left subtree <
node < right subtree). **Duplicate policy for this module: a value equal to an
existing node's value is inserted into that node's RIGHT subtree** — so duplicates
are kept (not silently dropped), and they'll show up adjacent to each other in a
sorted `InOrderTraversal`.

```csharp
public void Insert(T value)
```

Example: inserting `5, 3, 8, 5` in that order produces a tree where the second `5`
lives in the right subtree of the first `5`. `InOrderTraversal()` afterward
returns `[3, 5, 5, 8]`.

**Complexity:** O(h) time, where h is the tree's height (O(log n) if the tree
stays balanced, O(n) worst case for a degenerate/skewed tree). O(h) space for the
recursive call stack.

### Contains

Determine whether a value exists anywhere in the tree.

```csharp
public bool Contains(T value)
```

Example: on a tree built from `[5, 3, 8]`, `Contains(3)` returns `true`,
`Contains(100)` returns `false`. `Contains` on an empty tree returns `false`.

**Complexity:** O(h) time, O(1) space if written iteratively (O(h) if recursive).

### Delete

Remove a value from the tree if present, restructuring the tree so the BST
property still holds afterward.

```csharp
public bool Delete(T value)
```

Returns `true` if the value was found and removed, `false` if it wasn't present
(tree is left unchanged) or the tree was empty. Deleting from an empty tree must
not throw.

Example: on a tree built from `[5, 3, 8, 1, 4, 7, 9]`, `Delete(5)` removes the
root; the tree's `InOrderTraversal()` afterward is still fully sorted:
`[1, 3, 4, 7, 8, 9]`.

**Complexity:** O(h) time, O(h) space (recursion).

### InOrderTraversal

Return every value in the tree, visiting left subtree, then the node itself, then
right subtree — which for a BST always produces values in ascending sorted order.

```csharp
public List<T> InOrderTraversal()
```

Example: a tree built from `[5, 3, 8, 1, 4, 7, 9]` returns
`[1, 3, 4, 5, 7, 8, 9]`. An empty tree returns `[]`.

**Complexity:** O(n) time (visits every node once), O(n) space for the returned
list, plus O(h) recursion stack space.

### IsValidBST

Given a plain `TreeNode<int>?` tree (not necessarily built via `Insert`),
determine whether it satisfies the BST property **everywhere**: every node's value
must be strictly less than every value in its right subtree, and strictly greater
than every value in its left subtree — not just its immediate children.

```csharp
public static bool IsValidBST(TreeNode<int>? root)
```

Example: `root=10, left=5, right=15` where `5` itself has a right child of `15` is
**invalid**, even though `15 > 5` looks fine locally — that `15` is inside root's
left subtree and must be `< 10`. A `null` root is considered valid (vacuously
true — an empty tree has no violations).

**Complexity:** O(n) time (must visit every node), O(h) space (recursion stack).

### LevelOrderTraversal

Perform a breadth-first traversal: visit all nodes at depth 0, then depth 1, then
depth 2, etc., left to right within each level.

```csharp
public static List<int> LevelOrderTraversal(TreeNode<int>? root)
```

Example: for the tree
```
        1
       / \
      2   3
     / \ / \
    4  5 6  7
```
`LevelOrderTraversal` returns `[1, 2, 3, 4, 5, 6, 7]`. A `null` root returns `[]`.

**Complexity:** O(n) time (visits every node once), O(w) space where w is the
maximum width of the tree (the queue never holds more nodes than the widest level).

### LowestCommonAncestor

Given a **binary search tree** and two values `p` and `q` known to exist in it,
find the node that is the lowest (deepest) common ancestor of both.

```csharp
public static TreeNode<int>? LowestCommonAncestor(TreeNode<int>? root, int p, int q)
```

Example: on a BST, if `p` and `q` are on opposite sides of the root's value, the
root itself is the LCA. If one is an ancestor of the other, the ancestor is the
LCA. Assume `p` and `q` both exist in the tree when it's non-empty; a `null` root
returns `null`.

**Complexity:** O(h) time — you navigate directly toward the answer using BST
ordering instead of exploring both subtrees, which is what makes this simpler
than the general (non-BST) tree LCA problem. O(1) extra space if written
iteratively.

## Hints

<details>
<summary>Insert — Hint 1</summary>

Think about what happens at each node as you walk down from the root: compare the
new value to the current node's value. That comparison tells you which direction
to go. What should happen when you reach a spot where there's no node yet?

</details>

<details>
<summary>Insert — Hint 2</summary>

A recursive helper that takes a `TreeNode<T>?` and **returns** a `TreeNode<T>?`
works nicely here: if the node passed in is `null`, create and return a brand new
node. Otherwise, recurse into the correct child based on `CompareTo`, and
reassign that child to the helper's return value before returning the (unchanged)
current node.

</details>

<details>
<summary>Insert — Hint 3</summary>

```
InsertHelper(node, value):
    if node is null: return new node
    if value < node.Value: node.Left = InsertHelper(node.Left, value)
    else:                  node.Right = InsertHelper(node.Right, value)  // equal goes right
    return node
```
Call this from `Insert` and assign the result back to `Root`.

</details>

<details>
<summary>Contains — Hint 1</summary>

You don't need recursion for this one if you don't want it — a simple loop that
moves a "current node" pointer left or right based on comparison works fine.

</details>

<details>
<summary>Contains — Hint 2</summary>

At each step: if the current node is `null`, you've fallen off the tree — the
value isn't here. Otherwise compare the target value to the current node's value;
zero means found, negative means go left, positive means go right.

</details>

<details>
<summary>Contains — Hint 3</summary>

```
current = Root
while current is not null:
    cmp = value.CompareTo(current.Value)
    if cmp == 0: return true
    current = cmp < 0 ? current.Left : current.Right
return false
```

</details>

<details>
<summary>Delete — Hint 1</summary>

Delete is the hardest operation here because removing a node might leave a "hole"
that needs to be patched. There are three distinct shapes of node you might be
deleting, and each needs different handling. Think about what makes a leaf easy,
and why a node with two children is genuinely harder than a node with one.

</details>

<details>
<summary>Delete — Hint 2</summary>

The three cases:
1. **Leaf** (no children) — just remove it; nothing needs to take its place.
2. **One child** — the node's single child can simply take the deleted node's
   spot in the tree.
3. **Two children** — you can't just remove it (which child would replace it?).
   Instead, find the node's **in-order successor**: the smallest value in its
   right subtree (keep walking `.Left` from `node.Right` until you can't
   anymore). Copy that successor's *value* into the node you wanted to delete,
   then delete the successor from the right subtree instead — and the successor
   is guaranteed to only have a right child at most (never a left child, since
   you walked as far left as possible to find it), so that recursive delete
   falls into case 1 or 2.

</details>

<details>
<summary>Delete — Hint 3</summary>

Use a recursive helper `TreeNode<T>? DeleteHelper(TreeNode<T>? node, T value)`
that returns what should now occupy that spot in the tree:
```
DeleteHelper(node, value):
    if node is null: return null                      // not found
    cmp = value.CompareTo(node.Value)
    if cmp < 0: node.Left = DeleteHelper(node.Left, value); return node
    if cmp > 0: node.Right = DeleteHelper(node.Right, value); return node

    // cmp == 0, this is the node to delete
    if node.Left is null and node.Right is null: return null            // case 1
    if node.Left is null: return node.Right                             // case 2
    if node.Right is null: return node.Left                             // case 2

    successor = node.Right
    while successor.Left is not null: successor = successor.Left        // case 3
    node.Value = successor.Value
    node.Right = DeleteHelper(node.Right, successor.Value)
    return node
```
Track separately (e.g. a `bool` set to `true` only in the "found it" branch)
whether you actually removed something, so `Delete` can report `true`/`false`.

</details>

<details>
<summary>InOrderTraversal — Hint 1</summary>

"In-order" means: left subtree, then this node, then right subtree. Recursion
maps onto this almost word-for-word.

</details>

<details>
<summary>InOrderTraversal — Hint 2</summary>

Use a helper that takes the current node and the `List<T>` being built up, and
appends to that list as it recurses — rather than trying to concatenate
lists returned from each recursive call.

</details>

<details>
<summary>InOrderTraversal — Hint 3</summary>

```
InOrderHelper(node, results):
    if node is null: return
    InOrderHelper(node.Left, results)
    results.Add(node.Value)
    InOrderHelper(node.Right, results)
```

</details>

<details>
<summary>IsValidBST — Hint 1</summary>

It's tempting to just check `node.Value > node.Left.Value` and
`node.Value < node.Right.Value` at every node. **This is wrong.** Think about why:
a node deep in a subtree could satisfy its immediate parent's comparison while
still violating an ancestor further up.

</details>

<details>
<summary>IsValidBST — Hint 2</summary>

Concretely: `root=10`, left child `5`, and `5` has a right child `15`. Checking
only "`15` vs its parent `5`" passes (`15 > 5`). But `15` is inside root's *left*
subtree, so it must be `< 10` — and it isn't. You need to validate against a
*range* that narrows as you go deeper, not just the immediate parent.

</details>

<details>
<summary>IsValidBST — Hint 3</summary>

Carry a `(min, max)` exclusive bound down through the recursion:
```
IsValidBstHelper(node, min, max):
    if node is null: return true
    if min has a value and node.Value <= min: return false
    if max has a value and node.Value >= max: return false
    return IsValidBstHelper(node.Left, min, node.Value)
        and IsValidBstHelper(node.Right, node.Value, max)
```
Use `long?` for `min`/`max` (rather than `int?`) so that tightening a bound at
`int.MinValue` or `int.MaxValue` can't overflow.

</details>

<details>
<summary>LevelOrderTraversal — Hint 1</summary>

This is the one problem in this module that's easier iteratively than
recursively — the level-by-level structure maps directly onto a queue.

</details>

<details>
<summary>LevelOrderTraversal — Hint 2</summary>

Enqueue the root (if it's not null). Then loop while the queue has items: dequeue
a node, record its value, and enqueue whichever of its children aren't null. The
queue naturally processes nodes in level order because you enqueue children
before dequeuing anything from the next level.

</details>

<details>
<summary>LevelOrderTraversal — Hint 3</summary>

```
if root is null: return []
queue = new Queue<TreeNode<int>>()
queue.Enqueue(root)
results = []
while queue.Count > 0:
    node = queue.Dequeue()
    results.Add(node.Value)
    if node.Left is not null: queue.Enqueue(node.Left)
    if node.Right is not null: queue.Enqueue(node.Right)
return results
```

</details>

<details>
<summary>LowestCommonAncestor — Hint 1</summary>

Because this is specifically a BST (not a general tree), you don't need to
search both subtrees and combine results — the ordering property tells you
directly which way to go.

</details>

<details>
<summary>LowestCommonAncestor — Hint 2</summary>

At each node, compare `p` and `q` to the current node's value. If they're both
smaller, the LCA must be further left. If they're both larger, it must be
further right. What does it mean if they're on different sides (or one equals
the current node)?

</details>

<details>
<summary>LowestCommonAncestor — Hint 3</summary>

```
current = root
while current is not null:
    if p < current.Value and q < current.Value: current = current.Left
    elif p > current.Value and q > current.Value: current = current.Right
    else: return current   // split point (or one of p/q equals current.Value)
return null
```

</details>

## Running your work

```
cd modules/06-trees-bst/tests/TreesBst.Tests && dotnet test
```

## If you're stuck

`solution/Solution.cs` has a complete, working reference implementation of every
method in this module. It's worth a genuine, unhurried attempt first — struggling
with the Delete two-children case or the IsValidBST range trick and then working
it out is how the ideas actually stick. Peek only after you've tried, ideally
after trying at least one of the hint tiers above.

If you're offline with only a small local LLM for help, get more out of it by
being specific:
- Paste the **exact method signature** you're implementing (e.g.
  `public bool Delete(T value)`) and mention the class it's on and the
  constraint (`where T : IComparable<T>`).
- State which case is giving you trouble (e.g. "the two-children case in BST
  delete") rather than asking a vague "why doesn't my code work."
- Ask for the **approach in plain English first** — "what's the algorithm for
  finding the in-order successor?" — before asking for code. Small local models
  are much more reliable explaining an idea than generating correct C#, and
  understanding the idea is the actual point of this module.
