using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionFunc
{
    class Program
    {
        static void Main(string[] args)
        {
            act(2,3);
            act1(3,4);
            Console.WriteLine(fn(5,6));
            Console.WriteLine(fn1(7,8));
        }
        // We can now use Action<in T> delegate to pass a method as a parameter without explicitly defining the delegate. The compiler will take care of it. This delegate can accept parameters but without a return type.
        static Action<int, int> act = delegate (int a, int b)
         {
             Console.WriteLine("using Action<int,int> to calculate a + b : {0}", a + b);
         };
        static Action<int, int> act1 = (a,b) => Console.WriteLine("using Action<int,int> to calculate a + b : {0}", a + b);
        // Func delegate is different from Action<T> in the sense that it supports for return value.
        static Func<int, int, int> fn = delegate (int a, int b)
        {
            return a + b;
        };

        static Func<int, int, int> fn1 = (a, b) => a * b;
    }
}
