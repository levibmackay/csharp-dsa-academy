// Reference solution — only read this after a real attempt.

namespace Searching;

public static class Problems
{
    public static int BinarySearch(int[] sortedArr, int target)
    {
        int low = 0;
        int high = sortedArr.Length - 1;

        while (low <= high)
        {
            int mid = low + (high - low) / 2;

            if (sortedArr[mid] == target)
            {
                return mid;
            }
            else if (sortedArr[mid] < target)
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }

        return -1;
    }

    public static int SearchRotatedSortedArray(int[] nums, int target)
    {
        int low = 0;
        int high = nums.Length - 1;

        while (low <= high)
        {
            int mid = low + (high - low) / 2;

            if (nums[mid] == target)
            {
                return mid;
            }

            if (nums[low] <= nums[mid])
            {
                // left half [low..mid] is the contiguous, sorted half
                if (nums[low] <= target && target < nums[mid])
                {
                    high = mid - 1;
                }
                else
                {
                    low = mid + 1;
                }
            }
            else
            {
                // right half [mid..high] is the contiguous, sorted half
                if (nums[mid] < target && target <= nums[high])
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }
        }

        return -1;
    }

    public static int FindPeakElement(int[] nums)
    {
        int low = 0;
        int high = nums.Length - 1;

        while (low < high)
        {
            int mid = low + (high - low) / 2;

            if (nums[mid] < nums[mid + 1])
            {
                // uphill at mid: a peak must exist to the right
                low = mid + 1;
            }
            else
            {
                // downhill or flat at mid: a peak is at mid or to its left
                high = mid;
            }
        }

        return low;
    }

    public static int[] SearchRange(int[] nums, int target)
    {
        int first = FindBound(nums, target, findFirst: true);
        if (first == -1)
        {
            return new[] { -1, -1 };
        }

        int last = FindBound(nums, target, findFirst: false);
        return new[] { first, last };
    }

    private static int FindBound(int[] nums, int target, bool findFirst)
    {
        int low = 0;
        int high = nums.Length - 1;
        int result = -1;

        while (low <= high)
        {
            int mid = low + (high - low) / 2;

            if (nums[mid] == target)
            {
                result = mid;
                if (findFirst)
                {
                    high = mid - 1; // keep looking left for an earlier occurrence
                }
                else
                {
                    low = mid + 1; // keep looking right for a later occurrence
                }
            }
            else if (nums[mid] < target)
            {
                low = mid + 1;
            }
            else
            {
                high = mid - 1;
            }
        }

        return result;
    }
}
