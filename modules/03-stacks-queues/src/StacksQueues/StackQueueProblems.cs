namespace StacksQueues;

/// <summary>
/// Classic algorithm problems that are naturally solved using a stack or a queue.
/// </summary>
public static class StackQueueProblems
{
    /// <summary>
    /// Determines whether <paramref name="s"/> consists of properly balanced and
    /// nested brackets. <paramref name="s"/> contains only the characters
    /// '(' ')' '[' ']' '{' '}'.
    /// </summary>
    /// <param name="s">The string of bracket characters to validate.</param>
    /// <returns>True if every opening bracket is closed by the matching type of
    /// closing bracket in the correct order; otherwise false.</returns>
    public static bool IsValidParentheses(string s)
    {
        // TODO: Walk the string left to right using a Stack<char> (System.Collections.Generic.Stack<T>
        // is fine to use here — the goal of this problem is the algorithm, not re-implementing the stack).
        // Push opening brackets. When you see a closing bracket, the stack must be
        // non-empty and its top must be the matching opener — pop it. If not, return false.
        // At the end, the string is valid only if the stack is empty.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Evaluates a Reverse Polish Notation (postfix) arithmetic expression.
    /// Each token in <paramref name="tokens"/> is either an integer literal or one
    /// of the operators "+", "-", "*", "/". Integer division truncates toward zero
    /// (this is C#'s default int division behavior).
    /// </summary>
    /// <param name="tokens">The RPN tokens, in evaluation order.</param>
    /// <returns>The integer result of evaluating the expression.</returns>
    public static int EvalRPN(string[] tokens)
    {
        // TODO: Use a Stack<int>. For each token: if it's a number, push it.
        // If it's an operator, pop the top two values (b = pop, then a = pop — mind
        // the order for - and /), compute a OP b, and push the result.
        // After processing all tokens, the single remaining stack value is the answer.
        throw new NotImplementedException();
    }
}
