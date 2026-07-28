# Module 5: Hashing

Hash-based lookup is the workhorse of practical software: dictionaries,
sets, caches, deduplication, and grouping all reduce to "map a key to a
value in roughly constant time." In this module you'll build a simplified
hash map from scratch (so you understand exactly what's happening inside
one), then use hashing to solve three classic problems. By the end you
should be able to recognize "I need O(1) average lookup by key" as a
problem shape and reach for a hash map without hesitation.

## C# syntax you'll need

### Generics and the `where TKey : notnull` constraint

A generic type or method works over a placeholder type (`T`, `TKey`,
`TValue`, ...) that's filled in by the caller:

```csharp
public class Box<T>
{
    public T Value { get; set; }
}

var intBox = new Box<int> { Value = 5 };
var stringBox = new Box<string> { Value = "hello" };
```

You can constrain what a generic parameter is allowed to be with a
`where` clause. `where TKey : notnull` means "TKey may not be a nullable
reference or a `null` value" — you can't sensibly hash `null`, so hash-map
implementations (including the real `Dictionary<TKey,TValue>`) forbid it:

```csharp
public class SimpleHashMap<TKey, TValue> where TKey : notnull
{
    // ...
}
```

### Arrays and `List<T>`

An array (`T[]`) has a fixed size once created. `List<T>` is a resizable
wrapper around an array — you'll use both in this module: a fixed-size
array of *buckets*, where each bucket is a resizable `List<T>` of entries.

```csharp
var buckets = new List<int>[4];       // array of 4 (currently null) lists
buckets[0] = new List<int>();          // create the list for bucket 0
buckets[0].Add(17);
buckets[0].Add(42);
Console.WriteLine(buckets[0].Count);   // 2
```

Note that `new List<int>[4]` gives you an array whose four slots are all
`null` — you must assign a `new List<int>()` into a slot before adding to
it, exactly as in the snippet above.

### Tuples, including named tuple syntax

A tuple groups multiple values into one lightweight, unnamed-ish type.
Naming the elements makes them self-documenting:

```csharp
(string Name, int Age) person = ("Ada", 36);
Console.WriteLine(person.Name); // "Ada"
Console.WriteLine(person.Age);  // 36

// Deconstruction:
var (name, age) = person;
```

This module's bucket lists hold `(TKey Key, TValue Value)` tuples — one
tuple per stored entry:

```csharp
var bucket = new List<(string Key, int Value)>();
bucket.Add(("apples", 3));
Console.WriteLine(bucket[0].Key);   // "apples"
Console.WriteLine(bucket[0].Value); // 3
```

Tuples also support swapping and multi-assignment in one line:

```csharp
(a, b) = (b, a); // swap without a temp variable
```

### `Dictionary<TKey, TValue>`

The framework's built-in hash map. You'll use it directly for the
`Problems` methods in this module (see "why not `SimpleHashMap` for the
problems?" below):

```csharp
var counts = new Dictionary<string, int>();
counts["apple"] = 1;
counts["apple"] = counts["apple"] + 1; // now 2

if (counts.TryGetValue("banana", out int bananaCount))
{
    Console.WriteLine(bananaCount);
}
else
{
    Console.WriteLine("no bananas seen");
}

// GetValueOrDefault avoids a manual TryGetValue when you just want a
// fallback value (0 for int, null for reference types, etc.):
int pearCount = counts.GetValueOrDefault("pear"); // 0, doesn't throw
```

`TryGetValue` is almost always what you want over indexing with `[]`
directly, because indexing throws a `KeyNotFoundException` when the key
is missing.

### Nullable value types (`char?`)

Value types (`int`, `char`, `bool`, `double`, structs, ...) normally
cannot be `null`. Appending `?` makes a *nullable* version that can hold
either a real value or the absence of one:

```csharp
char? maybeChar = null;
maybeChar = 'x';

if (maybeChar is not null)
{
    Console.WriteLine(maybeChar.Value); // unwrap with .Value
}

// Or use the null-coalescing operator for a fallback:
char result = maybeChar ?? '?';
```

One of this module's problems returns `char?` specifically because "no
non-repeating character was found" is a legitimate outcome distinct from
any actual character value — `null` represents that case cleanly.

### Pattern matching

`is null` / `is not null` checks are clearer and safer than `== null`
(they can't be overridden by a misbehaving `==` operator):

```csharp
if (bucket is null)
{
    bucket = new List<(string, int)>();
}
```

Switch expressions let you match on a value and produce a result
concisely:

```csharp
string Describe(int loadFactorPercent) => loadFactorPercent switch
{
    < 50 => "underfull",
    < 75 => "healthy",
    _ => "needs a resize",
};
```

### LINQ

LINQ (Language Integrated Query) adds declarative, functional-style
operations over any `IEnumerable<T>` (arrays, `List<T>`, `Dictionary<T>`
values, etc.):

```csharp
var groups = new Dictionary<string, List<string>>();
// ... populate groups ...
List<List<string>> allGroups = groups.Values.ToList();

int[] nums = { 5, 1, 4, 2, 3 };
var sorted = nums.OrderBy(n => n).ToList();       // 1,2,3,4,5
var evens = nums.Where(n => n % 2 == 0).ToList(); // 2,4
```

You don't strictly need LINQ to solve this module's problems, but
`.Values.ToList()` (turning a dictionary's values into a list) is a
handy one-liner you'll likely reach for.

### Why `GetHashCode()` and `Equals()` matter

Every hash-based structure relies on two things being consistent for a
given key type:

1. **`GetHashCode()`** — converts a key into an `int` "bucket hint."
   Objects that are `Equal` **must** return the same hash code (the
   reverse isn't required — different objects can share a hash code,
   that's a *collision*, and it's expected and handled, not a bug).
2. **`Equals()`** — the real, authoritative test for "are these two keys
   actually the same key?" Hash codes only narrow down *which bucket* to
   look in; `Equals()` is what confirms a match once you're scanning
   entries inside that bucket.

For built-in types like `string` and `int`, both are already implemented
correctly and consistently, so you don't need to think about this for
this module's problems. But it's why, if you ever define your own class
and want to use it as a dictionary key, you must override both
`GetHashCode()` and `Equals()` together — never just one.

### Load factor and rehashing

**Load factor** = `Count / bucket-array-length`. It's a measure of how
"full" the hash map is. A low load factor means buckets mostly have 0-1
entries, so lookups are close to O(1). As load factor climbs, more keys
pile into the same buckets (more collisions), and operations degrade
toward O(n) — you're basically doing a linear scan of a list at that
point.

The fix is **rehashing**: when load factor crosses a threshold (this
module uses 0.75, a common real-world choice), allocate a bigger bucket
array (typically double the size) and reinsert every existing entry,
which recomputes each one's bucket index against the new, larger
array length. This is exactly the operation you'll implement as `Resize()`
in `SimpleHashMap<TKey,TValue>`.

Resizing is an O(n) operation, but because it only happens occasionally
(roughly every time the map doubles in size), the *amortized* cost per
`Put` stays O(1) on average — the expensive resizes are rare enough that
they don't dominate the average cost per operation.

### What you're building

`SimpleHashMap<TKey,TValue>` is a deliberately simplified version of what
`System.Collections.Generic.Dictionary<TKey,TValue>` does internally:
bucket by hash code, chain collisions in a list per bucket, and rehash
when it gets too full. The real Dictionary is more optimized (it doesn't
allocate a `List<T>` per bucket, for instance — it uses a flatter,
more cache-friendly layout), but the core idea is the same one you'll
implement here.

## Problems

### 1. SimpleHashMap<TKey, TValue> (separate chaining)

Implement a generic hash map using **separate chaining**: an array of
buckets, where each bucket is a `List<(TKey Key, TValue Value)>` holding
every entry whose key hashed into that bucket. Bucket index is computed
from `key.GetHashCode()` modulo the current bucket-array length. When the
load factor (`Count / bucket count`) exceeds 0.75 after an insert, double
the bucket-array size and reinsert every existing entry.

Required members:

```csharp
public void Put(TKey key, TValue value)
public bool TryGet(TKey key, out TValue value)
public bool Remove(TKey key)
public bool ContainsKey(TKey key)
public int Count { get; }
```

`Put` is an **upsert**: if `key` is already present, update its value
in place (don't add a duplicate entry, and don't increment `Count`).

Example:

```csharp
var map = new SimpleHashMap<string, int>();
map.Put("apples", 3);
map.Put("apples", 5);   // updates, doesn't duplicate
map.TryGet("apples", out int count); // true, count == 5
map.Count;               // 1
map.Remove("apples");    // true
map.TryGet("apples", out _); // false
```

Target complexity: O(1) average time for `Put`/`TryGet`/`Remove`/
`ContainsKey` (amortized across occasional resizes), O(n) total space.

### 2. GroupAnagrams

```csharp
public static List<List<string>> GroupAnagrams(string[] strs)
```

Given an array of strings, group the strings so that anagrams of each
other (same characters, any order, e.g. `"eat"` and `"tea"`) end up in
the same inner list. The order of the outer list, and the order of
strings within each inner group, does not matter.

Example:

```
Input:  ["eat", "tea", "tan", "ate", "nat", "bat"]
Output: [["ate","eat","tea"], ["nat","tan"], ["bat"]]   (order-independent)
```

```
Input:  []
Output: []
```

Target complexity: O(n · k log k) time (n strings, average length k —
dominated by sorting each string's characters to build a grouping key),
O(n · k) space.

### 3. FirstNonRepeatingChar

```csharp
public static char? FirstNonRepeatingChar(string s)
```

Return the first character in `s` that appears exactly once, scanning
left to right. If every character repeats (or `s` is empty), return
`null`.

Example:

```
Input:  "swiss"
Output: 'w'     ('s' repeats, 'w' is the first char with count 1)

Input:  "aabb"
Output: null    (every character repeats)
```

Target complexity: O(n) time, O(1) space (fixed-size alphabet assumption)
or O(n) space for a general-purpose dictionary-based count.

### 4. TwoSumOptimal

```csharp
public static int[] TwoSumOptimal(int[] nums, int target)
```

Given an array of integers and a target, return the indices of the two
numbers that add up to `target`, using a single hash-map pass (this is
the hashing-focused revisit of module 1's `TwoSum` — same problem, same
target complexity, just make sure you're solving it with a dictionary
rather than nested loops).

**If no two numbers in `nums` sum to `target`, throw an
`ArgumentException`.** Do not return a sentinel value like `[-1, -1]` —
the caller should be able to trust that a returned array is always a
valid answer.

Example:

```
Input:  nums = [2, 7, 11, 15], target = 9
Output: [0, 1]      (nums[0] + nums[1] == 9)

Input:  nums = [1, 2, 3], target = 100
Output: throws ArgumentException (no valid pair exists)
```

Target complexity: O(n) time, O(n) space.

## Hints

<details>
<summary>Hint: SimpleHashMap — nudge</summary>

You need an array where each slot can hold multiple entries (because
different keys can hash to the same bucket). What C# collection type
naturally grows as you add to it, and could live inside each array slot?

</details>

<details>
<summary>Hint: SimpleHashMap — approach</summary>

Store `private List<(TKey Key, TValue Value)>[] _buckets`. To find a
key's bucket: take `key.GetHashCode()`, mask off the sign bit (a hash
code can be negative, and you don't want a negative array index), then
use `%` against `_buckets.Length`. `Put` and `TryGet` both need to scan
the target bucket's list looking for an entry whose `.Key` equals the
key you're looking for — extract that scan into a small private helper
if you find yourself repeating it. Track `Count` yourself as a field
that `Put`/`Remove` update; don't recompute it by walking every bucket
every time someone reads `Count`.

</details>

<details>
<summary>Hint: SimpleHashMap — near-solution</summary>

```csharp
private int BucketIndex(TKey key) =>
    (key.GetHashCode() & 0x7FFFFFFF) % _buckets.Length;

public void Put(TKey key, TValue value)
{
    int index = BucketIndex(key);
    var bucket = _buckets[index] ??= new List<(TKey, TValue)>();

    for (int i = 0; i < bucket.Count; i++)
    {
        if (bucket[i].Key.Equals(key))
        {
            bucket[i] = (key, value); // upsert
            return;
        }
    }

    bucket.Add((key, value));
    _count++;

    if ((double)_count / _buckets.Length > 0.75)
    {
        Resize(); // allocate a bigger array, re-Put every old entry
    }
}
```

`Resize()` should allocate a new, larger `_buckets` array (double the
old length is the standard choice), reset `_count` to 0, then loop over
every entry in every old bucket and call `Put` again on the new array —
this naturally rehashes each entry against the new bucket count. Build
`TryGet`, `Remove`, and `ContainsKey` the same way: compute the bucket
index, then scan that one bucket's list.

</details>

<details>
<summary>Hint: GroupAnagrams — nudge</summary>

Two strings are anagrams if and only if they produce the *same result*
when you sort their characters. What if that sorted result became a
dictionary key?

</details>

<details>
<summary>Hint: GroupAnagrams — approach</summary>

Build a `Dictionary<string, List<string>>`. For each input string,
compute its sorted-character form (`"eat"` and `"tea"` both sort to
`"aet"`), and append the *original* string to the list stored under that
sorted key (creating the list first if the key is new). At the end,
`dictionary.Values` gives you the groups — convert to `List<List<string>>`.

</details>

<details>
<summary>Hint: GroupAnagrams — near-solution</summary>

```csharp
var groups = new Dictionary<string, List<string>>();
foreach (string s in strs)
{
    char[] chars = s.ToCharArray();
    Array.Sort(chars);
    string key = new string(chars);

    if (!groups.TryGetValue(key, out var group))
    {
        group = new List<string>();
        groups[key] = group;
    }
    group.Add(s);
}
return groups.Values.ToList();
```

</details>

<details>
<summary>Hint: FirstNonRepeatingChar — nudge</summary>

You need to know each character's total count *before* you can decide
whether the first occurrence you see is really "non-repeating" — that
implies two passes over the string.

</details>

<details>
<summary>Hint: FirstNonRepeatingChar — approach</summary>

Pass 1: build a `Dictionary<char, int>` (or a fixed-size `int[128]` array
if you're willing to assume ASCII) of character → count. Pass 2: walk the
string again in order, and return the first character whose count is
exactly 1. If you reach the end without finding one, return `null`.

</details>

<details>
<summary>Hint: FirstNonRepeatingChar — near-solution</summary>

```csharp
var counts = new Dictionary<char, int>();
foreach (char c in s)
{
    counts[c] = counts.GetValueOrDefault(c) + 1;
}
foreach (char c in s)
{
    if (counts[c] == 1)
    {
        return c;
    }
}
return null;
```

</details>

<details>
<summary>Hint: TwoSumOptimal — nudge</summary>

For each number, ask "have I already seen the *other* number I'd need
to reach the target?" rather than searching the rest of the array for it.

</details>

<details>
<summary>Hint: TwoSumOptimal — approach</summary>

Use a `Dictionary<int, int>` mapping a value you've seen to its index.
Walk the array once. For `nums[i]`, compute `complement = target -
nums[i]`. If `complement` is already a key in the dictionary, you've
found your pair — return `[dictionary[complement], i]`. Otherwise, record
`nums[i] -> i` and keep scanning. If the loop ends with nothing found,
throw.

</details>

<details>
<summary>Hint: TwoSumOptimal — near-solution</summary>

```csharp
var seen = new Dictionary<int, int>(); // value -> index
for (int i = 0; i < nums.Length; i++)
{
    int complement = target - nums[i];
    if (seen.TryGetValue(complement, out int complementIndex))
    {
        return new[] { complementIndex, i };
    }
    seen[nums[i]] = i;
}
throw new ArgumentException("No two sum solution exists for the given input.");
```

</details>

## Running your work

```
cd modules/05-hashing/tests/Hashing.Tests && dotnet test
```

## If you're stuck

`solution/Solution.cs` has the full reference implementation — but make
a real attempt first. Struggling productively (reading the hints above
in order, trying an approach, hitting a wall, trying again) is where the
learning actually happens; jumping straight to the solution skips that.

If you're offline with only a small local LLM to help, it will do much
better with a narrow, well-specified question than an open-ended one.
Concretely: paste it the exact method signature and constraints, and ask
for an **approach description in plain English first** — not code. For
example:

> "I need to implement this C# method:
> `public static char? FirstNonRepeatingChar(string s)` — it should
> return the first character in `s` that appears exactly once, or null
> if none does. What's an O(n)-time approach using a dictionary? Describe
> the algorithm in words, don't write code yet."

Once you understand the approach it describes, implement it yourself.
Only ask for code as a last resort, and even then, ask it to explain the
code line by line afterward so you're not just copy-pasting something
you don't understand.
