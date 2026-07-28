// Reference solution — only read this after a real attempt.
//
// This file lives outside any .csproj (it is plain reference material, not
// compiled as part of the project), so it reuses the same namespace/class/method
// names as the src stubs without causing a build conflict.

namespace StacksQueues;

public class ArrayStack<T>
{
    private T[] _items;
    private int _count;

    public ArrayStack()
    {
        _items = new T[4];
        _count = 0;
    }

    public int Count => _count;

    public bool IsEmpty => _count == 0;

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
        if (IsEmpty)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        _count--;
        T item = _items[_count];
        _items[_count] = default!; // avoid holding a stale reference
        return item;
    }

    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        return _items[_count - 1];
    }
}

public class ArrayQueue<T>
{
    private T[] _items;
    private int _head;
    private int _count;

    public ArrayQueue()
    {
        _items = new T[4];
        _head = 0;
        _count = 0;
    }

    public int Count => _count;

    public bool IsEmpty => _count == 0;

    public void Enqueue(T item)
    {
        if (_count == _items.Length)
        {
            var newItems = new T[_items.Length * 2];
            for (int i = 0; i < _count; i++)
            {
                // Walk the logical order starting at _head, wrapping around the old array.
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
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty.");
        }

        T item = _items[_head];
        _items[_head] = default!; // avoid holding a stale reference
        _head = (_head + 1) % _items.Length;
        _count--;
        return item;
    }

    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Queue is empty.");
        }

        return _items[_head];
    }
}

public static class StackQueueProblems
{
    public static bool IsValidParentheses(string s)
    {
        var stack = new Stack<char>();

        foreach (char c in s)
        {
            switch (c)
            {
                case '(':
                case '[':
                case '{':
                    stack.Push(c);
                    break;

                case ')':
                    if (stack.Count == 0 || stack.Pop() != '(') return false;
                    break;

                case ']':
                    if (stack.Count == 0 || stack.Pop() != '[') return false;
                    break;

                case '}':
                    if (stack.Count == 0 || stack.Pop() != '{') return false;
                    break;
            }
        }

        return stack.Count == 0;
    }

    public static int EvalRPN(string[] tokens)
    {
        var stack = new Stack<int>();

        foreach (string token in tokens)
        {
            switch (token)
            {
                case "+":
                case "-":
                case "*":
                case "/":
                    int b = stack.Pop();
                    int a = stack.Pop();
                    int result = token switch
                    {
                        "+" => a + b,
                        "-" => a - b,
                        "*" => a * b,
                        "/" => a / b, // C# int division truncates toward zero already
                        _ => throw new InvalidOperationException("Unreachable"),
                    };
                    stack.Push(result);
                    break;

                default:
                    stack.Push(int.Parse(token));
                    break;
            }
        }

        return stack.Pop();
    }
}
