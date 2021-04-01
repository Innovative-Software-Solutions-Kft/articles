using System;
using System.Collections.Generic;
public static class SomeExtension{
    public static bool NotGreaterButNotNecessarilyLessOrEqual<T>(
     this T inVar, T toCompareTo)
    {
         var comparer = Comparer<T>.Default;
         return !(comparer.Compare(inVar, toCompareTo)>0);
    }
    public static bool NotLessButNotNecessarilyGreaterOrEqual<T>(
     this T inVar, T toCompareTo)
    {
        if(inVar == null)
            return true;
         var comparer = Comparer<T>.Default;
         return !(comparer.Compare(inVar, toCompareTo)<0);
    }
}
public class Test
{
    public static void Main()
    {
       int? myInt = null;
       Console.WriteLine("here you go: null NG 0 = {0}",
       myInt.NotGreaterButNotNecessarilyLessOrEqual<int?>(0));
       Console.WriteLine("here you go: null NL 0 = {0}",
       myInt.NotLessButNotNecessarilyGreaterOrEqual<int?>(0));
    }
}
