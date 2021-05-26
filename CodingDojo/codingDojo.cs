using System;
using System.Collections.Generic;

//solution 1 - iterative checking
public bool IsPalindrome(string candidate)
    {
        var length = candidate.Length;
        for (int i = 0; i < length; ++i)
        {
            if (!(candidate[i] == candidate[length - i - 1]))
                return false;
        }
        return true;
    }
//solution 2 - recursive
public bool IsPalindrome(string candidate)
    {
        if (candidate == null || candidate.Length <= 1)
            return true;
        return (candidate[0] == candidate[candidate.Length - 1]) && IsPalindrome(candidate.Substring(1, candidate.Length - 2));
    }
//solution 3 - reverse string manually
public string Reverse(string intStr)
    {
        char[] cArray = intStr.ToCharArray();
        string reverse = String.Empty;
        for (int i = cArray.Length - 1; i > -1; i--)
        {
            reverse += cArray[i];
        }
        return reverse;
    }

public bool IsPalindrome(string candidate)
{
    return candidate == Reverse(candidate);
}
//solution 4 - reverse string by original method
if (candidate is null || candidate.Length <= 1)
{
    return true;
}
char[] chars = candidate.ToCharArray();
Array.Reverse(chars);
string reversed = new(chars);
return candidate == reversed;
// solution 5 - functional programming
public bool IsPalindrome(string candidate)
{
    var chars = candidate?.ToCharArray() ?? Array.Empty<char>();
    return chars
        .Zip(chars.Reverse())
        .Take((candidate?.Length ?? 0) / 2)
        .All(t => t.First == t.Second);
}