using Searching;

namespace Searching.Tests;

public class BinarySearchTests
{
    [Fact]
    public void EmptyArray_ReturnsNotFound()
    {
        int[] arr = Array.Empty<int>();
        Assert.Equal(-1, Problems.BinarySearch(arr, 5));
    }

    [Fact]
    public void SingleElement_Found()
    {
        int[] arr = { 7 };
        Assert.Equal(0, Problems.BinarySearch(arr, 7));
    }

    [Fact]
    public void SingleElement_NotFound()
    {
        int[] arr = { 7 };
        Assert.Equal(-1, Problems.BinarySearch(arr, 3));
    }

    [Fact]
    public void TargetAtFirstIndex()
    {
        int[] arr = { -8, -3, 0, 5, 9, 12, 45 };
        Assert.Equal(0, Problems.BinarySearch(arr, -8));
    }

    [Fact]
    public void TargetAtLastIndex()
    {
        int[] arr = { -8, -3, 0, 5, 9, 12, 45 };
        Assert.Equal(6, Problems.BinarySearch(arr, 45));
    }

    [Fact]
    public void TargetInMiddle()
    {
        int[] arr = { -8, -3, 0, 5, 9, 12, 45 };
        Assert.Equal(4, Problems.BinarySearch(arr, 9));
    }

    [Fact]
    public void TargetNotPresent()
    {
        int[] arr = { 1, 3, 5, 7 };
        Assert.Equal(-1, Problems.BinarySearch(arr, 6));
    }

    [Fact]
    public void DuplicatesPresent_ReturnsAnIndexOfTarget()
    {
        int[] arr = { 1, 2, 2, 2, 2, 3, 4 };
        int result = Problems.BinarySearch(arr, 2);
        Assert.InRange(result, 0, arr.Length - 1);
        Assert.Equal(2, arr[result]);
    }
}

public class SearchRotatedSortedArrayTests
{
    [Fact]
    public void NoRotation_PivotZero()
    {
        int[] nums = { 0, 1, 2, 4, 5, 6, 7 };
        Assert.Equal(5, Problems.SearchRotatedSortedArray(nums, 6));
    }

    [Fact]
    public void FullyRotated_PivotNearEnd()
    {
        // Base sorted array [0,1,2,4,5,6,7] rotated right by 6 (n - 1):
        // the pivot sits right before the last element.
        int[] nums = { 1, 2, 4, 5, 6, 7, 0 };
        Assert.Equal(6, Problems.SearchRotatedSortedArray(nums, 0));
    }

    [Fact]
    public void TargetAtPivot()
    {
        int[] nums = { 4, 5, 6, 7, 0, 1, 2 };
        Assert.Equal(4, Problems.SearchRotatedSortedArray(nums, 0));
    }

    [Fact]
    public void TargetNotPresent()
    {
        int[] nums = { 4, 5, 6, 7, 0, 1, 2 };
        Assert.Equal(-1, Problems.SearchRotatedSortedArray(nums, 3));
    }

    [Fact]
    public void SingleElement_Found()
    {
        int[] nums = { 5 };
        Assert.Equal(0, Problems.SearchRotatedSortedArray(nums, 5));
    }

    [Fact]
    public void SingleElement_NotFound()
    {
        int[] nums = { 5 };
        Assert.Equal(-1, Problems.SearchRotatedSortedArray(nums, 1));
    }

    [Fact]
    public void TwoElements_RotatedFound()
    {
        int[] nums = { 3, 1 };
        Assert.Equal(1, Problems.SearchRotatedSortedArray(nums, 1));
    }

    [Fact]
    public void TwoElements_RotatedNotFound()
    {
        int[] nums = { 3, 1 };
        Assert.Equal(-1, Problems.SearchRotatedSortedArray(nums, 2));
    }
}

public class FindPeakElementTests
{
    [Fact]
    public void SingleElement_IsPeak()
    {
        int[] nums = { 5 };
        Assert.Equal(0, Problems.FindPeakElement(nums));
    }

    [Fact]
    public void StrictlyIncreasing_PeakAtEnd()
    {
        int[] nums = { 1, 2, 3, 4, 5 };
        Assert.Equal(4, Problems.FindPeakElement(nums));
    }

    [Fact]
    public void StrictlyDecreasing_PeakAtStart()
    {
        int[] nums = { 5, 4, 3, 2, 1 };
        Assert.Equal(0, Problems.FindPeakElement(nums));
    }

    [Fact]
    public void PeakInMiddle()
    {
        int[] nums = { 1, 2, 3, 1 };
        Assert.Equal(2, Problems.FindPeakElement(nums));
    }

    [Fact]
    public void MultiplePeaks_ReturnedIndexIsAValidPeak()
    {
        int[] nums = { 1, 2, 1, 3, 5, 6, 4 };
        int result = Problems.FindPeakElement(nums);

        AssertIsPeak(nums, result);
    }

    private static void AssertIsPeak(int[] nums, int index)
    {
        long left = index - 1 >= 0 ? nums[index - 1] : long.MinValue;
        long right = index + 1 < nums.Length ? nums[index + 1] : long.MinValue;

        Assert.True(nums[index] > left, $"nums[{index}] should be greater than its left neighbor");
        Assert.True(nums[index] > right, $"nums[{index}] should be greater than its right neighbor");
    }
}

public class SearchRangeTests
{
    [Fact]
    public void TargetNotPresent()
    {
        int[] nums = { 5, 7, 7, 8, 8, 8, 10 };
        Assert.Equal(new[] { -1, -1 }, Problems.SearchRange(nums, 6));
    }

    [Fact]
    public void EmptyArray()
    {
        int[] nums = Array.Empty<int>();
        Assert.Equal(new[] { -1, -1 }, Problems.SearchRange(nums, 1));
    }

    [Fact]
    public void AllElementsAreTarget()
    {
        int[] nums = { 4, 4, 4, 4 };
        Assert.Equal(new[] { 0, 3 }, Problems.SearchRange(nums, 4));
    }

    [Fact]
    public void SingleOccurrence()
    {
        int[] nums = { 1, 2, 3, 4, 5 };
        Assert.Equal(new[] { 2, 2 }, Problems.SearchRange(nums, 3));
    }

    [Fact]
    public void MultipleOccurrences_AtStart()
    {
        int[] nums = { 2, 2, 2, 5, 8 };
        Assert.Equal(new[] { 0, 2 }, Problems.SearchRange(nums, 2));
    }

    [Fact]
    public void MultipleOccurrences_InMiddle()
    {
        int[] nums = { 5, 7, 7, 8, 8, 8, 10 };
        Assert.Equal(new[] { 3, 5 }, Problems.SearchRange(nums, 8));
    }

    [Fact]
    public void MultipleOccurrences_AtEnd()
    {
        int[] nums = { 1, 2, 9, 9, 9 };
        Assert.Equal(new[] { 2, 4 }, Problems.SearchRange(nums, 9));
    }
}
