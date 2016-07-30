using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentDataStructures
{
    class Program
    {
        static void Main(string[] args)
        {
            TestLazy1();
        }

        private static void TestLazy()
        {
            Lazy<Person> lazyPerson = new Lazy<Person>();
            Console.WriteLine("Lazy object created");
            Console.WriteLine("has person been created {0}", lazyPerson.IsValueCreated ? "Yes" : "No");
            Console.WriteLine("Setting Name");
            lazyPerson.Value.Name = "Andy"; // Creates the person object on fetching Value
            Console.WriteLine("has person been created {0}", lazyPerson.IsValueCreated ? "Yes" : "No");
            Console.WriteLine("Setting Age");
            lazyPerson.Value.Age = 21; // Re-uses same object from first call to Value
            Person andy = lazyPerson.Value;
            Console.WriteLine(andy);
        }

        private static void TestLazy1()
        {
            // once creation has completed, no further creation will be initiated.
            var lazyPerson = new Lazy<Person>(() => new Person("Andy"));
            //Lazy<Person> lazyPerson = new Lazy<Person>(LazyThreadSafetyMode.PublicationOnly);
            Console.WriteLine("Lazy object created");
            Console.WriteLine("has person been created {0}", lazyPerson.IsValueCreated ? "Yes" : "No");

            Task<Person> p1 = Task.Run<Person>(() => lazyPerson.Value);
            Task<Person> p2 = Task.Run<Person>(() => lazyPerson.Value);

            p1.Wait();
            p2.Wait();
            Console.WriteLine("has person been created {0}", lazyPerson.IsValueCreated ? "Yes" : "No");

            // both tasks are returning the same reference.
            Console.WriteLine("are both tasks returning the same reference: {0}", object.ReferenceEquals(p1.Result, p2.Result));
        }

    }
}
