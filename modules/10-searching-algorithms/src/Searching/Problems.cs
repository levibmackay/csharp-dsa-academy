namespace Searching;

/// <summary>
/// Module 10: Searching Algorithms.
/// Implement each method below. Replace the TODO and the
/// <see cref="NotImplementedException"/> with your own working code.
/// See the module README.md for full problem statements, examples,
/// and hints.
/// </summary>
public static class Problems
{
    /// <summary>
    /// Given an array <paramref name="sortedArr"/> sorted in ascending
    /// order and a value <paramref name="target"/>, return the index of
    /// <paramref name="target"/> if it exists, or -1 if it doesn't.
    /// Target complexity: O(log n) time, O(1) space.
    /// </summary>
    public static int BinarySearch(int[] sortedArr, int target)
    {
        // TODO: classic iterative binary search. Initialize low = 0 and
        // high = sortedArr.Length - 1. While low <= high, compute
        // mid = low + (high - low) / 2 and compare sortedArr[mid] to
        // target, shrinking the range accordingly.
        throw new NotImplementedException();
    }

    /// <summary>
    /// <paramref name="nums"/> was originally sorted in ascending order
    /// with all-distinct elements, then rotated at some unknown pivot
    /// (e.g. [0,1,2,4,5,6,7] -> [4,5,6,7,0,1,2]). Return the index of
    /// <paramref name="target"/> if it exists, or -1 if it doesn't.
    /// Target complexity: O(log n) time, O(1) space.
    /// </summary>
    public static int SearchRotatedSortedArray(int[] nums, int target)
    {
        // TODO: modified binary search. At each mid, one of the two
        // halves (relative to mid) is always properly sorted. Compare
        // nums[low], nums[mid], and target to figure out which half is
        // sorted, then check whether target falls inside that half's
        // range to decide which direction to search next.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Return the index of any element in <paramref name="nums"/> that is
    /// strictly greater than both of its neighbors (out-of-bounds
    /// neighbors count as -infinity). The array may contain multiple
    /// valid peaks; returning the index of any one of them is correct.
    /// Target complexity: O(log n) time, O(1) space.
    /// </summary>
    public static int FindPeakElement(int[] nums)
    {
        // TODO: binary search on the "slope". Compare nums[mid] to
        // nums[mid + 1]: if it's increasing, a peak must exist to the
        // right; otherwise a peak must exist at mid or to its left.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Given <paramref name="nums"/> sorted in ascending order (possibly
    /// with duplicates) and a value <paramref name="target"/>, return a
    /// two-element array [firstIndex, lastIndex] giving the first and
    /// last position of target in the array, or [-1, -1] if target is
    /// not present.
    /// Target complexity: O(log n) time, O(1) space (two biased binary
    /// searches for the left and right boundaries).
    /// </summary>
    public static int[] SearchRange(int[] nums, int target)
    {
        // TODO: run two binary searches. In the first, when
        // nums[mid] == target, record mid and keep searching left
        // (high = mid - 1) to find the first occurrence. In the second,
        // when nums[mid] == target, record mid and keep searching right
        // (low = mid + 1) to find the last occurrence.
        throw new NotImplementedException();
    }
}
