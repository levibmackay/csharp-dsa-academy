// Reference solution — only read this after a real attempt.

namespace DynamicProgramming;

public static class Problems
{
    public static long ClimbingStairs(int n)
    {
        if (n == 0) return 1;
        if (n == 1) return 1;

        long[] ways = new long[n + 1];
        ways[0] = 1;
        ways[1] = 1;
        for (int i = 2; i <= n; i++)
        {
            ways[i] = ways[i - 1] + ways[i - 2];
        }

        return ways[n];
    }

    public static int CoinChange(int[] coins, int amount)
    {
        const int infinity = int.MaxValue / 2; // avoid overflow when adding 1

        int[] dp = new int[amount + 1];
        for (int i = 1; i <= amount; i++)
        {
            dp[i] = infinity;
        }
        // dp[0] = 0 by default (Array init to 0)

        for (int i = 1; i <= amount; i++)
        {
            foreach (int coin in coins)
            {
                if (coin <= i && dp[i - coin] + 1 < dp[i])
                {
                    dp[i] = dp[i - coin] + 1;
                }
            }
        }

        return dp[amount] >= infinity ? -1 : dp[amount];
    }

    public static int LongestCommonSubsequence(string a, string b)
    {
        int n = a.Length;
        int m = b.Length;
        int[,] dp = new int[n + 1, m + 1];

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                if (a[i - 1] == b[j - 1])
                {
                    dp[i, j] = dp[i - 1, j - 1] + 1;
                }
                else
                {
                    dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                }
            }
        }

        return dp[n, m];
    }

    public static int Knapsack01(int[] weights, int[] values, int capacity)
    {
        int n = weights.Length;
        int[,] dp = new int[n + 1, capacity + 1];

        for (int i = 1; i <= n; i++)
        {
            int weight = weights[i - 1];
            int value = values[i - 1];

            for (int w = 0; w <= capacity; w++)
            {
                // Skip item i.
                dp[i, w] = dp[i - 1, w];

                // Take item i, if it fits.
                if (weight <= w)
                {
                    int takeValue = dp[i - 1, w - weight] + value;
                    if (takeValue > dp[i, w])
                    {
                        dp[i, w] = takeValue;
                    }
                }
            }
        }

        return dp[n, capacity];
    }
}
