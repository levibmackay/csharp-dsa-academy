# Module 8: Graphs — BFS & DFS

Learn to represent relationships between things (people, cities, web pages,
grid cells) as a graph, and to systematically visit every reachable node
using breadth-first search (BFS) and depth-first search (DFS). By the end you
should be able to build an adjacency-list graph from scratch, traverse it
both ways, and recognize that a 2D grid is secretly a graph too.

## C# syntax you'll need

If you've been away from C# for a while, work through this section first —
everything here is used in the stubs.

### `Dictionary<TKey, TValue>`

A hash map. This module uses one as the graph's adjacency list: each vertex
(an `int`) maps to a `List<int>` of its neighbors.

```csharp
var adjacency = new Dictionary<int, List<int>>();

// Indexer: read/write by key. Throws KeyNotFoundException if the key is
// missing and you try to READ it (writing with the indexer always works —
// it inserts or overwrites).
adjacency[1] = new List<int>();

// ContainsKey: check existence before touching the value.
if (!adjacency.ContainsKey(1))
{
    adjacency[1] = new List<int>();
}

// TryGetValue: the idiomatic "look it up, and tell me if it was there"
// pattern — avoids a double lookup (ContainsKey + indexer) and avoids the
// KeyNotFoundException entirely.
if (adjacency.TryGetValue(1, out List<int>? neighbors))
{
    Console.WriteLine(neighbors.Count);
}

// .Keys gives you all keys as an IEnumerable<TKey> (handy for "all vertices").
foreach (int vertex in adjacency.Keys)
{
    Console.WriteLine(vertex);
}
```

### `List<T>`

A resizable array. Used here for each vertex's neighbor list and for
traversal-order results.

```csharp
var order = new List<int>();
order.Add(1);              // append
order.Add(2);
bool has3 = order.Contains(3); // linear-time membership check
int count = order.Count;
```

### `Queue<T>` — first-in, first-out (used by BFS)

```csharp
var queue = new Queue<int>();
queue.Enqueue(1);   // add to the back
queue.Enqueue(2);
int front = queue.Dequeue(); // remove and return from the front -> 1
int size = queue.Count;
```

Think of a queue like a line at a store: whoever got in line first gets
served first. BFS uses this to explore level-by-level — everything one hop
away gets visited before anything two hops away.

### `Stack<T>` — last-in, first-out (the iterative-DFS alternative)

```csharp
var stack = new Stack<int>();
stack.Push(1);   // add to the top
stack.Push(2);
int top = stack.Pop(); // remove and return from the top -> 2 (not 1!)
```

A stack is like a pile of plates: the last one you put down is the first one
you pick back up. This module's `Dfs` is implemented recursively rather than
with an explicit `Stack<int>` — see the note in the DFS problem section below
for why, and how the two approaches differ.

### `HashSet<T>`

An unordered collection with O(1) average-case add/contains, and no
duplicates. Used to track "have I visited this vertex already?" — exactly
the kind of membership check a `List<T>` would do in slow linear time.

```csharp
var visited = new HashSet<int>();
visited.Add(1);
bool seen = visited.Contains(1); // true, and O(1) average case
visited.Add(1); // no-op, HashSet silently ignores duplicate adds
```

### `IEnumerable<T>` basics

`IEnumerable<T>` is the most general "a sequence of things you can loop over
with `foreach`" interface in .NET. `List<int>`, `Dictionary<...>.Keys`,
arrays, and `Queue<T>` are all `IEnumerable<T>` (or `IEnumerable<TKey>`, etc.)
under the hood. The `Graph.Vertices` property in this module returns
`IEnumerable<int>` rather than `List<int>` specifically because callers only
need to iterate it — exposing a narrower interface than `List<int>` is good
practice when you don't want callers mutating your internal collection.

```csharp
IEnumerable<int> Numbers() // any method returning a sequence
{
    yield return 1;
    yield return 2;
}

foreach (int n in Numbers())
{
    Console.WriteLine(n);
}

// LINQ works on any IEnumerable<T>:
List<int> asList = Numbers().ToList();
bool any = Numbers().Any(n => n > 1);
```

### Jagged arrays `int[][]` vs. 2D arrays `int[,]` — a common C# gotcha

C# has **two different** array shapes for grid-like data, and they are NOT
interchangeable:

- **`int[,]`** — a true rectangular 2D array. One block of memory, fixed
  rows × columns, accessed with a single indexer: `grid[row, col]`.
- **`int[][]`** — a "jagged" array: an array of arrays. Each row is its own
  independent `int[]` object, so rows could (in theory) have different
  lengths. Accessed with two separate indexers: `grid[row][col]`.

`NumIslands` in this module takes `int[][]` (jagged), which is what you'll
see most often in interview-style problems and in this repo's tests. Watch
the syntax carefully — mixing up `grid[row, col]` and `grid[row][col]` is a
classic compile error.

```csharp
// Jagged: array of arrays. Each row is a separate int[] allocated on its own.
int[][] jagged = new int[][]
{
    new int[] { 1, 0, 1 },
    new int[] { 0, 1, 0 },
};
int cell = jagged[0][2]; // two indexers -> 1

// True 2D: one rectangular block.
int[,] twoD = new int[2, 3]
{
    { 1, 0, 1 },
    { 0, 1, 0 },
};
int cell2 = twoD[0, 2]; // one indexer, comma-separated -> 1
```

`grid.Length` on a jagged array gives you the number of rows; `grid[0].Length`
gives you the number of columns in that particular row (you'll typically
assume all rows are the same length for a "grid").

### Recursion refresher

A recursive method calls itself on a smaller version of the problem until it
hits a **base case** that stops the recursion. Every recursive call pushes a
new frame onto the call stack; when the base case is hit, the calls unwind
back out.

```csharp
static int Factorial(int n)
{
    if (n <= 1) return 1;       // base case — stops the recursion
    return n * Factorial(n - 1); // recursive case — smaller subproblem
}
```

For DFS, the "smaller subproblem" at each vertex is "visit this vertex, then
recursively DFS from each of its unvisited neighbors." The base case is
implicit: a vertex whose neighbors are all already visited simply returns
without recursing further, and the call stack unwinds back to the caller to
try the next neighbor. A `HashSet<int> visited` shared across all recursive
calls is what prevents infinite recursion on a graph with cycles.

## Problems

### 1. Graph construction and operations

Implement an undirected, unweighted graph backed by an adjacency list — a
`Dictionary<int, List<int>>` mapping each vertex to its neighbors.

```csharp
public class Graph
{
    public IEnumerable<int> Vertices { get; }
    public void AddVertex(int v);
    public void AddEdge(int a, int b);
    public List<int> Neighbors(int v);
}
```

**Examples:**

```csharp
var graph = new Graph();
graph.AddEdge(1, 2);   // implicitly creates vertices 1 and 2 if missing
graph.AddVertex(3);    // isolated vertex, no edges

graph.Neighbors(1);    // [2]
graph.Neighbors(2);    // [1]
graph.Neighbors(3);    // []
graph.Neighbors(99);   // [] (vertex not in graph — don't throw)
```

**Complexity target:** `AddVertex`, `AddEdge`, and `Neighbors` are all O(1)
average case (dictionary lookup/insert, plus O(1) amortized list append for
`AddEdge`). Space is O(V + E) for V vertices and E edges (each edge is
stored twice, once per direction).

### 2. Bfs

Given a graph and a starting vertex, return the vertices in breadth-first
traversal order: visit the start, then everything one edge away, then
everything two edges away, and so on.

```csharp
public static List<int> Bfs(Graph graph, int start)
```

**Examples:**

Graph with edges `1-2`, `1-3`, `2-4`:

```csharp
GraphAlgorithms.Bfs(graph, 1); // [1, 2, 3, 4]
```

Single isolated vertex:

```csharp
GraphAlgorithms.Bfs(graph, 5); // [5]
```

**Complexity target:** O(V + E) time (every vertex enqueued once, every edge
examined once), O(V) space for the visited set and queue.

### 3. Dfs

Given a graph and a starting vertex, return the vertices in depth-first
traversal order: visit the start, then dive as deep as possible down one
neighbor before backtracking to try the next.

```csharp
public static List<int> Dfs(Graph graph, int start)
```

**A note on implementation choice:** this module's `Dfs` is implemented
**recursively** rather than with an explicit `Stack<int>`. This keeps the
code simple and gives a deterministic visit order that exactly mirrors each
vertex's `Neighbors` list order (visit neighbor[0] and everything reachable
from it, before moving on to neighbor[1]). An **iterative** version using
`Stack<int>` is a valid alternative — and uses less call-stack memory on
very deep graphs, avoiding a potential `StackOverflowException` — but its
visit order can come out differently, because pushing neighbors onto a stack
and then popping them back off visits them in *reverse* of push order unless
you're careful about the order you push them in.

**Examples:**

Graph built with `AddEdge(1,2)`, `AddEdge(1,3)`, `AddEdge(2,4)` (so vertex
1's neighbor list is `[2, 3]` and vertex 2's is `[1, 4]`, in insertion
order):

```csharp
GraphAlgorithms.Dfs(graph, 1); // [1, 2, 4, 3]
```

Single isolated vertex:

```csharp
GraphAlgorithms.Dfs(graph, 5); // [5]
```

**Complexity target:** O(V + E) time, O(V) space for the visited set plus
O(V) worst-case call-stack depth (a "long chain" graph).

### 4. HasPath

Given a graph and two vertices, determine whether any path connects them
(directly or through intermediate vertices).

```csharp
public static bool HasPath(Graph graph, int source, int destination)
```

**Examples:**

Graph with edges `1-2`, `2-3`:

```csharp
GraphAlgorithms.HasPath(graph, 1, 3); // true  (path: 1 -> 2 -> 3)
GraphAlgorithms.HasPath(graph, 1, 1); // true  (source == destination)
```

Graph with edges `1-2` and an isolated vertex `3`:

```csharp
GraphAlgorithms.HasPath(graph, 1, 3); // false (no path)
```

**Complexity target:** O(V + E) time, O(V) space — internally this is just a
BFS or DFS from `source`, checking whether `destination` is ever reached
(ideally with an early exit as soon as it's found, rather than always
traversing the whole reachable component).

### 5. CountConnectedComponents

Given a graph, count how many separate connected components it has (a
connected component is a maximal set of vertices where every vertex can
reach every other vertex in the set, but not vertices outside it). Unlike
`Bfs`/`Dfs`/`HasPath`, this looks at the **whole graph**, not just what's
reachable from one starting vertex.

```csharp
public static int CountConnectedComponents(Graph graph)
```

**Examples:**

Graph with edges `1-2`, `2-3` (one component: `{1,2,3}`), edge `4-5` (a
second component), and isolated vertex `6` (a third component):

```csharp
GraphAlgorithms.CountConnectedComponents(graph); // 3
```

Empty graph (no vertices at all):

```csharp
GraphAlgorithms.CountConnectedComponents(graph); // 0
```

**Complexity target:** O(V + E) time (every vertex is visited exactly once
across all the component traversals combined), O(V) space.

### 6. NumIslands

The classic "Number of Islands" problem. Given a grid of `0`s (water) and
`1`s (land), count the number of islands — groups of land cells connected
4-directionally (up/down/left/right; **not** diagonally).

**Graphs aren't always adjacency lists.** A grid is a graph too — each cell
is a vertex, and its up-to-4 neighbors are computed on the fly from its
row/column position (`row±1, col` and `row, col±1`) rather than being
pre-computed and stored in a `Dictionary`/`List` like the `Graph` class
above does. This is a distinct, and very common, way to represent a graph:
whenever "neighbor" can be derived from position/coordinates by a formula,
you usually don't need to build an explicit adjacency list at all.

```csharp
public static int NumIslands(int[][] grid)
```

**Examples:**

```csharp
int[][] grid1 =
{
    new[] { 1, 1, 0, 0, 0 },
    new[] { 1, 1, 0, 0, 0 },
    new[] { 0, 0, 1, 0, 0 },
    new[] { 0, 0, 0, 1, 1 },
};
GraphAlgorithms.NumIslands(grid1); // 3

int[][] grid2 =
{
    new[] { 1, 0 },
    new[] { 0, 1 },
};
GraphAlgorithms.NumIslands(grid2); // 2 (diagonal doesn't connect them)
```

**Complexity target:** O(rows × cols) time (every cell visited at most a
constant number of times), O(rows × cols) space for the visited tracking
structure (or O(1) extra space if you mutate the input grid in place instead
— either is acceptable, but be explicit in your code about which you did).

## Hints

### Graph construction and operations

<details>
<summary>Hint 1</summary>

Think of `_adjacency` as the single source of truth. `AddVertex` should be a
"insert this key if it's not already there" operation — it must be safe to
call on a vertex that already exists (no-op, don't wipe out its neighbors).

</details>

<details>
<summary>Hint 2</summary>

`AddEdge(a, b)` should call your `AddVertex` logic for both `a` and `b`
first (so callers never have to remember to add vertices themselves), then
append `b` to `a`'s list AND append `a` to `b`'s list — undirected means both
directions need recording.

`Neighbors(v)` should use `TryGetValue` and return an empty `List<int>` (not
throw, not return `null`) when `v` isn't a known vertex.

</details>

<details>
<summary>Hint 3 (near-solution)</summary>

```csharp
public void AddVertex(int v)
{
    if (!_adjacency.ContainsKey(v))
    {
        _adjacency[v] = new List<int>();
    }
}

public void AddEdge(int a, int b)
{
    AddVertex(a);
    AddVertex(b);
    _adjacency[a].Add(b);
    _adjacency[b].Add(a);
}

public List<int> Neighbors(int v)
{
    if (_adjacency.TryGetValue(v, out var neighbors))
    {
        return neighbors;
    }
    return new List<int>();
}
```

</details>

### Bfs

<details>
<summary>Hint 1</summary>

You need three pieces of state: a `List<int>` for the result order, a
`HashSet<int>` for what's already been visited (or queued — mark it visited
the moment you enqueue it, not when you dequeue it, or you'll enqueue
duplicates), and a `Queue<int>` driving the traversal.

</details>

<details>
<summary>Hint 2</summary>

Mark `start` visited and enqueue it before the loop even begins. Then loop
while the queue isn't empty: dequeue a vertex, record it in the result list,
and for each of its neighbors that ISN'T yet in `visited`, mark it visited
and enqueue it.

</details>

<details>
<summary>Hint 3 (near-solution)</summary>

```csharp
var order = new List<int>();
var visited = new HashSet<int> { start };
var queue = new Queue<int>();
queue.Enqueue(start);

while (queue.Count > 0)
{
    int current = queue.Dequeue();
    order.Add(current);

    foreach (int neighbor in graph.Neighbors(current))
    {
        if (visited.Add(neighbor)) // HashSet.Add returns false if already present
        {
            queue.Enqueue(neighbor);
        }
    }
}
return order;
```

</details>

### Dfs

<details>
<summary>Hint 1</summary>

Write a small private recursive helper method — don't try to cram
everything into the public `Dfs` method. The public method's job is just to
set up the shared `visited` set and result list, then kick off the
recursion from `start`.

</details>

<details>
<summary>Hint 2</summary>

The recursive helper's job at each vertex: mark it visited, record it in the
result list, then loop over its neighbors and recurse into any that aren't
visited yet. The base case is implicit — a vertex with no unvisited
neighbors just returns, and the recursion naturally unwinds.

</details>

<details>
<summary>Hint 3 (near-solution)</summary>

```csharp
public static List<int> Dfs(Graph graph, int start)
{
    var order = new List<int>();
    var visited = new HashSet<int>();
    DfsVisit(graph, start, visited, order);
    return order;
}

private static void DfsVisit(Graph graph, int v, HashSet<int> visited, List<int> order)
{
    visited.Add(v);
    order.Add(v);

    foreach (int neighbor in graph.Neighbors(v))
    {
        if (!visited.Contains(neighbor))
        {
            DfsVisit(graph, neighbor, visited, order);
        }
    }
}
```

</details>

### HasPath

<details>
<summary>Hint 1</summary>

You've already built `Bfs` (or `Dfs`) — this problem doesn't need new
traversal logic, just a check bolted onto one. Handle `source == destination`
as a trivial `true` up front.

</details>

<details>
<summary>Hint 2</summary>

The simplest correct approach: run `Bfs(graph, source)` and check whether
the result `.Contains(destination)`. That's O(V + E) and totally fine. For a
small efficiency bonus, write a direct BFS loop that returns `true` the
instant `destination` is dequeued or discovered, instead of always exploring
the entire component first.

</details>

<details>
<summary>Hint 3 (near-solution)</summary>

```csharp
public static bool HasPath(Graph graph, int source, int destination)
{
    if (source == destination) return true;

    var visited = new HashSet<int> { source };
    var queue = new Queue<int>();
    queue.Enqueue(source);

    while (queue.Count > 0)
    {
        int current = queue.Dequeue();
        foreach (int neighbor in graph.Neighbors(current))
        {
            if (neighbor == destination) return true;
            if (visited.Add(neighbor)) queue.Enqueue(neighbor);
        }
    }
    return false;
}
```

</details>

### CountConnectedComponents

<details>
<summary>Hint 1</summary>

The key difference from the other problems: you must consider **every**
vertex in the graph, not just what's reachable from one start. Keep a single
`visited` set that persists across multiple traversals.

</details>

<details>
<summary>Hint 2</summary>

Loop over `graph.Vertices`. For each vertex not yet in `visited`, that's the
start of a brand-new, previously-unseen component: increment a counter, then
run a BFS (or DFS) from it and fold every vertex that traversal visits into
your shared `visited` set (so later iterations of the outer loop skip them).

</details>

<details>
<summary>Hint 3 (near-solution)</summary>

```csharp
public static int CountConnectedComponents(Graph graph)
{
    var visited = new HashSet<int>();
    int count = 0;

    foreach (int vertex in graph.Vertices)
    {
        if (!visited.Contains(vertex))
        {
            count++;
            foreach (int v in Bfs(graph, vertex))
            {
                visited.Add(v);
            }
        }
    }
    return count;
}
```

</details>

### NumIslands

<details>
<summary>Hint 1</summary>

There's no `Graph` object here — the grid itself IS the graph, with
neighbors computed from `(row, col)` arithmetic. You still need some
"visited" tracking, though, or you'll count the same island more than once
(or loop forever). A `bool[][]` the same shape as the grid works well.

</details>

<details>
<summary>Hint 2</summary>

Double loop over every `(row, col)`. Whenever you find a land cell
(`grid[row][col] == 1`) that hasn't been visited yet, that's a brand-new
island: increment your counter, then flood-fill outward from that cell
(BFS or DFS, your choice) marking every 4-directionally-reachable land cell
visited so the outer loop skips them later. Don't forget bounds checking —
`row-1` and `col-1` can go negative, `row+1`/`col+1` can run past the grid.

</details>

<details>
<summary>Hint 3 (near-solution)</summary>

```csharp
public static int NumIslands(int[][] grid)
{
    if (grid == null || grid.Length == 0) return 0;
    int rows = grid.Length, cols = grid[0].Length;
    var visited = new bool[rows][];
    for (int i = 0; i < rows; i++) visited[i] = new bool[cols];

    int islands = 0;
    for (int row = 0; row < rows; row++)
    {
        for (int col = 0; col < cols; col++)
        {
            if (grid[row][col] == 1 && !visited[row][col])
            {
                islands++;
                FloodFill(grid, visited, row, col, rows, cols); // BFS/DFS helper
            }
        }
    }
    return islands;
}

// FloodFill: BFS/DFS from (startRow, startCol), checking bounds and
// grid[r][c] == 1 && !visited[r][c] before enqueueing/recursing into each
// of the 4 neighbor directions (-1,0), (1,0), (0,-1), (0,1).
```

</details>

## Running your work

```
cd modules/08-graphs-bfs-dfs/tests/Graphs.Tests && dotnet test
```

## If you're stuck

`solution/Solution.cs` has a complete, commented reference implementation —
but try to get genuinely stuck first. Productive struggle (tracing a BFS
queue and a DFS call stack by hand on a small graph you draw on paper) is
where the learning actually happens; reading the answer too early just makes
it feel familiar without making it stick.

If you're offline with only a weak local Ollama model for help, get more out
of it by being specific:

- Paste the **exact method signature** you're implementing, not a vague
  description ("I'm implementing `public static List<int> Dfs(Graph graph,
  int start)` where `Graph` wraps a `Dictionary<int, List<int>>` — here's my
  current attempt: ...").
- State the **constraint or behavior** you're unsure about ("`Neighbors` on a
  vertex that was never added should return an empty list, not throw or
  return null — how do I express that with `TryGetValue`?").
- Ask for **approach before code** ("walk me through, in plain English, how
  flood-fill counts islands on a grid — don't write C# yet") — small local
  models tend to hallucinate less when reasoning in words first, and you'll
  learn more by translating the approach into code yourself once you
  understand the steps.
