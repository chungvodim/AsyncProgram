using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Signaling
{
    class Program
    {
        private static readonly object guard = new object();
        static void Main(string[] args)
        {
            TestSemaphoreSlim();
            Console.WriteLine("finish waiting");
        }

        private static void TestSemaphoreSlim()
        {
            MySemaphoreSlim mss = new MySemaphoreSlim();
            mss.Run();
        }

        private static void TestManualResetEventSlim()
        {
            MyManualResetEvent mmre = new MyManualResetEvent();
            mmre.MRES_SetWaitReset();
            //mmre.MRES_SpinCountWaitHandle();
        }

        private static void TestMonitorSignaling()
        {
            var ms = new MonitorSemaphore(1,10);
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 20; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => 
                {
                    ms.Enter();
                    Console.WriteLine(ms.CurrentCount);
                }));
            }
            Task.WaitAll(tasks.ToArray());
        }

    }
}
