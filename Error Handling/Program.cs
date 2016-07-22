using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Error_Handling
{
    class Program
    {
        static void Main(string[] args)
        {
            Task task = Task.Factory.StartNew(() => Import(@"C:\data\2.xml"));
            try
            {
                task.Wait();
            }
            catch (AggregateException errors)
            {
                foreach (Exception error in errors.Flatten().InnerExceptions)
                {
                    Console.WriteLine("{0} : {1}", error.GetType().Name, error.Message);
                }
            }
        }

        private static void Import(string v)
        {
            throw new NotImplementedException();
        }
    }
}
