# Module 3: Stacks & Queues

Learn how a stack (Last-In-First-Out) and a queue (First-In-First-Out) work under
the hood by building both from a plain C# array, then use those ideas to solve two
classic interview problems: validating bracket strings and evaluating postfix
arithmetic.

## C# syntax you'll need

### Generic classes (`<T>`)

A generic class lets you write one implementation that works for any type,
decided when the caller instantiates it:

```csharp
public class Box<T>
{
    private T _value;

    public void Set(T value) => _value = value;
    public T Get() => _value;
}

var intBox = new Box<int>();
intBox.Set(42);

var stringBox = new Box<string>();
stringBox.Set("hello");
```

`T` is just a placeholder name (by convention a single capital letter, or a
descriptive name prefixed with `T` like `TItem`). Everywhere you see `T` in this
module's stubs, think "whatever type the caller picked."

### Arrays and resizing

C# arrays (`T[]`) are fixed-size once created. To "grow" one, you allocate a new,
bigger array and copy elements over — there's no built-in resize-in-place.

```csharp
int[] items = new int[4];       // array of 4 ints, all default(int) == 0
items[0] = 10;
Console.WriteLine(items.Length); // 4, NOT a method call — Length is a property

// Array.Resize allocates a new backing array under the hood and copies elements
// [0.._items.Length) into it, then repoints the ref parameter at the new array.
Array.Resize(ref items, 8);
Console.WriteLine(items.Length); // 8
```

Note `Array.Resize` takes the array **by reference** (`ref`) because it has to
replace the variable's value with a brand-new array object — see the `ref`
section below.

### `ref` parameters

Normally, C# passes arguments by value — the method gets a copy. `ref` lets a
method reassign the caller's variable itself:

```csharp
void Increment(ref int x)
{
    x = x + 1;
}

int n = 5;
Increment(ref n);
Console.WriteLine(n); // 6
```

Both the method signature and the call site must say `ref`. This is exactly what
`Array.Resize(ref _items, newSize)` relies on: it swaps `_items` for a new,
bigger array.

### Properties (`=>` expression-bodied members)

A property looks like a field from the outside but can run code:

```csharp
public class Counter
{
    private int _count;

    // Expression-bodied read-only property — shorthand for "get { return _count == 0; }"
    public bool IsEmpty => _count == 0;

    public int Count => _count;
}
```

You'll expose `Count` and `IsEmpty` this way on both `ArrayStack<T>` and
`ArrayQueue<T>`.

### Exceptions: throwing and testing for them

```csharp
public int Divide(int a, int b)
{
    if (b == 0)
    {
        throw new InvalidOperationException("Cannot divide by zero.");
    }
    return a / b;
}
```

In tests, xUnit's `Assert.Throws<TException>(() => ...)` checks that calling the
given code throws the expected exception type:

```csharp
Assert.Throws<InvalidOperationException>(() => someStack.Pop());
```

### `default` / `default!`

`default(T)` gives you the "zero value" of any type — `0` for `int`, `null` for
reference types, all-zero struct for value types. `default!` is the same, but the
trailing `!` (the "null-forgiving operator") tells the compiler "trust me, I know
this might be null, don't warn me" — useful when clearing out a slot after a pop
so the array doesn't keep a stray reference alive:

```csharp
T item = _items[_count];
_items[_count] = default!; // clear the slot
```

### The `Stack<T>` and `Queue<T>` you're *not* building — but will use

.NET ships two BCL (Base Class Library) collection types with the same
behavior you're implementing by hand here:

- `System.Collections.Generic.Stack<T>` — `Push`, `Pop`, `Peek`, `Count`.
- `System.Collections.Generic.Queue<T>` — `Enqueue`, `Dequeue`, `Peek`, `Count`.

In the "Problems" section below (`IsValidParentheses` and `EvalRPN`), you're
free — encouraged, even — to use the real `Stack<T>` from the BCL, since the
point of those two problems is the *algorithm*, not re-proving you can build a
stack (you already will have, in `ArrayStack<T>`). `ImplicitUsings` is enabled
in this project, and `System.Collections.Generic` is one of the automatically
available namespaces, so you can write `new Stack<int>()` with no `using`
statement needed.

### Circular buffers (the tricky part of `ArrayQueue<T>`)

A naive array-backed queue that always dequeues from index 0 has to shift every
remaining element left on every `Dequeue` — O(n) per operation. A **circular
buffer** avoids that by tracking a `_head` index that moves forward (wrapping
around) instead of shifting data:

```
Capacity 4, backing array indices: [0] [1] [2] [3]

Enqueue(1), Enqueue(2), Enqueue(3):
  _head = 0, _count = 3
  [1] [2] [3] [ ]
   ^head        ^ next enqueue goes to index (0+3)%4 = 3

Dequeue() -> returns 1, _head becomes (0+1)%4 = 1, _count = 2
  [ ] [2] [3] [ ]
       ^head

Enqueue(4), Enqueue(5):
  first: tail index = (1+2)%4 = 3  -> [ ] [2] [3] [4]
  second: tail index = (1+3)%4 = 0 -> [5] [2] [3] [4]
                                        ^ wrapped around to index 0!
  _count = 4 (full)
```

The two formulas that make this work, both using C#'s `%` (modulo/remainder)
operator:

- **Where does the next enqueued item go?**
  `tailIndex = (_head + _count) % _items.Length`
- **Where does `_head` move to after a dequeue?**
  `_head = (_head + 1) % _items.Length`

`%` wraps the index back to `0` once it would otherwise run past the end of the
array — e.g. `3 % 4 == 3`, but `4 % 4 == 0`. That's the whole trick: "add, then
wrap."

When you resize a full circular buffer, you can't just `Array.Resize` in place,
because the logical front-to-back order of elements may not match their raw
array-index order (the buffer might be "wrapped," like `[5] [2] [3] [4]` above,
where index 0 is logically the *last* element, not the first). Instead, allocate
a new array and copy elements out **in logical order starting from `_head`**,
landing them at indices `0, 1, 2, ...` in the new array, then reset `_head = 0`.

### `switch` expressions and pattern matching (used in `EvalRPN`)

A `switch` *expression* (not the older `switch` *statement*) lets you compute a
value concisely:

```csharp
string op = "+";
int a = 2, b = 3;

int result = op switch
{
    "+" => a + b,
    "-" => a - b,
    "*" => a * b,
    "/" => a / b,
    _ => throw new InvalidOperationException("Unknown operator"), // '_' = default case
};
```

### Integer division truncates toward zero

C#'s `/` operator on two `int`s does integer division, and — unlike some
languages — it **truncates toward zero**, not toward negative infinity:

```csharp
Console.WriteLine(7 / 2);   // 3
Console.WriteLine(-7 / 2);  // -3   (not -4)
```

This matches the RPN problem's requirement exactly, so `a / b` with plain `int`s
already does the right thing — no extra `Math.Truncate` needed.

## Problems

### 1. `ArrayStack<T>`

Implement a generic, resizable, array-backed stack (LIFO — Last In, First Out).

```csharp
public class ArrayStack<T>
{
    public int Count { get; }
    public bool IsEmpty { get; }

    public void Push(T item);
    public T Pop();   // throws InvalidOperationException if empty
    public T Peek();  // throws InvalidOperationException if empty
}
```

**Example:**

```csharp
var stack = new ArrayStack<int>();
stack.Push(1);
stack.Push(2);
stack.Push(3);
stack.Pop();   // returns 3, Count is now 2
stack.Peek();  // returns 2, Count is still 2
```

**Complexity target:** `Push` and `Pop` are amortized O(1) (occasional resizes
are O(n) but happen rarely enough that the *average* cost per call stays O(1)).
`Peek` is O(1). Space is O(n) for n stored elements.

### 2. `ArrayQueue<T>`

Implement a generic, resizable, circular-buffer-backed queue (FIFO — First In,
First Out).

```csharp
public class ArrayQueue<T>
{
    public int Count { get; }
    public bool IsEmpty { get; }

    public void Enqueue(T item);
    public T Dequeue();  // throws InvalidOperationException if empty
    public T Peek();     // throws InvalidOperationException if empty
}
```

**Example:**

```csharp
var queue = new ArrayQueue<int>();
queue.Enqueue(1);
queue.Enqueue(2);
queue.Enqueue(3);
queue.Dequeue(); // returns 1, Count is now 2
queue.Peek();    // returns 2, Count is still 2
```

**Complexity target:** `Enqueue` and `Dequeue` are amortized O(1). `Peek` is
O(1). Space is O(n).

### 3. `StackQueueProblems.IsValidParentheses`

Given a string containing only the characters `(`, `)`, `[`, `]`, `{`, `}`,
determine whether the brackets are balanced: every opening bracket has a
matching closing bracket of the same type, in the correct (properly nested)
order.

```csharp
public static bool IsValidParentheses(string s)
```

**Examples:**

```csharp
StackQueueProblems.IsValidParentheses("()[]{}");   // true
StackQueueProblems.IsValidParentheses("([)]");     // false — wrong nesting order
StackQueueProblems.IsValidParentheses("{[()()]}"); // true
```

**Complexity target:** O(n) time, O(n) space (the stack, worst case all
openers).

### 4. `StackQueueProblems.EvalRPN`

Evaluate an arithmetic expression given in Reverse Polish Notation (postfix
notation). `tokens` is an array where each element is either an integer literal
(as a string, possibly negative, e.g. `"-7"`) or one of the operators `"+"`,
`"-"`, `"*"`, `"/"`. Integer division truncates toward zero.

```csharp
public static int EvalRPN(string[] tokens)
```

**Examples:**

```csharp
StackQueueProblems.EvalRPN(new[] { "2", "1", "+", "3", "*" }); // (2 + 1) * 3 = 9
StackQueueProblems.EvalRPN(new[] { "4", "13", "5", "/", "+" }); // 4 + (13 / 5) = 4 + 2 = 6
```

**Complexity target:** O(n) time, O(n) space.

## Hints

### `ArrayStack<T>`

<details>
<summary>Hint</summary>

You need one backing array field (`T[]`) and one `int` field tracking how many
slots are in use. `Push` writes at that count and then increments it; `Pop`
decrements first and then reads. Resize check happens at the *start* of `Push`.

<details>
<summary>Next hint (approach)</summary>

- `Push`: if `_count == _items.Length`, call `Array.Resize(ref _items, _items.Length * 2)` first.
  Then `_items[_count] = item; _count++;`
- `Pop`: if `IsEmpty`, throw. Otherwise `_count--;` then return `_items[_count]`
  (optionally clear that slot to `default!` afterward to avoid holding a stale
  reference).
- `Peek`: if `IsEmpty`, throw. Otherwise return `_items[_count - 1]` without
  touching `_count`.

<details>
<summary>Near-solution</summary>

```csharp
public void Push(T item)
{
    if (_count == _items.Length)
    {
        Array.Resize(ref _items, _items.Length * 2);
    }
    _items[_count] = item;
    _count++;
}

public T Pop()
{
    if (IsEmpty) throw new InvalidOperationException("Stack is empty.");
    _count--;
    return _items[_count];
}
```

`Peek` is `Pop` minus the decrement/removal — just read `_items[_count - 1]`.

</details>
</details>
</details>

### `ArrayQueue<T>`

<details>
<summary>Hint</summary>

Re-read the "Circular buffers" section in the syntax refresher above — this
problem is entirely about getting the modulo index math right. You need three
fields: the backing array, `_head` (index of the front element), and `_count`.

<details>
<summary>Next hint (approach)</summary>

- `Enqueue`: if full, allocate a new array double the size, copy elements out
  starting from `_head` in logical order into indices `0, 1, 2, ...`, reset
  `_head = 0`. Then compute `int tailIndex = (_head + _count) % _items.Length;`
  store the item there, and `_count++`.
- `Dequeue`: if empty, throw. Otherwise read `_items[_head]`, then
  `_head = (_head + 1) % _items.Length; _count--;` and return what you read.
- `Peek`: if empty, throw. Otherwise return `_items[_head]` — no mutation.

<details>
<summary>Near-solution</summary>

```csharp
public void Enqueue(T item)
{
    if (_count == _items.Length)
    {
        var newItems = new T[_items.Length * 2];
        for (int i = 0; i < _count; i++)
        {
            newItems[i] = _items[(_head + i) % _items.Length];
        }
        _items = newItems;
        _head = 0;
    }

    int tailIndex = (_head + _count) % _items.Length;
    _items[tailIndex] = item;
    _count++;
}

public T Dequeue()
{
    if (IsEmpty) throw new InvalidOperationException("Queue is empty.");
    T item = _items[_head];
    _head = (_head + 1) % _items.Length;
    _count--;
    return item;
}
```

</details>
</details>
</details>

### `IsValidParentheses`

<details>
<summary>Hint</summary>

Think about what a stack naturally represents here: the openers you're still
"waiting to close," most-recent-first. Every closing bracket must match the
*most recently opened, still-unclosed* bracket.

<details>
<summary>Next hint (approach)</summary>

Walk the string one character at a time. If it's an opener (`(`, `[`, `{`),
push it. If it's a closer, the string can only be valid if the stack is
non-empty *and* popping it gives you the matching opener — otherwise return
`false` immediately. After the loop, the string is valid only if the stack
ended up empty (no unclosed openers left over).

<details>
<summary>Near-solution</summary>

```csharp
var stack = new Stack<char>();
foreach (char c in s)
{
    if (c == '(' || c == '[' || c == '{')
    {
        stack.Push(c);
    }
    else
    {
        if (stack.Count == 0) return false;
        char open = stack.Pop();
        bool matches = (c == ')' && open == '(')
                     || (c == ']' && open == '[')
                     || (c == '}' && open == '{');
        if (!matches) return false;
    }
}
return stack.Count == 0;
```

</details>
</details>
</details>

### `EvalRPN`

<details>
<summary>Hint</summary>

Postfix notation is exactly what a stack is built for: numbers get pushed, and
when you hit an operator, its operands are already sitting on top of the stack.

<details>
<summary>Next hint (approach)</summary>

For each token: try to treat it as a number (`int.Parse` or `int.TryParse`) and
push it if it is one. Otherwise it's an operator — pop **twice**. The order
matters for `-` and `/`: the value popped *second* is the left-hand operand.
Compute the result and push it back. At the end exactly one value remains on
the stack — that's the answer.

<details>
<summary>Near-solution</summary>

```csharp
var stack = new Stack<int>();
foreach (string token in tokens)
{
    if (int.TryParse(token, out int number))
    {
        stack.Push(number);
        continue;
    }

    int b = stack.Pop(); // popped first = right-hand operand
    int a = stack.Pop(); // popped second = left-hand operand
    int result = token switch
    {
        "+" => a + b,
        "-" => a - b,
        "*" => a * b,
        "/" => a / b,
        _ => throw new InvalidOperationException($"Unknown operator: {token}"),
    };
    stack.Push(result);
}
return stack.Pop();
```

Watch out: `"-7"` must parse as the number `-7`, not the subtraction operator —
that's exactly why checking `int.TryParse` first (rather than checking for a
leading `-`) is the safe approach.

</details>
</details>
</details>

## Running your work

From the repo root:

```bash
cd modules/03-stacks-queues/tests/StacksQueues.Tests
dotnet test
```

This compiles your `src/StacksQueues` implementation against the test suite in
this folder and reports pass/fail for every test. All tests failing with
`NotImplementedException` is the expected starting state — that's "red," and
your job is to turn it "green."

## If you're stuck

`solution/Solution.cs` in this module's folder holds a complete, working
reference implementation of everything above. It is deliberately **not** part
of any `.csproj`, so it won't interfere with your build — it's just there for
you to read if you get truly stuck.

Try for real first. Struggling with the circular-buffer index math for a while
*before* peeking is where the actual learning happens — if you copy the answer
immediately, the idea won't stick.

If you do reach for the local LLM for help, you'll get much better answers if
you:

1. Paste the **exact method signature** you're implementing (e.g.
   `public void Enqueue(T item)`), not a vague description.
2. State the **constraints** explicitly (e.g. "backed by a circular buffer
   array, must resize by doubling, must be amortized O(1)").
3. Ask for the **approach first, not code** — e.g. "walk me through the index
   math for a circular buffer enqueue, don't write the code yet" — then ask for
   code only once the approach makes sense to you. A weak local model is much
   more reliable explaining a concept in words than generating correct C# on
   the first try.
