// Reference solution — only read this after a real attempt.
//
// This file lives outside any .csproj on purpose: it is reference
// material only, not compiled as part of the module. If you want to
// run it, copy the method bodies into
// modules/01-arrays-strings/src/ArraysStrings/Problems.cs yourself.

namespace ArraysStrings;

public static class Problems
{
    public static int[] TwoSum(int[] nums, int target)
    {
        var seen = new Dictionary<int, int>(); // value -> index
        for (int i = 0; i < nums.Length; i++)
        {
            int complement = target - nums[i];
            if (seen.TryGetValue(complement, out int complementIndex))
            {
                return new[] { complementIndex, i };
            }
            seen[nums[i]] = i;
        }

        throw new ArgumentException("No two sum solution exists for the given input.");
    }

    public static void ReverseString(char[] s)
    {
        int left = 0;
        int right = s.Length - 1;
        while (left < right)
        {
            (s[left], s[right]) = (s[right], s[left]);
            left++;
            right--;
        }
    }

    public static bool IsAnagram(string a, string b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }

        var counts = new int[128]; // assume ASCII input
        foreach (char c in a)
        {
            counts[c]++;
        }
        foreach (char c in b)
        {
            counts[c]--;
        }

        foreach (int count in counts)
        {
            if (count != 0)
            {
                return false;
            }
        }

        return true;
    }

    public static int MaxSubArray(int[] nums)
    {
        int bestSum = nums[0];
        int currentSum = nums[0];

        for (int i = 1; i < nums.Length; i++)
        {
            currentSum = Math.Max(nums[i], currentSum + nums[i]);
            bestSum = Math.Max(bestSum, currentSum);
        }

        return bestSum;
    }

    public static void RotateArray(int[] nums, int k)
    {
        if (nums.Length == 0)
        {
            return;
        }

        k %= nums.Length;
        if (k == 0)
        {
            return;
        }

        Reverse(nums, 0, nums.Length - 1);
        Reverse(nums, 0, k - 1);
        Reverse(nums, k, nums.Length - 1);
    }

    private static void Reverse(int[] nums, int start, int end)
    {
        while (start < end)
        {
            (nums[start], nums[end]) = (nums[end], nums[start]);
            start++;
            end--;
        }
    }
}
