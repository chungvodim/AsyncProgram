using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskRelationships
{
    class Program
    {
        static void Main(string[] args)
        {
            //Chaining Tasks (Continuations)
            int i = 42;
            Task<int> firstTask = Task.Factory.StartNew<int>((ci) => { Console.WriteLine("First Task take {0}", i); return (int)(ci) + 1; }, i);
            Task secondTask = firstTask.ContinueWith(ft => { Console.WriteLine("Second Task, First task returned {0}", ft.Result); }, TaskContinuationOptions.OnlyOnRanToCompletion);
            Task errorHandler = firstTask.ContinueWith(ft => Console.WriteLine(ft.Exception), TaskContinuationOptions.OnlyOnFaulted);
            secondTask.Wait();
        }
    }
}
