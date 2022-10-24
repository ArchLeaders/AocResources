using System.Data.SqlTypes;

namespace AocModelExtractor.Extensions
{
    public static class DirectoryExt
    {
        public static void Copy(string srcDir, string dstDir, bool overwriteFiles = false)
        {
            if (Directory.Exists(srcDir)) {
                Parallel.ForEach(Directory.EnumerateFiles(srcDir), file => {
                    string dstFile = file.Replace(srcDir, dstDir);
                    Directory.CreateDirectory(Path.GetDirectoryName(dstFile) ?? "");
                    File.Copy(file, dstFile, overwriteFiles);
                });
            }
        }
    }
}
