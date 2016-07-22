using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ErrorHandling
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
                errors.Handle(IgnoreXmlErrors);
                foreach (Exception error in errors.Flatten().InnerExceptions)
                {
                    Console.WriteLine("{0} : {1}", error.GetType().Name, error.Message);
                }
            }
        }

        private static bool IgnoreXmlErrors(Exception arg)
        {
            return (arg.GetType() == typeof(XmlException));
        }

        private static void Import(string v)
        {
            throw new NotImplementedException();
        }
    }
}
