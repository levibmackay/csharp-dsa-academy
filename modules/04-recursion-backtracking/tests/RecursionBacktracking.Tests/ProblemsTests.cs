using RecursionBacktracking;

namespace RecursionBacktracking.Tests;

public class ProblemsTests
{
    // ---------- Factorial ----------

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    [InlineData(5, 120)]
    [InlineData(10, 3628800)]
    [InlineData(20, 2432902008176640000)]
    public void Factorial_ReturnsExpectedResult(int n, long expected)
    {
        Assert.Equal(expected, Problems.Factorial(n));
    }

    [Fact]
    public void Factorial_NegativeInput_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problems.Factorial(-1));
    }

    // ---------- Fibonacci ----------

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(2, 1)]
    [InlineData(3, 2)]
    [InlineData(4, 3)]
    [InlineData(5, 5)]
    [InlineData(10, 55)]
    [InlineData(30, 832040)]
    public void Fibonacci_ReturnsExpectedResult(int n, long expected)
    {
        Assert.Equal(expected, Problems.Fibonacci(n));
    }

    [Fact]
    public void Fibonacci_NegativeInput_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Problems.Fibonacci(-1));
    }

    [Fact]
    public void Fibonacci_LargeInput_CompletesQuickly()
    {
        // Without memoization, naive recursive Fibonacci(45) takes a very long
        // time (exponential blowup). With memoization it should return near-instantly.
        long result = Problems.Fibonacci(45);

        Assert.Equal(1134903170, result);
    }

    // ---------- Permutations ----------

    [Fact]
    public void Permutations_SingleElement_ReturnsOnePermutation()
    {
        var result = Problems.Permutations(new[] { 1 });

        Assert.Single(result);
        AssertContainsSequence(result, 1);
    }

    [Fact]
    public void Permutations_TwoElements_ReturnsBothOrders()
    {
        var result = Problems.Permutations(new[] { 1, 2 });

        Assert.Equal(2, result.Count);
        AssertContainsSequence(result, 1, 2);
        AssertContainsSequence(result, 2, 1);
    }

    [Fact]
    public void Permutations_ThreeElements_ReturnsAllSixPermutations()
    {
        var result = Problems.Permutations(new[] { 1, 2, 3 });

        Assert.Equal(6, result.Count);

        AssertContainsSequence(result, 1, 2, 3);
        AssertContainsSequence(result, 1, 3, 2);
        AssertContainsSequence(result, 2, 1, 3);
        AssertContainsSequence(result, 2, 3, 1);
        AssertContainsSequence(result, 3, 1, 2);
        AssertContainsSequence(result, 3, 2, 1);
    }

    [Fact]
    public void Permutations_EmptyArray_ReturnsSingleEmptyPermutation()
    {
        var result = Problems.Permutations(Array.Empty<int>());

        Assert.Single(result);
        Assert.Empty(result[0]);
    }

    // ---------- Subsets ----------

    [Fact]
    public void Subsets_EmptyArray_ReturnsOnlyEmptySet()
    {
        var result = Problems.Subsets(Array.Empty<int>());

        Assert.Single(result);
        Assert.Empty(result[0]);
    }

    [Fact]
    public void Subsets_SingleElement_ReturnsEmptyAndFullSet()
    {
        var result = Problems.Subsets(new[] { 1 });

        Assert.Equal(2, result.Count);
        AssertContainsSequence(result);
        AssertContainsSequence(result, 1);
    }

    [Fact]
    public void Subsets_ThreeElements_ReturnsAllEightSubsets()
    {
        var result = Problems.Subsets(new[] { 1, 2, 3 });

        Assert.Equal(8, result.Count); // 2^3

        AssertContainsSequence(result);
        AssertContainsSequence(result, 1);
        AssertContainsSequence(result, 2);
        AssertContainsSequence(result, 3);
        AssertContainsSequence(result, 1, 2);
        AssertContainsSequence(result, 1, 3);
        AssertContainsSequence(result, 2, 3);
        AssertContainsSequence(result, 1, 2, 3);
    }

    // ---------- CountNQueensSolutions ----------

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 0)]
    [InlineData(3, 0)]
    [InlineData(4, 2)]
    [InlineData(5, 10)]
    [InlineData(6, 4)]
    [InlineData(8, 92)]
    public void CountNQueensSolutions_ReturnsExpectedCount(int n, int expected)
    {
        Assert.Equal(expected, Problems.CountNQueensSolutions(n));
    }

    // List<int> does not override Equals, so Assert.Contains(expectedList, results)
    // would compare by reference and always fail. This helper instead checks that
    // at least one inner list is element-for-element equal (via SequenceEqual) to
    // the expected sequence, regardless of which List<int> instance holds it.
    private static void AssertContainsSequence(List<List<int>> results, params int[] expected)
    {
        Assert.Contains(results, list => list.SequenceEqual(expected));
    }
}
