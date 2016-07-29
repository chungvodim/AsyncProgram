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
        static void Main(string[] args)
        {
        }

        private static void Produce(object obj)
        {
            var queue = (Queue<int>)obj;
            var rnd = new Random();
            while (true)
            {
                lock (queue)
                {
                    queue.Enqueue(rnd.Next(100));
                    Monitor.Pulse(queue);
                }
                Thread.Sleep(rnd.Next(2000));
            }
        }
        // the consumer thread consumes resources only when there is actually work available on the queue
        private static void Consume(object obj)
        {
            var queue = (Queue<int>)obj;
            while (true)
            {
                int val;
                lock (queue)
                {
                    while (queue.Count == 0)
                    {
                        Monitor.Wait(queue);
                    }
                    val = queue.Dequeue();
                }
                ProcessValue(val);
            }
        }

        private static void ProcessValue(int val)
        {
            throw new NotImplementedException();
        }
    }
}
