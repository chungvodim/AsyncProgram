using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadSafety
{
    class Program
    {
        static void Main(string[] args)
        {
            const int iterations = 100000;
            const int numTasks = 2;
            List<Task> tasks = new List<Task>();
            int value = 0;
            for (int nTask = 0; nTask < numTasks; nTask++)
            {
                Task t = Task.Factory.StartNew(() =>
                {
                    IncrementValue(ref value, iterations);
                });
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Expected value: {0}, Actual value: {1}", numTasks * iterations, value);
        }
        private static void IncrementValue(ref int value, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                value++;
            }
        }

    }
}
