namespace Sorting.Tests;

public class ProblemsTests
{
    public static IEnumerable<object[]> SortColorsCases()
    {
        yield return new object[] { new int[] { }, new int[] { } };
        yield return new object[] { new[] { 0 }, new[] { 0 } };
        yield return new object[] { new[] { 1 }, new[] { 1 } };
        yield return new object[] { new[] { 2 }, new[] { 2 } };
        yield return new object[] { new[] { 0, 1, 2 }, new[] { 0, 1, 2 } };
        yield return new object[] { new[] { 2, 1, 0 }, new[] { 0, 1, 2 } };
        yield return new object[] { new[] { 2, 0, 2, 1, 1, 0 }, new[] { 0, 0, 1, 1, 2, 2 } };
        yield return new object[] { new[] { 0, 0, 0 }, new[] { 0, 0, 0 } };
        yield return new object[] { new[] { 1, 1, 1 }, new[] { 1, 1, 1 } };
        yield return new object[] { new[] { 2, 2, 2 }, new[] { 2, 2, 2 } };
        yield return new object[] { new[] { 1, 0, 1, 2, 1, 0, 2 }, new[] { 0, 0, 1, 1, 1, 2, 2 } };
        yield return new object[] { new[] { 2, 2, 1, 1, 0, 0 }, new[] { 0, 0, 1, 1, 2, 2 } };
    }

    [Theory]
    [MemberData(nameof(SortColorsCases))]
    public void SortColors_SortsIntoZeroOneTwoOrder(int[] input, int[] expected)
    {
        Problems.SortColors(input);
        Assert.Equal(expected, input);
    }

    [Fact]
    public void SortColors_LargerMixedInput_SortsAscending()
    {
        int[] input = { 2, 0, 1, 2, 1, 0, 0, 2, 1, 0, 1, 2, 0 };
        int[] expected = { 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2 };

        Problems.SortColors(input);

        Assert.Equal(expected, input);
    }
}
