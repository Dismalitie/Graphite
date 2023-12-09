using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphite
{
    internal class Funcs
    {
        // Sourced from GLib.CS
        public void DecompressFileGZip(string compressedFilePath, string decompressedFilePath)
        {
            using (FileStream compressedFileStream = new FileStream(compressedFilePath, FileMode.Open, FileAccess.Read))
            using (FileStream decompressedFileStream = File.Create(decompressedFilePath))
            using (GZipStream gzipStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
            {
                gzipStream.CopyTo(decompressedFileStream);
            }
        }

        public void CompressFileGZip(string sourceFilePath, string compressedFilePath)
        {
            using (FileStream sourceFileStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
            using (FileStream compressedFileStream = File.Create(compressedFilePath))
            using (GZipStream gzipStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
            {
                sourceFileStream.CopyTo(gzipStream);
            }
        }
    }
}
