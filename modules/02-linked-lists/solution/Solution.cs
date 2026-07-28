// Reference solution — only read this after a real attempt.
//
// This file is NOT part of any .csproj build. It's here purely as
// reference material once you've genuinely tried the problems yourself.
// It mirrors the exact namespace, classes, and method signatures found in
// src/LinkedLists/, so you can compare your approach line-for-line.

namespace LinkedLists;

public class Node<T>
{
    public T Value { get; set; }
    public Node<T>? Next { get; set; }

    public Node(T value)
    {
        Value = value;
        Next = null;
    }
}

public class SinglyLinkedList<T>
{
    public Node<T>? Head { get; private set; }
    public int Count { get; private set; }

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

    public void AddFirst(T value)
    {
        var node = new Node<T>(value) { Next = Head };
        Head = node;
        Count++;
    }

    public bool Remove(T value)
    {
        Node<T>? previous = null;
        var current = Head;

        while (current is not null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Value, value))
            {
                if (previous is null)
                {
                    Head = current.Next;
                }
                else
                {
                    previous.Next = current.Next;
                }
                Count--;
                return true;
            }

            previous = current;
            current = current.Next;
        }

        return false;
    }

    public bool Contains(T value)
    {
        var current = Head;
        while (current is not null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Value, value))
            {
                return true;
            }
            current = current.Next;
        }
        return false;
    }

    public List<T> ToList()
    {
        var result = new List<T>();
        var current = Head;
        while (current is not null)
        {
            result.Add(current.Value);
            current = current.Next;
        }
        return result;
    }
}

public static class LinkedListAlgorithms
{
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

    public static bool HasCycle(Node<int>? head)
    {
        var slow = head;
        var fast = head;

        while (fast is not null && fast.Next is not null)
        {
            slow = slow!.Next;
            fast = fast.Next.Next;

            if (ReferenceEquals(slow, fast))
            {
                return true;
            }
        }

        return false;
    }

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

    public static Node<int>? MergeTwoSorted(Node<int>? a, Node<int>? b)
    {
        var dummy = new Node<int>(0);
        var tail = dummy;

        while (a is not null && b is not null)
        {
            if (a.Value <= b.Value)
            {
                tail.Next = a;
                a = a.Next;
            }
            else
            {
                tail.Next = b;
                b = b.Next;
            }
            tail = tail.Next;
        }

        tail.Next = a ?? b;

        return dummy.Next;
    }
}
