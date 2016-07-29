/*
 * C# Barrier class is synchronization primitives used in .NET threading. Barrier is used in an algorithm which composed of multiple phases. 
 * In this Barrier synchronization, we have multiple threads working on a single algorithm. 
 * Algorithm works in phases. All threads must complete phase 1 then they can continue to phase 2. 
 * Until all the threads do not complete the phase 1, all threads must wait for all the threads to reach at phase 1.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SynchronizationPrimitive
{
    class Program
    {
        static Barrier barrier = new Barrier(participantCount: 5);
        static void Main(string[] args)
        {
            Task[] tasks = new Task[5];

            for (int i = 0; i < 5; ++i)
            {
                int j = i;
                tasks[j] = Task.Factory.StartNew(() =>
                {
                    GetDataAndStoreData(j);
                });
            }

            Task.WaitAll(tasks);

            Console.WriteLine("Backup completed");
            Console.ReadLine();


        }

        static void GetDataAndStoreData(int index)
        {
            Console.WriteLine("Getting data from server: " + index);
            Thread.Sleep(TimeSpan.FromSeconds(2));

            barrier.SignalAndWait();

            Console.WriteLine("Send data to Backup server: " + index);

            barrier.SignalAndWait();
        }
    }
}
