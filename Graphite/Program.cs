using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace Graphite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("No args! Run \"graphite help\" for more info");
                return;
            }

            Console.Write("\r\n█▀▀ █▀█ ▄▀█ █▀█ █░█ █ ▀█▀ █▀▀\r\n█▄█ █▀▄ █▀█ █▀▀ █▀█ █ ░█░ ██▄\n");
            Console.WriteLine("Batch Package Installer\n");

            if (args[0] == "get")
            {
                WebClient wc = new WebClient();
                string indexfile = args[1];
                Console.WriteLine("Pulling index...\n");
                string[] assets = null;
                try
                {
                    assets = wc.DownloadString(indexfile).Split('\n');
                }
                catch
                {
                    Console.WriteLine("Index not found!");
                    return;
                }
                foreach (string line in assets)
                {
                    if (line == "") { return; }
                    Console.WriteLine("Getting \"" + line + "\"...");
                    wc.DownloadFile(args[1] + "/" + line, line);
                    Console.WriteLine("\"" + line + "\" processed\n");
                }
                Console.WriteLine("Packages processed");
            }

            if (args[0] == "del")
            {
                WebClient wc = new WebClient();
                string indexfile = args[1];
                Console.WriteLine("Pulling index...\n");
                string[] assets = null;
                try
                {
                    assets = wc.DownloadString(indexfile).Split('\n');
                }
                catch
                {
                    Console.WriteLine("Index not found!");
                    return;
                }
                foreach (string line in assets)
                {
                    if (line == "") { return; }
                    Console.WriteLine("Removing \"" + line + "\"...");
                    File.Delete(Environment.CurrentDirectory + "\\" + line);
                    Console.WriteLine("\"" + line + "\" removed\n");
                }
                Console.WriteLine("Packages removed");
            }

            if (args[0] == "index")
            {
                try
                {
                    string[] files = Directory.GetFiles(Environment.CurrentDirectory);
                    using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "\\graphite.gih"))
                    {
                        foreach (string file in files)
                        {
                            string fileName = Path.GetFileName(file);
                            sw.WriteLine(fileName);
                        }
                        Console.WriteLine("New static Graphite Installation Hive created at \"" + Environment.CurrentDirectory + "\\graphite\"");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            if (args[0] == "help")
            {
                Console.WriteLine("get [ path ]");
                Console.WriteLine("   Locates the index file at the specified URL and downloads all files.\n");
                Console.WriteLine("del [ path ]");
                Console.WriteLine("   Locates the index file at the specified URL and removes all local files included in the index.\n");
                Console.WriteLine("netdel [ path ]");
                Console.WriteLine("   Removes all files in the current directory included in the .npkg located at path.\n");
                Console.WriteLine("index");
                Console.WriteLine("   Creates a new Graphite Installation Hive.\n");
                Console.WriteLine("pack [ dir ] [ packname ]");
                Console.WriteLine("   Creates a new static .gpkg file with the package included.\n");
                Console.WriteLine("netpack [ path ] [ packname ]");
                Console.WriteLine("   Creates a new dynamic .npkg pack that will always extract the latest copy.\n");
                Console.WriteLine("unpack [ dir ]");
                Console.WriteLine("   Unpacks the static .gpkg file at dir to the current directory.\n");
                Console.WriteLine("netunpack [ path ]");
                Console.WriteLine("   Extracts dynamic .npkg files the the current directory.\n");
            }

            if (args[0] == "pack")
            {
                ZipFile.CreateFromDirectory(args[1], Environment.CurrentDirectory + "\\" + args[2] + ".gpkg");
                Console.WriteLine("Created new .gpkg file at \"" + Environment.CurrentDirectory + "\\" + args[2] + ".gpkg" + "\"");
            }

            if (args[0] == "unpack")
            {
                ZipFile.ExtractToDirectory(args[1], Environment.CurrentDirectory + "\\" + Path.GetFileNameWithoutExtension(args[1]));
                Console.WriteLine("Unpacked \"" + args[1] + "\" to \"" + Environment.CurrentDirectory + "\\" + Path.GetFileNameWithoutExtension(args[1]) + "\"");
            }

            if (args[0] == "netpack")
            {
                Funcs f = new Funcs();
                Console.WriteLine("Writing address...");
                StreamWriter sw = new StreamWriter(args[2] + ".tmp");
                sw.Write(args[1]);
                sw.Close();
                sw.Dispose();
                Console.WriteLine("Address written\n");
                f.CompressFileGZip(args[2] + ".tmp", args[2] + ".npkg");
                Console.WriteLine("Compressing...");
                File.Delete(args[2] + ".tmp");
                Console.WriteLine("Compressed");
            }

            if (args[0] == "netunpack")
            {
                Funcs f = new Funcs();
                Console.WriteLine("Decompressing .npkg file...");
                f.DecompressFileGZip(args[1], Path.GetFileNameWithoutExtension(args[1]) + ".tmp");
                string url = File.ReadAllText(Path.GetFileNameWithoutExtension(args[1]) + ".tmp");
                Console.WriteLine("Decompressed");

                WebClient wc = new WebClient();
                string indexfile = url;
                Console.WriteLine("Pulling index...\n");
                string[] assets = null;
                try
                {
                    assets = wc.DownloadString(indexfile).Split('\n');
                }
                catch
                {
                    Console.WriteLine("Index not found!");
                    return;
                }
                foreach (string line in assets)
                {
                    if (line == "") { return; }
                    Console.WriteLine("Getting \"" + line + "\"...");
                    wc.DownloadFile(url + "/" + line, line);
                    Console.WriteLine("\"" + line + "\" processed\n");
                }
                File.Delete(args[1] + ".tmp");
                Console.WriteLine("Packages processed");
            }

            if (args[0] == "netdel")
            {
                Funcs f = new Funcs();
                f.DecompressFileGZip(args[1], Environment.CurrentDirectory + "\\" + Path.GetFileNameWithoutExtension(args[1]) + ".tmp");

                WebClient wc = new WebClient();
                string indexfile = File.ReadAllText(Path.GetFileNameWithoutExtension(args[1]) + ".tmp");
                Console.WriteLine("Pulling index...\n");
                string[] assets = null;
                try
                {
                    assets = wc.DownloadString(indexfile).Split('\n');
                }
                catch
                {
                    Console.WriteLine("Index not found!");
                    return;
                }
                foreach (string line in assets)
                {
                    if (line == "") { return; }
                    Console.WriteLine("Removing \"" + line + "\"...");
                    File.Delete(Environment.CurrentDirectory + "\\" + line);
                    Console.WriteLine("\"" + line + "\" removed\n");
                }
                Console.WriteLine("Packages removed");
            }
        }
    }
}
