using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// A Task represents an asynchronous unit of work.
// compute-based tasks run on OS threads.
// compute-based tasks are assigned to thread pool threads.
namespace BasedTask
{
    class Program
    {
        static void Main(string[] args)
        {
            // Task t = Task.Factory.StartNew(Speak);
            // NET 4.5: Task t = Task.Run(Speak)
            Task t = new Task(Speak);
            t.Start();
            Console.WriteLine("Waiting for completion");
            // running tasks will simply be aborted if no foreground threads are executing
            t.Wait();
            // thread pool thread
            Task.Factory.StartNew(WhatTypeOfThreadAmI).Wait();
            // dedicated thread, which is destroyed once the task has completed
            Task.Factory.StartNew(WhatTypeOfThreadAmI, TaskCreationOptions.LongRunning).Wait();
            for (int i = 0; i < 10; i++)
            {
                int toCaptureI = i;
                Task.Factory.StartNew(() => Console.WriteLine("i: {0}",i));
                Task.Factory.StartNew(() => Console.WriteLine("toCaptureI: {0}", toCaptureI));
            }

            int n = 10;
            int r = 2;
            Task<int> part1 = Task.Factory.StartNew<int>(() => Double(n));
            Task<int> part2 = Task.Factory.StartNew<int>(() => Double(n - r));
            Task<int> part3 = Task.Factory.StartNew<int>(() => Double(r));
            List<Task<int>> task = new List<Task<int>>();
            task.Add(part1);
            task.Add(part2);
            task.Add(part3);
            Task.Factory.ContinueWhenAll(task.ToArray(),(ts) => {
                int i = 0;
                foreach (var item in ts)
                {
                    Console.WriteLine("task{0}: {1}", i, item.Result);
                    i++;
                }
                Console.WriteLine("part1/part2/part3: {0}/{1}/{2}", part1.Result, part2.Result, part3.Result);
            });

            Console.WriteLine("All Done");
            Console.ReadLine();
        }

        private static int Double(int n)
        {
            return n * 2;
        }

        private static void WhatTypeOfThreadAmI()
        {
            // if you want to have a long-running, asynchronous operation, using a thread pool for this purpose would be considered an abuse of the thread pool
            Console.WriteLine("I'm a {0} thread", Thread.CurrentThread.IsThreadPoolThread ? "Thread Pool" : "Custom");
        }

        private static void Speak()
        {
            Console.WriteLine("Hello World");
        }
    }
}
