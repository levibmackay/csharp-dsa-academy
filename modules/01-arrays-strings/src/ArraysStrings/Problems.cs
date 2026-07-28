using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;

namespace ArraysStrings;

/// <summary>
/// Module 1: Arrays &amp; Strings.
/// Implement each method below. Replace the TODO and the
/// <see cref="NotImplementedException"/> with your own working code.
/// See the module README.md for full problem statements, examples,
/// and hints.
/// </summary>
public static class Problems
{
    /// <summary>
    /// Given an array of integers <paramref name="nums"/> and an integer
    /// <paramref name="target"/>, return the indices of the two numbers
    /// that add up to <paramref name="target"/>. You may assume exactly
    /// one valid answer exists, and you may not use the same element twice.
    /// Target complexity: O(n) time, O(n) space, using a Dictionary&lt;int,int&gt;.
    /// </summary>
    public static int[] TwoSum(int[] nums, int target)
    {
        // Create dictionary to store a value. I will store the indices
        var seen = new Dictionary<int, int>();
        // Iterates through the list of nums for until it ends
        for (int i = 0; i < nums.Length; i++)
        {
            // For each index, the program subtracts the value of nums[i] from the target
            // This is done to avoid O(n^2) time complexity.
            // It allows us to get the number and the compliment at the same time.
            int complement = target - nums[i];
            // We then check the dictionary seen to see if the compliment is there.
            // The first few times that this for loop runs, it will simply skip this part
            // This is due to the fact that there is nothing in the dictionary yet
            if (seen.ContainsKey(complement))
            {
                // If the complement is found in the dictionary, return the indices
                // When returning multiple values in C#, you return them as an array
                // And we want to return the indices, not the values themselves
                return new int[] { seen[complement], i };
            }

            // Add the current number and its index to the dictionary
            // This is important especially the first couple times the loop runs. 
            // This adds the number to the seen list
            seen[nums[i]] = i;
        }
        // This line should never be reached because there's always a solution for valid inputs
        // The compiler was just giving me an error so I threw this in
        throw new InvalidOperationException("No two sum solution");
    }

    /// <summary>
    /// Reverse the character array <paramref name="s"/> in place.
    /// Target complexity: O(n) time, O(1) extra space.
    /// </summary>
    public static void ReverseString(char[] s)
    {
        // TODO: use two pointers (left/right) and swap characters,
        // moving them toward the middle.
        // They don't want me to simply reverse it. They want me to use pointers
        // Something tells me that I'm going to be using this a lot

        // First I create pointers. These don't actually have any value
        // They simply help me and the computer know what locations I am talking about in the array
        // Left starts at the beginning of the array and right at the end
        int left = 0;
        int right = s.Length - 1;

        // We want to stop this loop once the pointers reach the middle
        // This will happen when left > right or left = right
        while (left < right)
        {
            // Until that requirment is met, I create a temp character.
            // I set this to the index left in the array of characters s. 
            // I then set left index equal to right index and set right index to temp
            // This swaps the values of the array.
            char temp = s[left];
            s[left] = s[right];
            s[right] = temp;

            // Then to move the pointers I increase the value of left and decrease the value of right
            left++;
            right--;
        }
        // Real life uses:
        // Palindrome Check: Determining if a given word is a palindrome, such as checking if "racecar" reads the same forwards and backwards.
        // String Encryption: Reversing a string to create a simple encryption method (though this is not secure for real-world applications).
        // Data Structure Operations: Reversing elements in a stack or queue when processing data.
        // Text Processing: Reversing the order of words in a sentence, such as "hello world" becomes "world hello".
        // String Compression: Compressing strings by reversing them and identifying patterns that can be repeated.
        // Reversing Words for Meaning: Analyzing text to understand the structure or meaning of sentences by reversing words.
        // Sorting Algorithms: Reversing an array as part of a sorting algorithm to achieve specific orderings.
        // User Input Handling: Reversing user input for validation purposes, such as checking if a password is entered correctly when reversed.
        // Data Formatting: Reversing data for display purposes or compatibility with other systems.
        // Custom Algorithms: Implementing custom algorithms that require reversing as a step in their logic.


    }

    /// <summary>
    /// Return true if <paramref name="a"/> and <paramref name="b"/> are
    /// anagrams of each other (case-sensitive: 'A' and 'a' are different
    /// characters).
    /// Target complexity: O(n) time, O(1) extra space (fixed alphabet) or
    /// O(n) space if using a general-purpose dictionary.
    /// </summary>
    public static bool IsAnagram(string a, string b)
    {
        // TODO: compare character counts of both strings. Different
        // lengths can never be anagrams (short-circuit that case).
        // first i need to comepare the lengths of them. they can be anagrams
        if (a.Length != b.Length)
        {
            return false;
        }
        // I now need to make sure that the letters are the same. 
        // I need to convert the strings to chars
        // I started by trying to convert them to chars and then iterate through
        // But if it's an anagram, the letters/indices between the two lists would never match up
        // So I decided to count the amount of each letter in the string
        // I set it to 256 because of ASCII. 
        // It just sets all the values to 0 until I set them to something
        // Counter for A 
        int[] countA = new int[256];
        // Counter for B
        int[] countB = new int[256];

        // Then I iterate through each string (a and b)
        // And for each index. I add that count to my countA and countB arrays
        // This tallies up the amount of each kind of letter in each string.
        for (int i = 0; i < a.Length; i++)
        {
            countA[a[i]]++;
            countB[b[i]]++;
        }

        // Now that we have counted the amount of each characters, we can compare them
        for (int i = 0; i < 256; i++)
        {
            // I always find it's easier to check for != rather than =
            if (countA[i] != countB[i])
            {
                return false;
            }
        }

        // All of that works, then we return true. These word could be anagrams
        return true;
    }

    /// <summary>
    /// Return the largest sum of any contiguous, non-empty subarray of
    /// <paramref name="nums"/> (Kadane's algorithm). The array has at
    /// least one element and may contain negative numbers.
    /// Target complexity: O(n) time, O(1) extra space.
    /// </summary>
    public static int MaxSubArray(int[] nums)
    {
        // TODO: track a running "best sum ending here" and a global best,
        // resetting the running sum when it drops below the current element.
        // To start, I set the best and current sum to the first item of the array of numbers
        int bestSum = nums[0];
        int currentSum = nums[0];

        // Then I iterate through the list
        // Since I already have the value for 0, I start i = 1;
        for (int i = 1; i < nums.Length; i++)
        {
            // Update currentSum to be the maximum of nums[i] or currentSum + nums[i]
            // For each iteration, I need to compare the number to the currentSum + the number to see which is greater. 
            // Whichever value is greater is kept and assigned to currentSum using the Math.Max function.
            currentSum = Math.Max(nums[i], currentSum + nums[i]);

            // Update bestSum if currentSum is greater than bestSum
            // This if loop is pretty self-explanatory
            if (currentSum > bestSum)
            {
                bestSum = currentSum;
            }
        }

        // Finally we need to return the bestSum
        return bestSum;

    }

    /// <summary>
    /// Rotate <paramref name="nums"/> to the right by <paramref name="k"/>
    /// steps, in place. <paramref name="k"/> may be larger than
    /// nums.Length (handle that with a modulo).
    /// Target complexity: O(n) time, O(1) extra space.
    /// </summary>
    public static void RotateArray(int[] nums, int k)
    {
        // TODO: normalize k with k % nums.Length (guard against an empty
        // array), then use the "reverse three times" trick: reverse the
        // whole array, then reverse the first k elements, then reverse
        // the remaining elements.


        // Okay so my biggest thing for this one is going to be pointers. I need to be able to remove something from the front and then add it to the end.
        // But I don't want to go and pop and add every item to the end. what if they have lots of data/crap
        // what if I just changed the indices??
        // Example to help me visualize this
        // int[] test = {1,2,3,4,5}
        // int k = 2
        // my output needs to be {4,5,1,2,3}


        // First we need to make sure that something exists or our program will explode.
        if (nums.Length == 0) return;

        // Normalize k
        k = k % nums.Length;

        // Reverse the entire array
        Reverse(nums, 0, nums.Length - 1);

        // Reverse the first k elements
        Reverse(nums, 0, k - 1);

        // Reverse the remaining elements
        Reverse(nums, k, nums.Length - 1);

    }
    private static void Reverse(int[] nums, int start, int end)
    {
        while (start < end)
        {
            // Swap elements at start and end
            int temp = nums[start];
            nums[start] = nums[end];
            nums[end] = temp;

            // Move towards the middle
            start++;
            end--;
        }
    }
}
