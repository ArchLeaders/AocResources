using AocModelExtractor;
using AocModelExtractor.Extensions;
using System.Diagnostics;
using System.IO.Compression;

if (args.Length == 0) {
    Console.WriteLine("Please specify the path to your Age of Calamity game files.");
    Console.ReadLine();
    return;
}

string aoc = args[0];
var urls = Resource.Load("urls.json").ParseJson<Dictionary<string, string?>>()!;

// Download Cethleann
byte[] cethleann = await urls["cethleann"]!.DownloadAsync();

// Extract Cethleann
using ZipArchive arc = new(new MemoryStream(cethleann));
foreach (var entry in arc.Entries) {
    string file = $".\\Cethleann\\{entry.FullName}";
    Directory.CreateDirectory(Path.GetDirectoryName(file)!);
    entry.ExtractToFile(file, true);
}

if (string.IsNullOrEmpty(urls["cethleann-patch"])) {

    // Download Cethleann patch
    byte[] patch = await urls["cethleann-patch"]!.DownloadAsync();

    // Extract Cethleann patch
    using ZipArchive patchArc = new(new MemoryStream(patch));
    foreach (var entry in patchArc.Entries) {
        string file = $".\\Cethleann\\{entry.FullName}";
        entry.ExtractToFile(file, true);
    }
}


// Run Cethleann extractor
Directory.CreateDirectory(".\\extracted-rdb");
Process.Start($"Cethleann\\Cethleann.DataExtractor.exe", $"--rdb \".\\extracted-rdb\" \"{args[0]}\"").WaitForExit();

// Run extractor on embeded name list
string[] hashList = Resource.Load("hash-list").ToString().Split("\n");
Parallel.ForEach(hashList, hashMap => {
    string[] hashes = hashMap.Split(" ");
    string model = hashes[0];
    string ktid = hashes[1];
    string kidsobjdb = hashes[2];
    string folder = kidsobjdb.StartsWith("CharacterEditor") ? "CharacterEditor" : "FieldEditor4";

    File.Copy($".\\extracted-rdb\\{folder}\\g1m\\{model}.g1m\"", $".\\extracted-rdb\\{folder}\\ktid\\{model}.g1m");
    Process.Start($"Cethleann\\Nyotengu.KTID.exe", $".\\extracted-rdb\\KIDSSystemResource\\kidsobjdb\\{kidsobjdb} .\\extracted-rdb\\MaterialEditor\\g1t .\\extracted-rdb\\{folder}\\ktid\\{ktid}.ktid").WaitForExit();
});

// Cleanup

// Optional: download/install Noesis