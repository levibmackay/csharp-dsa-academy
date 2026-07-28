using TriesUnionFind;

namespace TriesUnionFind.Tests;

public class ProblemsTests
{
    [Fact]
    public void CountProvinces_AllConnected_ReturnsOne()
    {
        int[][] isConnected =
        {
            new[] { 1, 1, 1 },
            new[] { 1, 1, 1 },
            new[] { 1, 1, 1 },
        };

        Assert.Equal(1, Problems.CountProvinces(isConnected));
    }

    [Fact]
    public void CountProvinces_AllIsolated_ReturnsNCount()
    {
        int[][] isConnected =
        {
            new[] { 1, 0, 0 },
            new[] { 0, 1, 0 },
            new[] { 0, 0, 1 },
        };

        Assert.Equal(3, Problems.CountProvinces(isConnected));
    }

    [Fact]
    public void CountProvinces_MixedGroups_ReturnsTwo()
    {
        int[][] isConnected =
        {
            new[] { 1, 1, 0 },
            new[] { 1, 1, 0 },
            new[] { 0, 0, 1 },
        };

        Assert.Equal(2, Problems.CountProvinces(isConnected));
    }

    [Fact]
    public void CountProvinces_TransitiveConnection_MergesAcrossIntermediateCity()
    {
        // 0-1 and 1-2 are directly connected, but 0-2 is not directly
        // marked -- they must still land in the same province.
        int[][] isConnected =
        {
            new[] { 1, 1, 0 },
            new[] { 1, 1, 1 },
            new[] { 0, 1, 1 },
        };

        Assert.Equal(1, Problems.CountProvinces(isConnected));
    }

    [Fact]
    public void Reachable_ConnectedAndDisconnectedQueries_ReturnsExpectedResults()
    {
        var n = 5;
        int[][] edges =
        {
            new[] { 0, 1 },
            new[] { 1, 2 },
            new[] { 3, 4 },
        };
        var queries = new (int a, int b)[]
        {
            (0, 2), // connected via 0-1-2
            (0, 4), // disconnected: {0,1,2} vs {3,4}
            (3, 4), // directly connected
        };

        var result = Problems.Reachable(n, edges, queries);

        Assert.Equal(new[] { true, false, true }, result);
    }

    [Fact]
    public void Reachable_SelfQuery_IsAlwaysTrue()
    {
        var n = 3;
        int[][] edges = Array.Empty<int[]>();
        var queries = new (int a, int b)[] { (0, 0), (2, 2) };

        var result = Problems.Reachable(n, edges, queries);

        Assert.Equal(new[] { true, true }, result);
    }

    [Fact]
    public void Reachable_NoEdges_OnlySelfQueriesAreTrue()
    {
        var n = 4;
        int[][] edges = Array.Empty<int[]>();
        var queries = new (int a, int b)[] { (0, 1), (1, 1), (2, 3) };

        var result = Problems.Reachable(n, edges, queries);

        Assert.Equal(new[] { false, true, false }, result);
    }
}
