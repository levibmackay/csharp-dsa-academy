namespace Graphs;

/// <summary>
/// An undirected, unweighted graph backed by an adjacency list
/// (a Dictionary mapping each vertex to the List of its neighbors).
/// </summary>
public class Graph
{
    private readonly Dictionary<int, List<int>> _adjacency = new();

    /// <summary>
    /// All vertices currently in the graph.
    /// </summary>
    public IEnumerable<int> Vertices => _adjacency.Keys;

    /// <summary>
    /// Adds a vertex to the graph. If the vertex already exists, this is a no-op.
    /// </summary>
    /// <param name="v">The vertex to add.</param>
    public void AddVertex(int v)
    {
        // TODO: If `v` is not already a key in the adjacency dictionary,
        // add it with a new empty List<int> as its neighbor list.
        // Hint: Dictionary<TKey,TValue>.ContainsKey or TryGetValue/TryAdd.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds an undirected edge between <paramref name="a"/> and <paramref name="b"/>.
    /// Implicitly ensures both vertices exist (callers do NOT need to call
    /// AddVertex first).
    /// </summary>
    /// <param name="a">First vertex.</param>
    /// <param name="b">Second vertex.</param>
    public void AddEdge(int a, int b)
    {
        // TODO:
        // 1. Ensure both `a` and `b` exist as vertices (reuse AddVertex).
        // 2. Add `b` to `a`'s neighbor list, and `a` to `b`'s neighbor list
        //    (undirected -> both directions).
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the list of neighbors for vertex <paramref name="v"/>.
    /// </summary>
    /// <param name="v">The vertex whose neighbors to look up.</param>
    /// <returns>A List of adjacent vertices, or an empty list if `v` has no
    /// recorded neighbors.</returns>
    public List<int> Neighbors(int v)
    {
        // TODO: Use TryGetValue to return the neighbor list for `v`.
        // If `v` isn't in the graph, return a new empty List<int> rather
        // than throwing.
        throw new NotImplementedException();
    }
}
