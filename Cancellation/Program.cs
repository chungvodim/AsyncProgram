using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Cancellation
{
    class Program
    {
        static void Main(string[] args)
        {
            IImport impt = new Import();
            DataImport(impt);
        }

        public static void DataImport(IImport import)
        {
            var tcs = new CancellationTokenSource();
            CancellationToken ct = tcs.Token;
            Task importTask = import.ImportXmlFilesAsync(@"C:\data", ct);
            while (!importTask.IsCompleted)
            {
                Console.Write(".");
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    tcs.Cancel();
                }
                Thread.Sleep(250);
            }
        }

    }

    public interface IImport
    {
        Task ImportXmlFilesAsync(string dataDirectory);
        Task ImportXmlFilesAsync(string dataDirectory, CancellationToken ct);
    }

    public class Import : IImport
    {
        public Task ImportXmlFilesAsync(string dataDirectory)
        {
            return ImportXmlFilesAsync(dataDirectory, CancellationToken.None);
        }

        public Task ImportXmlFilesAsync(string dataDirectory, CancellationToken ct)
        {
            return Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Console.Write("_");
                    Thread.Sleep(250);
                    if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
                }
            }, ct);
        }

    }
}
