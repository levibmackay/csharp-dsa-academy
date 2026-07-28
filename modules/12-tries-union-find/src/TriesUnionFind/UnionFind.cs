namespace TriesUnionFind;

/// <summary>
/// A disjoint-set / union-find (DSU) structure over the elements
/// 0..size-1. Each element starts in its own set; Union merges two sets and
/// Find/Connected answer "which set is this in" / "are these two in the
/// same set" queries. Combines path compression (in Find) with union by
/// rank (in Union) so that after any sequence of operations, each operation
/// runs in near-O(1) amortized time (technically O(alpha(n)), where alpha
/// is the inverse Ackermann function -- for any n you could ever fit in
/// memory, alpha(n) is at most 4, so "near constant" is not an
/// exaggeration).
/// </summary>
public class UnionFind
{
    private readonly int[] _parent;
    private readonly int[] _rank;
    private int _countSets;

    /// <summary>
    /// The number of distinct sets currently remaining. Starts at
    /// <c>size</c> (every element in its own set) and decreases by one
    /// each time a Union call actually merges two previously-separate sets.
    /// </summary>
    public int CountSets => _countSets;

    /// <summary>
    /// Creates a union-find over elements 0..size-1, each initially in its
    /// own singleton set.
    /// </summary>
    /// <param name="size">The number of elements (must be &gt;= 0).</param>
    public UnionFind(int size)
    {
        // This constructor is plain structural setup (not the algorithmic
        // part of the exercise), so it's filled in for you: every element
        // starts as its own root (parent[i] == i) with rank 0, and every
        // element counts as its own set.
        _parent = new int[size];
        _rank = new int[size];

        for (var i = 0; i < size; i++)
        {
            _parent[i] = i;
            _rank[i] = 0;
        }

        _countSets = size;
    }

    /// <summary>
    /// Finds the representative ("root") of the set containing
    /// <paramref name="x"/>, applying path compression along the way so
    /// future Find calls on `x` (and everything on the path to its root)
    /// are faster.
    /// </summary>
    /// <param name="x">The element to find the root of.</param>
    /// <returns>The root element representing x's set.</returns>
    public int Find(int x)
    {
        // TODO: Path compression.
        // 1. Base case: if _parent[x] == x, x IS the root -- return x.
        // 2. Otherwise, recursively find the root of _parent[x], THEN set
        //    _parent[x] = <that root> before returning it. This is the
        //    "compression" step: it rewires x to point directly at the
        //    root instead of at its old (possibly distant) parent, so the
        //    next Find(x) is O(1).
        //    e.g.: _parent[x] = Find(_parent[x]); return _parent[x];
        throw new NotImplementedException();
    }

    /// <summary>
    /// Merges the sets containing <paramref name="x"/> and
    /// <paramref name="y"/> into one, using union by rank (the root with
    /// the smaller rank is attached under the root with the larger rank,
    /// which keeps the resulting trees shallow). If x and y are already in
    /// the same set, this is a no-op.
    /// </summary>
    /// <param name="x">First element.</param>
    /// <param name="y">Second element.</param>
    public void Union(int x, int y)
    {
        // TODO: Union by rank.
        // 1. Find the root of x and the root of y (rootX = Find(x), rootY = Find(y)).
        // 2. If rootX == rootY, they're already in the same set -- return,
        //    do nothing else (do NOT decrement _countSets).
        // 3. Otherwise, attach the shorter tree under the taller one:
        //    - if _rank[rootX] < _rank[rootY]: _parent[rootX] = rootY
        //    - else if _rank[rootX] > _rank[rootY]: _parent[rootY] = rootX
        //    - else (equal ranks): pick either as the new root, e.g.
        //      _parent[rootY] = rootX, and increment _rank[rootX] by 1
        //      (the tree got one level taller only in this tie case).
        // 4. Decrement _countSets by 1 (two sets became one).
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns true if <paramref name="x"/> and <paramref name="y"/> are
    /// currently in the same set.
    /// </summary>
    /// <param name="x">First element.</param>
    /// <param name="y">Second element.</param>
    public bool Connected(int x, int y)
    {
        // TODO: Two elements are in the same set exactly when they share
        // the same root: return Find(x) == Find(y).
        throw new NotImplementedException();
    }
}
