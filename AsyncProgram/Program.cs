using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// .NET 4.0 the maximum number of threads is determined by the amount of memory available
// the default stack size assigned by the linker is 1MB, and 2000 stacks times 1MB per stack equals around 2GB/
// You can try to squeeze more threads into your process by reducing your stack size

// default 1023 in Framework 4.0 (32-bit environment) (because the limitation of memory)
// default 32768 in Framework 4.0 (64-bit environment) (because the limitation of memory)
// 250 per core in Framework 3.5 => eight-core machine, this gives a maximum size to the thread pool of 2,000 threads
// 25 per core in Framework 2.0
// NET 3.5 changed the maximum number of threads in the thread pool to 250 per core
// NET 4.0 the maximum number of threads is determined by the amount of memory available (on most modern machines it will be 1,023 worker threads and 1,000 I/O threads).
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
