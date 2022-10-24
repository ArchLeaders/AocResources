using AocModelExtractor;
using AocModelExtractor.Extensions;
using System.Diagnostics;
using System.IO.Compression;

if (args.Length == 0) {
    Console.WriteLine("Please specify the path to your Age of Calamity game files.");
    Console.ReadLine();
    return;
}

long timeStamp = DateTime.Now.ToBinary();
string log = "";
string aoc = args[0];
var urls = Resource.Load("urls.json").ParseJson<Dictionary<string, string?>>()!;

// Install Cethleann
await urls["cethleann"]!.DownloadAndExtractAsync(".\\Cethleann", "Cethleann");

if (!string.IsNullOrEmpty(urls["cethleann-patch"])) {
    // Install Cethleann patch
    await urls["cethleann-patch"]!.DownloadAndExtractAsync(".\\Cethleann", "Cethleann Patch");
    }

// Merge directories
string[] dirs = Directory.GetDirectories(aoc);
if (dirs.Select(x => Path.GetFileName(x)).Contains("01002B00111A2000")) {
    Console.WriteLine("Merging AoC directories. . .");
    foreach (var dir in dirs.Where(x => Path.GetFileName(x).StartsWith("01002B00111A"))) {
        DirectoryExt.Copy(dir + "\\romfs\\asset", ".\\romfs\\asset", true);
        }
    aoc = ".\\romfs";
    }

// Run Cethleann extractor
Console.WriteLine("Extracting RDB Archives. . .");
Directory.CreateDirectory(".\\extracted-rdb");
Process.Start($".\\Cethleann\\Cethleann.DataExporter.exe", $"--nyotengu \".\\extracted-rdb\" \"{aoc}\\asset\"").WaitForExit();

// Load hash list
Console.WriteLine("Loading Hash List. . .");
string[] hashList = Resource.Load("hash-list").ToString().Split("\n");

// Patch g1t textures
Console.WriteLine("Patching g1t textures. . .");
Parallel.ForEach(hashList, hashMap => {
    try {
        string[] hashes = hashMap.Split(" ");
        string model = hashes[0];
        string ktid = hashes[1];
        string kidsobjdb = hashes[2];
        string folder = kidsobjdb.StartsWith("CharacterEditor") ? "CharacterEditor" : "FieldEditor4";

        Directory.CreateDirectory($".\\extracted-rdb\\{folder}\\merged\\{model}");
        File.Copy($".\\extracted-rdb\\{folder}\\g1m\\{model}.g1m", $".\\extracted-rdb\\{folder}\\merged\\{model}\\{model}.g1m", true);
        Process.Start($".\\Cethleann\\Nyotengu.KTID.exe", $".\\extracted-rdb\\KIDSSystemResource\\kidsobjdb\\{kidsobjdb} .\\extracted-rdb\\MaterialEditor\\g1t .\\extracted-rdb\\{folder}\\ktid\\{ktid}.ktid").WaitForExit();
        File.Move($".\\extracted-rdb\\{folder}\\ktid\\{ktid}.g1t", $".\\extracted-rdb\\{folder}\\merged\\{model}\\{ktid}.g1t", true);
    }
    catch (Exception ex) {
        string exs = $"[{DateTime.Now}] {ex}\n";
        log += exs;
        Console.WriteLine(exs);
    }
});

File.WriteAllText($".\\log-{timeStamp}.txt", log);

// Cleanup

// Optional: download/install Noesis