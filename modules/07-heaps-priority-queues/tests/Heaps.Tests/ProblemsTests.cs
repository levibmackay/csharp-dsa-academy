using System.Collections.Generic;
using System.Linq;
using Heaps;

namespace Heaps.Tests;

public class ProblemsTests
{
    // ---- FindKthLargest ----

    [Theory]
    [InlineData(new[] { 3, 2, 1, 5, 6, 4 }, 2, 5)]
    [InlineData(new[] { 3, 2, 3, 1, 2, 4, 5, 5, 6 }, 4, 4)]
    public void FindKthLargest_ReturnsExpectedValue(int[] nums, int k, int expected)
    {
        int result = Problems.FindKthLargest(nums, k);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void FindKthLargest_KEqualsOne_ReturnsMax()
    {
        int[] nums = { 4, 1, 9, 7 };

        int result = Problems.FindKthLargest(nums, 1);

        Assert.Equal(9, result);
    }

    [Fact]
    public void FindKthLargest_KEqualsArrayLength_ReturnsMin()
    {
        int[] nums = { 4, 1, 9, 7 };

        int result = Problems.FindKthLargest(nums, nums.Length);

        Assert.Equal(1, result);
    }

    [Fact]
    public void FindKthLargest_SingleElement_ReturnsThatElement()
    {
        int[] nums = { 42 };

        int result = Problems.FindKthLargest(nums, 1);

        Assert.Equal(42, result);
    }

    [Fact]
    public void FindKthLargest_WithDuplicates_HandlesTies()
    {
        int[] nums = { 1, 1, 1, 1 };

        int result = Problems.FindKthLargest(nums, 3);

        Assert.Equal(1, result);
    }

    // ---- TopKFrequent ----

    [Fact]
    public void TopKFrequent_ReturnsMostFrequentElements_AsSet()
    {
        int[] nums = { 1, 1, 1, 2, 2, 3 };

        var result = Problems.TopKFrequent(nums, 2);

        Assert.Equal(new HashSet<int> { 1, 2 }, result.ToHashSet());
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void TopKFrequent_KEqualsOne_ReturnsSingleMostFrequent()
    {
        int[] nums = { 5, 5, 5, 6, 6, 7 };

        var result = Problems.TopKFrequent(nums, 1);

        Assert.Single(result);
        Assert.Equal(5, result[0]);
    }

    [Fact]
    public void TopKFrequent_KEqualsDistinctCount_ReturnsAllDistinctValues()
    {
        int[] nums = { 1, 2, 3 };

        var result = Problems.TopKFrequent(nums, 3);

        Assert.Equal(new HashSet<int> { 1, 2, 3 }, result.ToHashSet());
    }

    [Fact]
    public void TopKFrequent_AllElementsSameFrequency_ReturnsKElements()
    {
        int[] nums = { 1, 2, 3, 4 };

        var result = Problems.TopKFrequent(nums, 2);

        Assert.Equal(2, result.Count);
        // Every returned value must have actually been in the input.
        Assert.All(result, value => Assert.Contains(value, nums));
    }

    [Fact]
    public void TopKFrequent_SingleElement_ReturnsIt()
    {
        int[] nums = { 9 };

        var result = Problems.TopKFrequent(nums, 1);

        Assert.Equal(new List<int> { 9 }, result);
    }

    // ---- MergeKSortedLists ----

    [Fact]
    public void MergeKSortedLists_MergesMultipleListsInOrder()
    {
        var lists = new List<List<int>>
        {
            new() { 1, 4, 5 },
            new() { 1, 3, 4 },
            new() { 2, 6 },
        };

        var result = Problems.MergeKSortedLists(lists);

        Assert.Equal(new List<int> { 1, 1, 2, 3, 4, 4, 5, 6 }, result);
    }

    [Fact]
    public void MergeKSortedLists_EmptyListOfLists_ReturnsEmpty()
    {
        var lists = new List<List<int>>();

        var result = Problems.MergeKSortedLists(lists);

        Assert.Empty(result);
    }

    [Fact]
    public void MergeKSortedLists_SomeEmptySublists_AreSkipped()
    {
        var lists = new List<List<int>>
        {
            new(),
            new() { 2, 5 },
            new(),
        };

        var result = Problems.MergeKSortedLists(lists);

        Assert.Equal(new List<int> { 2, 5 }, result);
    }

    [Fact]
    public void MergeKSortedLists_SingleList_ReturnsItUnchanged()
    {
        var lists = new List<List<int>> { new() { 1, 2, 3 } };

        var result = Problems.MergeKSortedLists(lists);

        Assert.Equal(new List<int> { 1, 2, 3 }, result);
    }

    [Fact]
    public void MergeKSortedLists_AllListsEmpty_ReturnsEmpty()
    {
        var lists = new List<List<int>> { new(), new(), new() };

        var result = Problems.MergeKSortedLists(lists);

        Assert.Empty(result);
    }

    [Fact]
    public void MergeKSortedLists_WithDuplicateValuesAcrossLists_PreservesAll()
    {
        var lists = new List<List<int>>
        {
            new() { 1, 1, 1 },
            new() { 1, 1 },
        };

        var result = Problems.MergeKSortedLists(lists);

        Assert.Equal(new List<int> { 1, 1, 1, 1, 1 }, result);
    }
}
