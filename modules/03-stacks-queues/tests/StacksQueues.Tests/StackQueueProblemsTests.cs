using StacksQueues;

namespace StacksQueues.Tests;

public class StackQueueProblemsTests
{
    [Theory]
    [InlineData("()", true)]
    [InlineData("()[]{}", true)]
    [InlineData("{[]}", true)]
    [InlineData("(]", false)]
    [InlineData("([)]", false)]
    [InlineData("{[()()]}", true)]
    [InlineData("", true)]
    [InlineData("(", false)]
    [InlineData(")", false)]
    [InlineData("(((((", false)]
    [InlineData("()()()", true)]
    public void IsValidParentheses_ReturnsExpectedResult(string input, bool expected)
    {
        bool result = StackQueueProblems.IsValidParentheses(input);

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(new[] { "2", "1", "+", "3", "*" }, 9)]     // (2 + 1) * 3 = 9
    [InlineData(new[] { "4", "13", "5", "/", "+" }, 6)]    // 4 + (13 / 5) = 4 + 2 = 6
    [InlineData(new[] { "10", "6", "9", "3", "+", "-11", "*", "/", "*", "17", "+", "5", "+" }, 22)]
    [InlineData(new[] { "5" }, 5)]                          // single literal
    [InlineData(new[] { "3", "4", "-" }, -1)]               // subtraction order matters
    [InlineData(new[] { "7", "2", "/" }, 3)]                // truncation toward zero
    [InlineData(new[] { "-7", "2", "/" }, -3)]              // truncation toward zero for negatives
    public void EvalRPN_ReturnsExpectedResult(string[] tokens, int expected)
    {
        int result = StackQueueProblems.EvalRPN(tokens);

        Assert.Equal(expected, result);
    }
}
