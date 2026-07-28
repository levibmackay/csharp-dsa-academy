using Graphs;

namespace Graphs.Tests;

public class GraphTests
{
    [Fact]
    public void AddVertex_AddsVertexWithNoNeighbors()
    {
        var graph = new Graph();

        graph.AddVertex(1);

        Assert.Contains(1, graph.Vertices);
        Assert.Empty(graph.Neighbors(1));
    }

    [Fact]
    public void AddVertex_CalledTwiceForSameVertex_DoesNotDuplicateOrClearNeighbors()
    {
        var graph = new Graph();
        graph.AddEdge(1, 2);

        graph.AddVertex(1);

        Assert.Single(graph.Vertices, 1);
        Assert.Contains(2, graph.Neighbors(1));
    }

    [Fact]
    public void AddEdge_AddsBothDirections()
    {
        var graph = new Graph();

        graph.AddEdge(1, 2);

        Assert.Contains(2, graph.Neighbors(1));
        Assert.Contains(1, graph.Neighbors(2));
    }

    [Fact]
    public void AddEdge_ImplicitlyCreatesVertices()
    {
        var graph = new Graph();

        graph.AddEdge(5, 6);

        Assert.Contains(5, graph.Vertices);
        Assert.Contains(6, graph.Vertices);
    }

    [Fact]
    public void Neighbors_OnVertexNotInGraph_ReturnsEmptyList()
    {
        var graph = new Graph();

        var neighbors = graph.Neighbors(99);

        Assert.NotNull(neighbors);
        Assert.Empty(neighbors);
    }

    [Fact]
    public void Vertices_ReflectsAllAddedVertices()
    {
        var graph = new Graph();
        graph.AddEdge(1, 2);
        graph.AddVertex(3);

        Assert.Equal(new[] { 1, 2, 3 }, graph.Vertices.OrderBy(v => v));
    }
}

public class GraphAlgorithmsTests
{
    [Fact]
    public void Bfs_SingleVertex_ReturnsOnlyThatVertex()
    {
        var graph = new Graph();
        graph.AddVertex(1);

        var order = GraphAlgorithms.Bfs(graph, 1);

        Assert.Equal(new List<int> { 1 }, order);
    }

    [Fact]
    public void Bfs_VisitsAllReachableVerticesInBreadthFirstOrder()
    {
        // 1 -- 2 -- 4
        // |
        // 3
        var graph = new Graph();
        graph.AddEdge(1, 2);
        graph.AddEdge(1, 3);
        graph.AddEdge(2, 4);

        var order = GraphAlgorithms.Bfs(graph, 1);

        Assert.Equal(new List<int> { 1, 2, 3, 4 }, order);
    }

    [Fact]
    public void Bfs_DisconnectedVertices_OnlyVisitsReachableComponent()
    {
        var graph = new Graph();
        graph.AddEdge(1, 2);
        graph.AddVertex(3); // disconnected

        var order = GraphAlgorithms.Bfs(graph, 1);

        Assert.Equal(new List<int> { 1, 2 }, order);
    }

    [Fact]
    public void Dfs_SingleVertex_ReturnsOnlyThatVertex()
    {
        var graph = new Graph();
        graph.AddVertex(1);

        var order = GraphAlgorithms.Dfs(graph, 1);

        Assert.Equal(new List<int> { 1 }, order);
    }

    [Fact]
    public void Dfs_VisitsAllReachableVerticesDeterministically()
    {
        // 1 -- 2 -- 4
        // |
        // 3
        // Neighbor lists (in AddEdge insertion order):
        //   1: [2, 3]
        //   2: [1, 4]
        //   3: [1]
        //   4: [2]
        // Recursive DFS from 1 visits 2 first (before 3), then dives into
        // 2's neighbors (1 already visited, so 4), backtracks, then visits 3.
        var graph = new Graph();
        graph.AddEdge(1, 2);
        graph.AddEdge(1, 3);
        graph.AddEdge(2, 4);

        var order = GraphAlgorithms.Dfs(graph, 1);

        Assert.Equal(new List<int> { 1, 2, 4, 3 }, order);
    }

    [Fact]
    public void Dfs_DisconnectedVertices_OnlyVisitsReachableComponent()
    {
        var graph = new Graph();
        graph.AddEdge(1, 2);
        graph.AddVertex(3); // disconnected

        var order = GraphAlgorithms.Dfs(graph, 1);

        Assert.Equal(new List<int> { 1, 2 }, order);
    }

    [Fact]
    public void HasPath_DirectlyConnectedVertices_ReturnsTrue()
    {
        var graph = new Graph();
        graph.AddEdge(1, 2);

        Assert.True(GraphAlgorithms.HasPath(graph, 1, 2));
    }

    [Fact]
    public void HasPath_IndirectlyConnectedVertices_ReturnsTrue()
    {
        var graph = new Graph();
        graph.AddEdge(1, 2);
        graph.AddEdge(2, 3);

        Assert.True(GraphAlgorithms.HasPath(graph, 1, 3));
    }

    [Fact]
    public void HasPath_SourceEqualsDestination_ReturnsTrue()
    {
        var graph = new Graph();
        graph.AddVertex(1);

        Assert.True(GraphAlgorithms.HasPath(graph, 1, 1));
    }

    [Fact]
    public void HasPath_NoPathExists_ReturnsFalse()
    {
        var graph = new Graph();
        graph.AddEdge(1, 2);
        graph.AddVertex(3); // disconnected from 1/2

        Assert.False(GraphAlgorithms.HasPath(graph, 1, 3));
    }

    [Fact]
    public void CountConnectedComponents_EmptyGraph_ReturnsZero()
    {
        var graph = new Graph();

        Assert.Equal(0, GraphAlgorithms.CountConnectedComponents(graph));
    }

    [Fact]
    public void CountConnectedComponents_SingleVertex_ReturnsOne()
    {
        var graph = new Graph();
        graph.AddVertex(1);

        Assert.Equal(1, GraphAlgorithms.CountConnectedComponents(graph));
    }

    [Fact]
    public void CountConnectedComponents_MultipleDisconnectedComponents_CountsEach()
    {
        // Component A: 1-2-3
        // Component B: 4-5
        // Component C: 6 (isolated)
        var graph = new Graph();
        graph.AddEdge(1, 2);
        graph.AddEdge(2, 3);
        graph.AddEdge(4, 5);
        graph.AddVertex(6);

        Assert.Equal(3, GraphAlgorithms.CountConnectedComponents(graph));
    }

    [Fact]
    public void CountConnectedComponents_FullyConnectedGraph_ReturnsOne()
    {
        var graph = new Graph();
        graph.AddEdge(1, 2);
        graph.AddEdge(2, 3);
        graph.AddEdge(3, 4);

        Assert.Equal(1, GraphAlgorithms.CountConnectedComponents(graph));
    }

    [Fact]
    public void NumIslands_EmptyGrid_ReturnsZero()
    {
        var grid = Array.Empty<int[]>();

        Assert.Equal(0, GraphAlgorithms.NumIslands(grid));
    }

    [Fact]
    public void NumIslands_AllWater_ReturnsZero()
    {
        var grid = new[]
        {
            new[] { 0, 0, 0 },
            new[] { 0, 0, 0 },
            new[] { 0, 0, 0 },
        };

        Assert.Equal(0, GraphAlgorithms.NumIslands(grid));
    }

    [Fact]
    public void NumIslands_AllLand_ReturnsOne()
    {
        var grid = new[]
        {
            new[] { 1, 1 },
            new[] { 1, 1 },
        };

        Assert.Equal(1, GraphAlgorithms.NumIslands(grid));
    }

    [Fact]
    public void NumIslands_SingleCellLand_ReturnsOne()
    {
        var grid = new[] { new[] { 1 } };

        Assert.Equal(1, GraphAlgorithms.NumIslands(grid));
    }

    [Fact]
    public void NumIslands_SingleCellWater_ReturnsZero()
    {
        var grid = new[] { new[] { 0 } };

        Assert.Equal(0, GraphAlgorithms.NumIslands(grid));
    }

    [Fact]
    public void NumIslands_MultipleSeparateIslands_CountsEach()
    {
        var grid = new[]
        {
            new[] { 1, 1, 0, 0, 0 },
            new[] { 1, 1, 0, 0, 0 },
            new[] { 0, 0, 1, 0, 0 },
            new[] { 0, 0, 0, 1, 1 },
        };

        Assert.Equal(3, GraphAlgorithms.NumIslands(grid));
    }

    [Fact]
    public void NumIslands_DiagonalLandCellsAreNotConnected()
    {
        // Diagonal adjacency does NOT count -- only 4-directional.
        var grid = new[]
        {
            new[] { 1, 0 },
            new[] { 0, 1 },
        };

        Assert.Equal(2, GraphAlgorithms.NumIslands(grid));
    }
}
