using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Signaling
{
    public class MyBarrier
    {
        Barrier barrier = new Barrier(participantCount: 5);
        public void TestStaticParticipation()
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

        public void TestDynamicParticipation()
        {
            int totalRecords = GetNumberOfRecords();

            Task[] tasks = new Task[totalRecords];

            for (int i = 0; i < totalRecords; ++i)
            {
                barrier.AddParticipant();

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

        public int GetNumberOfRecords()
        {
            return 20;
        }

        public void GetDataAndStoreData(int index)
        {
            Console.WriteLine("Getting data from server: " + index);
            Thread.Sleep(TimeSpan.FromSeconds(2));

            barrier.SignalAndWait();

            Console.WriteLine("Send data to Backup server: " + index);

            barrier.SignalAndWait();
        }
    }
}
