using TriesUnionFind;

namespace TriesUnionFind.Tests;

public class TrieTests
{
    [Fact]
    public void Search_OnEmptyTrie_ReturnsFalse()
    {
        var trie = new Trie();

        Assert.False(trie.Search("anything"));
    }

    [Fact]
    public void StartsWith_OnEmptyTrie_ReturnsFalse()
    {
        var trie = new Trie();

        Assert.False(trie.StartsWith("a"));
    }

    [Fact]
    public void Search_ExactWordAfterInsert_ReturnsTrue()
    {
        var trie = new Trie();

        trie.Insert("cat");

        Assert.True(trie.Search("cat"));
    }

    [Fact]
    public void Search_PrefixThatWasNeverInsertedAsWord_ReturnsFalse()
    {
        var trie = new Trie();

        trie.Insert("cat");

        Assert.False(trie.Search("ca"));
    }

    [Fact]
    public void Search_NonExistentWord_ReturnsFalse()
    {
        var trie = new Trie();

        trie.Insert("cat");

        Assert.False(trie.Search("dog"));
    }

    [Fact]
    public void StartsWith_KnownPrefix_ReturnsTrue()
    {
        var trie = new Trie();

        trie.Insert("cat");

        Assert.True(trie.StartsWith("ca"));
        Assert.True(trie.StartsWith("cat"));
    }

    [Fact]
    public void StartsWith_NonExistentPrefix_ReturnsFalse()
    {
        var trie = new Trie();

        trie.Insert("cat");

        Assert.False(trie.StartsWith("do"));
    }

    [Fact]
    public void PrefixWordThatIsAlsoAFullWord_BothSearchAndStartsWithAreTrue()
    {
        // "car" is inserted as a full word, and "carpet" extends past it.
        // The node for "car" must remain a valid end-of-word even though
        // the trie continues on beyond it.
        var trie = new Trie();

        trie.Insert("car");
        trie.Insert("carpet");

        Assert.True(trie.Search("car"));
        Assert.True(trie.Search("carpet"));
        Assert.True(trie.StartsWith("car"));
        Assert.False(trie.Search("carp"));
        Assert.True(trie.StartsWith("carp"));
    }

    [Fact]
    public void Insert_IsCaseSensitive()
    {
        // Convention for this module: lookups are case-sensitive.
        // "Cat" and "cat" are treated as entirely different words.
        var trie = new Trie();

        trie.Insert("Cat");

        Assert.True(trie.Search("Cat"));
        Assert.False(trie.Search("cat"));
        Assert.False(trie.StartsWith("cat"));
        Assert.True(trie.StartsWith("Ca"));
    }

    [Fact]
    public void Insert_SameWordTwice_IsIdempotent()
    {
        var trie = new Trie();

        trie.Insert("cat");
        trie.Insert("cat");

        Assert.True(trie.Search("cat"));
    }

    [Theory]
    [InlineData("dog")]
    [InlineData("do")]
    [InlineData("d")]
    public void MultipleWordsSharingAPrefix_AllRetrievableIndependently(string word)
    {
        var trie = new Trie();

        trie.Insert("dog");
        trie.Insert("do");
        trie.Insert("d");

        Assert.True(trie.Search(word));
    }
}
