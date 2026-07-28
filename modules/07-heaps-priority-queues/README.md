# Module 7: Heaps & Priority Queues

Learn to build and reason about a binary heap from scratch — the array-backed
tree structure behind priority queues, "top-k" problems, and merging sorted
sources. By the end you should be able to explain (and implement) sift-up and
sift-down from memory, without needing to look up the index formulas.

## C# syntax you'll need

If you've been away from C# for a while, work through this section first —
everything here is used in the stubs.

### Generics basics

A generic type or method is parameterized by a type placeholder (conventionally
`T`), so the same code works for `int`, `string`, or any other type without
duplicating it.

```csharp
// A generic method: T is decided by whatever you call it with.
static T First<T>(List<T> items) => items[0];

int firstInt = First(new List<int> { 1, 2, 3 });       // T = int
string firstStr = First(new List<string> { "a", "b" }); // T = string
```

You won't need to write your own generic types in this module (`MinHeap` is
specialized to `int` on purpose, to keep the index math front and center), but
you will use generic BCL types below.

### `List<T>`

`List<T>` is a resizable array. It's what backs our heap.

```csharp
var numbers = new List<int>();
numbers.Add(10);          // append
numbers.Add(5);
numbers[0] = 99;           // index like an array
int last = numbers[^1];    // ^1 = "1 from the end" (index-from-end operator)
numbers.RemoveAt(numbers.Count - 1); // remove last element
int count = numbers.Count; // current size
```

Swapping two elements by index uses C#'s tuple-deconstruction swap, which is
idiomatic and avoids a temp variable:

```csharp
(numbers[0], numbers[1]) = (numbers[1], numbers[0]);
```

### `Dictionary<TKey, TValue>`

A hash map. You'll use one to count element frequencies.

```csharp
var counts = new Dictionary<int, int>();
int value = 7;

// GetValueOrDefault returns 0 if the key isn't present yet — avoids a
// manual "if ContainsKey" check.
counts[value] = counts.GetValueOrDefault(value) + 1;

foreach (var (key, count) in counts) // deconstructing a KeyValuePair
{
    Console.WriteLine($"{key} appeared {count} times");
}
```

### Nullable reference types (`?`)

The stubs enable `<Nullable>enable</Nullable>`, so the compiler tracks which
reference types (like `string`) can be `null`. `string? name` means "may be
null"; plain `string name` means the compiler expects it never to be null and
will warn you if you assign `null` to it. This module's public APIs don't use
nullable reference types, but you'll see the `?` syntax elsewhere in the repo,
so it's worth recognizing.

### Tuples

A lightweight way to bundle a few values without declaring a class.

```csharp
(int Value, int ListIndex) pair = (42, 0);
Console.WriteLine(pair.Value);      // named tuple field access
var (v, i) = pair;                  // deconstruction into separate variables
```

`MergeKSortedLists` in the reference solution uses a tuple to track "which
value, from which list, at which position" inside the heap.

### The heap array-index math (the crux of this module)

A binary heap is a **complete binary tree** (every level full except possibly
the last, filled left-to-right) stored **flat in an array/list** — no pointers
needed. The trick is that a node's children and parent can be computed purely
from its index:

```
For a node at index i (0-based):
    left child index  = 2*i + 1
    right child index = 2*i + 2
    parent index       = (i - 1) / 2   (integer division, so it floors)
```

Why this works: think of the array laid out level by level. Index 0 is the
root. Its two children land at indices 1 and 2. Index 1's children land at 3
and 4; index 2's children land at 5 and 6. In general, level `L` starts right
after level `L-1` finishes, and each parent "claims" two consecutive slots for
its children — which is exactly what `2i+1` / `2i+2` compute. Parent-lookup is
just the algebraic inverse: given a child index, `(i-1)/2` (integer division
truncates, which correctly maps both `2i+1` and `2i+2` back to `i`).

```
        [0]
       /    \
    [1]      [2]
   /   \    /   \
 [3]   [4] [5]  [6]

Array: [ a0, a1, a2, a3, a4, a5, a6 ]
        idx 0  idx of a1's parent = (1-1)/2 = 0  ✓
                  idx of a4's parent = (4-1)/2 = 1 ✓ (integer division: 3/2 = 1)
```

Two operations keep the heap valid:

- **Sift up** (after inserting at the end): compare the new element to its
  parent; if it's smaller (for a min-heap), swap and repeat from the new
  position. Stop when it's not smaller than its parent, or it reaches the
  root.
- **Sift down** (after replacing the root, e.g. during extraction): compare
  the element to its smaller child; if the child is smaller, swap and repeat
  from the new position. Stop when it's smaller than both children, or it has
  no children left.

Both operations are O(log n) because the tree height is O(log n) for n
elements.

### `System.Collections.Generic.PriorityQueue<TElement, TPriority>`

.NET ships a built-in generic min-heap: `PriorityQueue<TElement, TPriority>`.
It's what you're building a hand-rolled version of in `MinHeap`, and it's what
you'd actually reach for in production code instead of writing your own.
Basic usage:

```csharp
var pq = new PriorityQueue<string, int>();
pq.Enqueue("low priority task", 10);
pq.Enqueue("urgent task", 1);         // lower number = dequeued first
string next = pq.Dequeue();           // "urgent task"
int count = pq.Count;
```

The `Problems` methods in this module are free to use either your own
`MinHeap` or the BCL `PriorityQueue` — pick whichever fits, and leave a
comment noting which you chose (the reference solution uses `PriorityQueue`
and explains why in a comment).

## Problems

### 1. MinHeap operations

Implement a min-heap of `int` backed by a `List<int>`.

```csharp
public class MinHeap
{
    public int Count { get; }
    public bool IsEmpty { get; }
    public void Insert(int value);
    public int ExtractMin();   // throws InvalidOperationException if empty
    public int Peek();         // throws InvalidOperationException if empty
}
```

**Example:**

```csharp
var heap = new MinHeap();
heap.Insert(5);
heap.Insert(2);
heap.Insert(8);
heap.Peek();       // 2
heap.ExtractMin();  // 2
heap.ExtractMin();  // 5
heap.Count;         // 1
```

**Complexity target:** `Insert` and `ExtractMin` are O(log n); `Peek` and
`IsEmpty`/`Count` are O(1). Space is O(n) for n stored elements.

### 2. FindKthLargest

Given an unsorted array of integers and an integer `k`, return the kth
largest element (1st largest is the maximum, not the 1st distinct value —
duplicates count individually).

```csharp
public static int FindKthLargest(int[] nums, int k)
```

**Examples:**

- `FindKthLargest([3, 2, 1, 5, 6, 4], 2)` → `5`
- `FindKthLargest([3, 2, 3, 1, 2, 4, 5, 5, 6], 4)` → `4`

**Complexity target:** O(n log k) time using a size-k heap, O(k) space. (A
full sort would be O(n log n) — better if k is small relative to n, which is
the point of the heap approach.)

### 3. TopKFrequent

Given an array of integers and an integer `k`, return the `k` most frequently
occurring values. **The order of the returned list does not matter** — tests
compare it as a set.

```csharp
public static List<int> TopKFrequent(int[] nums, int k)
```

**Examples:**

- `TopKFrequent([1, 1, 1, 2, 2, 3], 2)` → `{1, 2}` (order doesn't matter)
- `TopKFrequent([5, 5, 5, 6, 6, 7], 1)` → `{5}`

**Complexity target:** O(n log k) time (counting is O(n), heap operations on
at most k+1 elements at a time are O(log k)), O(n) space for the frequency
map.

### 4. MergeKSortedLists

Given a list of `k` lists, each already sorted in ascending order, merge them
into one fully sorted list. This previews the classic "merge k sorted linked
lists" interview problem, generalized to plain `List<int>` so you can focus on
the heap mechanics instead of node-pointer bookkeeping.

```csharp
public static List<int> MergeKSortedLists(List<List<int>> lists)
```

**Examples:**

- `MergeKSortedLists([[1,4,5], [1,3,4], [2,6]])` → `[1,1,2,3,4,4,5,6]`
- `MergeKSortedLists([])` → `[]`

**Complexity target:** O(N log k) time, where N is the total number of
elements across all lists and k is the number of lists — the heap never holds
more than k elements at once. O(N) space for the result (plus O(k) for the
heap).

## Hints

### MinHeap

<details>
<summary>Hint 1</summary>

Think of the heap purely as index arithmetic on a `List<int>` — you never
need a `Node` class with left/right pointers. Re-read the "heap array-index
math" section above until `2i+1`, `2i+2`, and `(i-1)/2` feel automatic.

</details>

<details>
<summary>Hint 2</summary>

`Insert`: `_items.Add(value)`, then starting from the last index, compare with
the parent index `(i-1)/2`; if the current value is smaller, swap and move `i`
to the parent index; repeat until you stop swapping or reach index 0.

`ExtractMin`: save `_items[0]` to return later. Copy the last element into
index 0, then remove the last slot (`_items.RemoveAt(_items.Count - 1)`). If
the list isn't empty, sift down from index 0: compare with both children
(guard against a child index going past `_items.Count - 1`), swap with the
smaller child if it's smaller than the current node, and repeat from the new
position.

</details>

<details>
<summary>Hint 3 (near-solution)</summary>

```csharp
private void SiftUp(int index)
{
    while (index > 0)
    {
        int parent = (index - 1) / 2;
        if (_items[index] >= _items[parent]) break;
        (_items[index], _items[parent]) = (_items[parent], _items[index]);
        index = parent;
    }
}

private void SiftDown(int index)
{
    int count = _items.Count;
    while (true)
    {
        int left = 2 * index + 1, right = 2 * index + 2, smallest = index;
        if (left < count && _items[left] < _items[smallest]) smallest = left;
        if (right < count && _items[right] < _items[smallest]) smallest = right;
        if (smallest == index) break;
        (_items[index], _items[smallest]) = (_items[smallest], _items[index]);
        index = smallest;
    }
}
```

Wire `Insert` to call `SiftUp` after adding, and `ExtractMin` to call
`SiftDown(0)` after moving the last element to the root (only if the heap
isn't empty afterward).

</details>

### FindKthLargest

<details>
<summary>Hint 1</summary>

Sorting and indexing would work but is O(n log n). A heap lets you do better
when k is small: you only need to track the k largest values seen so far, not
sort everything.

</details>

<details>
<summary>Hint 2</summary>

Keep a **min-heap of size at most k**. Push every value in; whenever the
heap's size exceeds k, pop the minimum (it can't be among the k largest).
After processing all values, the heap's minimum (its root) is exactly the kth
largest.

</details>

<details>
<summary>Hint 3 (near-solution)</summary>

```csharp
var minHeap = new PriorityQueue<int, int>();
foreach (int num in nums)
{
    minHeap.Enqueue(num, num);
    if (minHeap.Count > k) minHeap.Dequeue();
}
return minHeap.Peek();
```

</details>

### TopKFrequent

<details>
<summary>Hint 1</summary>

Two separate steps: first figure out *how often* each value occurs, then
figure out *which* values occur most.

</details>

<details>
<summary>Hint 2</summary>

Step 1: build a `Dictionary<int, int>` mapping value → count. Step 2: push
each `(value, count)` pair into a heap keyed on count, capping the heap at
size k the same way as `FindKthLargest` — evict the current minimum-frequency
entry whenever size exceeds k.

</details>

<details>
<summary>Hint 3 (near-solution)</summary>

```csharp
var counts = new Dictionary<int, int>();
foreach (int num in nums) counts[num] = counts.GetValueOrDefault(num) + 1;

var minHeap = new PriorityQueue<int, int>();
foreach (var (value, frequency) in counts)
{
    minHeap.Enqueue(value, frequency);
    if (minHeap.Count > k) minHeap.Dequeue();
}
return minHeap.UnorderedItems.Select(entry => entry.Element).ToList();
```

</details>

### MergeKSortedLists

<details>
<summary>Hint 1</summary>

At any point during the merge, the next value to output must be the smallest
"current head" among all the lists that still have unconsumed elements. A
heap is a natural fit for repeatedly finding "the smallest of several
candidates."

</details>

<details>
<summary>Hint 2</summary>

Seed the heap with the first element of every non-empty list, but you need to
remember *which list* and *which position* each value came from so that when
you pop it, you know what to push next. A tuple `(value, listIndex,
elementIndex)` works well, with the heap priority being `value`.

</details>

<details>
<summary>Hint 3 (near-solution)</summary>

```csharp
var minHeap = new PriorityQueue<(int Value, int ListIndex, int ElementIndex), int>();
for (int listIndex = 0; listIndex < lists.Count; listIndex++)
{
    if (lists[listIndex].Count > 0)
    {
        int first = lists[listIndex][0];
        minHeap.Enqueue((first, listIndex, 0), first);
    }
}

var result = new List<int>();
while (minHeap.Count > 0)
{
    var (value, listIndex, elementIndex) = minHeap.Dequeue();
    result.Add(value);

    int next = elementIndex + 1;
    if (next < lists[listIndex].Count)
    {
        int nextValue = lists[listIndex][next];
        minHeap.Enqueue((nextValue, listIndex, next), nextValue);
    }
}
return result;
```

</details>

## Running your work

```
cd modules/07-heaps-priority-queues/tests/Heaps.Tests && dotnet test
```

## If you're stuck

`solution/Solution.cs` has a complete, commented reference implementation —
but try to get genuinely stuck first. Productive struggle (drawing the heap
array on paper, tracing sift-up/sift-down by hand for 5-6 elements) is where
the learning actually happens; reading the answer too early just makes it
feel familiar without making it stick.

If you're offline with only a weak local Ollama model for help, get more out
of it by being specific:

- Paste the **exact method signature** you're implementing, not a vague
  description ("I'm implementing `public int ExtractMin()` on a min-heap
  backed by `List<int>` — here's my current attempt: ...").
- State the **constraint or behavior** you're unsure about ("should throw
  `InvalidOperationException` when empty — how do I check that before
  touching the list?").
- Ask for **approach before code** ("walk me through the sift-down steps in
  words first, don't write C# yet") — small local models tend to hallucinate
  less when reasoning in plain language before generating syntax, and you'll
  learn more by writing the code yourself once you understand the steps.
