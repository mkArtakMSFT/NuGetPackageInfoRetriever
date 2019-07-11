using Newtonsoft.Json;
using NuGetDownloadCountRetriever.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NuGetDownloadCountRetriever
{
    public class PackageInfoRetriever
    {
        private readonly HttpClient httpClient;

        public PackageInfoRetriever(HttpClient client)
        {
            /// Source for this url was taken from: https://github.com/NuGet/Home/issues/2596
            client.BaseAddress = new Uri("https://api-v2v3search-0.nuget.org/query", UriKind.Absolute);
            this.httpClient = client;
        }

        public async Task<PackageInfo> GetPackageInfoAsync(string packageId)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                throw new ArgumentException();
            }

            string result = await this.httpClient.GetStringAsync("?q=packageid:" + packageId);
            var info = JsonConvert.DeserializeObject<PackageInfo>(result);
            return info;
        }
    }
}
