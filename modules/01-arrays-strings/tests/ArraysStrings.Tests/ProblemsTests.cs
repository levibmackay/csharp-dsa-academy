using ArraysStrings;

namespace ArraysStrings.Tests;

public class TwoSumTests
{
    [Fact]
    public void ReturnsIndices_ForSimpleCase()
    {
        int[] nums = { 2, 7, 11, 15 };
        int[] result = Problems.TwoSum(nums, 9);
        Assert.Equal(new[] { 0, 1 }, result);
    }

    [Fact]
    public void ReturnsIndices_WhenAnswerIsNotAtStart()
    {
        int[] nums = { 3, 2, 4 };
        int[] result = Problems.TwoSum(nums, 6);
        Assert.Equal(new[] { 1, 2 }, result);
    }

    [Fact]
    public void HandlesDuplicateValues()
    {
        int[] nums = { 3, 3 };
        int[] result = Problems.TwoSum(nums, 6);
        Assert.Equal(new[] { 0, 1 }, result);
    }

    [Fact]
    public void HandlesNegativeNumbers()
    {
        int[] nums = { -1, -2, -3, -4, -5 };
        int[] result = Problems.TwoSum(nums, -8);
        Assert.Equal(new[] { 2, 4 }, result);
    }
}

public class ReverseStringTests
{
    [Fact]
    public void ReversesEvenLengthArray()
    {
        char[] s = "abcd".ToCharArray();
        Problems.ReverseString(s);
        Assert.Equal("dcba", new string(s));
    }

    [Fact]
    public void ReversesOddLengthArray()
    {
        char[] s = "hello".ToCharArray();
        Problems.ReverseString(s);
        Assert.Equal("olleh", new string(s));
    }

    [Fact]
    public void HandlesSingleElement()
    {
        char[] s = "x".ToCharArray();
        Problems.ReverseString(s);
        Assert.Equal("x", new string(s));
    }

    [Fact]
    public void HandlesEmptyArray()
    {
        char[] s = Array.Empty<char>();
        Problems.ReverseString(s);
        Assert.Empty(s);
    }
}

public class IsAnagramTests
{
    [Theory]
    [InlineData("anagram", "nagaram", true)]
    [InlineData("rat", "car", false)]
    [InlineData("", "", true)]
    [InlineData("a", "a", true)]
    [InlineData("ab", "abc", false)]
    [InlineData("Anagram", "anagram", false)] // case-sensitive
    [InlineData("aabbcc", "abcabc", true)]
    public void ChecksAnagramCorrectly(string a, string b, bool expected)
    {
        Assert.Equal(expected, Problems.IsAnagram(a, b));
    }
}

public class MaxSubArrayTests
{
    [Fact]
    public void HandlesMixedPositiveAndNegative()
    {
        int[] nums = { -2, 1, -3, 4, -1, 2, 1, -5, 4 };
        Assert.Equal(6, Problems.MaxSubArray(nums));
    }

    [Fact]
    public void HandlesSingleElement()
    {
        int[] nums = { 5 };
        Assert.Equal(5, Problems.MaxSubArray(nums));
    }

    [Fact]
    public void HandlesAllNegative()
    {
        int[] nums = { -3, -1, -2, -4 };
        Assert.Equal(-1, Problems.MaxSubArray(nums));
    }

    [Fact]
    public void HandlesAllPositive()
    {
        int[] nums = { 1, 2, 3, 4 };
        Assert.Equal(10, Problems.MaxSubArray(nums));
    }

    [Fact]
    public void HandlesSingleNegativeElement()
    {
        int[] nums = { -5 };
        Assert.Equal(-5, Problems.MaxSubArray(nums));
    }
}

public class RotateArrayTests
{
    [Fact]
    public void RotatesByThree()
    {
        int[] nums = { 1, 2, 3, 4, 5, 6, 7 };
        Problems.RotateArray(nums, 3);
        Assert.Equal(new[] { 5, 6, 7, 1, 2, 3, 4 }, nums);
    }

    [Fact]
    public void RotatesByOne()
    {
        int[] nums = { 1, 2 };
        Problems.RotateArray(nums, 1);
        Assert.Equal(new[] { 2, 1 }, nums);
    }

    [Fact]
    public void HandlesKGreaterThanLength()
    {
        int[] nums = { 1, 2, 3 };
        Problems.RotateArray(nums, 4); // 4 % 3 == 1
        Assert.Equal(new[] { 3, 1, 2 }, nums);
    }

    [Fact]
    public void HandlesKEqualToLength_NoChange()
    {
        int[] nums = { 1, 2, 3 };
        Problems.RotateArray(nums, 3);
        Assert.Equal(new[] { 1, 2, 3 }, nums);
    }

    [Fact]
    public void HandlesEmptyArray()
    {
        int[] nums = Array.Empty<int>();
        Problems.RotateArray(nums, 5);
        Assert.Empty(nums);
    }

    [Fact]
    public void HandlesSingleElement()
    {
        int[] nums = { 42 };
        Problems.RotateArray(nums, 7);
        Assert.Equal(new[] { 42 }, nums);
    }

    [Fact]
    public void HandlesKZero_NoChange()
    {
        int[] nums = { 1, 2, 3, 4 };
        Problems.RotateArray(nums, 0);
        Assert.Equal(new[] { 1, 2, 3, 4 }, nums);
    }
}
