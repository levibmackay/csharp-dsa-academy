namespace Graphs;

/// <summary>
/// Static algorithms that operate on <see cref="Graph"/> (and, for
/// NumIslands, directly on a grid) using breadth-first and depth-first
/// traversal.
/// </summary>
public static class GraphAlgorithms
{
    /// <summary>
    /// Performs a breadth-first traversal of <paramref name="graph"/> starting
    /// at <paramref name="start"/>, returning vertices in the order they were
    /// first visited.
    /// </summary>
    /// <param name="graph">The graph to traverse.</param>
    /// <param name="start">The starting vertex.</param>
    /// <returns>Vertices in BFS visit order.</returns>
    public static List<int> Bfs(Graph graph, int start)
    {
        // TODO: Classic BFS.
        // 1. Create a List<int> `order` for the result, a HashSet<int> `visited`,
        //    and a Queue<int> `queue`.
        // 2. Mark `start` visited, enqueue it.
        // 3. While the queue isn't empty: Dequeue a vertex, add it to `order`,
        //    then for each unvisited neighbor: mark visited and Enqueue it.
        // 4. Return `order`.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Performs a depth-first traversal of <paramref name="graph"/> starting
    /// at <paramref name="start"/>, returning vertices in the order they were
    /// first visited.
    ///
    /// This is implemented RECURSIVELY (rather than with an explicit
    /// Stack&lt;int&gt;) for simplicity and a deterministic visit order that
    /// exactly mirrors each vertex's Neighbors list. An iterative version
    /// using Stack&lt;int&gt; is possible and uses less call-stack memory on
    /// very deep graphs, but its visit order can differ depending on the
    /// order neighbors are pushed (since a stack reverses the order you pop
    /// things back out in).
    /// </summary>
    /// <param name="graph">The graph to traverse.</param>
    /// <param name="start">The starting vertex.</param>
    /// <returns>Vertices in DFS visit order.</returns>
    public static List<int> Dfs(Graph graph, int start)
    {
        // TODO:
        // 1. Create a List<int> `order` and HashSet<int> `visited`.
        // 2. Call a private recursive helper, e.g. DfsVisit(graph, start, visited, order).
        // 3. Return `order`.
        throw new NotImplementedException();
    }

    // TODO: Implement a private recursive helper for Dfs, e.g.:
    // private static void DfsVisit(Graph graph, int v, HashSet<int> visited, List<int> order)
    // {
    //     mark v visited, add to order, then recurse into each unvisited neighbor.
    // }

    /// <summary>
    /// Determines whether a path exists between <paramref name="source"/> and
    /// <paramref name="destination"/> in <paramref name="graph"/>.
    /// </summary>
    /// <param name="graph">The graph to search.</param>
    /// <param name="source">Starting vertex.</param>
    /// <param name="destination">Target vertex.</param>
    /// <returns>True if a path exists (including source == destination
    /// when source is a vertex in the graph); false otherwise.</returns>
    public static bool HasPath(Graph graph, int source, int destination)
    {
        // TODO: Run a BFS (or DFS) from `source` and check whether
        // `destination` is ever visited. You can reuse Bfs(graph, source)
        // and check `.Contains(destination)`, or write a direct traversal
        // for efficiency (early-exit once destination is found).
        throw new NotImplementedException();
    }

    /// <summary>
    /// Counts the number of connected components across the whole graph
    /// (i.e. across all vertices, not just those reachable from one start).
    /// </summary>
    /// <param name="graph">The graph to inspect.</param>
    /// <returns>The number of connected components.</returns>
    public static int CountConnectedComponents(Graph graph)
    {
        // TODO:
        // 1. Create a HashSet<int> `visited`.
        // 2. For each vertex in graph.Vertices: if not yet visited, that's a
        //    new component -- run a BFS/DFS from it, add every visited
        //    vertex to `visited`, and increment a counter.
        // 3. Return the counter.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Classic "Number of Islands" flood-fill problem. Given a grid where
    /// 0 = water and 1 = land, counts the number of islands (groups of land
    /// cells connected 4-directionally: up/down/left/right).
    ///
    /// This works directly on the grid rather than building a Graph /
    /// adjacency list -- neighbors are computed on the fly from row/col
    /// arithmetic instead of being stored.
    /// </summary>
    /// <param name="grid">A jagged array of 0s (water) and 1s (land).</param>
    /// <returns>The number of islands.</returns>
    public static int NumIslands(int[][] grid)
    {
        // TODO:
        // 1. Handle the empty-grid edge case (grid == null || grid.Length == 0).
        // 2. Create a visited structure, e.g. a bool[][] the same shape as
        //    `grid` (or mutate the grid in place by setting visited land to
        //    0 -- either approach is fine, but be explicit about which one
        //    you pick).
        // 3. Loop over every (row, col). When you find an unvisited land
        //    cell (grid[row][col] == 1 and not visited), that's a new
        //    island: increment the count and flood-fill outward (BFS or
        //    DFS) marking every 4-directionally-connected land cell visited.
        // 4. Return the island count.
        throw new NotImplementedException();
    }
}
