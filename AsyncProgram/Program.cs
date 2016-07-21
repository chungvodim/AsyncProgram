﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncProgram
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create task and start it.
            // ... Wait for it to complete.
            Task task = new Task(ProcessDataAsync);
            Console.WriteLine("Start task");
            task.Start();
            Console.WriteLine("Please wait patiently until task finishes.");
            task.Wait();
            Console.ReadLine();
        }

        static async void ProcessDataAsync()
        {
            // Start the HandleFile method.
            Task<int> task = HandleFileAsync("C:\\Temp\\test1.txt");

            // Control returns here before HandleFileAsync returns.
            // ... Prompt the user.
            Console.WriteLine("Please wait patiently while I do something important.");

            // Wait for the HandleFile task to complete.
            // ... Display its results.
            int x = await task;
            Console.WriteLine("Count: " + x);
        }

        static async Task<int> HandleFileAsync(string file)
        {
            Console.WriteLine("HandleFile enter");
            int count = 0;

            // Read in the specified file.
            // ... Use async StreamReader method.
            using (StreamReader reader = new StreamReader(file))
            {
                string v = await reader.ReadToEndAsync();

                // ... Process the file data somehow.
                count += v.Length;

                // ... A slow-running computation.
                //     Dummy code.
                for (int i = 0; i < 10000; i++)
                {
                    int x = v.GetHashCode();
                    if (x == 0)
                    {
                        count--;
                    }
                }
            }
            Console.WriteLine("HandleFile exit");
            return count;
        }
    }
}
