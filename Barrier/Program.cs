using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barrier
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
