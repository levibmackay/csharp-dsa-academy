using DynamicProgramming;

namespace DynamicProgramming.Tests;

public class ProblemsTests
{
    // ---------- ClimbingStairs ----------

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(3, 3)]
    [InlineData(4, 5)]
    [InlineData(5, 8)]
    [InlineData(10, 89)]
    public void ClimbingStairs_ReturnsExpectedWayCount(int n, long expected)
    {
        Assert.Equal(expected, Problems.ClimbingStairs(n));
    }

    [Fact]
    public void ClimbingStairs_LargeN_DoesNotOverflowInt()
    {
        // n=50 already exceeds int.MaxValue for the fibonacci-style count,
        // which is exactly why the method returns long.
        long result = Problems.ClimbingStairs(50);
        Assert.True(result > int.MaxValue);
        Assert.Equal(20365011074L, result);
    }

    // ---------- CoinChange ----------

    [Fact]
    public void CoinChange_AmountZero_ReturnsZero()
    {
        Assert.Equal(0, Problems.CoinChange(new[] { 1, 2, 5 }, 0));
    }

    [Fact]
    public void CoinChange_StandardCase_ReturnsFewestCoins()
    {
        // 11 = 5 + 5 + 1 -> 3 coins
        Assert.Equal(3, Problems.CoinChange(new[] { 1, 2, 5 }, 11));
    }

    [Fact]
    public void CoinChange_ExactSingleCoin_ReturnsOne()
    {
        Assert.Equal(1, Problems.CoinChange(new[] { 1, 2, 5 }, 5));
    }

    [Fact]
    public void CoinChange_ImpossibleAmount_ReturnsMinusOne()
    {
        Assert.Equal(-1, Problems.CoinChange(new[] { 2 }, 3));
    }

    [Fact]
    public void CoinChange_NoCoinsAvailable_ReturnsMinusOneUnlessAmountZero()
    {
        Assert.Equal(-1, Problems.CoinChange(Array.Empty<int>(), 7));
        Assert.Equal(0, Problems.CoinChange(Array.Empty<int>(), 0));
    }

    [Fact]
    public void CoinChange_SingleDenominationExactMultiple_ReturnsExactCount()
    {
        Assert.Equal(4, Problems.CoinChange(new[] { 3 }, 12));
    }

    // ---------- LongestCommonSubsequence ----------

    [Fact]
    public void Lcs_BothEmpty_ReturnsZero()
    {
        Assert.Equal(0, Problems.LongestCommonSubsequence("", ""));
    }

    [Fact]
    public void Lcs_OneEmpty_ReturnsZero()
    {
        Assert.Equal(0, Problems.LongestCommonSubsequence("", "abc"));
        Assert.Equal(0, Problems.LongestCommonSubsequence("abc", ""));
    }

    [Fact]
    public void Lcs_NoCommonCharacters_ReturnsZero()
    {
        Assert.Equal(0, Problems.LongestCommonSubsequence("abc", "xyz"));
    }

    [Fact]
    public void Lcs_ClassicExample_ReturnsExpectedLength()
    {
        // "ace" is a subsequence of both -> length 3
        Assert.Equal(3, Problems.LongestCommonSubsequence("abcde", "ace"));
    }

    [Fact]
    public void Lcs_IdenticalStrings_ReturnsFullLength()
    {
        Assert.Equal(5, Problems.LongestCommonSubsequence("hello", "hello"));
    }

    [Theory]
    [InlineData("AGGTAB", "GXTXAYB", 4)] // "GTAB"
    public void Lcs_KnownCases_ReturnExpectedLength(string a, string b, int expected)
    {
        Assert.Equal(expected, Problems.LongestCommonSubsequence(a, b));
    }

    // ---------- Knapsack01 ----------

    [Fact]
    public void Knapsack01_EmptyItems_ReturnsZero()
    {
        Assert.Equal(0, Problems.Knapsack01(Array.Empty<int>(), Array.Empty<int>(), 10));
    }

    [Fact]
    public void Knapsack01_ZeroCapacity_ReturnsZero()
    {
        Assert.Equal(0, Problems.Knapsack01(new[] { 1, 2, 3 }, new[] { 10, 20, 30 }, 0));
    }

    [Fact]
    public void Knapsack01_SingleItemFits_ReturnsItsValue()
    {
        Assert.Equal(10, Problems.Knapsack01(new[] { 5 }, new[] { 10 }, 5));
    }

    [Fact]
    public void Knapsack01_SingleItemExceedsCapacity_ReturnsZero()
    {
        Assert.Equal(0, Problems.Knapsack01(new[] { 20 }, new[] { 100 }, 5));
    }

    [Fact]
    public void Knapsack01_StandardCase_ReturnsOptimalValue()
    {
        // weights: 1, 3, 4, 5 ; values: 1, 4, 5, 7 ; capacity 7
        // best: items with weight 3+4=7 -> value 4+5=9
        int result = Problems.Knapsack01(
            new[] { 1, 3, 4, 5 },
            new[] { 1, 4, 5, 7 },
            7);
        Assert.Equal(9, result);
    }

    [Fact]
    public void Knapsack01_AllItemsFit_ReturnsSumOfAllValues()
    {
        int result = Problems.Knapsack01(
            new[] { 1, 2, 3 },
            new[] { 10, 20, 30 },
            100);
        Assert.Equal(60, result);
    }
}
