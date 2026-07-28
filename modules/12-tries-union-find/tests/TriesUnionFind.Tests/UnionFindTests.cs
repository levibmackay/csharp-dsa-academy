using TriesUnionFind;

namespace TriesUnionFind.Tests;

public class UnionFindTests
{
    [Fact]
    public void SingleElement_IsItsOwnSet()
    {
        var uf = new UnionFind(1);

        Assert.Equal(0, uf.Find(0));
        Assert.True(uf.Connected(0, 0));
        Assert.Equal(1, uf.CountSets);
    }

    [Fact]
    public void NewUnionFind_EveryElementIsItsOwnSet()
    {
        var uf = new UnionFind(5);

        Assert.Equal(5, uf.CountSets);
        for (var i = 0; i < 5; i++)
        {
            Assert.Equal(i, uf.Find(i));
        }
    }

    [Fact]
    public void Union_MergesTwoSets_ConnectedBecomesTrueAndCountSetsDecreases()
    {
        var uf = new UnionFind(5);

        uf.Union(0, 1);

        Assert.True(uf.Connected(0, 1));
        Assert.Equal(4, uf.CountSets);
    }

    [Fact]
    public void Union_OnAlreadyConnectedElements_IsNoOpAndDoesNotChangeCountSets()
    {
        var uf = new UnionFind(5);
        uf.Union(0, 1);

        uf.Union(1, 0);
        uf.Union(0, 1);

        Assert.Equal(4, uf.CountSets);
    }

    [Fact]
    public void Connected_OnUnrelatedElements_IsFalse()
    {
        var uf = new UnionFind(5);

        uf.Union(0, 1);

        Assert.False(uf.Connected(0, 2));
        Assert.False(uf.Connected(2, 3));
    }

    [Fact]
    public void CountSets_AfterSeveralUnions_ReflectsDistinctRemainingGroups()
    {
        var uf = new UnionFind(10);

        uf.Union(0, 1);
        uf.Union(2, 3);
        uf.Union(4, 5);
        uf.Union(0, 2);

        // groups: {0,1,2,3}, {4,5}, {6}, {7}, {8}, {9} -> 6 sets
        Assert.Equal(6, uf.CountSets);
        Assert.True(uf.Connected(1, 3));
        Assert.False(uf.Connected(1, 4));
    }

    [Fact]
    public void Union_TransitiveChain_AllElementsEndUpConnected()
    {
        var uf = new UnionFind(4);

        uf.Union(0, 1);
        uf.Union(1, 2);
        uf.Union(2, 3);

        Assert.True(uf.Connected(0, 3));
        Assert.Equal(1, uf.CountSets);
    }

    [Fact]
    public void WorkedExampleTrace_FromReadme_MatchesStepByStepExpectations()
    {
        // Mirrors the README's worked example: parent = [0,1,2,3,4] for 5
        // elements, then union(0,1), union(2,3), union(0,2), then Find(3).
        var uf = new UnionFind(5);

        // After union(0, 1): 0 and 1 are one set; everything else alone.
        uf.Union(0, 1);
        Assert.True(uf.Connected(0, 1));
        Assert.Equal(4, uf.CountSets);

        // After union(2, 3): 2 and 3 are one set.
        uf.Union(2, 3);
        Assert.True(uf.Connected(2, 3));
        Assert.Equal(3, uf.CountSets);

        // After union(0, 2): the {0,1} set and {2,3} set merge into one
        // set of size 4; element 4 remains alone.
        uf.Union(0, 2);
        Assert.True(uf.Connected(0, 3));
        Assert.True(uf.Connected(1, 2));
        Assert.False(uf.Connected(0, 4));
        Assert.Equal(2, uf.CountSets);

        // Find(3) must resolve to the same root as every other element in
        // the merged set (path compression must not change *which* set
        // anything belongs to, only the internal pointers).
        var root = uf.Find(3);
        Assert.Equal(uf.Find(0), root);
        Assert.Equal(uf.Find(1), root);
        Assert.Equal(uf.Find(2), root);
    }
}
