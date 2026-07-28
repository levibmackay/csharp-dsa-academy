# Module 10: Searching Algorithms

Learn to search sorted (and cleverly-disguised sorted) arrays in
O(log n) time by hand-rolling binary search and its variants — no
`Array.BinarySearch`, no shortcuts. By the end you'll be comfortable
adapting the core binary-search skeleton to rotated arrays, "find a
peak" problems, and finding the first/last occurrence of a value.

## C# syntax you'll need

If you haven't touched C# in a while, this section is a dense
refresher. Every snippet below is valid, runnable C#. Read it once
now, then come back to it as reference while you solve the problems.

### Arrays and indexing

```csharp
int[] sortedArr = { -8, -3, 0, 5, 9, 12, 45 };

int length = sortedArr.Length;     // property, NOT a method (no parens)
int first = sortedArr[0];          // -8
int last = sortedArr[sortedArr.Length - 1]; // 45

// Valid indices are 0 .. Length - 1. Indexing outside that range
// throws an IndexOutOfRangeException at runtime — C# does not
// silently clamp or wrap like some languages do.
```

Arrays can absolutely contain negative numbers — nothing in this
module assumes non-negative values. `int` in C# is a signed 32-bit
integer, so negatives, zero, and positives all behave normally with
`<`, `>`, `==`, arithmetic, etc.

### Integer division and the overflow-safe midpoint

```csharp
int low = 0;
int high = 9;

// DON'T do this — it's the classic binary search bug:
// int mid = (low + high) / 2;
// If low and high are both huge, (low + high) can overflow a 32-bit
// int and wrap around to a negative number, giving a garbage index.

// DO this instead — mathematically equivalent, never overflows:
int mid = low + (high - low) / 2;

// Integer division truncates toward zero (it does NOT round):
Console.WriteLine(7 / 2);   // 3, not 3.5 and not 4
Console.WriteLine(-7 / 2);  // -3 (truncates toward zero, not -4)
```

Always use `low + (high - low) / 2` for a midpoint in this module —
it's the habit worth building now, even though these exercises use
small arrays where overflow can't actually happen.

### The `while` loop binary-search skeleton

Every problem in this module is a variation on this shape. Memorize
this skeleton — you'll adapt it, not reinvent it, for every problem
below.

```csharp
public static int BinarySearchSkeleton(int[] sortedArr, int target)
{
    int low = 0;
    int high = sortedArr.Length - 1;

    while (low <= high)   // note <=, not < — a single-element range
                           // (low == high) is still valid to check
    {
        int mid = low + (high - low) / 2;

        if (sortedArr[mid] == target)
        {
            return mid;                 // found it
        }
        else if (sortedArr[mid] < target)
        {
            low = mid + 1;               // target must be to the right
        }
        else
        {
            high = mid - 1;              // target must be to the left
        }
    }

    return -1;   // low > high: the search space is empty, target isn't here
}
```

The loop invariant is: "if `target` is anywhere in the array, it's
somewhere in `sortedArr[low..high]` inclusive." Each iteration either
returns the answer or shrinks that range by roughly half. When
`low > high`, the range is empty and the target genuinely isn't
present — that's when you return the sentinel value `-1`.

### Returning `-1` as a "not found" sentinel

`-1` is a common convention for "no valid index" because array
indices can never be negative, so `-1` can't be confused with a real
answer. Every method in this module that "doesn't find" something
returns `-1` (or `[-1, -1]` for the pair-returning problem) rather
than throwing an exception — check for it at the call site the same
way you'd check any other return value.

### `Array.BinarySearch` exists — but don't use it here

.NET ships a built-in `Array.BinarySearch(array, value)` that returns
the index of `value` in a sorted array (or a negative number encoding
an insertion point if not found). It's good to know it exists for
production code — but the entire point of this module is to build the
algorithm yourself, so implementing your own is required. Using the
built-in defeats the exercise.

### What "O(log n)" means, intuitively

Binary search is O(log n) because **each comparison eliminates half
of the remaining search space**. Starting from `n` elements:

```
n -> n/2 -> n/4 -> n/8 -> ... -> 1
```

The number of times you can halve `n` before reaching 1 is
`log2(n)`. For an array of 1,000,000 elements, that's only about 20
comparisons — versus up to 1,000,000 for a linear scan. That gap
grows enormously as `n` grows, which is why "can I binary search
this?" is one of the first questions worth asking about any sorted
(or sorted-ish) data.

## Problems

All methods below are `public static` methods on a class named
`Problems`.

### 1. BinarySearch

Given an array sorted in **ascending** order and a target value,
return the index of `target` if it exists, or `-1` if it doesn't.

```csharp
public static int BinarySearch(int[] sortedArr, int target)
```

**Examples**

| Input | Output |
|---|---|
| `sortedArr = [-8, -3, 0, 5, 9, 12, 45]`, `target = 9` | `4` |
| `sortedArr = [1, 3, 5, 7]`, `target = 6` | `-1` (not present) |

**Target complexity:** O(log n) time, O(1) space.

### 2. SearchRotatedSortedArray

An ascending sorted array of **distinct** integers has been rotated
at some unknown pivot (e.g. `[0,1,2,4,5,6,7]` becomes
`[4,5,6,7,0,1,2]`). Given the rotated array and a target, return its
index, or `-1` if it's not present.

```csharp
public static int SearchRotatedSortedArray(int[] nums, int target)
```

**Examples**

| Input | Output |
|---|---|
| `nums = [4,5,6,7,0,1,2]`, `target = 0` | `4` |
| `nums = [4,5,6,7,0,1,2]`, `target = 3` | `-1` (not present) |

**Target complexity:** O(log n) time, O(1) space.

### 3. FindPeakElement

A "peak" is an element strictly greater than both of its neighbors.
Treat indices outside the array bounds as `-infinity` (so the first
element is a peak if it's greater than the second element, and the
last element is a peak if it's greater than the second-to-last). The
array may contain multiple peaks — return the index of **any one of
them**.

```csharp
public static int FindPeakElement(int[] nums)
```

**Examples**

| Input | Output | Why |
|---|---|---|
| `nums = [1, 2, 3, 1]` | `2` | `nums[2] = 3` is greater than both neighbors |
| `nums = [1, 2, 1, 3, 5, 6, 4]` | `1` or `5` | both index 1 (`2`) and index 5 (`6`) are valid peaks |

**Target complexity:** O(log n) time, O(1) space.

### 4. SearchRange

Given an array sorted in ascending order that may contain
**duplicates**, and a target value, return a two-element array
`[firstIndex, lastIndex]` giving the first and last position of
`target` in the array. If `target` isn't present, return `[-1, -1]`.

```csharp
public static int[] SearchRange(int[] nums, int target)
```

**Examples**

| Input | Output |
|---|---|
| `nums = [5,7,7,8,8,8,10]`, `target = 8` | `[3, 5]` |
| `nums = [5,7,7,8,8,8,10]`, `target = 6` | `[-1, -1]` (not present) |

**Target complexity:** O(log n) time, O(1) space (two biased binary
searches — no linear scanning to find the boundaries).

## Hints

### BinarySearch

<details><summary>Hint</summary>

You already have the exact skeleton for this in the syntax section
above. What are `low` and `high` initialized to, and what's the loop
condition?

</details>

<details><summary>Hint</summary>

`low = 0`, `high = sortedArr.Length - 1`. Loop `while (low <= high)`.
Compute `mid = low + (high - low) / 2`. Compare `sortedArr[mid]` to
`target` and shrink the range accordingly.

</details>

<details><summary>Hint</summary>

```csharp
int low = 0;
int high = sortedArr.Length - 1;
while (low <= high)
{
    int mid = low + (high - low) / 2;
    if (sortedArr[mid] == target) return mid;
    if (sortedArr[mid] < target) low = mid + 1;
    else high = mid - 1;
}
return -1;
```

</details>

### SearchRotatedSortedArray

<details><summary>Hint</summary>

You can't just compare `nums[mid]` to `target` and decide left/right
the normal way, because the array isn't fully sorted anymore. But
here's the key idea: even though the *whole* array isn't sorted, at
least one of the two halves around `mid` always **is** contiguously
sorted. If you can figure out which half is sorted, and whether
`target` falls inside that sorted half's range, you know which
direction to go.

</details>

<details><summary>Hint</summary>

At each step, compare `nums[low]` to `nums[mid]`:

- If `nums[low] <= nums[mid]`, the **left** half (`low` to `mid`) is
  the properly sorted, contiguous one. Check whether `target` falls
  within `nums[low] <= target < nums[mid]` — if so, search the left
  half (`high = mid - 1`); otherwise search the right half
  (`low = mid + 1`).
- Otherwise, the **right** half (`mid` to `high`) must be the sorted
  one. Check whether `target` falls within
  `nums[mid] < target <= nums[high]` — if so, search the right half
  (`low = mid + 1`); otherwise search the left half
  (`high = mid - 1`).

This works because in a rotated sorted array, one side of any
midpoint is always a normal ascending run — the rotation "break point"
can only be on one side at a time.

</details>

<details><summary>Hint</summary>

```csharp
int low = 0;
int high = nums.Length - 1;
while (low <= high)
{
    int mid = low + (high - low) / 2;
    if (nums[mid] == target) return mid;

    if (nums[low] <= nums[mid])
    {
        // left half [low..mid] is sorted
        if (nums[low] <= target && target < nums[mid])
        {
            high = mid - 1;
        }
        else
        {
            low = mid + 1;
        }
    }
    else
    {
        // right half [mid..high] is sorted
        if (nums[mid] < target && target <= nums[high])
        {
            low = mid + 1;
        }
        else
        {
            high = mid - 1;
        }
    }
}
return -1;
```

</details>

### FindPeakElement

<details><summary>Hint</summary>

You don't need to find *the* peak, just *a* peak, so you don't need
to look at the whole array. What happens if you compare `nums[mid]`
to its right neighbor, `nums[mid + 1]`? Which direction is guaranteed
to still contain a peak?

</details>

<details><summary>Hint</summary>

This is the "slope trick": if `nums[mid] < nums[mid + 1]`, the array
is going *uphill* at `mid`, so there must be a peak somewhere to the
right (worst case, the last element is a peak because it's greater
than -infinity beyond the array). Move `low = mid + 1`. Otherwise
(`nums[mid] >= nums[mid + 1]`), the array is going *downhill* or flat
at `mid`, so there must be a peak at `mid` or to its left — move
`high = mid`. Note this variant uses `high = mid` (not `mid - 1`)
because `mid` itself could still be the answer.

</details>

<details><summary>Hint</summary>

```csharp
int low = 0;
int high = nums.Length - 1;
while (low < high)   // note: strictly <, and no -1/+1 asymmetry
{
    int mid = low + (high - low) / 2;
    if (nums[mid] < nums[mid + 1])
    {
        low = mid + 1;   // uphill: peak is to the right
    }
    else
    {
        high = mid;      // downhill/flat: peak is at mid or to the left
    }
}
return low;   // low == high, and it's guaranteed to be a peak
```

</details>

### SearchRange

<details><summary>Hint</summary>

A normal binary search stops the instant it finds `target`. Here you
need the *first* and *last* positions among possibly many duplicates.
What if, instead of stopping when `nums[mid] == target`, you recorded
the index and kept narrowing the search in one direction to see if
there's an even earlier (or later) occurrence?

</details>

<details><summary>Hint</summary>

This is called a **biased binary search**. Write one binary search
that finds the *leftmost* occurrence: when `nums[mid] == target`,
don't return yet — record `mid` as the best answer so far, then keep
searching the **left** half (`high = mid - 1`) in case an earlier
occurrence exists. Write a second, near-identical binary search for
the *rightmost* occurrence: when `nums[mid] == target`, record `mid`
and keep searching the **right** half (`low = mid + 1`). Run both,
and if the leftmost search found nothing, the answer is `[-1, -1]`.

</details>

<details><summary>Hint</summary>

```csharp
public static int[] SearchRange(int[] nums, int target)
{
    int first = FindBound(nums, target, findFirst: true);
    if (first == -1) return new[] { -1, -1 };
    int last = FindBound(nums, target, findFirst: false);
    return new[] { first, last };
}

private static int FindBound(int[] nums, int target, bool findFirst)
{
    int low = 0;
    int high = nums.Length - 1;
    int result = -1;
    while (low <= high)
    {
        int mid = low + (high - low) / 2;
        if (nums[mid] == target)
        {
            result = mid;
            if (findFirst) high = mid - 1;  // keep looking left
            else low = mid + 1;             // keep looking right
        }
        else if (nums[mid] < target)
        {
            low = mid + 1;
        }
        else
        {
            high = mid - 1;
        }
    }
    return result;
}
```

</details>

## Running your work

From the repo root:

```
cd modules/10-searching-algorithms/tests/Searching.Tests && dotnet test
```

This builds `Searching.csproj` (your implementation), builds the test
project, and runs every `[Fact]`/`[Theory]` in this module. A
`NotImplementedException` in any method you haven't finished yet will
show up as a failing (red) test for that case — that's expected until
you implement it.

## If you're stuck

Struggle first. Genuinely wrestling with a wrong approach and figuring
out *why* it's wrong is where most of the learning happens — don't
skip straight to the answer. Try:

- Re-reading the "C# syntax you'll need" section above.
- Working through the hints in order — each one gives away a bit more.
- Tracing through the example inputs by hand, on paper (draw the
  array and the `low`/`mid`/`high` pointers), before writing code.

Once you've made a real attempt, `solution/Solution.cs` has a complete,
correct reference implementation of every method in this module (same
signatures, same namespace). It is **not** wired into any `.csproj`,
so it won't affect your build or tests — open it directly in your
editor when you want to compare notes.

**Asking your local (offline) LLM well:** a small local model does
much better with a narrow, well-specified question than with "help me
with binary search." Give it:

1. The **exact method signature** (copy-paste it): e.g.
   `public static int SearchRotatedSortedArray(int[] nums, int target)`
2. The **constraints**: "array was ascending sorted then rotated at an
   unknown pivot," "all elements are distinct," "must be O(log n)."
3. Ask for the **approach or algorithm name first, no code yet** —
   e.g. "What's the algorithmic idea here? Don't write C# yet, just
   explain it in plain English or pseudocode." Read and understand
   the approach, *then* ask for the C# implementation as a separate
   follow-up. This stops a weak model from handing you plausible-
   looking code you can't debug, and forces you to understand the
   idea rather than just pasting a fix.
