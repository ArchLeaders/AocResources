﻿using AocModelExtractor;
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

// Download Cethleann
Console.WriteLine("Downloading Cethleann. . .");
byte[] cethleann = await urls["cethleann"]!.DownloadAsync();

// Extract Cethleann
Console.WriteLine("Extracting Cethleann. . .");
using ZipArchive arc = new(new MemoryStream(cethleann));
foreach (var entry in arc.Entries) {
    string file = $".\\Cethleann\\{entry.FullName}";
    if (entry.Length > 0) {
        entry.ExtractToFile(file, true);
    }
    else {
        Directory.CreateDirectory(file);
    }
}

if (!string.IsNullOrEmpty(urls["cethleann-patch"])) {

    // Download Cethleann patch
    Console.WriteLine("Downloading Cethleann Patch. . .");
    byte[] patch = await urls["cethleann-patch"]!.DownloadAsync();

    // Extract Cethleann patch
    Console.WriteLine("Extracting Cethleann Patch. . .");
    using ZipArchive patchArc = new(new MemoryStream(patch));
    foreach (var entry in patchArc.Entries) {
        string file = $".\\Cethleann\\{entry.FullName}";
        entry.ExtractToFile(file, true);
    }
}


// Run Cethleann extractor
Console.WriteLine("Extracting RDB Archives. . . (this might take a while)");
Directory.CreateDirectory(".\\extracted-rdb");
Process.Start($".\\Cethleann\\Cethleann.DataExporter.exe", $"--nyotengu \".\\extracted-rdb\" \"{args[0]}\\asset\"").WaitForExit();

// Load hash list
Console.WriteLine("Loading Hash List. . .");
string[] hashList = Resource.Load("hash-list").ToString().Split("\n");

// Patch g1t textures
Console.WriteLine("Patching g1t textures. . . (this might take a while)");
Parallel.ForEach(hashList, hashMap => {
    try {
        string[] hashes = hashMap.Split(" ");
        string model = hashes[0];
        string ktid = hashes[1];
        string kidsobjdb = hashes[2];
        string folder = kidsobjdb.StartsWith("CharacterEditor") ? "CharacterEditor" : "FieldEditor4";

        File.Copy($".\\extracted-rdb\\{folder}\\g1m\\{model}.g1m", $".\\extracted-rdb\\{folder}\\ktid\\{model}.g1m", true);
        Process.Start($".\\Cethleann\\Nyotengu.KTID.exe", $".\\extracted-rdb\\KIDSSystemResource\\kidsobjdb\\{kidsobjdb} .\\extracted-rdb\\MaterialEditor\\g1t .\\extracted-rdb\\{folder}\\ktid\\{ktid}.ktid").WaitForExit();
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