using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskRelationships
{
    class Program
    {
        static void Main(string[] args)
        {
            //Chaining Tasks (Continuations)
            int i = 42;
            Task<int> firstTask = Task.Factory.StartNew<int>((ci) => { Console.WriteLine("First Task take {0}", i); /*throw new Exception("Errorrrrrrrr") ;*/ return (int)(ci) + 1; }, i);
            Task secondTask = firstTask.ContinueWith(ft => { Console.WriteLine("Second Task, First task returned {0}", ft.Result); }, TaskContinuationOptions.OnlyOnRanToCompletion);
            Task errorHandler = firstTask.ContinueWith(ft => Console.WriteLine(ft.Exception.InnerException.Message), TaskContinuationOptions.OnlyOnFaulted);

            Console.WriteLine("Do independent things");
            Console.WriteLine("start waiting");

            try
            {
                secondTask.Wait();
                errorHandler.Wait();
            }
            catch (AggregateException errors)
            {
                foreach (Exception error in errors.Flatten().InnerExceptions)
                {
                    Console.WriteLine("fuck {0} : {1}", error.GetType().Name, error.Message);
                }
            }

            //Nested Task
            Task.Factory.StartNew(() =>
            {
                Task childTask = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("start child task1");
                    Thread.Sleep(10000);
                    Console.WriteLine("finish child task1");
                });
            }).Wait();

            Task.Factory.StartNew(() => 
            {
                Task childTask = Task.Factory.StartNew(() => 
                {
                    Console.WriteLine("start child task2");
                    Thread.Sleep(5000);
                    Console.WriteLine("finish child task2");
                }, TaskCreationOptions.AttachedToParent);
            }).Wait();
            Console.WriteLine("finish waiting");
        }
    }
}
