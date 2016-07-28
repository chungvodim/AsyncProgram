using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSafety
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Task> tasks = new List<Task>();
            SmallBusiness sb = new SmallBusiness(1000, 1000);
            for (int nTask = 0; nTask < 10; nTask++)
            {
                Task t = Task.Factory.StartNew(() =>
                {
                    sb.UnsafeReceivePayment(10);
                    Console.WriteLine("NetWorth: {0}", sb.UnsafeNetWorth);
                });
                //t.Wait();
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("finish waiting");
        }

        private static void TestInterLocked()
        {
            const int iterations = 100000000;
            const int numTasks = 2;
            List<Task> tasks = new List<Task>();
            int value = 0;
            for (int nTask = 0; nTask < numTasks; nTask++)
            {
                Task t = Task.Factory.StartNew(() =>
                {
                    IncrementValue(ref value, iterations, true);
                });
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Expected value: {0}, Actual value: {1}", numTasks * iterations, value);
        }

        // The Interlocked static class provides methods to turn nonatomic operations into atomic ones. It generally only works on single pieces of data up to 64 bits in length
        // Interlocked has three basic operations: Increment, Decrement, and Add 
        // Interlocked will affect code run significantly slower (4 to 10 times slower depending on hardware)
        private static void IncrementValue(ref int value, int iterations, bool hasInterlocked)
        {
            if (hasInterlocked)
            {
                for (int i = 0; i < iterations; i++)
                {
                    Interlocked.Increment(ref value);
                }
            }
            else
            {
                for (int i = 0; i < iterations; i++)
                {
                    //1.Copy the value from the variable into a register
                    //2.Increment the value in the register
                    //3.Copy the new value to the variable
                    value++;
                }
            }
        }
    }
}
