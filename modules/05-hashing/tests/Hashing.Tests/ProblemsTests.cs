using Hashing;

namespace Hashing.Tests;

public class GroupAnagramsTests
{
    private static HashSet<string> NormalizedGroup(List<string> group)
    {
        var sorted = group.Select(s => s).OrderBy(s => s, StringComparer.Ordinal).ToList();
        return new HashSet<string>(sorted);
    }

    private static HashSet<string> AsCanonicalGroups(List<List<string>> groups)
    {
        // Represent each group as a single "|"-joined, sorted string so the
        // outer collection can be compared order-independently too.
        return groups
            .Select(g => string.Join("|", NormalizedGroup(g).OrderBy(s => s, StringComparer.Ordinal)))
            .ToHashSet();
    }

    [Fact]
    public void GroupsAnagramsTogether_NormalCase()
    {
        string[] strs = { "eat", "tea", "tan", "ate", "nat", "bat" };

        List<List<string>> result = Problems.GroupAnagrams(strs);

        var expected = new HashSet<string>
        {
            string.Join("|", new[] { "ate", "eat", "tea" }),
            string.Join("|", new[] { "nat", "tan" }),
            string.Join("|", new[] { "bat" }),
        };

        Assert.Equal(expected, AsCanonicalGroups(result));
    }

    [Fact]
    public void EmptyArray_ReturnsEmptyList()
    {
        List<List<string>> result = Problems.GroupAnagrams(Array.Empty<string>());

        Assert.Empty(result);
    }

    [Fact]
    public void SingleString_ReturnsOneGroupWithThatString()
    {
        List<List<string>> result = Problems.GroupAnagrams(new[] { "abc" });

        Assert.Single(result);
        Assert.Equal(new HashSet<string> { "abc" }, new HashSet<string>(result[0]));
    }

    [Fact]
    public void AllAnagrams_ReturnsSingleGroup()
    {
        string[] strs = { "abc", "bca", "cab" };

        List<List<string>> result = Problems.GroupAnagrams(strs);

        Assert.Single(result);
        Assert.Equal(new HashSet<string> { "abc", "bca", "cab" }, new HashSet<string>(result[0]));
    }

    [Fact]
    public void NoAnagrams_ReturnsOneGroupPerString()
    {
        string[] strs = { "abc", "def", "ghi" };

        List<List<string>> result = Problems.GroupAnagrams(strs);

        Assert.Equal(3, result.Count);
        Assert.All(result, group => Assert.Single(group));
    }
}

public class FirstNonRepeatingCharTests
{
    [Fact]
    public void ReturnsFirstUniqueCharacter_NormalCase()
    {
        Assert.Equal('w', Problems.FirstNonRepeatingChar("swiss"));
    }

    [Fact]
    public void ReturnsNull_WhenEveryCharacterRepeats()
    {
        Assert.Null(Problems.FirstNonRepeatingChar("aabb"));
    }

    [Fact]
    public void SingleCharacter_ReturnsThatCharacter()
    {
        Assert.Equal('x', Problems.FirstNonRepeatingChar("x"));
    }

    [Fact]
    public void EmptyString_ReturnsNull()
    {
        Assert.Null(Problems.FirstNonRepeatingChar(""));
    }
}

public class TwoSumOptimalTests
{
    [Fact]
    public void ReturnsIndices_ForSimpleCase()
    {
        int[] nums = { 2, 7, 11, 15 };
        int[] result = Problems.TwoSumOptimal(nums, 9);
        Assert.Equal(new[] { 0, 1 }, result);
    }

    [Fact]
    public void HandlesNegativeNumbers()
    {
        int[] nums = { -1, -2, -3, -4, -5 };
        int[] result = Problems.TwoSumOptimal(nums, -8);
        Assert.Equal(new[] { 2, 4 }, result);
    }

    [Fact]
    public void HandlesDuplicateValues()
    {
        int[] nums = { 3, 3 };
        int[] result = Problems.TwoSumOptimal(nums, 6);
        Assert.Equal(new[] { 0, 1 }, result);
    }

    [Fact]
    public void NoSolution_ThrowsArgumentException()
    {
        int[] nums = { 1, 2, 3 };
        Assert.Throws<ArgumentException>(() => Problems.TwoSumOptimal(nums, 100));
    }
}
