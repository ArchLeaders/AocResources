using System.IO.Compression;

namespace AocModelExtractor.Extensions
{
    internal static class DownloadExt
    {
        public static async Task DownloadAndExtractAsync(this string url, string dstDir, string key = "package")
        {
            Console.WriteLine($"Downloading {key}. . .");
            using HttpClient client = new();
            byte[] data = await client.GetByteArrayAsync(url + $"?v={Random.Shared.Next(0, 15)}");

            Console.WriteLine($"Extracting {key}. . .");
            using ZipArchive noesisArc = new(new MemoryStream(data));
            foreach (var entry in noesisArc.Entries) {
                string file = $"{dstDir}\\{entry.FullName}";
                if (entry.Length > 0) {
                    Directory.CreateDirectory(Path.GetDirectoryName(file)!);
                    entry.ExtractToFile(file, true);
                }
            }
        }
    }
}
