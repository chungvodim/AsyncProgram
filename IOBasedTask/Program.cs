using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOBasedTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Task<string> downloadTask = DownloadWebPageAsync("http://www.rocksolidknowledge.com/5SecondPage.aspx");
            while (!downloadTask.IsCompleted)
            {
                Console.Write(".");
                Thread.Sleep(250);
            }
            Console.WriteLine(downloadTask.Result);
        }
        private static string DownloadWebPage(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            {
                // this will return the content of the web page
                return reader.ReadToEnd();
            }
        }

        private static Task<string> DownloadWebPageAsync(string url)
        {
            return Task.Factory.StartNew(() => DownloadWebPage(url));
        }

        // keeping the number of threads as small as possible
        private static Task<string> BetterDownloadWebPageAsync(string url)
        {
            WebRequest request = WebRequest.Create(url);
            IAsyncResult ar = request.BeginGetResponse(null, null);
            Task<string> downloadTask =
            Task.Factory.FromAsync<string>(ar, iar =>
            {
                using (WebResponse response = request.EndGetResponse(iar))
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            });
            return downloadTask;
        }
    }
}
