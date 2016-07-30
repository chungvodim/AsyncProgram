using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            Task task = DoSomething();
            task.Wait();
            //Console.Read();
        }

        public static async Task DoSomething()
        {
            await DoAnything();
        }

        private static async Task DoAnything()
        {
            Console.WriteLine("before delay");
            // Thread.Sleep consume memory for sleeping thread, although thread doesn’t consume any CPU-based resources
            //Thread.Sleep(2000)
            // Task.Delay allow the thread to be free to serve other requests
            // When you are ready to continue using CPU resources again, you can obtain a thread(not necessarily the same one) and continue processing
            await Task.Delay(2000);
            Console.WriteLine("after delay");
        }
    }
}
