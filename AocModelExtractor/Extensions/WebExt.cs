namespace AocModelExtractor.Extensions
{
    internal static class WebExt
    {
        public static async Task<byte[]> DownloadAsync(this string url)
        {
            using HttpClient client = new();
            return await client.GetByteArrayAsync(url + $"?v={Random.Shared.Next(0, 15)}");
        }
    }
}
