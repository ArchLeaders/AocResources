﻿using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace AocModelExtractor
{
    public class Resource
    {
        internal static readonly string Namespace = typeof(Resource).Namespace ?? "AocModelExtractor";
        internal static readonly string BaseUrl = $"https://raw.githubusercontent.com/ArchLeaders/{Namespace}/master";

        public byte[] Data { get; set; }
        public Resource(byte[] data) => Data = data;
        public Resource(string name, string root)
        {
            if (File.Exists($"{root}/{name}")) {
                Data = File.ReadAllBytes($"{root}/{name}");
            }
            else if (File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}/{root}/{name}")) {
                Data = File.ReadAllBytes($"{AppDomain.CurrentDomain.BaseDirectory}/{root}/{name}");
            }
            else {

                string embed = $"{root}/{name}".ToCommonPath().Replace("/", ".");
                Stream? stream = Assembly.GetCallingAssembly()?.GetManifestResourceStream(embed);

                if (stream != null) {
                    using BinaryReader reader = new(stream);
                    Data = reader.ReadBytes((int)stream.Length);
                }
                else {
                    try {
                        string url = $"{BaseUrl}/{$"{root}/{name}".ToCommonPath()}?v={Random.Shared.Next(1, 100)}";
                        using HttpClient client = new();
                        Data = client.GetByteArrayAsync(url).Result;
                    }
                    catch (Exception ex) {
                        Debug.WriteLine(ex);
                        throw new FileNotFoundException($"Could not find, extract, or download the file '{root}/{name}'.");
                    }
                }
            }
        }


        public override string ToString() => Encoding.UTF8.GetString(Data);
        public string ToString(Encoding encoding) => encoding.GetString(Data);
        public dynamic? ParseJson() => JsonSerializer.Deserialize<dynamic>(Data);
        public T? ParseJson<T>() => JsonSerializer.Deserialize<T>(Data);

        public static Resource Load(string name, string? root = null) => new(name, root ?? Namespace);
    }

    public static class PathExt
    {
        public static string ToCommonPath(this string path) => path.Replace("\\", "/");
    }
}
