# Module 11: Dynamic Programming (Bottom-Up / Tabulation)

**Learning objective:** learn to solve optimization and counting problems by building
a table of sub-problem answers from the smallest case upward, instead of recursing
down from the big case. By the end of this module you should be able to define a DP
state, write its recurrence in words and math, and translate that recurrence into a
loop that fills an array or 2D table.

> **Connection to module 4:** in module 4 you did **top-down** dynamic programming —
> you wrote the natural recursive solution first, then added a cache (memoization) so
> repeated sub-problems weren't recomputed. This module is about **bottom-up
> tabulation**: the *exact same recurrences*, but instead of recursing and caching on
> demand, you pre-allocate a table, fill in the base cases, and iterate forward until
> the table is complete. No recursion, no call stack, no cache — just a loop. If you
> get stuck on "what's the recurrence?", it's worth remembering that it's the same
> question you answered in module 4; only the direction of construction has changed.

---

## C# syntax you'll need

### 1D arrays

```csharp
int[] dp = new int[n + 1];   // length n+1, every element defaults to 0
dp[0] = 1;                   // base case
for (int i = 1; i <= n; i++)
{
    dp[i] = dp[i - 1] + 1;   // fill forward using earlier entries
}
```

- `new int[n + 1]` allocates an array of `n + 1` ints, **all initialized to `0`**
  (C# always zero-initializes numeric arrays — you don't need to loop and set
  zeros yourself).
- The `+ 1` sizing trick shows up constantly in DP: if your state is "answer for
  amount `i`" for `i` from `0` to `n`, you need `n + 1` slots (index `0` through
  index `n` inclusive), not `n`.
- `dp.Length` gives you the array's size.

### 2D arrays (rectangular) vs jagged arrays

C# has two different things that both look like "2D arrays":

```csharp
// Rectangular 2D array — ONE array object, fixed rectangular shape.
int[,] dp = new int[n + 1, m + 1];   // note the comma inside the brackets
dp[2, 3] = 7;                         // comma-indexing, not dp[2][3]
int rows = dp.GetLength(0);           // n + 1
int cols = dp.GetLength(1);           // m + 1
```

```csharp
// Jagged array — an array OF arrays, each row can be a different length.
int[][] dp = new int[n + 1][];
for (int i = 0; i <= n; i++)
{
    dp[i] = new int[m + 1];          // you must allocate each row yourself
}
dp[2][3] = 7;                         // bracket-per-dimension indexing
```

For this module, prefer the **rectangular** form (`int[,]`) for 2D DP tables
(`LongestCommonSubsequence`, `Knapsack01`) — it's simpler to allocate (one line,
every cell already zeroed) and the problems here don't need ragged row lengths.
Just remember: comma inside one set of brackets (`dp[i, j]`), and `GetLength(0)`
/ `GetLength(1)` instead of `.Length` for each dimension.

### Array defaults

Every element of a freshly allocated `int[]` or `int[,]` starts at `0`. This is
extremely useful for DP: "0 items considered, 0 capacity used → 0 value" or
"empty prefix vs empty prefix → 0 common length" often falls out for free because
row/column `0` is simply left at its default of `0`. Watch for it in the LCS and
knapsack recurrences below — the `i == 0` or `j == 0` cases don't need to be
written explicitly if you loop starting from `1`.

### `Math.Min` and `Math.Max`

```csharp
int best = Math.Min(dp[i - 1], dp[i - 2]);   // smaller of two ints
int best2 = Math.Max(a, b);                   // larger of two ints
```

Both are static methods on the `Math` class, take two (or more, via overloads)
values of the same numeric type, and return that type. No `using` needed beyond
what's already implicit — `Math` is in `System`, which is globally usable via
`ImplicitUsings` in this project.

### `long` vs `int` and overflow

`int` in C# is a 32-bit signed integer, max value `2,147,483,647`
(`int.MaxValue`). Counting problems like `ClimbingStairs` grow roughly like the
Fibonacci sequence, which **blows past `int.MaxValue` well before `n` gets
large** (around `n = 46`). If you accumulate into an `int` and the true answer
exceeds that range, you silently get a wrong, wrapped-around (possibly negative)
number — no crash, no warning. That's why `ClimbingStairs` returns `long` (a
64-bit signed integer, max value about `9.2 * 10^18`):

```csharp
long[] ways = new long[n + 1];
ways[0] = 1;
ways[1] = 1;
for (int i = 2; i <= n; i++)
{
    ways[i] = ways[i - 1] + ways[i - 2];   // long + long => long, no overflow here
}
```

The loop index `i` can stay an `int` (it's just counting up to `n`, which is a
normal-sized `int` parameter) — only the *accumulated counts* need to be `long`.

### String indexing and `Length`

```csharp
string s = "hello";
char c = s[0];        // 'h' — strings are indexed like arrays, 0-based
int len = s.Length;   // 5 — property, not a method (no parentheses)
```

Strings are immutable and behave like read-only `char` arrays for indexing
purposes. `s[i]` gives you the character at position `i`; `s.Length` gives you
the count of characters. Both `LongestCommonSubsequence` parameters are compared
character-by-character using this indexing.

### The general tabulation pattern

Every problem in this module follows the same shape:

1. **Define the state** — what does `dp[i]` (or `dp[i, j]`) *mean*, in one
   sentence? ("the fewest coins to make amount `i`", "the max value using the
   first `i` items with capacity `w`", etc.)
2. **Size the table** to cover every state you'll need, usually with a `+ 1` for
   an empty/zero base case.
3. **Fill in the base case(s)** — the smallest sub-problems, often index `0`,
   which are frequently free thanks to C#'s zero-initialization.
4. **Loop forward**, computing each new cell from cells you've *already filled*
   (smaller amounts, shorter prefixes, fewer items — never a cell that depends
   on itself or on something not yet computed).
5. **Read the answer** off the last cell(s) you filled — usually `dp[n]` or
   `dp[n, m]`.

```csharp
// Skeleton — fill dp[1..n] from dp[0] upward.
int[] dp = new int[n + 1];
dp[0] = /* base case */;
for (int i = 1; i <= n; i++)
{
    dp[i] = /* combine one or more of dp[i-1], dp[i-2], ... */;
}
return dp[n];
```

---

## Problems

### 1. ClimbingStairs

You're climbing a staircase with `n` steps. Each move you can climb either 1 or
2 steps. Count the number of **distinct ways** to reach the top.

```csharp
public static long ClimbingStairs(int n)
```

- `ClimbingStairs(0)` → `1` (there is exactly one way to be at the bottom
  already: take zero steps)
- `ClimbingStairs(2)` → `2` (1+1, or 2)
- `ClimbingStairs(4)` → `5` (1+1+1+1, 1+1+2, 1+2+1, 2+1+1, 2+2)

**Target complexity:** O(n) time, O(1) or O(n) space.

### 2. CoinChange

Given an array of coin `coins` (unlimited supply of each denomination) and a
target `amount`, return the **fewest number of coins** needed to make exactly
`amount`. If it's impossible, return `-1`.

```csharp
public static int CoinChange(int[] coins, int amount)
```

Think of it as a 1D table where `dp[i]` = the minimum number of coins that sum
to exactly `i`. `dp[0]` is `0` (zero coins needed to make amount zero — the
base case). For every other `i`, try each coin `c`: if `c <= i`, then one
option is "use one coin of value `c`, plus however many coins it took to make
`i - c`", i.e. `dp[i - c] + 1`. Take the best (minimum) option across all
coins.

- `CoinChange([1, 2, 5], 11)` → `3` (5 + 5 + 1)
- `CoinChange([2], 3)` → `-1` (3 is not reachable using only 2s)
- `CoinChange([1, 2, 5], 0)` → `0` (no coins needed)

**Target complexity:** O(amount * coins.Length) time.

### 3. LongestCommonSubsequence

Given two strings `a` and `b`, return the length of their **longest common
subsequence** — the longest sequence of characters that appears in both
strings *in the same relative order*, but not necessarily contiguously (unlike
a substring).

```csharp
public static int LongestCommonSubsequence(string a, string b)
```

- `LongestCommonSubsequence("abcde", "ace")` → `3` (the subsequence `"ace"`)
- `LongestCommonSubsequence("abc", "xyz")` → `0` (nothing in common)

**⚠️ Off-by-one trap:** the DP table is sized `(a.Length + 1) x (b.Length + 1)`,
**not** `a.Length x b.Length`. `dp[i, j]` represents the answer for the
*prefixes* `a[0..i)` (the first `i` characters of `a`) and `b[0..j)` (the first
`j` characters of `b`) — **not** for `a[i]` and `b[j]` directly. Row/column `0`
represents an *empty* prefix, which is why `dp[0, j]` and `dp[i, 0]` are always
`0` (an empty string shares no characters with anything). This means when
you're filling in `dp[i, j]`, the *characters currently being compared* are
`a[i - 1]` and `b[j - 1]` — one index back from `i` and `j` — because `i` and
`j` count *how many characters of the prefix*, while the array itself is
0-indexed.

Recurrence:

```
dp[i, j] = dp[i-1, j-1] + 1                    if a[i-1] == b[j-1]
dp[i, j] = Math.Max(dp[i-1, j], dp[i, j-1])    otherwise
```

**Target complexity:** O(a.Length * b.Length) time and space.

### 4. Knapsack01

You have `n` items, each with a `weight` and a `value`. You have a knapsack
that can carry total weight up to `capacity`. Each item can be taken **at most
once** (0/1 — you either take it whole or leave it, no fractional items, no
duplicates). Maximize total value without exceeding `capacity`.

```csharp
public static int Knapsack01(int[] weights, int[] values, int capacity)
```

- `Knapsack01([1, 3, 4, 5], [1, 4, 5, 7], 7)` → `9` (take the weight-3 and
  weight-4 items: 3+4=7 weight, 4+5=9 value)
- `Knapsack01([20], [100], 5)` → `0` (the only item doesn't fit)

State: `dp[i, w]` = the maximum value achievable using only the **first `i`
items**, with a knapsack capacity of `w`. For each item `i` (1-indexed, so it
corresponds to `weights[i - 1]` / `values[i - 1]`), you have exactly two
choices, and you take whichever is better:

```
dp[i, w] = dp[i-1, w]                                              // skip item i
dp[i, w] = dp[i-1, w - weights[i-1]] + values[i-1]                 // take item i (only if weights[i-1] <= w)
```

`dp[0, w] = 0` for every `w` (no items considered yet → no value, regardless of
capacity) — this is free thanks to array zero-initialization.

**Target complexity:** O(n * capacity) time.

---

## Hints

### ClimbingStairs

<details><summary>Hint 1</summary>

How many ways are there to reach step `n` if the very last move you make is a
single 1-step? How many ways if the last move is a 2-step? Every valid path
ends in exactly one of those two moves.

</details>

<details><summary>Hint 2</summary>

State: let `ways[i]` = the number of distinct ways to reach step `i`. If the
last move to reach step `i` was a 1-step, you must have been at step `i - 1`
beforehand (and there are `ways[i - 1]` ways to have gotten there). If the last
move was a 2-step, you were at step `i - 2` (`ways[i - 2]` ways). These two
cases cover every path and never overlap, so you can just add them.

</details>

<details><summary>Hint 3 (near-solution)</summary>

Recurrence: `ways[i] = ways[i - 1] + ways[i - 2]`, for `i >= 2`, with base
cases `ways[0] = 1` and `ways[1] = 1`. Allocate a `long[n + 1]` (or just track
two rolling `long` variables if you want O(1) space), fill it in a loop from
`i = 2` to `n`, and return `ways[n]`. Special-case `n == 0` and `n == 1`
up front since they don't have both an `i-1` and `i-2` to reference.

</details>

### CoinChange

<details><summary>Hint 1</summary>

Think about the smallest sub-problem: how many coins does it take to make
amount `0`? What if a coin's value is bigger than the amount you're currently
trying to make — can you use it at all for that amount?

</details>

<details><summary>Hint 2</summary>

State: `dp[i]` = fewest coins needed to make amount `i` exactly (or "impossible"
if it can't be done). To fill in `dp[i]`, consider every coin denomination `c`
in `coins`. If `c <= i`, one way to make amount `i` is: make amount `i - c`
optimally, then add one more coin of value `c`. That costs `dp[i - c] + 1`
coins. You want the minimum of that over every coin.

</details>

<details><summary>Hint 3 (near-solution)</summary>

Recurrence: `dp[i] = min over all coins c <= i of (dp[i - c] + 1)`, with
`dp[0] = 0`. Represent "impossible so far" with a large sentinel value (e.g.
`int.MaxValue / 2` — using `/ 2` instead of `int.MaxValue` itself avoids
integer overflow when you later add `1` to it) instead of a magic `-1` inside
the table, and only convert to `-1` in your final return statement if
`dp[amount]` never got updated away from that sentinel. Loop `i` from `1` to
`amount`, and inside that loop, loop over every coin.

</details>

### LongestCommonSubsequence

<details><summary>Hint 1</summary>

Think about the *last* characters of the two strings you're comparing right
now. If they match, that character can obviously be part of the common
subsequence. If they don't match, you have to give up on using *one* of the two
current characters (the string's last character) — but you get to choose which
one.

</details>

<details><summary>Hint 2</summary>

State: `dp[i, j]` = length of the LCS of the first `i` characters of `a` and
the first `j` characters of `b`. If `a[i - 1] == b[j - 1]` (the *last*
characters of those two prefixes match), that shared character extends
whatever the best answer was for the prefixes one character shorter each:
`dp[i - 1, j - 1] + 1`. If they don't match, the best you can do is the better
of "drop the last character of `a`'s prefix" or "drop the last character of
`b`'s prefix" — you don't know which is better without comparing.

</details>

<details><summary>Hint 3 (near-solution)</summary>

Recurrence:
`dp[i, j] = dp[i-1, j-1] + 1` if `a[i-1] == b[j-1]`, else
`dp[i, j] = Math.Max(dp[i-1, j], dp[i, j-1])`.
Base case: `dp[0, j] = dp[i, 0] = 0` for all `i, j` (free from zero-init).
Allocate `int[,] dp = new int[a.Length + 1, b.Length + 1]`, loop `i` from `1`
to `a.Length` and, nested inside, `j` from `1` to `b.Length`, and return
`dp[a.Length, b.Length]`.

</details>

### Knapsack01

<details><summary>Hint 1</summary>

For each item, one at a time, you only ever have two choices: leave it out of
the knapsack, or put it in (if it still fits). What's the best value if you
*don't* have this item available at all? What's the best value if you *do* take
it — what capacity do you have left over, and what value have you already
locked in?

</details>

<details><summary>Hint 2</summary>

State: `dp[i, w]` = max value achievable considering only the first `i` items
with a capacity limit of `w`. "Skip item `i`" means the best you can do is
exactly as good as if item `i` didn't exist: `dp[i - 1, w]`. "Take item `i`"
(only possible if `weights[i - 1] <= w`) means you lock in `values[i - 1]` and
have `w - weights[i - 1]` capacity left to fill optimally using the *previous*
items: `dp[i - 1, w - weights[i - 1]] + values[i - 1]`.

</details>

<details><summary>Hint 3 (near-solution)</summary>

Recurrence:
`dp[i, w] = dp[i-1, w]` (always a valid option), and if `weights[i-1] <= w`,
also consider `dp[i-1, w - weights[i-1]] + values[i-1]`, taking `Math.Max` of
whichever options apply. Base case `dp[0, w] = 0` for all `w` (free from
zero-init). Allocate `int[,] dp = new int[n + 1, capacity + 1]`, loop `i` from
`1` to `n`, nested inside loop `w` from `0` to `capacity`, and return
`dp[n, capacity]`.

</details>

---

## Running your work

```
cd modules/11-dynamic-programming/tests/DynamicProgramming.Tests && dotnet test
```

This restores, builds `DynamicProgramming` (the project under `src/`), builds the
test project, and runs every test in `ProblemsTests.cs`. Every test will fail with
`NotImplementedException` until you fill in the method bodies in
`src/DynamicProgramming/Problems.cs`.

## If you're stuck

`solution/Solution.cs` holds a complete, correct, commented reference
implementation of all four methods. It is **not** wired into any `.csproj`, so it
won't compile or interfere with your work — it's there purely for you to read.

**Try for real before you peek.** Struggling with a recurrence for a while — even
getting it wrong a few times — is where the actual learning happens. Read the
problem statement, re-read the "C# syntax you'll need" section above, work through
the Hints one at a time (they're ordered from a gentle nudge to a near-complete
answer), and attempt an implementation before opening the solution file.

If you do need to ask your local Ollama model for help, keep in mind it's a weak
model with no internet access and no memory of this README — so give it everything
it needs in one shot:

- **Paste the exact method signature** (e.g.
  `public static int CoinChange(int[] coins, int amount)`), not a paraphrase.
- **State the constraints** explicitly (amount could be 0, coins array could be
  empty, etc.) — a weak model won't infer edge cases on its own.
- **Ask for the approach or recurrence first, separately from code** — e.g. "What
  should the DP state `dp[i]` represent for this problem, and what's the
  recurrence relation?" Get that in words/math before asking it to generate any
  C#. If you ask for code immediately, a weak model is far more likely to
  hallucinate a plausible-looking but subtly wrong implementation, and you'll have
  no way to check it without understanding the recurrence yourself anyway.
