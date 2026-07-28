namespace TriesUnionFind;

/// <summary>
/// A trie (prefix tree) over lowercase-or-not strings -- lookups are
/// case-sensitive (i.e. "Cat" and "cat" are treated as different words).
/// Each node holds a Dictionary&lt;char, TrieNode&gt; mapping the next
/// character to the child node for that character, so both Insert and
/// Search/StartsWith run in O(L) time where L is the length of the word or
/// prefix -- independent of how many other words are stored.
/// </summary>
public class Trie
{
    /// <summary>
    /// A single node in the trie. Private and nested inside Trie because
    /// nothing outside the trie should ever construct or manipulate a node
    /// directly -- it's an implementation detail, not part of the public API.
    /// </summary>
    private class TrieNode
    {
        /// <summary>
        /// Maps each next character to the child node reached by following
        /// that character. Using a Dictionary (rather than, say, a
        /// fixed-size array of 26 slots) means the trie works for any
        /// characters (letters, digits, punctuation) and doesn't waste
        /// memory on branches that were never used.
        /// </summary>
        public Dictionary<char, TrieNode> Children { get; } = new();

        /// <summary>
        /// True if a complete word ends at this node (i.e. some call to
        /// Insert stopped here). A node can be "on the path" of a longer
        /// word (IsEndOfWord == false) or itself be a valid word ending
        /// (IsEndOfWord == true) or both at once (e.g. "car" and "carpet"
        /// both inserted -- the node for "car" has IsEndOfWord == true even
        /// though the trie continues on to "carpet").
        /// </summary>
        public bool IsEndOfWord { get; set; }
    }

    private readonly TrieNode _root = new();

    /// <summary>
    /// Inserts <paramref name="word"/> into the trie. Inserting the same
    /// word twice, or inserting a word that is a prefix of (or shares a
    /// prefix with) an existing word, is safe and simply reuses/extends the
    /// existing path.
    /// </summary>
    /// <param name="word">The word to insert. Must not be null.</param>
    public void Insert(string word)
    {
        // TODO:
        // 1. Start a `current` pointer at _root.
        // 2. For each character `c` in `word`:
        //    - If current.Children does NOT already have a node for `c`,
        //      create a new TrieNode and add it under key `c`
        //      (current.Children[c] = new TrieNode()).
        //    - Move `current` to current.Children[c].
        // 3. After the loop, mark current.IsEndOfWord = true (you've
        //    walked/created the full path for `word`).
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns true only if <paramref name="word"/> was previously inserted
    /// as a complete word (an exact match, not just a prefix of some other
    /// inserted word).
    /// </summary>
    /// <param name="word">The word to look up.</param>
    /// <example>
    /// trie.Insert("cat");
    /// trie.Search("cat");  // true
    /// trie.Search("ca");   // false -- "ca" was never inserted as its own word
    /// </example>
    public bool Search(string word)
    {
        // TODO:
        // 1. Walk the trie from _root following each character of `word`,
        //    exactly like Insert does -- but this time, if a character is
        //    ever missing from Children (use TryGetValue), return false
        //    immediately: the word was never inserted.
        // 2. If you successfully walk the whole word, return
        //    current.IsEndOfWord (it must be a *complete* word, not merely
        //    a path that happens to exist because a longer word was inserted).
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns true if any inserted word starts with <paramref name="prefix"/>
    /// (the prefix itself does not need to have been inserted as a complete
    /// word). An empty prefix matches as long as the trie is non-empty is
    /// not required by tests here, but is a reasonable "true" case since
    /// every word starts with the empty string.
    /// </summary>
    /// <param name="prefix">The prefix to look up.</param>
    /// <example>
    /// trie.Insert("cat");
    /// trie.StartsWith("ca");   // true
    /// trie.StartsWith("dog");  // false
    /// </example>
    public bool StartsWith(string prefix)
    {
        // TODO: Same walk as Search, but you don't care about IsEndOfWord --
        // you only care whether the path for `prefix` exists at all. If you
        // make it through every character without a missing child, return
        // true; otherwise false.
        throw new NotImplementedException();
    }
}
