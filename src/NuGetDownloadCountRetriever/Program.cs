using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NuGetDownloadCountRetriever
{
    class Program
    {
        static async Task Main(string[] args)
        {
            PackageInfoRetriever retriever = new PackageInfoRetriever(new HttpClient());

            IDictionary<string, string> packageCache = new Dictionary<string, string>();

            using (StreamReader reader = new StreamReader(@"C:\Users\Artak\Desktop\packageNames.txt"))
            using (StreamWriter writer = new StreamWriter(@"D:\packageCounts.txt", false))
            {
                while (!reader.EndOfStream)
                {
                    string packageId = await reader.ReadLineAsync();
                    if (!string.IsNullOrWhiteSpace(packageId))
                    {
                        packageId = packageId.Split('_')[0];
                        string normalizedPackageId = packageId.ToLowerInvariant();
                        if (!packageCache.ContainsKey(normalizedPackageId))
                        {
                            packageCache.Add(normalizedPackageId, string.Empty);
                            var info = await retriever.GetPackageInfoAsync(packageId);
                            var data = info.Data.FirstOrDefault();
                            if (data == null)
                            {
                                Console.Out.WriteLine($"Packge {packageId} is not found");
                            }
                            else
                            {
                                string log = $"{data.Id}\t{data.TotalDownloads}";
                                await writer.WriteLineAsync(log);
                                Console.Out.WriteLine(log);
                            }
                        }
                    }
                }
            }
        }
    }
}
