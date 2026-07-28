namespace Sorting;

/// <summary>
/// Module 9: Sorting Algorithms — applied problems.
/// Implement each method below. Replace the TODO and the
/// <see cref="NotImplementedException"/> with your own working code.
/// See the module README.md for full problem statements, examples,
/// and hints.
/// </summary>
public static class Problems
{
    /// <summary>
    /// Given <paramref name="nums"/>, an array containing only the values
    /// 0, 1, and 2, sort it in place so all 0s come first, then all 1s,
    /// then all 2s (the Dutch National Flag problem). Do this in one pass
    /// without calling a general-purpose sort.
    /// Target complexity: O(n) time, O(1) extra space.
    /// </summary>
    public static void SortColors(int[] nums)
    {
        // TODO: use three pointers — low, mid, high — that partition the
        // array into four regions as mid scans through:
        //   [0, low)      known 0s
        //   [low, mid)    known 1s
        //   [mid, high]   unknown, still to be examined
        //   (high, end)   known 2s
        // Look at nums[mid] and swap it into place, growing the known
        // regions until mid crosses high. See the README hints for the
        // exact swap/advance rules for each of nums[mid] == 0, 1, 2.
        throw new NotImplementedException();
    }
}
