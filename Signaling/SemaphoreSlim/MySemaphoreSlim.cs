﻿/*
 * SemaphoreSlim is a lightweight alternative to Semaphore that limits the number of threads that can access a resource or pool of resources concurrently
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Signaling
{
    public class MySemaphoreSlim
    {
        private SemaphoreSlim semaphore;
        // A padding interval to make the output more orderly.
        private int padding;
        public void Run()
        {
            // Create the semaphore.
            // MaximumCount denotes the maximum number of threads that can enter concurrently.
            // InitialCount denotes the initial number of threads which can enter the Semaphore directly.
            semaphore = new SemaphoreSlim(0, 3);
            Console.WriteLine("{0} tasks can enter the semaphore.", semaphore.CurrentCount);
            Task[] tasks = new Task[5];

            // Create and start five numbered tasks.
            for (int i = 0; i <= 4; i++)
            {
                tasks[i] = Task.Run(() => {
                    // Each task begins by requesting the semaphore.
                    Console.WriteLine("Task {0} begins and waits for the semaphore.", Task.CurrentId);
                    semaphore.Wait();

                    Interlocked.Add(ref padding, 100);

                    Console.WriteLine("Task {0} enters the semaphore.", Task.CurrentId);

                    // The task just sleeps for 1+ seconds.
                    Thread.Sleep(1000 + padding);

                    Console.WriteLine("Task {0} releases the semaphore; previous count: {1}.", Task.CurrentId, semaphore.Release());
                });
            }

            // Wait for half a second, to allow all the tasks to start and block.
            Thread.Sleep(500);

            // Restore the semaphore count to its maximum value.
            Console.Write("Main thread calls Release(3) --> ");
            // can release maximum 3 threads at one time
            semaphore.Release(3);
            Console.WriteLine("{0} tasks can enter the semaphore.", semaphore.CurrentCount);
            // Main thread waits for the tasks to complete.
            Task.WaitAll(tasks);

            Console.WriteLine("Main thread exits.");
        }
    }
}
