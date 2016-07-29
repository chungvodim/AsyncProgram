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
        private static readonly object guard = new object();
        static void Main(string[] args)
        {
            TestLock();
            Console.WriteLine("finish waiting");
        }

        private static void TestMonitor()
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
        }

        private static void TestLock()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 2; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => IncreaseValue(true)));
            }
            Task.WaitAll(tasks.ToArray());
        }

        private static void IncreaseValue(bool hasLock)
        {
            if (hasLock)
            {
                lock (guard)
                {
                    Console.WriteLine("i'm using this function, pls wait for your turn");
                    Thread.Sleep(5000);
                    Console.WriteLine("your turn");
                }
            }
            else
            {
                Console.WriteLine("i'm using this function without lock, pls wait for your turn");
                Thread.Sleep(5000);
                Console.WriteLine("your turn");
            }
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
