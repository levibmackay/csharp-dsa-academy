// Reference solution — only read this after a real attempt.
//
// This file lives outside any .csproj (the solution/ directory is not
// referenced by src/TriesUnionFind/TriesUnionFind.csproj or
// tests/TriesUnionFind.Tests/TriesUnionFind.Tests.csproj) so it is never
// compiled as part of the build or test run. It's here purely as reading
// material once you've given the problems a real shot.

namespace TriesUnionFind;

public class Trie
{
    private class TrieNode
    {
        public Dictionary<char, TrieNode> Children { get; } = new();
        public bool IsEndOfWord { get; set; }
    }

    private readonly TrieNode _root = new();

    public void Insert(string word)
    {
        var current = _root;

        foreach (var c in word)
        {
            if (!current.Children.TryGetValue(c, out var next))
            {
                next = new TrieNode();
                current.Children[c] = next;
            }

            current = next;
        }

        current.IsEndOfWord = true;
    }

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

        return current.IsEndOfWord;
    }

    public bool StartsWith(string prefix)
    {
        var current = _root;

        foreach (var c in prefix)
        {
            if (!current.Children.TryGetValue(c, out var next))
            {
                return false;
            }

            current = next;
        }

        return true;
    }
}

public class UnionFind
{
    private readonly int[] _parent;
    private readonly int[] _rank;
    private int _countSets;

    public int CountSets => _countSets;

    public UnionFind(int size)
    {
        _parent = new int[size];
        _rank = new int[size];

        for (var i = 0; i < size; i++)
        {
            _parent[i] = i;
            _rank[i] = 0;
        }

        _countSets = size;
    }

    public int Find(int x)
    {
        if (_parent[x] == x)
        {
            return x;
        }

        _parent[x] = Find(_parent[x]);
        return _parent[x];
    }

    public void Union(int x, int y)
    {
        var rootX = Find(x);
        var rootY = Find(y);

        if (rootX == rootY)
        {
            return;
        }

        if (_rank[rootX] < _rank[rootY])
        {
            _parent[rootX] = rootY;
        }
        else if (_rank[rootX] > _rank[rootY])
        {
            _parent[rootY] = rootX;
        }
        else
        {
            _parent[rootY] = rootX;
            _rank[rootX]++;
        }

        _countSets--;
    }

    public bool Connected(int x, int y)
    {
        return Find(x) == Find(y);
    }
}

public static class Problems
{
    public static int CountProvinces(int[][] isConnected)
    {
        var n = isConnected.Length;
        var uf = new UnionFind(n);

        for (var i = 0; i < n; i++)
        {
            for (var j = i + 1; j < n; j++)
            {
                if (isConnected[i][j] == 1)
                {
                    uf.Union(i, j);
                }
            }
        }

        return uf.CountSets;
    }

    public static bool[] Reachable(int n, int[][] edges, (int a, int b)[] queries)
    {
        var uf = new UnionFind(n);

        foreach (var edge in edges)
        {
            uf.Union(edge[0], edge[1]);
        }

        var result = new bool[queries.Length];

        for (var i = 0; i < queries.Length; i++)
        {
            result[i] = uf.Connected(queries[i].a, queries[i].b);
        }

        return result;
    }
}
