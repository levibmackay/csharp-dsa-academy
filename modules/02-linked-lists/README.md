# Module 2: Linked Lists

**Learning objective:** Build a working singly linked list from scratch in C#, then implement four classic pointer-manipulation algorithms (reverse, cycle detection, find middle, merge sorted lists) that show up constantly in technical interviews and in real systems code. By the end, you should be comfortable reasoning about chains of references without an array underneath you.

## C# syntax you'll need

### Generics: `class Node<T>`

A generic class works with any type `T` you plug in at usage time, instead of being hard-coded to one type (like `int` or `string`).

```csharp
public class Box<T>
{
    public T Value { get; set; }

    public Box(T value)
    {
        Value = value;
    }
}

// Usage:
var intBox = new Box<int>(42);
var stringBox = new Box<string>("hello");
Console.WriteLine(intBox.Value);    // 42
Console.WriteLine(stringBox.Value); // hello
```

`T` is just a placeholder name (by convention a single capital letter, or something descriptive like `TKey`). The compiler substitutes the real type at each usage site and checks it for you — no casting, no boxing for reference types.

### Nullable reference types: `Node<T>?`

With `<Nullable>enable</Nullable>` turned on (it is, in this project's `.csproj`), the compiler tracks whether a reference can be `null` and warns you if you dereference something that might be `null` without checking first.

```csharp
public class Node<T>
{
    public T Value { get; set; }
    public Node<T>? Next { get; set; }   // "?" means this CAN be null
}
```

`Node<T> Next` (no `?`) would tell the compiler "this is never null" — but the last node in a list must have a `null` next pointer, so it has to be `Node<T>?`. Anywhere you see a `?` after a type, that variable might be `null` and the compiler will nag you to check before using it.

### Classes vs records

A `class` is the standard reference type with full control over its members. A `record` is a newer C# feature aimed at immutable, value-like data — it gets free structural equality (two records with the same field values are `==`) and a compact syntax:

```csharp
public class PersonClass
{
    public string Name { get; set; } = "";
}

public record PersonRecord(string Name);

var c1 = new PersonClass { Name = "Ada" };
var c2 = new PersonClass { Name = "Ada" };
Console.WriteLine(c1 == c2); // False — classes compare by reference by default

var r1 = new PersonRecord("Ada");
var r2 = new PersonRecord("Ada");
Console.WriteLine(r1 == r2); // True — records compare by value
```

For this module we use plain `class` for `Node<T>` and `SinglyLinkedList<T>` because nodes are mutable (their `Next` pointer changes as we build/modify the list) and identity matters (two different nodes might happen to hold the same `Value`, and we need to tell them apart).

### Properties: get/set, auto-properties

A property looks like a field from the outside but can run code on read (`get`) or write (`set`).

```csharp
public class Example
{
    // Auto-property: compiler generates a hidden backing field for you.
    public int Count { get; set; }

    // Auto-property with a private setter: readable from anywhere,
    // but only settable from inside this class.
    public int ReadOnlyFromOutside { get; private set; }

    public void Increment()
    {
        ReadOnlyFromOutside++; // fine, we're inside the class
    }
}
```

You'll see `public int Count { get; private set; }` on `SinglyLinkedList<T>` — callers can read `Count`, but only the list's own methods (`AddLast`, `Remove`, etc.) are allowed to change it.

### `readonly`

A `readonly` field can only be assigned in its declaration or in a constructor — never afterward. It's a compiler-enforced guarantee that a value won't change once the object exists.

```csharp
public class Config
{
    public readonly string Name;

    public Config(string name)
    {
        Name = name; // OK: still inside the constructor
    }

    public void Rename(string newName)
    {
        // Name = newName; // Compile error: cannot assign to readonly field outside constructor
    }
}
```

We don't strictly need `readonly` in this module's stubs, but you'll see the keyword elsewhere in the codebase, so it's worth recognizing.

### Reference semantics (classes) vs value semantics (structs/primitives)

This is the single most important concept for linked lists. When you assign or pass a `class` instance, you're copying a *reference* (like a pointer) — both variables point at the same underlying object. When you assign or pass a value type (`int`, `bool`, `struct`), you get an independent *copy*.

```csharp
// Value semantics:
int a = 5;
int b = a;
b = 10;
Console.WriteLine(a); // 5 — unaffected, b was a copy

// Reference semantics:
var node1 = new Node<int>(5);
var node2 = node1;
node2.Value = 10;
Console.WriteLine(node1.Value); // 10 — node1 and node2 point at the SAME object!
```

This is exactly why linked lists work: `current.Next = someNode` doesn't copy `someNode` — it makes `current`'s `Next` field point at the exact same object that `someNode` refers to. Splicing, reversing, and cycle detection are all just rearranging which reference points where.

### Null-conditional operator: `?.`

`?.` short-circuits to `null` instead of throwing if the left side is `null`, letting you chain safely.

```csharp
Node<int>? node = null;
var value = node?.Value; // does NOT throw; value is default(int?) i.e. null
Console.WriteLine(value); // (blank / null)

Node<int>? head = new Node<int>(1);
var nextValue = head?.Next?.Value; // head exists, but head.Next is null, so this is null too
```

Without `?.` you'd need an explicit `if (node is not null)` check before touching `node.Value`.

### Pattern matching: `is null`, `is not null`

Prefer `is null` / `is not null` over `== null` / `!= null` in modern C# — it can't be fooled by an overloaded `==` operator and reads clearly.

```csharp
Node<int>? current = someHead;

while (current is not null)
{
    Console.WriteLine(current.Value);
    current = current.Next;
}

if (current is null)
{
    Console.WriteLine("Reached the end of the list.");
}
```

### `while` loops for traversal

Linked lists don't support indexing (`list[3]`) the way arrays do — the only way to reach the 4th node is to walk there one `Next` at a time, starting from `Head`. The standard shape:

```csharp
var current = head;
while (current is not null)
{
    // do something with current.Value
    current = current.Next;
}
```

Every traversal, search, and print operation in this module follows that same pattern.

### What is a linked list, and how is it different from an array or `List<T>`?

An **array** (or `List<T>`, which is backed by an array) stores its elements in one contiguous block of memory. That gives you O(1) random access by index (`arr[5]` jumps straight there), but inserting or removing from the middle means shifting every element after it — O(n).

A **linked list** stores each element in its own separately-allocated `Node`, and each node holds a reference to the next one. There's no contiguous block and no indexing — reaching the 5th node means walking through the first four. But once you're *at* a node, inserting or removing next to it is O(1): you just rewire a couple of `Next` pointers, nothing shifts.

| Operation | Array / `List<T>` | Singly linked list |
|---|---|---|
| Access by index | O(1) | O(n) |
| Insert/remove at front | O(n) (shift everything) | O(1) |
| Insert/remove at known position | O(n) (shift) | O(1) (rewire pointers) |
| Memory layout | Contiguous | Scattered, one allocation per node |

`Queue<T>` and `Stack<T>` in .NET are built on similar ideas (FIFO / LIFO access without needing random indexing), which is part of why a linked list is a natural fit for implementing them — but in this module you're building the underlying node-and-pointer structure yourself, not using `Queue<T>`/`Stack<T>`.

## Problems

### Build the `SinglyLinkedList<T>` wrapper class

Implement the generic wrapper in `SinglyLinkedList.cs`. It should track `Head` (the first node, or `null` if empty) and `Count` (number of elements), and support:

- `void AddLast(T value)` — append a new value at the end of the list.
- `void AddFirst(T value)` — prepend a new value at the front of the list.
- `bool Remove(T value)` — remove the *first* node whose value equals `value`; return whether one was found and removed.
- `bool Contains(T value)` — return whether any node holds `value`.
- `int Count { get; }` — number of elements currently in the list.
- `Node<T>? Head { get; }` — exposed so algorithms/tests can traverse the raw chain.
- `List<T> ToList()` — return the values, in order, as a `List<T>` (handy for assertions in tests).

### `Reverse`

**Statement:** Given the head of a singly linked list of `int`, reverse the list in place (by rewiring `Next` pointers, not by copying values into new nodes) and return the new head.

**Signature:**
```csharp
public static Node<int>? Reverse(Node<int>? head)
```

**Examples:**
- Input: `1->2->3->4` &rarr; Output: `4->3->2->1`
- Input: `1` (single node) &rarr; Output: `1`
- Input: `null` (empty list) &rarr; Output: `null`

**Complexity target:** O(n) time, O(1) extra space.

### `HasCycle`

**Statement:** Given the head of a singly linked list of `int`, determine whether it contains a cycle — i.e., whether following `Next` pointers ever revisits a node instead of eventually reaching `null`.

**Signature:**
```csharp
public static bool HasCycle(Node<int>? head)
```

**Examples:**
- Input: `1->2->3->4` (ends in `null`) &rarr; Output: `false`
- Input: `1->2->3->4` where node `4`'s `Next` is rewired back to node `2` &rarr; Output: `true`

**Complexity target:** O(n) time, O(1) extra space (Floyd's tortoise-and-hare — do not use a `HashSet` of visited nodes, that would be O(n) space).

### `FindMiddle`

**Statement:** Given the head of a singly linked list of `int`, return the middle node. If the list has an even number of nodes (two "middle" candidates), return the **second** of the two.

**Signature:**
```csharp
public static Node<int>? FindMiddle(Node<int>? head)
```

**Examples:**
- Input: `1->2->3->4->5` (odd length) &rarr; Output: node holding `3`
- Input: `1->2->3->4` (even length) &rarr; Output: node holding `3` (the second middle)
- Input: `1->2` &rarr; Output: node holding `2`

**Complexity target:** O(n) time, O(1) extra space (fast/slow pointers, single pass).

### `MergeTwoSorted`

**Statement:** Given the heads of two ascending-sorted singly linked lists of `int`, merge them into a single ascending-sorted list and return its head. Reuse the existing nodes — don't allocate new `Node<int>` instances for the merged result.

**Signature:**
```csharp
public static Node<int>? MergeTwoSorted(Node<int>? a, Node<int>? b)
```

**Examples:**
- Input: `1->3->5` and `2->4->6` &rarr; Output: `1->2->3->4->5->6`
- Input: `null` and `1->3->5` &rarr; Output: `1->3->5`
- Input: `-5->-1->0->4` and `-3->-1->2->4` &rarr; Output: `-5->-3->-1->-1->0->2->4->4`

**Complexity target:** O(n + m) time, O(1) extra space.

## Hints

<details>
<summary>Hint: SinglyLinkedList.AddLast / AddFirst</summary>

**Nudge:** `AddFirst` is the easy one — think about what has to change when the very first node of the list becomes something new.

**Approach:** For `AddFirst`, create a `Node<T>` whose `Next` is set to the current `Head`, then make that new node the `Head`. For `AddLast`, if the list is empty, the new node just becomes `Head`. Otherwise, you have to walk from `Head` all the way to the node whose `Next` is `null` (the current last node), then set *that* node's `Next` to your new node.

**Near-solution:**
```csharp
public void AddLast(T value)
{
    var node = new Node<T>(value);
    if (Head is null)
    {
        Head = node;
    }
    else
    {
        var current = Head;
        while (current.Next is not null)
        {
            current = current.Next;
        }
        current.Next = node;
    }
    Count++;
}
```
Write `AddFirst` yourself following the same shape — it's shorter.
</details>

<details>
<summary>Hint: SinglyLinkedList.Remove</summary>

**Nudge:** You can't remove a node by just looking at it — to unlink it, you need to change the `Next` pointer of the node *before* it. That means you need to track two pointers as you walk: "current" and "previous."

**Approach:** Start with `previous = null` and `current = Head`. Walk forward, comparing `current.Value` to the target with `EqualityComparer<T>.Default.Equals(...)`. If it matches: when `previous` is `null` you're removing the head (`Head = current.Next`), otherwise splice around it (`previous.Next = current.Next`). Decrement `Count` and return `true`. If you fall off the end without a match, return `false`.

**Near-solution:**
```csharp
public bool Remove(T value)
{
    Node<T>? previous = null;
    var current = Head;

    while (current is not null)
    {
        if (EqualityComparer<T>.Default.Equals(current.Value, value))
        {
            if (previous is null) Head = current.Next;
            else previous.Next = current.Next;
            Count--;
            return true;
        }
        previous = current;
        current = current.Next;
    }
    return false;
}
```
</details>

<details>
<summary>Hint: SinglyLinkedList.Contains / ToList</summary>

**Nudge:** Both are pure traversals — no mutation needed. Reuse the `while (current is not null)` pattern from the syntax refresher above.

**Approach:** `Contains` walks and returns `true` the moment it finds a matching value (using `EqualityComparer<T>.Default.Equals`), or `false` after the loop ends. `ToList` walks and appends every value to a `new List<T>()`, returning it at the end.

**Near-solution:** These are short enough that if you understand the traversal pattern, write them directly. If stuck, revisit the `Remove` hint above — it's the same traversal skeleton with different logic inside the loop.
</details>

<details>
<summary>Hint: Reverse</summary>

**Nudge:** You need to walk forward through the list while making each node's `Next` point *backward*. The danger is: once you overwrite `current.Next`, you've lost your only way to reach the rest of the list — so save it first.

**Approach:** Keep three references as you go: `previous` (starts `null`), `current` (starts at `head`), and a temporary `next`. On each iteration: save `next = current.Next` before you touch anything, then rewire `current.Next = previous`, then slide both `previous` and `current` forward one step (`previous = current; current = next`). When `current` becomes `null`, `previous` is your new head.

**Near-solution:**
```csharp
public static Node<int>? Reverse(Node<int>? head)
{
    Node<int>? previous = null;
    var current = head;
    while (current is not null)
    {
        var next = current.Next;
        current.Next = previous;
        previous = current;
        current = next;
    }
    return previous;
}
```
</details>

<details>
<summary>Hint: HasCycle</summary>

**Nudge:** A `HashSet<Node<int>>` of visited nodes would work but costs O(n) space — the target here is O(1) space, which means you need two pointers moving at different speeds instead.

**Approach:** Start `slow` and `fast` both at `head`. On each step, move `slow` forward one node and `fast` forward two nodes. If there's no cycle, `fast` (or `fast.Next`) will hit `null` and you return `false`. If there IS a cycle, `fast` will eventually lap `slow` from behind and they'll point at the exact same node — check that with `ReferenceEquals` (not `==`, which for a custom class without an equality override behaves the same, but `ReferenceEquals` makes the intent explicit).

**Near-solution:**
```csharp
public static bool HasCycle(Node<int>? head)
{
    var slow = head;
    var fast = head;
    while (fast is not null && fast.Next is not null)
    {
        slow = slow!.Next;
        fast = fast.Next.Next;
        if (ReferenceEquals(slow, fast)) return true;
    }
    return false;
}
```
</details>

<details>
<summary>Hint: FindMiddle</summary>

**Nudge:** Same fast/slow pointer idea as `HasCycle`, but this time you're not checking for equality — you're using the fact that when `fast` finishes the list, `slow` will be exactly halfway.

**Approach:** Start both `slow` and `fast` at `head`. Loop while `fast is not null && fast.Next is not null`, advancing `slow` by one and `fast` by two each iteration. When the loop stops, `slow` is sitting on the answer. Trace through a 4-node list (`1->2->3->4`) by hand to convince yourself this lands on node `3` (the second middle), not node `2`.

**Near-solution:**
```csharp
public static Node<int>? FindMiddle(Node<int>? head)
{
    var slow = head;
    var fast = head;
    while (fast is not null && fast.Next is not null)
    {
        slow = slow!.Next;
        fast = fast.Next.Next;
    }
    return slow;
}
```
</details>

<details>
<summary>Hint: MergeTwoSorted</summary>

**Nudge:** You're not creating any new `Node<int>` objects — you're re-pointing `Next` references on the existing nodes from both lists to weave them into one chain. A common trick to avoid special-casing "what's the very first node of the result" is to start with a throwaway dummy node.

**Approach:** Create `var dummy = new Node<int>(0);` (this one throwaway node IS allowed — it's never part of the returned result) and `var tail = dummy;`. While both `a` and `b` are non-null, compare `a.Value` and `b.Value`; attach the smaller one to `tail.Next`, advance that list's pointer, and advance `tail` to the node you just attached. When the loop ends, one of `a`/`b` is `null` and the other might have remaining nodes — attach whichever is left directly: `tail.Next = a ?? b;`. Return `dummy.Next` (skipping over the dummy itself).

**Near-solution:**
```csharp
public static Node<int>? MergeTwoSorted(Node<int>? a, Node<int>? b)
{
    var dummy = new Node<int>(0);
    var tail = dummy;
    while (a is not null && b is not null)
    {
        if (a.Value <= b.Value) { tail.Next = a; a = a.Next; }
        else { tail.Next = b; b = b.Next; }
        tail = tail.Next;
    }
    tail.Next = a ?? b;
    return dummy.Next;
}
```
</details>

## Running your work

```
cd modules/02-linked-lists/tests/LinkedLists.Tests && dotnet test
```

## If you're stuck

`solution/Solution.cs` has a complete, correct reference implementation of everything in this module — but try to genuinely struggle with each problem first. Productive struggle (getting it wrong, rereading the hints, tracing through an example by hand) is where the learning actually happens; jumping straight to the solution skips it.

If you're offline with only a weak local LLM to lean on, get specific with it instead of asking vague questions like "how do I reverse a linked list." Paste the *exact* method signature and constraints, e.g.:

> "I have `public static Node<int>? Reverse(Node<int>? head)` in C#. `Node<int>` has `Value` (int) and `Next` (`Node<int>?`). I need to reverse the list by rewiring `Next` pointers, O(1) extra space, no new node allocations. Walk me through the *approach* in plain English first — don't write code yet."

Weak local models tend to do better with a narrow, well-specified question and a two-step ask (approach first, code second) than with an open-ended "solve this for me." If the first answer looks off, tell it exactly which part is wrong rather than re-asking the same question.
