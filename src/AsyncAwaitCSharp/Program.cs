using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitCSharp
{
    class Program
    {
        public static void Main(string[] args)
        {
            Program program = new Program();

            while (true)
            {
                try
                {
                    Console.WriteLine("Operation mode :");
                    Console.WriteLine("\t 1. Normal");
                    Console.WriteLine("\t 2. AsyncAndWait");
                    Console.WriteLine("\t 3. AsyncParalel");
                    Console.WriteLine("\t 0. Quit");

                    Console.Write("Input operation mode (1/2/3/0): ");
                    string inputText = Console.ReadLine();
                    int inputInt = int.Parse(inputText);

                    if (inputInt == 0)
                        break;

                    DownloadMode mode = (DownloadMode)inputInt;
                    program.Execute(mode);
                }
                catch
                {
                    Console.WriteLine("Error process, try again");
                }

                Console.WriteLine();
            }

            Console.WriteLine("Finish");
        }

        private List<string> PrepData()
        {
            return new List<string>()
            {

                "https://www.yahoo.com",
                "https://www.google.com",
                "https://microsoft.com",
                "https://www.cnn.com",
                "https://www.codeproject.com",
                "https://www.stackoverflow.com"
            };
        }

        private void Execute(DownloadMode mode)
        {
            Console.WriteLine($"===== Execution Mode = {mode.ToString()} =======");
            var websites = this.PrepData();
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            switch (mode)
            {
                case DownloadMode.Normal:
                    Normal(websites);
                    break;

                case DownloadMode.AsyncAndWait:
                    AsyncAndWait(websites)
                        .Wait();
                    break;

                case DownloadMode.AsyncParalel:
                    AsyncParalel(websites)
                        .Wait();
                    break;

                default:
                    break;
            }

            watch.Stop();
            var elapseMs = watch.ElapsedMilliseconds;

            Console.WriteLine($"Total execution time = {elapseMs}");
            Console.WriteLine(Environment.NewLine);
        }

        private void Normal(IList<string> websites)
        {
            foreach (var url in websites)
            {
                WebsiteDataModel wdm = DoDownload(url);
                Console.Write(wdm.ToString());
            }
        }

        private async Task AsyncAndWait(IList<string> websites)
        {
            foreach (var url in websites)
            {
                WebsiteDataModel wdm = await Task.Run(() => DoDownload(url));
                Console.Write(wdm.ToString());
            }
        }

        private async Task AsyncParalel(IList<string> websites)
        {
            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();
            foreach (var url in websites)
            {
                tasks.Add(Task.Run(() => DoDownload(url)));
            }

            WebsiteDataModel[] dataModels = await Task.WhenAll(tasks);

            foreach(WebsiteDataModel wdm in dataModels)
            {
                Console.Write(wdm.ToString());
            }
        }

        //DoDownload does normal download (synchronous)
        private WebsiteDataModel DoDownload(string url)
        {
            WebClient wc = new WebClient();
            string data = wc.DownloadString(url);

            return new WebsiteDataModel(url, data);
        }
    }
}
