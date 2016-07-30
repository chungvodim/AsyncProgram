using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentDataStructures.Lazy
{
    public class Person
    {
        public Person()
        {
            Thread.Sleep(2000);
            Console.WriteLine("Created");
        }
        public string Name { get; set; }
        public int Age { get; set; }
        public override string ToString()
        {
            return string.Format("Name: {0}, Age: {1}", Name, Age);
        }
    }
}
