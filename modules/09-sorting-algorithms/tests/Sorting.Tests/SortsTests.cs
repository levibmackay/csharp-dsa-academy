namespace Sorting.Tests;

public class SortsTests
{
    public static IEnumerable<object[]> SortCases()
    {
        yield return new object[] { new int[] { }, new int[] { } };
        yield return new object[] { new[] { 1 }, new[] { 1 } };
        yield return new object[] { new[] { 1, 2, 3, 4, 5 }, new[] { 1, 2, 3, 4, 5 } };
        yield return new object[] { new[] { 5, 4, 3, 2, 1 }, new[] { 1, 2, 3, 4, 5 } };
        yield return new object[] { new[] { 5, 3, 8, 1, 9, 2 }, new[] { 1, 2, 3, 5, 8, 9 } };
        yield return new object[] { new[] { 2, 2, 2, 2 }, new[] { 2, 2, 2, 2 } };
        yield return new object[] { new[] { 4, 1, 4, 2, 1, 3 }, new[] { 1, 1, 2, 3, 4, 4 } };
        yield return new object[] { new[] { -3, -1, -7, 2, 0, -2 }, new[] { -7, -3, -2, -1, 0, 2 } };
        yield return new object[] { new[] { 0, -1, 5, -10, 3 }, new[] { -10, -1, 0, 3, 5 } };
    }

    [Theory]
    [MemberData(nameof(SortCases))]
    public void BubbleSort_SortsAscending(int[] input, int[] expected)
    {
        Sorts.BubbleSort(input);
        Assert.Equal(expected, input);
    }

    [Theory]
    [MemberData(nameof(SortCases))]
    public void InsertionSort_SortsAscending(int[] input, int[] expected)
    {
        Sorts.InsertionSort(input);
        Assert.Equal(expected, input);
    }

    [Theory]
    [MemberData(nameof(SortCases))]
    public void MergeSort_SortsAscending(int[] input, int[] expected)
    {
        Sorts.MergeSort(input);
        Assert.Equal(expected, input);
    }

    [Theory]
    [MemberData(nameof(SortCases))]
    public void QuickSort_SortsAscending(int[] input, int[] expected)
    {
        Sorts.QuickSort(input);
        Assert.Equal(expected, input);
    }

    [Fact]
    public void BubbleSort_LargerRandomInput_SortsAscending()
    {
        int[] input = { 42, 17, 5, 99, 23, 8, 61, 4, 76, 12, 0, -5, 33 };
        int[] expected = (int[])input.Clone();
        Array.Sort(expected);

        Sorts.BubbleSort(input);

        Assert.Equal(expected, input);
    }

    [Fact]
    public void MergeSort_LargerRandomInput_SortsAscending()
    {
        int[] input = { 42, 17, 5, 99, 23, 8, 61, 4, 76, 12, 0, -5, 33 };
        int[] expected = (int[])input.Clone();
        Array.Sort(expected);

        Sorts.MergeSort(input);

        Assert.Equal(expected, input);
    }

    [Fact]
    public void QuickSort_LargerRandomInput_SortsAscending()
    {
        int[] input = { 42, 17, 5, 99, 23, 8, 61, 4, 76, 12, 0, -5, 33 };
        int[] expected = (int[])input.Clone();
        Array.Sort(expected);

        Sorts.QuickSort(input);

        Assert.Equal(expected, input);
    }

    [Fact]
    public void InsertionSort_LargerRandomInput_SortsAscending()
    {
        int[] input = { 42, 17, 5, 99, 23, 8, 61, 4, 76, 12, 0, -5, 33 };
        int[] expected = (int[])input.Clone();
        Array.Sort(expected);

        Sorts.InsertionSort(input);

        Assert.Equal(expected, input);
    }
}
