// Reference solution — only read this after a real attempt.
//
// This file lives outside any .csproj (the solution/ directory is not
// referenced by src/Graphs/Graphs.csproj or tests/Graphs.Tests/Graphs.Tests.csproj)
// so it is never compiled as part of the build or test run. It's here purely
// as reading material once you've given the problems a real shot.

namespace Graphs;

public class Graph
{
    private readonly Dictionary<int, List<int>> _adjacency = new();

    public IEnumerable<int> Vertices => _adjacency.Keys;

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
}

public static class GraphAlgorithms
{
    public static List<int> Bfs(Graph graph, int start)
    {
        var order = new List<int>();
        var visited = new HashSet<int>();
        var queue = new Queue<int>();

        visited.Add(start);
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            order.Add(current);

            foreach (var neighbor in graph.Neighbors(current))
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return order;
    }

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

        foreach (var neighbor in graph.Neighbors(v))
        {
            if (!visited.Contains(neighbor))
            {
                DfsVisit(graph, neighbor, visited, order);
            }
        }
    }

    public static bool HasPath(Graph graph, int source, int destination)
    {
        if (source == destination)
        {
            return true;
        }

        var visited = new HashSet<int> { source };
        var queue = new Queue<int>();
        queue.Enqueue(source);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            foreach (var neighbor in graph.Neighbors(current))
            {
                if (neighbor == destination)
                {
                    return true;
                }

                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        return false;
    }

    public static int CountConnectedComponents(Graph graph)
    {
        var visited = new HashSet<int>();
        var count = 0;

        foreach (var vertex in graph.Vertices)
        {
            if (!visited.Contains(vertex))
            {
                count++;
                var component = Bfs(graph, vertex);
                foreach (var v in component)
                {
                    visited.Add(v);
                }
            }
        }

        return count;
    }

    public static int NumIslands(int[][] grid)
    {
        if (grid == null || grid.Length == 0)
        {
            return 0;
        }

        var rows = grid.Length;
        var cols = grid[0].Length;
        var visited = new bool[rows][];
        for (var i = 0; i < rows; i++)
        {
            visited[i] = new bool[cols];
        }

        var islands = 0;

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                if (grid[row][col] == 1 && !visited[row][col])
                {
                    islands++;
                    FloodFill(grid, visited, row, col, rows, cols);
                }
            }
        }

        return islands;
    }

    private static void FloodFill(int[][] grid, bool[][] visited, int startRow, int startCol, int rows, int cols)
    {
        var queue = new Queue<(int Row, int Col)>();
        queue.Enqueue((startRow, startCol));
        visited[startRow][startCol] = true;

        while (queue.Count > 0)
        {
            var (row, col) = queue.Dequeue();

            (int dRow, int dCol)[] directions =
            {
                (-1, 0), (1, 0), (0, -1), (0, 1)
            };

            foreach (var (dRow, dCol) in directions)
            {
                var newRow = row + dRow;
                var newCol = col + dCol;

                var inBounds = newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols;
                if (inBounds && grid[newRow][newCol] == 1 && !visited[newRow][newCol])
                {
                    visited[newRow][newCol] = true;
                    queue.Enqueue((newRow, newCol));
                }
            }
        }
    }
}
