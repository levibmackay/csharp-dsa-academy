using StacksQueues;

namespace StacksQueues.Tests;

public class ArrayStackTests
{
    [Fact]
    public void NewStack_IsEmpty()
    {
        var stack = new ArrayStack<int>();

        Assert.True(stack.IsEmpty);
        Assert.Equal(0, stack.Count);
    }

    [Fact]
    public void Push_IncreasesCount_AndIsNotEmpty()
    {
        var stack = new ArrayStack<int>();

        stack.Push(1);

        Assert.False(stack.IsEmpty);
        Assert.Equal(1, stack.Count);
    }

    [Fact]
    public void Peek_ReturnsTopItem_WithoutRemovingIt()
    {
        var stack = new ArrayStack<int>();
        stack.Push(1);
        stack.Push(2);

        int top = stack.Peek();

        Assert.Equal(2, top);
        Assert.Equal(2, stack.Count);
    }

    [Fact]
    public void Pop_ReturnsItemsInLifoOrder()
    {
        var stack = new ArrayStack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);

        Assert.Equal(3, stack.Pop());
        Assert.Equal(2, stack.Pop());
        Assert.Equal(1, stack.Pop());
        Assert.True(stack.IsEmpty);
    }

    [Fact]
    public void Pop_OnEmptyStack_Throws()
    {
        var stack = new ArrayStack<int>();

        Assert.Throws<InvalidOperationException>(() => stack.Pop());
    }

    [Fact]
    public void Peek_OnEmptyStack_Throws()
    {
        var stack = new ArrayStack<int>();

        Assert.Throws<InvalidOperationException>(() => stack.Peek());
    }

    [Fact]
    public void Push_BeyondInitialCapacity_ResizesAndPreservesOrder()
    {
        var stack = new ArrayStack<int>();

        for (int i = 0; i < 100; i++)
        {
            stack.Push(i);
        }

        Assert.Equal(100, stack.Count);

        for (int i = 99; i >= 0; i--)
        {
            Assert.Equal(i, stack.Pop());
        }

        Assert.True(stack.IsEmpty);
    }

    [Fact]
    public void PushPopPush_InterleavedOperations_MaintainsCorrectState()
    {
        var stack = new ArrayStack<string>();

        stack.Push("a");
        stack.Push("b");
        Assert.Equal("b", stack.Pop());

        stack.Push("c");
        stack.Push("d");
        Assert.Equal("d", stack.Pop());
        Assert.Equal("c", stack.Pop());
        Assert.Equal("a", stack.Pop());
        Assert.True(stack.IsEmpty);
    }

    [Fact]
    public void WorksWithReferenceTypes()
    {
        var stack = new ArrayStack<string>();
        stack.Push("hello");

        Assert.Equal("hello", stack.Pop());
    }
}
