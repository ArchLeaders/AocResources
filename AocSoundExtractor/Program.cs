using System.Diagnostics;

if (args.Length == 0) {
    Console.WriteLine("Please specify the path to your Age of Calamity game files. (ending in '\\romfs')");
    Console.ReadLine();
    return;
}

Stopwatch watch = Stopwatch.StartNew();

Directory.CreateDirectory("extracted-ktss");
Parallel.ForEach(Directory.EnumerateFiles($"{args[0]}\\asset\\data"), file => {
    byte[] data = File.ReadAllBytes(file);
    byte[] header = new byte[] { 0x4B, 0x54, 0x53, 0x53 };
    int pos = 0;
    bool found = false;
    for (int i = 4; i < data.Length; i += 4) {
        if (data[pos..i].SequenceEqual(header)) {
            found = true;
            break;
        }
        pos += 4;
    }

    string fname = Path.GetFileNameWithoutExtension(file);
    if (found) {
        File.WriteAllBytes($".\\extracted-ktss\\{Path.GetFileNameWithoutExtension(file)}.ktss", data[pos..]);
        Console.WriteLine($"Extracted {fname}");
    }
    else {
        Console.WriteLine($"Invalid KTSS file: {fname}");
    }
});

Console.WriteLine($"Extracted sound files in {watch.ElapsedMilliseconds / 1000.0} seconds ({watch.ElapsedMilliseconds}ms).");