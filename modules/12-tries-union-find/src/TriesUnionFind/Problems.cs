namespace TriesUnionFind;

/// <summary>
/// Applied problems that are solved using <see cref="UnionFind"/>. Both
/// problems here are classic "dynamic connectivity" questions: given a set
/// of direct connections, answer questions about which things end up in
/// the same group.
/// </summary>
public static class Problems
{
    /// <summary>
    /// Given an n x n adjacency matrix <paramref name="isConnected"/> where
    /// isConnected[i][j] == 1 means city i and city j are DIRECTLY
    /// connected (the matrix is symmetric and isConnected[i][i] == 1 for
    /// all i), returns the number of provinces -- groups of cities that
    /// are directly or indirectly connected.
    ///
    /// Must be solved using UnionFind: union every pair (i, j) where
    /// isConnected[i][j] == 1, then the answer is UnionFind.CountSets.
    /// </summary>
    /// <param name="isConnected">n x n symmetric 0/1 adjacency matrix.</param>
    /// <returns>The number of provinces.</returns>
    /// <example>
    /// isConnected = [[1,1,0],
    ///                [1,1,0],
    ///                [0,0,1]]
    /// Problems.CountProvinces(isConnected); // 2
    /// </example>
    /// <remarks>Time: O(n^2 * alpha(n)). Space: O(n).</remarks>
    public static int CountProvinces(int[][] isConnected)
    {
        // TODO:
        // 1. Let n = isConnected.Length. Create a new UnionFind(n).
        // 2. For each pair (i, j) with i < j (no need to check j <= i,
        //    the matrix is symmetric and the diagonal is always 1), if
        //    isConnected[i][j] == 1, call uf.Union(i, j).
        // 3. Return uf.CountSets.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Given n nodes (0..n-1), an undirected edge list <paramref name="edges"/>
    /// (each entry is a 2-element array [a, b] meaning a-b is an edge), and
    /// a list of (a, b) tuple queries, returns a bool[] where result[i]
    /// indicates whether queries[i].a and queries[i].b are in the same
    /// connected component.
    ///
    /// Must be solved using UnionFind: union every edge first, then answer
    /// each query with a single Connected() call.
    /// </summary>
    /// <param name="n">The number of nodes (0..n-1).</param>
    /// <param name="edges">Undirected edges, each a 2-element [a, b] array.</param>
    /// <param name="queries">
    /// Queries as named tuples (a, b) -- see the README's tuple refresher
    /// if the (int a, int b)[] syntax is unfamiliar.
    /// </param>
    /// <returns>
    /// A bool[] the same length as queries; result[i] is true iff
    /// queries[i].a and queries[i].b are connected.
    /// </returns>
    /// <example>
    /// int n = 5;
    /// int[][] edges = { new[] { 0, 1 }, new[] { 1, 2 }, new[] { 3, 4 } };
    /// var queries = new (int a, int b)[] { (0, 2), (0, 4), (3, 4) };
    /// Problems.Reachable(n, edges, queries); // [true, false, true]
    /// </example>
    /// <remarks>Time: O((n + E + Q) * alpha(n)). Space: O(n).</remarks>
    public static bool[] Reachable(int n, int[][] edges, (int a, int b)[] queries)
    {
        // TODO:
        // 1. Create a new UnionFind(n).
        // 2. For each edge in `edges`, call uf.Union(edge[0], edge[1]).
        // 3. Create a bool[queries.Length] result array.
        // 4. For each query at index i, set result[i] = uf.Connected(queries[i].a, queries[i].b).
        // 5. Return result.
        throw new NotImplementedException();
    }
}
