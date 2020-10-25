using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;
using Octokit;

namespace HelloHome.Central.Update
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Connecting GitHub");
            var oc = new GitHubClient(new ProductHeaderValue("vdesmedt"));
            var latestRelease = await oc.Repository.Release.GetLatest("vdesmedt", "HelloHomeCentral");

            var asset = latestRelease.Assets[0];
            Console.WriteLine($"Found asset {asset.Name}");
            var binPackFilename = Path.Combine(Environment.CurrentDirectory, asset.Name);

            Console.WriteLine($"Downloading to {binPackFilename}");
            var res = await new HttpClient().GetAsync(asset.BrowserDownloadUrl);
            using (var binPack = File.OpenWrite(binPackFilename))
            {
                await res.Content.CopyToAsync(binPack);
                binPack.Close();
            }

            Console.WriteLine("Extracing...");
            using (var binPack = File.OpenRead(binPackFilename))
            {
                Stream gzipStream = new GZipInputStream(binPack);
    
                TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream, Encoding.Default);
                tarArchive.ExtractContents(Environment.CurrentDirectory);
                tarArchive.Close();
                
                gzipStream.Close();
            }

            Console.WriteLine("Deleting asset");
            File.Delete(binPackFilename);
        }
            
    }
}