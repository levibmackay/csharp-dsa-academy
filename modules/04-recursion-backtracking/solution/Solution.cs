// Reference solution — only read this after a real attempt.
//
// This file lives outside any .csproj (it is plain reference material, not
// compiled as part of the project), so it reuses the same namespace/class/method
// names as the src stub without causing a build conflict.

namespace RecursionBacktracking;

public static class Problems
{
    public static long Factorial(int n)
    {
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative.");
        }

        if (n == 0)
        {
            return 1;
        }

        return n * Factorial(n - 1);
    }

    public static long Fibonacci(int n)
    {
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative.");
        }

        var cache = new Dictionary<int, long>();
        return FibonacciMemo(n, cache);
    }

    private static long FibonacciMemo(int n, Dictionary<int, long> cache)
    {
        if (n == 0) return 0;
        if (n == 1) return 1;

        if (cache.TryGetValue(n, out long cached))
        {
            return cached;
        }

        long result = FibonacciMemo(n - 1, cache) + FibonacciMemo(n - 2, cache);
        cache[n] = result;
        return result;
    }

    public static List<List<int>> Permutations(int[] nums)
    {
        var results = new List<List<int>>();
        var current = new List<int>();
        var used = new bool[nums.Length];

        PermutationsBacktrack(nums, used, current, results);
        return results;
    }

    private static void PermutationsBacktrack(
        int[] nums, bool[] used, List<int> current, List<List<int>> results)
    {
        if (current.Count == nums.Length)
        {
            results.Add(new List<int>(current));
            return;
        }

        for (int i = 0; i < nums.Length; i++)
        {
            if (used[i]) continue;

            used[i] = true;
            current.Add(nums[i]);

            PermutationsBacktrack(nums, used, current, results);

            // Backtrack: undo the choice so the next iteration starts clean.
            current.RemoveAt(current.Count - 1);
            used[i] = false;
        }
    }

    public static List<List<int>> Subsets(int[] nums)
    {
        var results = new List<List<int>>();
        var current = new List<int>();

        SubsetsBacktrack(nums, 0, current, results);
        return results;
    }

    private static void SubsetsBacktrack(
        int[] nums, int index, List<int> current, List<List<int>> results)
    {
        // Every partial state is itself a valid subset — record it immediately.
        results.Add(new List<int>(current));

        for (int i = index; i < nums.Length; i++)
        {
            current.Add(nums[i]);
            SubsetsBacktrack(nums, i + 1, current, results);
            current.RemoveAt(current.Count - 1); // backtrack
        }
    }

    public static int CountNQueensSolutions(int n)
    {
        var usedColumns = new bool[n];
        // A diagonal (top-left to bottom-right) is constant along row - col;
        // shift by (n - 1) so the index never goes negative.
        var usedDiagonals1 = new bool[2 * n - 1 > 0 ? 2 * n - 1 : 1];
        // The other diagonal (top-right to bottom-left) is constant along row + col.
        var usedDiagonals2 = new bool[2 * n - 1 > 0 ? 2 * n - 1 : 1];

        return CountNQueensBacktrack(n, 0, usedColumns, usedDiagonals1, usedDiagonals2);
    }

    private static int CountNQueensBacktrack(
        int n, int row, bool[] usedColumns, bool[] usedDiagonals1, bool[] usedDiagonals2)
    {
        if (row == n)
        {
            return 1; // placed a queen in every row without conflict
        }

        int count = 0;

        for (int col = 0; col < n; col++)
        {
            int diag1 = row - col + n - 1; // shifted row - col
            int diag2 = row + col;

            if (usedColumns[col] || usedDiagonals1[diag1] || usedDiagonals2[diag2])
            {
                continue; // this square is attacked, try the next column
            }

            usedColumns[col] = true;
            usedDiagonals1[diag1] = true;
            usedDiagonals2[diag2] = true;

            count += CountNQueensBacktrack(n, row + 1, usedColumns, usedDiagonals1, usedDiagonals2);

            // Backtrack.
            usedColumns[col] = false;
            usedDiagonals1[diag1] = false;
            usedDiagonals2[diag2] = false;
        }

        return count;
    }
}
