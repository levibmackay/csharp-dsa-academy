# Module 1: Arrays &amp; Strings

Learn to manipulate C#'s core linear data structures — arrays, `List<T>`, `string`, and `char[]` — and practice the two-pointer, hashing, and running-sum techniques that solve the vast majority of array/string interview questions. By the end you'll be comfortable choosing between in-place O(1)-space tricks and hash-based O(n) lookups.

## C# syntax you'll need

If you haven't touched C# in a while, this section is a dense refresher. Every snippet below is valid, runnable C#. Read it once now, then come back to it as reference while you solve the problems.

### Arrays

```csharp
// Fixed-size, declared with a type and square brackets.
int[] nums = new int[5];          // all zeros: [0, 0, 0, 0, 0]
int[] nums2 = { 1, 2, 3, 4, 5 };  // array literal
int[] nums3 = new int[] { 1, 2, 3 };

int length = nums.Length;          // property, NOT a method (no parens)
int first = nums2[0];
nums2[0] = 99;                     // arrays are mutable in place

// Arrays are reference types: passing one to a method lets the
// method mutate the caller's array without returning anything.
void ZeroOut(int[] arr)
{
    for (int i = 0; i < arr.Length; i++)
    {
        arr[i] = 0;
    }
}
ZeroOut(nums2); // nums2 is now all zeros — no `ref` needed for this

// Useful static helpers
Array.Empty<int>();                // a reusable empty array (avoid `new int[0]`)
Array.Reverse(nums2);              // reverses in place
Array.Sort(nums2);                 // sorts in place
```

### `List<T>` — the resizable array

```csharp
using System.Collections.Generic;

var list = new List<int>();       // empty, growable
list.Add(10);
list.Add(20);
list.Insert(0, 5);                 // insert at index
list.RemoveAt(1);                  // remove by index
int count = list.Count;            // NOT .Length — Count for List<T>
bool has = list.Contains(10);
int[] backToArray = list.ToArray();
List<int> fromArray = nums2.ToList();
```

Use `int[]`/`char[]` when the size is fixed and you want the tightest
possible memory layout (which is what most of these problems ask for).
Use `List<T>` when you need to grow/shrink dynamically.

### `Dictionary<TKey, TValue>` — the hash map

```csharp
using System.Collections.Generic;

var seen = new Dictionary<int, int>();   // e.g. value -> index
seen[5] = 0;                              // add or overwrite key 5
seen.Add(7, 1);                           // add; throws if key 7 already exists

if (seen.ContainsKey(5)) { /* ... */ }

// TryGetValue is the idiomatic, single-lookup way to check-and-read:
if (seen.TryGetValue(5, out int index))
{
    Console.WriteLine($"5 is at index {index}");
}

foreach (KeyValuePair<int, int> kvp in seen)
{
    Console.WriteLine($"{kvp.Key} -> {kvp.Value}");
}
```

`Dictionary<int,int>` is the workhorse for turning an O(n²) nested-loop
search into an O(n) single pass — you've seen this idea before, C#'s
version is just `TryGetValue`.

### `string` vs `char[]` — mutability matters

```csharp
string s = "hello";
// s[0] = 'H';   // COMPILE ERROR — strings are immutable in C#.
// Every "modification" of a string actually creates a brand new string.

char[] chars = s.ToCharArray();   // copy the string into a mutable array
chars[0] = 'H';                    // fine — arrays are mutable
string rebuilt = new string(chars); // "Hello"

// Common string operations
int len = s.Length;
char c = s[2];                     // indexing a string reads a char (read-only)
string sub = s.Substring(1, 3);    // "ell" (startIndex, length)
bool eq = s == "hello";            // == compares content for strings, not reference
string joined = string.Concat("a", "b");
string joined2 = $"{s} world";     // string interpolation
foreach (char ch in s) { /* iterate characters */ }
```

**Key idea:** when a problem says "reverse this string in place" or
"in O(1) extra space," they really mean `char[]`, because `string` in
C# can never be mutated in place.

### `out` and `ref` parameters

```csharp
// `out`: the method MUST assign the parameter before returning.
// Used to "return" more than one value without a tuple.
bool TryParseNumber(string text, out int result)
{
    return int.TryParse(text, out result);
}

if (TryParseNumber("42", out int value))
{
    Console.WriteLine(value); // 42
}

// `ref`: pass a variable by reference so the method can read AND
// write the caller's variable. Caller must already have a value.
void Increment(ref int x) => x++;
int n = 5;
Increment(ref n); // n is now 6

// Arrays and other reference types (List<T>, Dictionary<>, etc.) are
// already passed "by reference" in the sense that mutating their
// CONTENTS is visible to the caller — you only need `ref`/`out` when
// you want to reassign the parameter itself to a whole new object.
```

None of this module's methods require `out`/`ref` in their public
signatures — arrays are mutated through their contents directly — but
you'll see the pattern elsewhere (e.g. `int.TryParse`), so it's worth
knowing.

### Nullable reference types

This project has `<Nullable>enable</Nullable>` turned on, which means
the compiler tracks whether a reference type (`string`, `int[]`, any
class) is allowed to be `null`.

```csharp
string name = "levi";       // non-nullable: compiler warns if you assign null
string? maybeName = null;   // nullable: the `?` says "this can be null"

if (maybeName != null)
{
    Console.WriteLine(maybeName.Length); // safe after the null check
}
```

None of the method signatures in this module use `?`, meaning the
parameters are guaranteed non-null by the caller (the tests never pass
`null`) — you don't need null checks for this module's problems.

### Tuples and multi-value returns

```csharp
// Anonymous tuple: quick way to bundle values without declaring a class.
(int, int) MinMax(int[] nums)
{
    return (nums.Min(), nums.Max());
}

var result = MinMax(new[] { 3, 1, 4 });
Console.WriteLine(result.Item1); // 1 (unnamed access)

// Named tuple elements — much more readable:
(int min, int max) MinMaxNamed(int[] nums)
{
    return (nums.Min(), nums.Max());
}

var r = MinMaxNamed(new[] { 3, 1, 4 });
Console.WriteLine(r.min); // 1
Console.WriteLine(r.max); // 4

// Deconstruction
var (lo, hi) = MinMaxNamed(new[] { 3, 1, 4 });
```

None of this module's required signatures return tuples (`TwoSum`
returns `int[]`), but tuples are extremely handy for one-line swaps:

```csharp
(a, b) = (b, a); // swap without a temp variable — used for reversing in place
```

### LINQ basics (optional, but good to recognize)

```csharp
using System.Linq;

int[] nums = { 5, 3, 8, 1 };
int max = nums.Max();
int sum = nums.Sum();
bool any = nums.Any(x => x > 6);
int[] sorted = nums.OrderBy(x => x).ToArray();
int[] evens = nums.Where(x => x % 2 == 0).ToArray();
```

LINQ is convenient but usually **not** how you hit the O(n)/O(1)
targets asked for below — most LINQ methods scan the whole sequence
and often allocate new collections. Prefer explicit loops in this
module so you can reason precisely about time and space complexity.

## Problems

### 1. TwoSum

Given an array of integers and a target value, return the indices of
the two numbers that add up to the target. You may assume exactly one
valid answer exists, and you cannot use the same array element twice.

```csharp
public static int[] TwoSum(int[] nums, int target)
```

**Examples**

| Input | Output |
|---|---|
| `nums = [2, 7, 11, 15]`, `target = 9` | `[0, 1]` (`2 + 7 == 9`) |
| `nums = [3, 2, 4]`, `target = 6` | `[1, 2]` (`2 + 4 == 6`) |

**Target complexity:** O(n) time, O(n) space — use `Dictionary<int,int>`.

### 2. ReverseString

Reverse a character array **in place** (don't allocate a new array or
return a new value).

```csharp
public static void ReverseString(char[] s)
```

**Examples**

| Input | Output (array contents after the call) |
|---|---|
| `['h','e','l','l','o']` | `['o','l','l','e','h']` |
| `['a','b']` | `['b','a']` |

**Target complexity:** O(n) time, O(1) extra space.

### 3. IsAnagram

Return `true` if two strings are anagrams of each other (same
characters, same counts, different order allowed). Comparison is
**case-sensitive** — `'A'` and `'a'` are different characters.

```csharp
public static bool IsAnagram(string a, string b)
```

**Examples**

| Input | Output |
|---|---|
| `a = "anagram"`, `b = "nagaram"` | `true` |
| `a = "rat"`, `b = "car"` | `false` |
| `a = "Anagram"`, `b = "anagram"` | `false` (case-sensitive) |

**Target complexity:** O(n) time, O(1) extra space (a fixed-size count
array works if you assume ASCII input).

### 4. MaxSubArray

Given an integer array (at least one element, may contain negative
numbers), find the contiguous subarray with the largest sum and
return that sum. This is the classic **Kadane's algorithm**.

```csharp
public static int MaxSubArray(int[] nums)
```

**Examples**

| Input | Output | Why |
|---|---|---|
| `[-2, 1, -3, 4, -1, 2, 1, -5, 4]` | `6` | subarray `[4, -1, 2, 1]` |
| `[-3, -1, -2, -4]` | `-1` | all negative: best is the single largest element |

**Target complexity:** O(n) time, O(1) extra space.

### 5. RotateArray

Rotate an integer array to the **right** by `k` steps, in place. `k`
can be larger than the array length (handle it with modulo).

```csharp
public static void RotateArray(int[] nums, int k)
```

**Examples**

| Input | Output (array contents after the call) |
|---|---|
| `nums = [1,2,3,4,5,6,7]`, `k = 3` | `[5,6,7,1,2,3,4]` |
| `nums = [1,2,3]`, `k = 4` | `[3,1,2]` (`4 % 3 == 1`) |

**Target complexity:** O(n) time, O(1) extra space.

## Hints

### TwoSum

<details><summary>Hint</summary>

A brute-force nested loop checking every pair is O(n²). Can you avoid
the second loop by remembering what you've already seen?

</details>

<details><summary>Hint</summary>

As you scan the array once, for each `nums[i]`, compute
`complement = target - nums[i]`. If you've already seen `complement`
earlier in the array, you're done. Otherwise, record `nums[i]` (and
its index) so later elements can find it.

</details>

<details><summary>Hint</summary>

```csharp
var seen = new Dictionary<int, int>(); // value -> index
for (int i = 0; i < nums.Length; i++)
{
    int complement = target - nums[i];
    if (seen.TryGetValue(complement, out int j))
    {
        return new[] { j, i };
    }
    seen[nums[i]] = i;
}
throw new ArgumentException("no solution");
```

</details>

### ReverseString

<details><summary>Hint</summary>

You need O(1) extra space, so you can't build a new array. What
classic pointer technique swaps elements from both ends toward the
middle?

</details>

<details><summary>Hint</summary>

Two pointers: `left` starting at index 0, `right` starting at
`s.Length - 1`. Swap `s[left]` and `s[right]`, then move `left` up and
`right` down, stopping when they meet or cross.

</details>

<details><summary>Hint</summary>

```csharp
int left = 0;
int right = s.Length - 1;
while (left < right)
{
    (s[left], s[right]) = (s[right], s[left]); // tuple swap
    left++;
    right--;
}
```

</details>

### IsAnagram

<details><summary>Hint</summary>

If the two strings have different lengths, they can never be
anagrams — check that first and return early. Otherwise, what would
happen if you counted how many times each character appears in each
string?

</details>

<details><summary>Hint</summary>

Use a fixed-size `int[]` (size 128 covers ASCII) as a character count
table. Increment counts for characters in `a`, decrement for
characters in `b`. If every slot ends at zero, the strings are
anagrams.

</details>

<details><summary>Hint</summary>

```csharp
if (a.Length != b.Length) return false;

var counts = new int[128];
foreach (char c in a) counts[c]++;
foreach (char c in b) counts[c]--;

foreach (int count in counts)
{
    if (count != 0) return false;
}
return true;
```

</details>

### MaxSubArray

<details><summary>Hint</summary>

At each position, you have a choice: extend the current subarray, or
start a brand new subarray right here. Which one wins depends on
whether the running sum so far is helping or hurting you.

</details>

<details><summary>Hint</summary>

Track two values as you scan left to right: `currentSum` (the best
sum of a subarray ending exactly at the current index) and `bestSum`
(the best `currentSum` seen anywhere so far). At each element, decide:
is it better to add this element to `currentSum`, or to start fresh
with just this element? That's `Math.Max(nums[i], currentSum + nums[i])`.

</details>

<details><summary>Hint</summary>

```csharp
int bestSum = nums[0];
int currentSum = nums[0];
for (int i = 1; i < nums.Length; i++)
{
    currentSum = Math.Max(nums[i], currentSum + nums[i]);
    bestSum = Math.Max(bestSum, currentSum);
}
return bestSum;
```

</details>

### RotateArray

<details><summary>Hint</summary>

First handle the easy trap: `k` can be bigger than the array length,
and an empty array can't be rotated at all. What operation turns any
`k` into a value between `0` and `nums.Length - 1`?

</details>

<details><summary>Hint</summary>

`k %= nums.Length` (after guarding against `nums.Length == 0`). For
the O(1)-space rotation itself, there's a neat trick: reversing the
*whole* array, then reversing the first `k` elements, then reversing
the remaining elements, produces a right-rotation by `k`. Try it on
paper with `[1,2,3,4,5,6,7]`, `k=3`.

</details>

<details><summary>Hint</summary>

```csharp
if (nums.Length == 0) return;
k %= nums.Length;
if (k == 0) return;

Reverse(nums, 0, nums.Length - 1);
Reverse(nums, 0, k - 1);
Reverse(nums, k, nums.Length - 1);

// helper:
static void Reverse(int[] nums, int start, int end)
{
    while (start < end)
    {
        (nums[start], nums[end]) = (nums[end], nums[start]);
        start++;
        end--;
    }
}
```

</details>

## Running your work

From the repo root:

```
cd modules/01-arrays-strings/tests/ArraysStrings.Tests && dotnet test
```

This builds `ArraysStrings.csproj` (your implementation), builds the
test project, and runs every `[Fact]`/`[Theory]` in this module. A
`NotImplementedException` in any method you haven't finished yet will
show up as a failing (red) test for that case — that's expected until
you implement it.

## If you're stuck

Struggle first. Genuinely wrestling with a wrong approach and figuring
out *why* it's wrong is where most of the learning happens — don't
skip straight to the answer. Try:

- Re-reading the "C# syntax you'll need" section above.
- Working through the hints in order — each one gives away a bit more.
- Tracing through the example inputs by hand, on paper, before writing code.

Once you've made a real attempt, `solution/Solution.cs` has a complete,
correct reference implementation of every method in this module (same
signatures, same namespace). It is **not** wired into any `.csproj`,
so it won't affect your build or tests — open it directly in your
editor when you want to compare notes.

**Asking your local (offline) LLM well:** a small local model does
much better with a narrow, well-specified question than with "help me
with two sum." Give it:

1. The **exact method signature** (copy-paste it): e.g.
   `public static int[] TwoSum(int[] nums, int target)`
2. The **constraints**: "exactly one solution exists," "can't reuse
   the same element," "target is O(n) time using a Dictionary."
3. Ask for the **approach first, no code yet** — e.g. "Explain the
   algorithm in plain English/pseudocode, don't write C# yet." Read
   and understand the approach, *then* ask for the C# implementation
   as a separate follow-up. This stops a weak model from handing you
   plausible-looking code you can't debug, and forces you to
   understand the idea rather than just pasting a fix.
