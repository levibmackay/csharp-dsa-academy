using Hashing;

namespace Hashing.Tests;

public class SimpleHashMapTests
{
    [Fact]
    public void PutAndTryGet_ReturnsStoredValue()
    {
        var map = new SimpleHashMap<string, int>();
        map.Put("one", 1);

        bool found = map.TryGet("one", out int value);

        Assert.True(found);
        Assert.Equal(1, value);
    }

    [Fact]
    public void TryGet_MissingKey_ReturnsFalseAndDefaultValue()
    {
        var map = new SimpleHashMap<string, int>();

        bool found = map.TryGet("missing", out int value);

        Assert.False(found);
        Assert.Equal(default, value);
    }

    [Fact]
    public void ContainsKey_ReflectsPresence()
    {
        var map = new SimpleHashMap<string, int>();
        map.Put("a", 1);

        Assert.True(map.ContainsKey("a"));
        Assert.False(map.ContainsKey("b"));
    }

    [Fact]
    public void Put_SameKeyTwice_UpsertsValueWithoutDoubleCountingCount()
    {
        var map = new SimpleHashMap<string, int>();
        map.Put("a", 1);
        map.Put("a", 2);

        Assert.Equal(1, map.Count);
        Assert.True(map.TryGet("a", out int value));
        Assert.Equal(2, value);
    }

    [Fact]
    public void Count_TracksNumberOfDistinctKeys()
    {
        var map = new SimpleHashMap<string, int>();
        Assert.Equal(0, map.Count);

        map.Put("a", 1);
        map.Put("b", 2);
        map.Put("c", 3);

        Assert.Equal(3, map.Count);
    }

    [Fact]
    public void Remove_ExistingKey_ReturnsTrueAndRemovesEntry()
    {
        var map = new SimpleHashMap<string, int>();
        map.Put("a", 1);

        bool removed = map.Remove("a");

        Assert.True(removed);
        Assert.False(map.ContainsKey("a"));
        Assert.Equal(0, map.Count);
    }

    [Fact]
    public void Remove_MissingKey_ReturnsFalse()
    {
        var map = new SimpleHashMap<string, int>();

        Assert.False(map.Remove("nope"));
    }

    [Fact]
    public void Remove_ThenReAdd_WorksCorrectly()
    {
        var map = new SimpleHashMap<string, int>();
        map.Put("a", 1);
        map.Remove("a");
        map.Put("a", 42);

        Assert.True(map.TryGet("a", out int value));
        Assert.Equal(42, value);
        Assert.Equal(1, map.Count);
    }

    [Fact]
    public void HandlesManyIntegerKeys_IncludingCollisionsAndResize()
    {
        var map = new SimpleHashMap<int, int>();

        // Default capacity is small; inserting 100 entries forces the
        // load factor threshold to be crossed multiple times, exercising
        // resize/rehash.
        for (int i = 0; i < 100; i++)
        {
            map.Put(i, i * i);
        }

        Assert.Equal(100, map.Count);

        for (int i = 0; i < 100; i++)
        {
            Assert.True(map.TryGet(i, out int value));
            Assert.Equal(i * i, value);
        }
    }

    [Fact]
    public void HandlesManyStringKeys_AllRetrievableAfterResize()
    {
        var map = new SimpleHashMap<string, int>();

        for (int i = 0; i < 50; i++)
        {
            map.Put($"key-{i}", i);
        }

        Assert.Equal(50, map.Count);

        for (int i = 0; i < 50; i++)
        {
            Assert.True(map.TryGet($"key-{i}", out int value));
            Assert.Equal(i, value);
        }

        Assert.False(map.TryGet("key-999", out int missing));
        Assert.Equal(default, missing);
    }

    [Fact]
    public void CollidingKeys_AllRemainRetrievable()
    {
        // Keys engineered to collide: same hash code, different identity.
        var map = new SimpleHashMap<CollidingKey, string>();
        var keyA = new CollidingKey(1);
        var keyB = new CollidingKey(2);
        var keyC = new CollidingKey(3);

        map.Put(keyA, "a");
        map.Put(keyB, "b");
        map.Put(keyC, "c");

        Assert.Equal(3, map.Count);
        Assert.True(map.TryGet(keyA, out string valueA));
        Assert.True(map.TryGet(keyB, out string valueB));
        Assert.True(map.TryGet(keyC, out string valueC));
        Assert.Equal("a", valueA);
        Assert.Equal("b", valueB);
        Assert.Equal("c", valueC);
    }

    // A key type whose GetHashCode always returns the same value, forcing
    // every instance into the same bucket regardless of bucket-array size.
    private sealed class CollidingKey
    {
        public int Id { get; }

        public CollidingKey(int id) => Id = id;

        public override int GetHashCode() => 42;

        public override bool Equals(object? obj) =>
            obj is CollidingKey other && other.Id == Id;
    }
}
