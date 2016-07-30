using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            TickTockAsync();
            Console.Read();
        }

        public static void DoSomething()
        {
            DoAnything().Wait();
        }

        private static async Task DoAnything()
        {
            Console.WriteLine("before delay");
            // Thread.Sleep consume memory for sleeping thread, although thread doesn’t consume any CPU-based resources
            //Thread.Sleep(2000);
            // Task.Delay allow the thread to be free to serve other requests
            // When you are ready to continue using CPU resources again, you can obtain a thread(not necessarily the same one) and continue processing
            await Task.Delay(1000);
            Console.WriteLine("after delay");
        }

        public static async Task TestWhenAll()
        {
            List<Task<string>> tasks = new List<Task<string>>();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew((c) => { return string.Format("task {0}", c); }, i));
            }
            // be equivalent Task.Factory.ContinueWhenAll
            Task allTasks = Task.WhenAll(tasks);
            try
            {
                await allTasks;
                tasks.ForEach(t => UpdateUI(t.Result));
            }
            catch (Exception ex)
            {
                allTasks.Exception.Handle(exception =>
                {
                    Console.WriteLine(exception.Message);
                    return true;
                });
            }
        }

        private static void UpdateUI(string result)
        {
            Console.WriteLine(result);
        }

        private static async void TickTockAsync()
        {
            Console.WriteLine("Starting Clock");
            while (true)
            {
                Console.Write("Tick ");
                await Task.Delay(500);
                Console.WriteLine("Tock");
                await Task.Delay(500);
            }
        }
    }
}
