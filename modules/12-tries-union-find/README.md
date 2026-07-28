# 12 — Tries & Union-Find

Learn two data structures that solve very different problems efficiently: the **trie** (a.k.a. prefix tree), which answers "does this word/prefix exist?" in time proportional only to the word's length, and **union-find** (a.k.a. disjoint-set), which answers "are these two things connected?" across a constantly-growing set of connections in near-constant time. By the end you'll be able to build both from scratch and use union-find to solve two classic applied "connectivity" problems.

This is explicitly the **hardest module conceptually** in this curriculum. Union-find in particular (path compression + union by rank) trips people up on a first pass — that's normal. Take the syntax refresher and worked example slowly, and don't feel bad about re-reading the trace two or three times.

## C# syntax you'll need

### `Dictionary<TKey, TValue>`

A dictionary maps keys to values with (amortized) O(1) lookup, insert, and update. You'll use `Dictionary<char, TrieNode>` inside the trie.

```csharp
var ages = new Dictionary<string, int>();

// Adding / updating — the indexer either adds a new key or overwrites an existing one.
ages["Alice"] = 30;
ages["Bob"] = 25;
ages["Alice"] = 31; // overwrites

// Reading with the indexer throws if the key is missing — usually NOT what you want
// for "does this key exist" logic, since it can crash your program:
int aliceAge = ages["Alice"]; // fine, "Alice" exists
// int missing = ages["Carol"]; // would throw KeyNotFoundException

// ContainsKey: check existence without reading the value.
if (ages.ContainsKey("Bob"))
{
    Console.WriteLine("Bob is in the dictionary");
}

// TryGetValue: the idiomatic, safe way to "look up, and tell me if it was there".
// This is an `out` parameter (see below) — it's how the method hands you a second
// result (the value) alongside its normal bool return value (whether the key existed).
if (ages.TryGetValue("Carol", out int carolAge))
{
    Console.WriteLine($"Carol is {carolAge}");
}
else
{
    Console.WriteLine("Carol not found");
}
```

### `ref` and `out` parameters, briefly

Normally, C# passes arguments *by value* — the method gets a copy, and changes inside it don't affect the caller's variable. `out` (and `ref`) parameters are the exception: they let a method write back into a variable the caller passed in.

- `out int carolAge` above means: "this method will assign a value to `carolAge` before it returns (on the `true` path); I promise not to read it as an input." You don't need to initialize `carolAge` before the call.
- `ref` is the two-way version: the method can both read the caller's existing value and write a new one.

You won't write your own `out`/`ref` parameters in this module, but `TryGetValue`'s `out int carolAge` — and the equivalent `out TrieNode? next` pattern you'll use inside the trie — relies on this, so it's worth recognizing.

### Nested and private classes

A class can be declared *inside* another class. Marking it `private` means only the outer class can see or use it — exactly what you want for an implementation detail like a trie node that callers of `Trie` should never construct directly.

```csharp
public class Outer
{
    private class Inner
    {
        public int Value { get; set; }
    }

    public int MakeAndReadInner()
    {
        var inner = new Inner { Value = 42 };
        return inner.Value;
    }
}

// Code outside Outer cannot do `new Outer.Inner()` — it doesn't compile.
```

### Auto-properties

`public bool IsEndOfWord { get; set; }` is an **auto-property**: shorthand for a property with a compiler-generated private backing field. You get to write `node.IsEndOfWord = true;` and `if (node.IsEndOfWord)` without manually declaring a `private bool _isEndOfWord;` field yourself.

```csharp
public class Example
{
    public int Count { get; set; }              // readable and writable from outside
    public int ReadOnlyCount { get; }            // settable only in the constructor
    public Dictionary<char, int> Map { get; } = new(); // initialized inline, reference itself is read-only
}
```

Note the last line: `{ get; }` with no `set` means the *reference* can't be reassigned from outside, but if it's a collection (like a `Dictionary`), callers can still mutate its *contents* (add/remove keys) through the getter — that's exactly the pattern `TrieNode.Children` uses.

### Tuples: `(int a, int b)`, named elements, and deconstruction

A tuple bundles multiple values into one without declaring a whole class. C# tuples can have **named elements**, which makes the field names show up in IntelliSense/errors instead of generic `Item1`/`Item2`:

```csharp
(int a, int b) point = (3, 4);
Console.WriteLine(point.a); // 3
Console.WriteLine(point.b); // 4

// Deconstruction: unpack a tuple into separate variables in one line.
var (x, y) = point;
Console.WriteLine(x); // 3
Console.WriteLine(y); // 4

// Tuples are great as lightweight return values for "return two things":
(int min, int max) FindRange(int[] nums)
{
    return (nums.Min(), nums.Max());
}

var (lo, hi) = FindRange(new[] { 5, 1, 9, 3 });
```

### Arrays of tuples

You can have an array where each element is itself a tuple — this is exactly the shape of the `queries` parameter you'll implement against in `Problems.Reachable`:

```csharp
var queries = new (int a, int b)[]
{
    (0, 2),
    (1, 4),
    (3, 3),
};

foreach (var query in queries)
{
    Console.WriteLine($"Is {query.a} connected to {query.b}?");
}

// Or deconstruct right in the loop:
foreach (var (a, b) in queries)
{
    Console.WriteLine($"Is {a} connected to {b}?");
}
```

### Jagged arrays: `int[][]`

`int[][]` is an **array of arrays** ("jagged array") — not to be confused with `int[,]` (a true 2D rectangular array, which this module does not use). Each row is its own independently-sized `int[]` object. This is the shape used for the adjacency matrix in `CountProvinces`.

```csharp
int[][] matrix = new int[3][];
matrix[0] = new[] { 1, 1, 0 };
matrix[1] = new[] { 1, 1, 0 };
matrix[2] = new[] { 0, 0, 1 };

// Or, more compactly, as a literal:
int[][] matrix2 =
{
    new[] { 1, 1, 0 },
    new[] { 1, 1, 0 },
    new[] { 0, 0, 1 },
};

int rows = matrix.Length;      // 3
int cols = matrix[0].Length;   // 3 (assuming a square matrix)
int cell = matrix[0][1];       // 1 — row 0, column 1
```

## Trie node structure — why a Dictionary child map works

A trie stores a set of strings by sharing common prefixes as a single path through a tree. Each node represents "the string spelled out by the path from the root to here," and has:

- **A map from "next character" to "child node"** — `Dictionary<char, TrieNode> Children`. If you've inserted "cat" and "car", both words share the `c -> a` path, then split into two children (`t` and `r`) at the "ca" node.
- **A flag marking whether a complete word ends here** — `IsEndOfWord`. Without this, you couldn't tell the difference between "someone inserted 'car'" and "the path for 'car' merely exists because 'carpet' was inserted."

Why a `Dictionary<char, TrieNode>` specifically (rather than, say, a fixed array of 26 slots for `a`-`z`)? Two reasons:

1. **Flexibility** — it works for any character set (uppercase, digits, punctuation, non-English alphabets) without wasting memory on unused slots.
2. **The complexity guarantee still holds** — dictionary lookups are amortized O(1), so walking a word of length L still costs O(L) total, exactly like an array would, just without committing to a fixed alphabet size.

Every operation (`Insert`, `Search`, `StartsWith`) is just "walk the tree one character at a time, following `Children`." That's the whole trick — the tree's *shape* does the work of grouping strings by shared prefix, so you never compare whole strings against each other.

## Union-Find (Disjoint-Set Union) — the deep dive

### What problem does it solve?

**Dynamic connectivity**: you have a set of elements, and you're told (incrementally, one pair at a time) that certain pairs are "connected" (same group). At any point you want to answer: *"are element X and element Y in the same group?"* — efficiently, even after thousands of union operations and thousands of queries interleaved.

You could solve this by re-running a graph traversal (BFS/DFS) from scratch every time someone asks "are X and Y connected?" — but that's O(V + E) *per query*, which is slow if you have many queries. Union-find answers each query in near-constant time instead, after paying a small cost per union.

### The parent array representation

Union-find represents groups as a forest of trees, stored compactly as one array: `parent[i]` is the parent of element `i`. An element whose `parent[i] == i` is a **root** — the representative of its whole set/tree.

Initially, before any unions, every element is its own root (its own set of size 1):

```
index:   0  1  2  3  4
parent:  0  1  2  3  4
```

To merge two sets, you don't merge every element — you just point one **root** at the other root. That's `Union`. To answer "what set is X in," you walk `parent[X] -> parent[parent[X]] -> ...` until you hit a node whose parent is itself (a root). That's `Find`. Two elements are connected exactly when `Find(x) == Find(y)`.

### Path compression

Without any optimization, repeated unions can build long chains (`4 -> 3 -> 2 -> 1 -> 0`), making `Find` slow (O(n) in the worst case) for elements deep in the chain.

**Path compression** fixes this: every time `Find(x)` walks up to the root, it rewires every node it visited along the way to point *directly* at the root, instead of at its old (possibly distant) parent. The next `Find` on any of those nodes is then O(1).

In words, before/after:

```
BEFORE Find(3), with chain 3 -> 2 -> 1 -> 0 (0 is root):

  parent:  0  0  1  2
  index:   0  1  2  3

  Tree shape:
      0
      |
      1
      |
      2
      |
      3

AFTER Find(3) (with path compression), every node visited on the way
to the root (1, 2, and 3 itself) now points directly at the root (0):

  parent:  0  0  0  0
  index:   0  1  2  3

  Tree shape:
      0
     /|\
    1 2 3
```

The next time you call `Find(1)`, `Find(2)`, or `Find(3)`, it's a single O(1) hop — no chain-walking needed.

### Union by rank

Path compression alone helps, but you also want to avoid *creating* deep chains in the first place. **Union by rank** does this: each root tracks a `rank` (roughly, "an upper bound on the height of the tree rooted here" — it's not always the *exact* height once path compression starts flattening things, but it's a safe upper bound, which is all the algorithm needs). When merging two different sets:

- If the ranks differ, attach the **shorter** tree's root under the **taller** tree's root. The taller tree's height doesn't increase — the shorter tree was already not the bottleneck.
- If the ranks are **equal**, it doesn't matter which root you attach under which — but doing so makes the resulting tree one level taller, so you increment the winning root's rank by 1.

This guarantees the tree height only grows logarithmically in the number of elements, which by itself would make `Find` O(log n). Combined with path compression, it gets even better.

### Why near-O(1) amortized, and inverse Ackermann

With *both* path compression and union by rank, the amortized cost per operation (across any sequence of `m` union/find operations on `n` elements) is `O(alpha(n))`, where `alpha` is the **inverse Ackermann function** — a function that grows so slowly that for any `n` you could ever represent in a real computer's memory, `alpha(n) <= 4`. In practice, this means: treat union-find operations as constant time. You don't need to understand the Ackermann function itself to use this — just know the guarantee is about as strong as "effectively O(1)" gets in algorithm analysis.

### Worked example trace

Start with 5 elements, each its own set:

```
parent = [0, 1, 2, 3, 4]
rank   = [0, 0, 0, 0, 0]
```

**Step 1: `Union(0, 1)`**

- `Find(0)` = 0 (already a root). `Find(1)` = 1 (already a root).
- Roots differ (0 != 1). Ranks are equal (`rank[0] == rank[1] == 0`), so it's a tie: attach root 1 under root 0, and bump `rank[0]` to 1.

```
parent = [0, 0, 2, 3, 4]
rank   = [1, 0, 0, 0, 0]
```

**Step 2: `Union(2, 3)`**

- `Find(2)` = 2, `Find(3)` = 3. Roots differ, ranks tied (both 0). Attach root 3 under root 2, bump `rank[2]` to 1.

```
parent = [0, 0, 2, 2, 4]
rank   = [1, 0, 1, 0, 0]
```

**Step 3: `Union(0, 2)`**

- `Find(0)` = 0 (root). `Find(2)` = 2 (root). Roots differ.
- `rank[0] == 1` and `rank[2] == 1` — tied again. Attach root 2 under root 0 (by our tie-breaking convention: second argument's root goes under the first argument's root), and bump `rank[0]` to 2.

```
parent = [0, 0, 0, 2, 4]
rank   = [2, 0, 1, 0, 0]
```

Note: element 3's parent is still `2`, not `0` yet — `Union` only rewires the two *roots* it looked at (0 and 2); it doesn't touch every element transitively. Element 4 was never involved in any union, so it's still alone. The forest now looks like:

```
Set containing {0,1,2,3}:        Set containing {4}:
        0                              4
       /|
      1 2
        |
        3
```

**Step 4: `Find(3)` with path compression**

- `parent[3] == 2`, not 3, so 3 is not a root. Recurse: `Find(2)`.
- `parent[2] == 0`, not 2, so 2 is not a root. Recurse: `Find(0)`.
- `parent[0] == 0` — 0 IS the root. Return 0.
- Unwinding the recursion: `parent[2]` gets set to 0 (it already was — no visible change here since 2's parent was already the root). `parent[3]` gets set to 0 (this **is** a real change — 3 used to point at 2, now it points directly at 0).

```
parent = [0, 0, 0, 0, 4]
rank   = [2, 0, 1, 0, 0]   <- rank is untouched by Find; only Union changes rank
```

After this `Find(3)` call, every element in `{0, 1, 2, 3}` is either already a root (0) or points directly at the root (1, 2, 3 all have `parent[i] == 0`). Any future `Find` on any of them is an immediate O(1) lookup.

## Problems

### `Trie`

A trie (prefix tree) supporting word insertion, exact-word lookup, and prefix lookup.

**Exact public API:**

```csharp
public class Trie
{
    public void Insert(string word);
    public bool Search(string word);
    public bool StartsWith(string prefix);
}
```

- `Insert(word)` — adds `word` to the trie. Safe to call multiple times with the same word.
- `Search(word)` — returns `true` only if `word` was inserted as a **complete** word (not just a prefix of some longer inserted word).
- `StartsWith(prefix)` — returns `true` if **any** inserted word starts with `prefix` (the prefix itself doesn't need to have been inserted on its own).

Lookups are **case-sensitive** — `"Cat"` and `"cat"` are different words.

Example:

```csharp
var trie = new Trie();
trie.Insert("cat");
trie.Insert("car");

trie.Search("cat");      // true  — inserted exactly
trie.Search("ca");       // false — "ca" was never inserted as a complete word
trie.StartsWith("ca");   // true  — "cat" and "car" both start with "ca"
trie.StartsWith("do");   // false
```

**Complexity target:** O(L) per operation, where L is the length of the word/prefix — independent of how many words are stored.

### `UnionFind`

A disjoint-set structure over elements `0..size-1`, using path compression and union by rank.

**Exact public API:**

```csharp
public class UnionFind
{
    public UnionFind(int size);
    public int Find(int x);
    public void Union(int x, int y);
    public bool Connected(int x, int y);
    public int CountSets { get; }
}
```

- `UnionFind(size)` — creates `size` elements (`0..size-1`), each initially its own set.
- `Find(x)` — returns the root representing x's set, applying path compression.
- `Union(x, y)` — merges x's and y's sets (union by rank). No-op if already in the same set.
- `Connected(x, y)` — true if x and y are currently in the same set.
- `CountSets` — the number of distinct sets remaining.

Example:

```csharp
var uf = new UnionFind(5);
uf.Union(0, 1);
uf.Union(2, 3);

uf.Connected(0, 1); // true
uf.Connected(0, 2); // false
uf.CountSets;       // 3  ({0,1}, {2,3}, {4})
```

**Complexity target:** near-O(1) amortized per operation (technically O(alpha(n))).

### `Problems.CountProvinces`

```csharp
public static int CountProvinces(int[][] isConnected);
```

`isConnected` is an n x n adjacency matrix (`isConnected[i][j] == 1` means cities i and j are **directly** connected; the matrix is symmetric and `isConnected[i][i] == 1` for all i). A **province** is a group of cities connected directly or indirectly (transitively). Return the number of provinces.

Must be solved using `UnionFind`: union every directly-connected pair, then the answer is `UnionFind.CountSets`.

Example:

```csharp
int[][] isConnected =
{
    new[] { 1, 1, 0 },
    new[] { 1, 1, 0 },
    new[] { 0, 0, 1 },
};

Problems.CountProvinces(isConnected); // 2
```

**Complexity target:** O(n^2 \* alpha(n)) time, O(n) space.

### `Problems.Reachable`

```csharp
public static bool[] Reachable(int n, int[][] edges, (int a, int b)[] queries);
```

`n` nodes (`0..n-1`), an undirected edge list `edges` (each entry a 2-element `[a, b]` array), and a list of `(a, b)` tuple `queries`. Return a `bool[]` where `result[i]` indicates whether `queries[i].a` and `queries[i].b` are in the same connected component.

Must be solved using `UnionFind`: union all edges first, then answer each query with a single `Connected()` call.

Example:

```csharp
int n = 5;
int[][] edges = { new[] { 0, 1 }, new[] { 1, 2 }, new[] { 3, 4 } };
var queries = new (int a, int b)[] { (0, 2), (0, 4), (3, 4) };

Problems.Reachable(n, edges, queries); // [true, false, true]
```

**Complexity target:** O((n + E + Q) \* alpha(n)) time, O(n) space.

## Hints

### Trie

<details>
<summary>Hint 1 — nudge</summary>

All three methods (`Insert`, `Search`, `StartsWith`) do the *same walk*: start at the root, and for each character in the input string, move to the child for that character. The only differences are (a) what you do when a child is missing, and (b) what you check once you reach the end.

</details>

<details>
<summary>Hint 2 — approach</summary>

- `Insert`: if a child is missing, **create** it and keep going. At the end, mark the final node's `IsEndOfWord = true`.
- `Search`: if a child is missing, **return false immediately** — the word was never inserted. At the end, return the final node's `IsEndOfWord` (must be a complete word, not just any path).
- `StartsWith`: if a child is missing, **return false immediately**. At the end, return `true` unconditionally — you don't care about `IsEndOfWord`, only that the path exists.

</details>

<details>
<summary>Hint 3 — near-solution</summary>

```csharp
public bool Search(string word)
{
    var current = _root;

    foreach (var c in word)
    {
        if (!current.Children.TryGetValue(c, out var next))
        {
            return false;
        }

        current = next;
    }

    return current.IsEndOfWord; // for StartsWith, this line would just be `return true;`
}
```

`Insert` follows the same shape, but instead of returning `false` on a missing child, it does `next = new TrieNode(); current.Children[c] = next;` and continues the loop.

</details>

### UnionFind

<details>
<summary>Hint 1 — nudge</summary>

`Find` and `Union` both rely on the idea of a "root": an element whose `parent[i] == i`. Everything else is either directly or indirectly pointing toward a root.

</details>

<details>
<summary>Hint 2 — approach</summary>

`Find(x)`: if `x` is its own parent, it's the root — return it. Otherwise, recursively find the root of `parent[x]`, and **before returning**, overwrite `parent[x]` with that root (this is path compression — it flattens the tree as a side effect of searching it).

`Union(x, y)`: find the root of each. If they're already equal, do nothing. Otherwise, attach the lower-rank root under the higher-rank root (or, on a tie, pick one arbitrarily and bump its rank by 1), and decrement the set count.

</details>

<details>
<summary>Hint 3 — near-solution (concrete restatement of path compression + union by rank)</summary>

Path compression, in pseudocode:

```
Find(x):
    if parent[x] == x:
        return x
    parent[x] = Find(parent[x])   # rewire x directly to the root on the way back up
    return parent[x]
```

Union by rank, in pseudocode:

```
Union(x, y):
    rootX = Find(x)
    rootY = Find(y)
    if rootX == rootY:
        return  # already same set, nothing to do

    if rank[rootX] < rank[rootY]:
        parent[rootX] = rootY          # shorter tree goes under taller tree
    else if rank[rootX] > rank[rootY]:
        parent[rootY] = rootX
    else:                               # tie: pick either, and the winner grows by 1
        parent[rootY] = rootX
        rank[rootX] += 1

    countSets -= 1
```

</details>

### Problems.CountProvinces

<details>
<summary>Hint 1 — nudge</summary>

You don't need any graph traversal code here at all — this is a direct application of `UnionFind`. Every `1` in the matrix (off the diagonal) is an edge to union.

</details>

<details>
<summary>Hint 2 — approach</summary>

Create a `UnionFind` sized to the matrix. Loop over every pair `(i, j)` with `i < j` (skip the diagonal and don't double-process symmetric pairs — though re-`Union`-ing the same pair twice is harmless, just wasted work). Wherever `isConnected[i][j] == 1`, call `Union(i, j)`. When the loops finish, `CountSets` **is** the answer — no extra bookkeeping needed.

</details>

### Problems.Reachable

<details>
<summary>Hint 1 — nudge</summary>

Two phases: first build the connectivity (union every edge), *then* answer every query. Don't try to answer queries while you're still adding edges — union everything first.

</details>

<details>
<summary>Hint 2 — approach</summary>

`Union(edge[0], edge[1])` for every edge in `edges`. Then, for each query tuple, a single `Connected(query.a, query.b)` call gives you the answer directly — no need to re-derive anything.

</details>

## Running your work

```bash
cd modules/12-tries-union-find/tests/TriesUnionFind.Tests && dotnet test
```

## If you're stuck

`solution/Solution.cs` holds a complete, correct reference implementation of everything in this module — but try to get genuinely stuck first. Re-read the worked union-find trace above, try tracing your own small example by hand on paper (5-6 elements, 3-4 unions), and see where your mental model and the code disagree before peeking.

If you end up asking your local (offline, and comparatively weak) LLM for help, get much better answers by being specific:

- **Paste the exact method signature** you're implementing (e.g. `public int Find(int x)`), not just "help me with union-find."
- **State the constraints and invariants** explicitly (e.g. "parent[i] starts equal to i; I need path compression").
- **Ask for the approach in words first, before any code** — e.g. "Explain the steps for Find with path compression, don't write code yet." Weak local models tend to produce more reliable *reasoning* than reliable *code*; getting the approach right in plain English first, then translating it yourself, catches more bugs than asking for a finished snippet outright.
