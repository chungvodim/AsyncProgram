using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReaderWriterLock
{
    class Program
    {
        static void Main(string[] args)
        {
            TestRWLock();
            Console.WriteLine("Finish");
        }

        public static void TestRWLock()
        {
            List<Task> tasks = new List<Task>();
            var cache = new Cache();
            tasks.Add(Task.Factory.StartNew(() =>
            {
                Random rnd = new Random();
                while (true)
                {
                    cache.AddNewsItem(new NewsItem(rnd.Next(1000).ToString()));
                    Thread.Sleep(1000);
                }
            }));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                Random rnd = new Random();
                while (true)
                {
                    var newItems = cache.GetNews(rnd.Next(1000).ToString());
                    foreach (var item in newItems)
                    {
                        Console.Write(item.Tags + " ");
                    }
                    Console.WriteLine();
                    Thread.Sleep(1000);
                }
            }));
            Task.WaitAll(tasks.ToArray());
        }
    }
}
