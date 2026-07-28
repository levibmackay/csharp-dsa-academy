# Module 9: Sorting Algorithms

Learn how the classic comparison-based sorts actually work by implementing
four of them from scratch, then apply that intuition to a one-pass
three-way partitioning problem. By the end you should be able to reason
about *why* each sort has the time/space complexity it does, not just
recite the numbers.

## C# syntax you'll need

### Arrays

```csharp
int[] arr = { 5, 3, 8, 1 };   // array literal
int[] arr2 = new int[5];      // 5 elements, all default to 0

int first = arr[0];           // indexing (0-based)
arr[1] = 42;                  // assignment by index
int count = arr.Length;       // NOT arr.Length() — it's a property, no parens

// Loop over every index
for (int i = 0; i < arr.Length; i++)
{
    Console.WriteLine(arr[i]);
}

// Loop over every value (read-only, no index)
foreach (int value in arr)
{
    Console.WriteLine(value);
}
```

Arrays in C# are reference types — when you pass `int[] arr` into a
method, the method receives a reference to the *same* array. Mutating
`arr[i]` inside the method changes the caller's array too. That's exactly
what "sort in place" means for every method in this module: you mutate
the array you were given and return `void`.

### `for` and `while` loops

```csharp
// Classic counting loop
for (int i = 0; i < arr.Length; i++) { /* ... */ }

// Counting down
for (int i = arr.Length - 1; i >= 0; i--) { /* ... */ }

// while loop — condition checked before each iteration
int i = 0;
while (i < arr.Length)
{
    i++;
}

// do-while — body runs at least once
int j = 0;
do
{
    j++;
} while (j < 10);
```

### Tuple swap syntax

C# lets you swap two variables (or two array elements) in one line
without a temporary variable, using a tuple deconstruction:

```csharp
(arr[i], arr[j]) = (arr[j], arr[i]);
```

This evaluates the right-hand tuple `(arr[j], arr[i])` first, then
assigns its two values back into `arr[i]` and `arr[j]` respectively. It's
equivalent to the old three-line swap:

```csharp
int temp = arr[i];
arr[i] = arr[j];
arr[j] = temp;
```

You'll use this constantly in this module.

### Recursion basics

A recursive method calls itself on a smaller version of the problem
until it hits a **base case** that stops the recursion. Every recursive
method needs:

1. A base case (the simplest input, solved directly, no further
   recursive call).
2. A recursive case that makes the problem smaller and calls itself.

```csharp
private static int Factorial(int n)
{
    if (n <= 1)
    {
        return 1; // base case
    }
    return n * Factorial(n - 1); // recursive case, n is shrinking toward 1
}
```

Merge sort and quicksort are both naturally recursive: they solve a
range of the array by splitting it and recursing on smaller ranges,
until the range is small enough (0 or 1 elements) to be trivially
"sorted" already — that's the base case.

### Private static helper methods

It's normal (and expected in this module) to write small private helper
methods that your public method calls internally. They aren't part of
the public API, so mark them `private`:

```csharp
public static class Sorts
{
    public static void MergeSort(int[] arr)
    {
        // ... calls a private helper to do the recursive work
        MergeSortRange(arr, 0, arr.Length - 1);
    }

    private static void MergeSortRange(int[] arr, int low, int high)
    {
        // ...
    }
}
```

`static` means the method belongs to the class itself, not to an
instance of it — you never write `new Sorts()`, you just call
`Sorts.MergeSort(arr)` directly.

### `Array.Sort`, `IComparer<T>`, `Comparison<T>` (context only — don't use these)

.NET ships a built-in, highly-optimized `Array.Sort(arr)` that you could
call to sort an array in one line. **Don't use it in this module** — the
whole point here is to build the sorting logic yourself. It's worth
knowing it exists, though, because in real code you'd almost always
reach for it (or `.OrderBy(...)` on `IEnumerable<T>`) instead of hand
rolling a sort. If you ever need custom ordering, `Array.Sort` accepts
either an `IComparer<T>` (an object with a `Compare(x, y)` method) or a
`Comparison<T>` delegate (a plain function `(x, y) => int`). You won't
need either of those for this module — they're mentioned so the names
aren't a total mystery later.

### Comparison table

| Algorithm | Best | Average | Worst | Space | Stable? | In-place? | Why |
|---|---|---|---|---|---|---|---|
| Bubble Sort | O(n) | O(n²) | O(n²) | O(1) | Yes | Yes | Best case is one pass with no swaps (early-exit flag) on already-sorted input; otherwise every pass is O(n) and you need up to n passes, giving O(n²). |
| Insertion Sort | O(n) | O(n²) | O(n²) | O(1) | Yes | Yes | Best case is one comparison per element on already-sorted input (nothing to shift); worst case (reverse-sorted) shifts almost the whole sorted prefix for every new element. |
| Merge Sort | O(n log n) | O(n log n) | O(n log n) | O(n) | Yes | No | Always splits the array in half (log n levels) and does O(n) work merging at each level, regardless of input order — no early exit, no worst case. |
| Quicksort | O(n log n) | O(n log n) | O(n²) | O(1)\* | No | Yes | Average case splits roughly in half each partition (log n levels of O(n) work); worst case happens when the pivot is always the smallest/largest element (e.g. already-sorted input with a naive pivot), degenerating to n levels. |

\* Quicksort partitions in place (O(1) extra space for the swaps), but
the recursion itself uses O(log n) stack space on average (O(n) worst
case) — often noted separately from "auxiliary space."

**Stable** means equal elements keep their original relative order
after sorting. **In-place** means the algorithm uses O(1) (or O(log n)
for recursion stack) extra memory beyond the input array, rather than
allocating a new structure proportional to n.

## Problems

### `BubbleSort`

Sort an array of integers in ascending order using bubble sort: repeatedly
scan the array, swapping any pair of adjacent elements that are out of
order, until a full scan makes no swaps.

**Signature:**
```csharp
public static void BubbleSort(int[] arr)
```

**Examples:**
```
Input:  [5, 3, 8, 1, 9, 2]
Output: [1, 2, 3, 5, 8, 9]   (arr is modified in place)

Input:  [1, 2, 3]
Output: [1, 2, 3]            (already sorted — should still work, ideally with less work)
```

**Target complexity:** O(n²) time (worst/average), O(n) best case, O(1)
extra space.

### `InsertionSort`

Sort an array of integers in ascending order using insertion sort: build
up a sorted prefix of the array one element at a time, inserting each
new element into its correct position within that prefix by shifting
larger elements one slot to the right.

**Signature:**
```csharp
public static void InsertionSort(int[] arr)
```

**Examples:**
```
Input:  [5, 3, 8, 1, 9, 2]
Output: [1, 2, 3, 5, 8, 9]

Input:  [9, 7, 5, 3, 1]
Output: [1, 3, 5, 7, 9]
```

**Target complexity:** O(n²) time (worst/average), O(n) best case
(nearly-sorted input), O(1) extra space.

### `MergeSort`

Sort an array of integers in ascending order using merge sort:
recursively divide the array into halves until each piece has 0 or 1
elements (trivially sorted), then merge sorted pieces back together in
order.

**Signature:**
```csharp
public static void MergeSort(int[] arr)
```

**Examples:**
```
Input:  [5, 3, 8, 1, 9, 2]
Output: [1, 2, 3, 5, 8, 9]

Input:  []
Output: []
```

**Target complexity:** O(n log n) time in all cases, O(n) extra space.

### `QuickSort`

Sort an array of integers in ascending order using quicksort: choose a
pivot element, partition the array in place so smaller elements end up
left of the pivot and larger elements end up right of it (with the
pivot landing in its final sorted position), then recursively sort each
side.

**Signature:**
```csharp
public static void QuickSort(int[] arr)
```

**Examples:**
```
Input:  [5, 3, 8, 1, 9, 2]
Output: [1, 2, 3, 5, 8, 9]

Input:  [2, 2, 2, 2]
Output: [2, 2, 2, 2]
```

**Target complexity:** O(n log n) average time, O(n²) worst case, O(1)
extra space for partitioning (O(log n) recursion stack on average).

All four of the above are `static void` methods on a class called
`Sorts`, and all sort `int[] arr` in place, ascending.

### `SortColors`

Given an array `nums` that contains only the integers `0`, `1`, and `2`
(think: red, white, and blue paint cans, or traffic-light colors), sort
it in place so all the `0`s come first, then all the `1`s, then all the
`2`s. This is the classic "Dutch National Flag" problem. You must do it
in a single pass through the array, and you may **not** call a
general-purpose sort (no `Array.Sort`, no calling your own `Sorts`
methods) — the whole point is a specialized O(n) approach that beats
comparison sorting for this restricted input.

**Signature:**
```csharp
public static void SortColors(int[] nums)
```

**Examples:**
```
Input:  [2, 0, 2, 1, 1, 0]
Output: [0, 0, 1, 1, 2, 2]

Input:  [2, 1, 0]
Output: [0, 1, 2]
```

**Target complexity:** O(n) time, O(1) extra space.

This method is `static void SortColors(int[] nums)` on a class called
`Problems`.

## Hints

### BubbleSort

<details><summary>Hint 1</summary>

You need two loops. The outer loop determines how many passes you make
over the array (in the worst case, up to `arr.Length - 1` passes). The
inner loop walks through the array comparing each element to its
neighbor.

</details>

<details><summary>Hint 2</summary>

In the inner loop, for each `j` from `0` up to (but not including) the
end of the unsorted region, compare `arr[j]` and `arr[j + 1]`. If
`arr[j] > arr[j + 1]`, they're out of order — swap them using the tuple
swap syntax. After each full outer-loop pass, the largest remaining
element has "bubbled" to its correct position at the end, so you can
shrink the inner loop's range by one each time.

</details>

<details><summary>Hint 3 (near-solution)</summary>

Add a `bool swapped` flag, set to `false` at the start of each outer
pass and set to `true` whenever you perform a swap. If a full pass
completes with `swapped` still `false`, the array is already sorted —
`break` out of the outer loop immediately. This is what gives bubble
sort its O(n) best case on already-sorted input. Outer loop bound:
`for (int i = 0; i < arr.Length - 1; i++)`. Inner loop bound:
`for (int j = 0; j < arr.Length - 1 - i; j++)`.

</details>

### InsertionSort

<details><summary>Hint 1</summary>

Think of it like sorting a hand of playing cards: you pick up cards one
at a time and slide each one into its correct spot among the cards
you're already holding (which are sorted). Start your outer loop at
index `1`, not `0` — a single element is trivially "sorted" already.

</details>

<details><summary>Hint 2</summary>

For each index `i`, save `arr[i]` in a variable called `key` before you
overwrite anything. Then use an inner loop with an index `j` starting at
`i - 1`, moving backward, that shifts `arr[j]` one slot to the right
(`arr[j + 1] = arr[j]`) as long as `j >= 0` and `arr[j] > key`.

</details>

<details><summary>Hint 3 (near-solution)</summary>

After the inner `while` loop stops (either `j < 0` or `arr[j] <= key`),
the gap you've opened up at index `j + 1` is exactly where `key`
belongs — write `arr[j + 1] = key`. Structure:

```csharp
for (int i = 1; i < arr.Length; i++)
{
    int key = arr[i];
    int j = i - 1;
    while (j >= 0 && arr[j] > key)
    {
        arr[j + 1] = arr[j];
        j--;
    }
    arr[j + 1] = key;
}
```

</details>

### MergeSort

<details><summary>Hint 1</summary>

This one needs recursion. Write a `private static void` helper that
takes the array plus `low` and `high` index bounds (instead of
recursing on `MergeSort` directly, which only takes the array). Your
public `MergeSort(int[] arr)` should just call
`MergeSortRange(arr, 0, arr.Length - 1)`.

</details>

<details><summary>Hint 2</summary>

Base case: if `low >= high`, the range has 0 or 1 elements and is
already sorted — return immediately. Otherwise, compute
`int mid = low + (high - low) / 2;` (this avoids integer overflow —
prefer it over `(low + high) / 2`), recursively sort `[low, mid]` and
`[mid + 1, high]`, then merge the two sorted halves back together.

</details>

<details><summary>Hint 3 (near-solution)</summary>

The merge step: copy `arr[low..mid]` into a temporary `left` array and
`arr[mid+1..high]` into a temporary `right` array (both already sorted
from the recursive calls). Then walk `left` and `right` with two index
pointers, repeatedly comparing their front elements and writing the
smaller one back into `arr` (starting at index `low`), advancing
whichever pointer you just took from. When one temp array runs out,
copy the remainder of the other one straight into `arr`. Use `<=`
(not `<`) when comparing so that elements from the left half win ties —
that's what makes the sort stable.

</details>

### QuickSort

<details><summary>Hint 1</summary>

Like merge sort, this needs a `private static void` recursive helper
taking `(arr, low, high)`. Unlike merge sort, quicksort does its work
*before* recursing (in a partition step) rather than *after* (in a
merge step) — and it sorts in place, no temporary arrays needed.

</details>

<details><summary>Hint 2</summary>

Write a `private static int Partition(int[] arr, int low, int high)`
helper. Pick `arr[high]` (the last element in the range) as the pivot.
Walk through `arr[low..high-1]` with an index `j`, and maintain a
second index `i` marking the boundary of "elements confirmed to be
`<= pivot`". Whenever `arr[j] <= pivot`, increment `i` and swap
`arr[i]` with `arr[j]`. This is called the Lomuto partition scheme.

</details>

<details><summary>Hint 3 (near-solution)</summary>

After the loop in `Partition`, swap `arr[i + 1]` with `arr[high]` — this
puts the pivot in its final sorted position at index `i + 1` — and
return `i + 1`. Then in your recursive helper:

```csharp
private static void QuickSortRange(int[] arr, int low, int high)
{
    if (low >= high) return;
    int pivotIndex = Partition(arr, low, high);
    QuickSortRange(arr, low, pivotIndex - 1);
    QuickSortRange(arr, pivotIndex + 1, high);
}
```

Note the pivot itself is excluded from both recursive calls — it's
already in its final position.

</details>

### SortColors

<details><summary>Hint 1</summary>

Don't reach for a comparison sort here — think about the fact that
there are only three possible values. You can solve this with three
index pointers in a single pass: `low`, `mid`, and `high`.

</details>

<details><summary>Hint 2</summary>

Initialize `low = 0`, `mid = 0`, `high = nums.Length - 1`. The invariant
you maintain as `mid` scans forward is:
- `nums[0..low)` — everything before `low` is a known `0`.
- `nums[low..mid)` — everything between `low` and `mid` is a known `1`.
- `nums[mid..high]` — unexamined, still to be classified.
- `nums[(high+1)..end]` — everything after `high` is a known `2`.

Loop `while (mid <= high)` and look at `nums[mid]`.

</details>

<details><summary>Hint 3 (near-solution)</summary>

Three cases inside the loop, based on `nums[mid]`:

- If `nums[mid] == 0`: swap `nums[low]` and `nums[mid]`, then increment
  *both* `low` and `mid`. (Safe to advance `mid` here because the value
  swapped in from `nums[low]` was already known to be `0` or `1`.)
- If `nums[mid] == 1`: it's already in the right region — just
  increment `mid`.
- If `nums[mid] == 2`: swap `nums[mid]` and `nums[high]`, then decrement
  `high` only — **don't** advance `mid`, because the value just swapped
  in from `nums[high]` hasn't been examined yet and needs to be checked
  next.

</details>

## Running your work

```
cd modules/09-sorting-algorithms/tests/Sorting.Tests && dotnet test
```

## If you're stuck

`solution/Solution.cs` has a complete, commented reference
implementation for every method in this module — but try to get each
method working on your own first (even a slow, ugly version) before you
look. You'll retain far more that way, and the struggle is where the
actual learning happens.

If you're offline and asking a weak local LLM for help, you'll get much
better answers if you're specific instead of pasting the whole problem
and asking "how do I do this." Try:

1. **Ask for the algorithm name first, not code.** e.g. "I need to
   partition an array in place around a pivot for quicksort — what's
   this technique called and what's the general idea?" Get the concept
   straight before you ask anything about syntax.
2. **Paste the exact method signature and constraints**, not a
   paraphrase. e.g. "I'm implementing
   `public static void SortColors(int[] nums)` in C#, where nums only
   contains 0, 1, or 2. I need to do it in one pass, O(1) extra space,
   without calling a sort function. What's the three-pointer approach
   called and what are the invariants?" A weak model given a vague
   prompt ("write bubble sort") will often produce plausible-looking but
   subtly wrong code — a narrow, constrained question is much more
   likely to get a narrow, correct answer.
3. **Ask it to explain, not to write your file.** Once you understand
   the approach, implement it yourself. If you get stuck on a specific
   line, ask about that line specifically.
