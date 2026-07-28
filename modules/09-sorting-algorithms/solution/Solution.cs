// Reference solution — only read this after a real attempt.
//
// This file lives outside any .csproj on purpose: it is reference
// material only, not compiled as part of the module. If you want to
// run it, copy the method bodies into
// modules/09-sorting-algorithms/src/Sorting/Sorts.cs and Problems.cs
// yourself.

namespace Sorting;

public static class Sorts
{
    public static void BubbleSort(int[] arr)
    {
        for (int i = 0; i < arr.Length - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < arr.Length - 1 - i; j++)
            {
                if (arr[j] > arr[j + 1])
                {
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    swapped = true;
                }
            }

            // If a full pass made no swaps, the array is already sorted
            // and every further pass would be wasted work.
            if (!swapped)
            {
                break;
            }
        }
    }

    public static void InsertionSort(int[] arr)
    {
        for (int i = 1; i < arr.Length; i++)
        {
            int key = arr[i];
            int j = i - 1;

            // Shift every element greater than key one slot to the right.
            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j];
                j--;
            }

            arr[j + 1] = key;
        }
    }

    public static void MergeSort(int[] arr)
    {
        if (arr.Length < 2)
        {
            return;
        }

        MergeSortRange(arr, 0, arr.Length - 1);
    }

    private static void MergeSortRange(int[] arr, int low, int high)
    {
        if (low >= high)
        {
            return; // 0 or 1 elements: already sorted.
        }

        int mid = low + (high - low) / 2; // avoids overflow vs (low + high) / 2
        MergeSortRange(arr, low, mid);
        MergeSortRange(arr, mid + 1, high);
        Merge(arr, low, mid, high);
    }

    private static void Merge(int[] arr, int low, int mid, int high)
    {
        int leftLength = mid - low + 1;
        int rightLength = high - mid;
        int[] left = new int[leftLength];
        int[] right = new int[rightLength];

        Array.Copy(arr, low, left, 0, leftLength);
        Array.Copy(arr, mid + 1, right, 0, rightLength);

        int i = 0; // index into left
        int j = 0; // index into right
        int k = low; // index into arr

        while (i < leftLength && j < rightLength)
        {
            // <= keeps the merge stable: equal elements from the left
            // half are placed before equal elements from the right half.
            if (left[i] <= right[j])
            {
                arr[k++] = left[i++];
            }
            else
            {
                arr[k++] = right[j++];
            }
        }

        while (i < leftLength)
        {
            arr[k++] = left[i++];
        }

        while (j < rightLength)
        {
            arr[k++] = right[j++];
        }
    }

    public static void QuickSort(int[] arr)
    {
        if (arr.Length < 2)
        {
            return;
        }

        QuickSortRange(arr, 0, arr.Length - 1);
    }

    private static void QuickSortRange(int[] arr, int low, int high)
    {
        if (low >= high)
        {
            return;
        }

        int pivotIndex = Partition(arr, low, high);
        QuickSortRange(arr, low, pivotIndex - 1);
        QuickSortRange(arr, pivotIndex + 1, high);
    }

    // Lomuto partition scheme: chosen over Hoare here because it returns
    // the pivot's final resting index directly, which keeps the two
    // recursive sub-ranges obvious (low..pivotIndex-1 and
    // pivotIndex+1..high). Hoare's scheme is typically faster in practice
    // (fewer swaps) but its partition point isn't the pivot's final
    // index, which is a common source of off-by-one bugs for learners —
    // Lomuto is the clearer teaching choice.
    private static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high]; // last element as pivot
        int i = low - 1; // boundary of elements known to be <= pivot

        for (int j = low; j < high; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }

        (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
        return i + 1;
    }
}

public static class Problems
{
    public static void SortColors(int[] nums)
    {
        int low = 0;
        int mid = 0;
        int high = nums.Length - 1;

        // Invariant while mid <= high:
        //   [0, low)      all 0s
        //   [low, mid)    all 1s
        //   [mid, high]   unexamined
        //   (high, end)   all 2s
        while (mid <= high)
        {
            if (nums[mid] == 0)
            {
                (nums[low], nums[mid]) = (nums[mid], nums[low]);
                low++;
                mid++; // the swapped-in value at mid came from [0, low), which is
                       // always 0 or 1, so it's safe to advance mid too.
            }
            else if (nums[mid] == 1)
            {
                mid++;
            }
            else // nums[mid] == 2
            {
                (nums[mid], nums[high]) = (nums[high], nums[mid]);
                high--; // don't advance mid: the value swapped in from high is
                        // unexamined and still needs to be looked at.
            }
        }
    }
}
