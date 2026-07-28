# Module 4: Recursion & Backtracking

Learn to think recursively — a function that solves a problem by calling
itself on a smaller version of the same problem — and its close cousin
backtracking, where you explore a decision tree, undoing choices that don't
pan out. You'll build up from simple recursion (factorial), to recursion with
caching (memoized Fibonacci), to full backtracking search (permutations,
subsets, N-Queens).

## C# syntax you'll need

### Recursive methods

A recursive method is just a method that calls itself, with a **base case**
that stops the recursion and a **recursive case** that makes progress toward
that base case:

```csharp
public static int Sum(int n)
{
    if (n == 0) return 0;      // base case — stops the recursion
    return n + Sum(n - 1);      // recursive case — smaller subproblem
}

Sum(4); // 4 + Sum(3) = 4 + (3 + Sum(2)) = 4 + (3 + (2 + Sum(1))) = ... = 10
```

Every recursive method needs at least one base case reachable from every input,
or it recurses forever (and eventually throws a `StackOverflowException`,
which — unlike most exceptions — cannot be caught, and crashes the process).

### `long` vs `int`

`int` in C# is a 32-bit signed integer, capped at about 2.1 billion
(`2,147,483,647`). Factorials and Fibonacci numbers grow fast — `20!` is
already about 2.4 * 10^18, which overflows `int` silently (wraps around to a
nonsense value, no exception). `long` is a 64-bit signed integer, capped at
about 9.2 * 10^18, which is why this module's methods return `long`:

```csharp
long big = 2432902008176640000L; // the trailing L marks this as a long literal
```

### `Dictionary<TKey, TValue>`

A hash map / dictionary. You'll use `Dictionary<int, long>` as a memoization
cache — mapping "which input have I already solved" to "what was the answer."

```csharp
var cache = new Dictionary<int, long>();

cache[5] = 120; // add or overwrite the entry for key 5

if (cache.TryGetValue(5, out long value))
{
    // value is now 120; TryGetValue is the idiomatic way to check "is this
    // key present" and "get its value" in one call, without throwing if missing
    Console.WriteLine(value);
}

bool has = cache.ContainsKey(5); // true — just a presence check, no value needed
```

Avoid `cache[5]` to *read* a key that might not exist — that throws
`KeyNotFoundException`. Use `TryGetValue` (as above) or `ContainsKey` first.

### Memoization — the key technique of this module

Naive recursive Fibonacci recomputes the same subproblems over and over
exponentially:

```
Fibonacci(5)
├── Fibonacci(4)
│   ├── Fibonacci(3)
│   │   ├── Fibonacci(2) ...
│   │   └── Fibonacci(1)
│   └── Fibonacci(2)        <-- computed again!
│       ├── Fibonacci(1)
│       └── Fibonacci(0)
└── Fibonacci(3)             <-- computed again!
    ├── Fibonacci(2) ...     <-- and again!
    └── Fibonacci(1)
```

**Memoization** means: before doing the recursive work for a given input,
check a cache to see if you've already solved it. If so, return the cached
answer instantly. If not, do the work, then **store the result in the cache**
before returning, so future calls with the same input are instant.

The common C# pattern for this (used in `Problems.Fibonacci` in this module)
is a public method with a simple signature that creates the cache once, and a
private recursive helper method that takes the cache as an extra parameter and
does the actual work:

```csharp
public static long Fibonacci(int n)
{
    var cache = new Dictionary<int, long>();
    return FibonacciMemo(n, cache);
}

private static long FibonacciMemo(int n, Dictionary<int, long> cache)
{
    if (n == 0) return 0;
    if (n == 1) return 1;

    if (cache.TryGetValue(n, out long cached))
    {
        return cached; // already solved — skip the recursion entirely
    }

    long result = FibonacciMemo(n - 1, cache) + FibonacciMemo(n - 2, cache);
    cache[n] = result; // remember it for next time
    return result;
}
```

This turns Fibonacci from exponential time (roughly O(2^n) — computationally
infeasible past n ≈ 40) into linear time (O(n) — instant even for n = 1000+),
at the cost of O(n) extra space for the cache.

### `List<T>` — adding, removing, copying

```csharp
var list = new List<int>();
list.Add(1);           // [1]
list.Add(2);           // [1, 2]
list.RemoveAt(list.Count - 1); // remove the LAST element -> [1]

var copy = new List<int>(list); // a NEW list with the same elements right now
list.Add(99);                    // does NOT affect `copy` — separate list object
```

That last line matters a lot for backtracking: if you're building up one
shared `current` list as you recurse and only ever `Add`/`RemoveAt` on it,
storing a *result* means copying its contents at that moment
(`new List<int>(current)`) — not storing a reference to `current` itself,
which will keep changing as the recursion continues.

### The backtracking template

Backtracking problems (`Permutations`, `Subsets`, `CountNQueensSolutions`)
share a shape: build up a partial solution, and at each step, try a choice,
recurse deeper, then **undo that choice** ("backtrack") before trying the next
one:

```csharp
void Backtrack(/* current partial state */)
{
    if (/* current state is a complete solution */)
    {
        // record it (usually a copy, since `current` keeps mutating)
        return;
    }

    foreach (/* each possible next choice */)
    {
        // make the choice (mutate current state)
        Backtrack(/* updated state */);
        // undo the choice (mutate current state back) <- the "backtrack" step
    }
}
```

The undo step is what makes this different from plain recursion: you reuse the
*same* mutable state object across all branches of the search tree instead of
allocating a fresh copy per branch, which is far more memory-efficient — you
just have to be disciplined about undoing every change you made.

### `bool[]` as a "used" tracker

A plain array of booleans is a cheap, fast way to track "have I used this
index/column/diagonal already" during backtracking, since array indexing is
O(1):

```csharp
bool[] used = new bool[nums.Length]; // all false by default
used[2] = true;   // mark index 2 as used
used[2] = false;  // un-mark it (the "backtrack" step)
```

### `params int[]` (used in this module's tests, good to recognize)

`params` lets a method accept a variable number of arguments as if they were
an array:

```csharp
void Foo(params int[] values) { /* ... */ }

Foo(1, 2, 3);       // values = [1, 2, 3]
Foo();               // values = [] (empty array)
Foo(new[] { 1, 2 }); // also legal — pass an actual array directly
```

### LINQ's `SequenceEqual` (used in this module's tests)

`SequenceEqual` checks whether two sequences contain the same elements in the
same order:

```csharp
using System.Linq; // included automatically here via ImplicitUsings

var a = new List<int> { 1, 2, 3 };
var b = new List<int> { 1, 2, 3 };
Console.WriteLine(a.Equals(b));         // false! List<T> doesn't override Equals
Console.WriteLine(a.SequenceEqual(b));  // true — compares contents, not references
```

This matters because `List<T>` does **not** override `Equals`, so comparing
two lists with `==` or `.Equals` compares whether they're the *same object in
memory*, not whether they hold the same values — a common gotcha.

## Problems

### 1. `Problems.Factorial`

Compute n! recursively.

```csharp
public static long Factorial(int n)
```

**Examples:**

```csharp
Problems.Factorial(0);  // 1
Problems.Factorial(5);  // 120  (5 * 4 * 3 * 2 * 1)
```

Throws `ArgumentOutOfRangeException` if `n < 0`.

**Complexity target:** O(n) time (n recursive calls), O(n) space (the call
stack depth).

### 2. `Problems.Fibonacci`

Compute the n-th Fibonacci number (0-indexed) using recursion **with
memoization**.

```csharp
public static long Fibonacci(int n)
```

**Examples:**

```csharp
Problems.Fibonacci(0);  // 0
Problems.Fibonacci(1);  // 1
Problems.Fibonacci(10); // 55
```

Throws `ArgumentOutOfRangeException` if `n < 0`.

**Complexity target:** O(n) time, O(n) space — thanks to memoization. (A naive,
unmemoized recursive solution is O(2^n) time and would be far too slow for,
say, n = 45; the tests check this.)

### 3. `Problems.Permutations`

Return every distinct ordering (permutation) of a distinct-int array.

```csharp
public static List<List<int>> Permutations(int[] nums)
```

**Examples:**

```csharp
Problems.Permutations(new[] { 1, 2 });
// [[1, 2], [2, 1]]

Problems.Permutations(new[] { 1, 2, 3 });
// [[1,2,3], [1,3,2], [2,1,3], [2,3,1], [3,1,2], [3,2,1]]  (6 = 3!)
```

Order of the outer list doesn't matter; the tests check that all expected
permutations are present, not their order.

**Complexity target:** O(n * n!) time (n! permutations, each taking O(n) to
build/copy), O(n * n!) space for the output plus O(n) auxiliary space for the
recursion.

### 4. `Problems.Subsets`

Return the power set — every possible subset — of a distinct-int array,
including the empty subset and the full array itself.

```csharp
public static List<List<int>> Subsets(int[] nums)
```

**Examples:**

```csharp
Problems.Subsets(new[] { 1, 2 });
// [[], [1], [2], [1, 2]]

Problems.Subsets(Array.Empty<int>());
// [[]]  -- just the empty subset
```

Order doesn't matter; the tests check presence, not order. Within each subset,
elements should appear in the same relative order as in the input array (e.g.
for input `[1, 2, 3]`, the subset containing 1 and 3 should be `[1, 3]`, not
`[3, 1]`).

**Complexity target:** O(n * 2^n) time (2^n subsets, each up to O(n) to
build/copy), O(n * 2^n) space for the output plus O(n) auxiliary space for the
recursion.

### 5. `Problems.CountNQueensSolutions`

Return the count of distinct ways to place n non-attacking queens on an n x n
chessboard. Two queens attack each other if they share a row, a column, or
either diagonal.

```csharp
public static int CountNQueensSolutions(int n)
```

**Examples:**

```csharp
Problems.CountNQueensSolutions(1); // 1  (trivial: one queen, one square)
Problems.CountNQueensSolutions(4); // 2
Problems.CountNQueensSolutions(8); // 92 (the classic 8-queens puzzle)
```

**Complexity target:** Worst case is exponential in n (this is inherent to the
problem — there's no known polynomial solution), but a correct backtracking
solution with column/diagonal pruning should comfortably solve n = 8 in well
under a second. Space is O(n) for the tracking arrays plus O(n) recursion
depth.

## Hints

### `Factorial`

<details>
<summary>Hint</summary>

This is closer to the `Sum` example in the syntax refresher than you might
think — same shape, different operator (multiplication instead of addition)
and a different base case value.

<details>
<summary>Next hint (approach)</summary>

Check `n < 0` first and throw. Base case: `n == 0` returns `1`. Recursive
case: return `n * Factorial(n - 1)`.

<details>
<summary>Near-solution</summary>

```csharp
public static long Factorial(int n)
{
    if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
    if (n == 0) return 1;
    return n * Factorial(n - 1);
}
```

</details>
</details>
</details>

### `Fibonacci`

<details>
<summary>Hint</summary>

Re-read the "Memoization" section in the syntax refresher above — the
public/private-helper-with-cache-parameter pattern shown there is exactly what
this method needs.

<details>
<summary>Next hint (approach)</summary>

`Fibonacci(n)` validates `n` and then just creates a `Dictionary<int, long>`
and calls a private helper `FibonacciMemo(n, cache)`. That helper has base
cases for `n == 0` and `n == 1`, checks the cache before doing any recursive
work, and after computing `FibonacciMemo(n-1, cache) + FibonacciMemo(n-2,
cache)`, stores the result in the cache before returning it.

<details>
<summary>Near-solution</summary>

```csharp
public static long Fibonacci(int n)
{
    if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
    var cache = new Dictionary<int, long>();
    return FibonacciMemo(n, cache);
}

private static long FibonacciMemo(int n, Dictionary<int, long> cache)
{
    if (n == 0) return 0;
    if (n == 1) return 1;
    if (cache.TryGetValue(n, out long cached)) return cached;

    long result = FibonacciMemo(n - 1, cache) + FibonacciMemo(n - 2, cache);
    cache[n] = result;
    return result;
}
```

</details>
</details>
</details>

### `Permutations`

<details>
<summary>Hint</summary>

Re-read the "backtracking template" section above. At each step of the
recursion, the "choice" is: which not-yet-used number goes next in the
permutation being built.

<details>
<summary>Next hint (approach)</summary>

Keep a `List<int> current` (the permutation being built) and a `bool[] used`
(size `nums.Length`) marking which indices are already in `current`. When
`current.Count == nums.Length`, you've built a complete permutation — copy it
into the results list and return. Otherwise, loop over every index `i`: if
`used[i]` is true, skip it. Otherwise mark it used, add `nums[i]` to
`current`, recurse, then undo both (remove the last element of `current`,
unmark `used[i]`) before the loop moves to the next `i`.

<details>
<summary>Near-solution</summary>

```csharp
public static List<List<int>> Permutations(int[] nums)
{
    var results = new List<List<int>>();
    var current = new List<int>();
    var used = new bool[nums.Length];
    Backtrack(nums, used, current, results);
    return results;
}

private static void Backtrack(int[] nums, bool[] used, List<int> current, List<List<int>> results)
{
    if (current.Count == nums.Length)
    {
        results.Add(new List<int>(current)); // COPY — current keeps changing
        return;
    }

    for (int i = 0; i < nums.Length; i++)
    {
        if (used[i]) continue;

        used[i] = true;
        current.Add(nums[i]);

        Backtrack(nums, used, current, results);

        current.RemoveAt(current.Count - 1); // undo
        used[i] = false;                      // undo
    }
}
```

</details>
</details>
</details>

### `Subsets`

<details>
<summary>Hint</summary>

The key difference from `Permutations`: every partial state along the way is
itself a valid, complete subset — not just the full-length ones. And instead
of "try every unused value," the choice at each step is "include this
specific next value, or don't."

<details>
<summary>Next hint (approach)</summary>

Recurse with a `startIndex` parameter. At the top of the recursive call,
**immediately record a copy of `current`** as a subset (this covers the empty
subset on the very first call, and every prefix along the way). Then loop `i`
from `startIndex` to the end: add `nums[i]` to `current`, recurse with
`startIndex = i + 1` (never revisit earlier indices — that's what prevents
duplicate/reordered subsets), then remove it again (backtrack) before trying
the next `i`.

<details>
<summary>Near-solution</summary>

```csharp
public static List<List<int>> Subsets(int[] nums)
{
    var results = new List<List<int>>();
    var current = new List<int>();
    Backtrack(nums, 0, current, results);
    return results;
}

private static void Backtrack(int[] nums, int startIndex, List<int> current, List<List<int>> results)
{
    results.Add(new List<int>(current)); // record every partial state

    for (int i = startIndex; i < nums.Length; i++)
    {
        current.Add(nums[i]);
        Backtrack(nums, i + 1, current, results);
        current.RemoveAt(current.Count - 1); // undo
    }
}
```

</details>
</details>
</details>

### `CountNQueensSolutions`

<details>
<summary>Hint</summary>

Place exactly one queen per row, recursing row by row — this automatically
guarantees no two queens ever share a row, so the only conflicts left to check
per candidate square are column and diagonal.

<details>
<summary>Next hint (the diagonal-attack math)</summary>

For a queen at `(row, col)`:

- All squares on its "\\"-diagonal (top-left to bottom-right) share the same
  value of `row - col`. Since `row - col` can be negative, shift it by
  `n - 1` when using it as an array index: `diag1 = row - col + (n - 1)`.
- All squares on its "/"-diagonal (top-right to bottom-left) share the same
  value of `row + col`, which is always non-negative and ranges from `0` to
  `2n - 2`: `diag2 = row + col`.

So: three `bool[]` trackers — `usedColumns[n]`, `usedDiagonals1[2n-1]`,
`usedDiagonals2[2n-1]` — let you check "is this square attacked" in O(1),
instead of scanning previously placed queens.

<details>
<summary>Near-solution</summary>

```csharp
public static int CountNQueensSolutions(int n)
{
    var usedColumns = new bool[n];
    var usedDiag1 = new bool[2 * n - 1]; // row - col, shifted by n - 1
    var usedDiag2 = new bool[2 * n - 1]; // row + col

    return Backtrack(n, 0, usedColumns, usedDiag1, usedDiag2);
}

private static int Backtrack(int n, int row, bool[] usedColumns, bool[] usedDiag1, bool[] usedDiag2)
{
    if (row == n) return 1; // filled every row — one valid solution

    int count = 0;
    for (int col = 0; col < n; col++)
    {
        int d1 = row - col + n - 1;
        int d2 = row + col;

        if (usedColumns[col] || usedDiag1[d1] || usedDiag2[d2]) continue;

        usedColumns[col] = usedDiag1[d1] = usedDiag2[d2] = true;
        count += Backtrack(n, row + 1, usedColumns, usedDiag1, usedDiag2);
        usedColumns[col] = usedDiag1[d1] = usedDiag2[d2] = false; // undo
    }
    return count;
}
```

</details>
</details>
</details>

## Running your work

From the repo root:

```bash
cd modules/04-recursion-backtracking/tests/RecursionBacktracking.Tests
dotnet test
```

This compiles your `src/RecursionBacktracking` implementation against the test
suite in this folder and reports pass/fail for every test. All tests failing
with `NotImplementedException` is the expected starting state.

## If you're stuck

`solution/Solution.cs` in this module's folder holds a complete, working
reference implementation of everything above. It is deliberately **not** part
of any `.csproj`, so it won't interfere with your build — it's just there to
read if you get truly stuck.

Backtracking is genuinely one of the harder things to get right by
intuition alone — draw the decision tree on paper for a small example (`n = 3`
for `Permutations` or `Subsets`) before writing code, and trace through it by
hand. That exercise is worth more than reading the solution early.

If you do reach for the local LLM for help, you'll get much better answers if
you:

1. Paste the **exact method signature** you're implementing (e.g.
   `public static List<List<int>> Subsets(int[] nums)`), not a vague
   description like "the subsets problem."
2. State the **constraints** explicitly (e.g. "distinct ints, must include the
   empty subset, backtracking approach expected, target O(n * 2^n) time").
3. Ask for the **approach first, not code** — e.g. "explain how the
   start-index parameter prevents duplicate subsets, don't write code yet" —
   then ask for code only once you understand *why* it works. A weak local
   model is far more reliable explaining a concept in words than generating
   correct recursive C# on the first try, and code you don't understand won't
   help you on the next problem.
