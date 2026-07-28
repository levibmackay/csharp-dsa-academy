namespace DynamicProgramming;

/// <summary>
/// Bottom-up (tabulation) dynamic programming problems.
///
/// In module 4 you solved similar problems top-down: write the recursive
/// definition first, then cache (memoize) results as you compute them
/// on demand. Here you build the table bottom-up instead: start from the
/// smallest/base sub-problems, fill an array/table iteratively, and read
/// the final answer off the last cell(s). Same recurrences, opposite
/// direction of construction.
/// </summary>
public static class Problems
{
    /// <summary>
    /// Count the number of distinct ways to climb <paramref name="n"/> stairs,
    /// taking either 1 or 2 steps at a time.
    ///
    /// DP state: ways[i] = number of distinct ways to reach step i.
    /// Recurrence: ways[i] = ways[i-1] + ways[i-2], with ways[0] = 1, ways[1] = 1.
    /// (This is the Fibonacci recurrence in disguise, which is why the count
    /// grows fast enough that we return a long instead of an int.)
    /// </summary>
    /// <param name="n">Number of stairs, n &gt;= 0.</param>
    /// <returns>Number of distinct ways to climb n stairs.</returns>
    public static long ClimbingStairs(int n)
    {
        // TODO: build a bottom-up table (or two rolling variables) where
        // ways[i] = ways[i-1] + ways[i-2]. Handle n == 0 and n == 1 as base cases.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Compute the fewest number of coins needed to make up <paramref name="amount"/>
    /// using an unlimited supply of each denomination in <paramref name="coins"/>.
    /// Returns -1 if the amount cannot be made with the given coins.
    ///
    /// DP state: dp[i] = minimum number of coins needed to make amount i.
    /// Recurrence: dp[i] = min over every coin c &lt;= i of (dp[i - c] + 1),
    /// with dp[0] = 0 (zero coins needed to make amount 0) and dp[i] initialized
    /// to "infinity" (unreachable) before considering coins.
    /// </summary>
    /// <param name="coins">Available coin denominations (positive integers).</param>
    /// <param name="amount">Target amount, amount &gt;= 0.</param>
    /// <returns>Fewest coins to make amount, or -1 if impossible.</returns>
    public static int CoinChange(int[] coins, int amount)
    {
        // TODO: build dp[0..amount], dp[0] = 0, everything else starts "infinite".
        // For each amount i from 1..amount, try every coin and take the best.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Compute the length of the longest common subsequence (LCS) of
    /// <paramref name="a"/> and <paramref name="b"/> — the longest sequence of
    /// characters (not necessarily contiguous, but in order) that appears in both.
    ///
    /// DP state: dp[i][j] = length of the LCS of a[0..i) and b[0..j) (the first
    /// i characters of a and the first j characters of b). Table size is
    /// (a.Length + 1) x (b.Length + 1) to account for the empty-prefix base case.
    /// Recurrence:
    ///   dp[i][j] = dp[i-1][j-1] + 1                    if a[i-1] == b[j-1]
    ///   dp[i][j] = Math.Max(dp[i-1][j], dp[i][j-1])    otherwise
    /// with dp[0][j] = dp[i][0] = 0 (an empty string has no common subsequence
    /// with anything).
    /// </summary>
    /// <param name="a">First string.</param>
    /// <param name="b">Second string.</param>
    /// <returns>Length of the longest common subsequence.</returns>
    public static int LongestCommonSubsequence(string a, string b)
    {
        // TODO: build a (a.Length+1) x (b.Length+1) table. Remember dp[i][j]
        // refers to a[0..i) and b[0..j) — a[i-1]/b[j-1] are the "current" characters.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Solve the 0/1 knapsack problem: given item weights and values, choose a
    /// subset of items (each usable at most once) whose total weight does not
    /// exceed <paramref name="capacity"/>, maximizing total value.
    ///
    /// DP state: dp[i][w] = max total value achievable using only the first i
    /// items with capacity w.
    /// Recurrence, considering item i (1-indexed, weights[i-1]/values[i-1]):
    ///   dp[i][w] = dp[i-1][w]                                        if weights[i-1] &gt; w (can't take it)
    ///   dp[i][w] = Math.Max(dp[i-1][w], dp[i-1][w - weights[i-1]] + values[i-1])  otherwise (skip vs take)
    /// with dp[0][w] = 0 for all w (no items => no value).
    /// </summary>
    /// <param name="weights">Weight of each item.</param>
    /// <param name="values">Value of each item (same length as weights).</param>
    /// <param name="capacity">Maximum total weight allowed, capacity &gt;= 0.</param>
    /// <returns>Maximum total value achievable within capacity.</returns>
    public static int Knapsack01(int[] weights, int[] values, int capacity)
    {
        // TODO: build a (n+1) x (capacity+1) table. For each item, decide
        // skip vs take (only valid if it fits) and keep the better option.
        throw new NotImplementedException();
    }
}
