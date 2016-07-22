using System;
using System.Linq;

class Program
{
    static void Main()
    {
        //
        // Create a Func instance that has one parameter and one return value.
        // ... Parameter is an integer, result value is a string.
        //
        Func<int, string> func1 = (x) => string.Format("string = {0}", x);
        //
        // Func instance with two parameters and one result.
        // ... Receives bool and int, returns string.
        //
        Func<bool, int, string> func2 = (b, x) =>
            string.Format("string = {0} and {1}", b, x);
        //
        // Func instance that has no parameters and one result value.
        //
        Func<double> func3 = () => Math.PI / 2;

        //
        // Call the Invoke instance method on the anonymous functions.
        //
        Console.WriteLine(func1.Invoke(5));
        Console.WriteLine(func2.Invoke(true, 10));
        Console.WriteLine(func3.Invoke());

        Func<int, string> projection = x => "Value=" + x;
        int[] values = { 3, 7, 10 };

        var strings = values.Select(projection);
        foreach (string s in strings)
        {
            Console.WriteLine(s);
        }

        strings = values.Select(x => "Value=" + x);
        foreach (string s in strings)
        {
            Console.WriteLine(s);
        }
    }
}