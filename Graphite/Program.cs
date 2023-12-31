﻿using System;
using System.IO;
using System.Net;
using System.IO.Compression;

namespace Graphite
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ///<summary>
            /// dyn :: dynamic
            /// sta :: static
            /// g2  :: graphite 2 / gen 2
            /// gih :: graphite hive installer
            /// </summary>

            if (File.Exists(".\\updateService.exe"))
            {
                File.Delete(".\\updateService.exe");
            }

            if (args.Length == 0) // detect for args
            {
                Console.WriteLine("No args! Run \"graphite help\" for more info");
                return;
            }

            Funcs f = new Funcs();
            WebClient wc = new WebClient();

            if (args[0] == "help")
            {
                Console.WriteLine("get [ pack ] [ destination ]");
                Console.WriteLine("   Locates the index file at the specified URL (pack) and downloads all files to destination.\n");
                Console.WriteLine("del [ pack ] [ destination ]");
                Console.WriteLine("   Locates the index file at the specified URL (pack) and removes all local files included in the index at destination.\n");
                Console.WriteLine("netdel [ path ] [ destination ]");
                Console.WriteLine("   Removes all files at destination included in the .npkg located at path.\n");
                Console.WriteLine("index [ path ] [ destination ]");
                Console.WriteLine("   Indexes path, creating a new Graphite Installation Hive at destination\n");
                Console.WriteLine("pack [ dir ] [ packname ] [ destination ]");
                Console.WriteLine("   Creates a new static .gpkg file at destination with the package included.\n");
                Console.WriteLine("netpack [ path ] [ packname ] [ destination ]");
                Console.WriteLine("   Creates a new dynamic .npkg pack from path at destination that will always extract the latest copy.\n");
                Console.WriteLine("unpack [ dir ] [ destination ]");
                Console.WriteLine("   Unpacks the static .gpkg file at dir to the destination.\n");
                Console.WriteLine("netunpack [ path ] [ destination ]");
                Console.WriteLine("   Extracts the dynamic .npkg from path to destination.\n");
                Console.WriteLine("update");
                Console.WriteLine("   Updates and reloads Graphite.\n");
                Console.WriteLine("info");
                Console.WriteLine("   Displays info about Graphite.\n");
                return;
            }

            if (args[0] == "get") // legacy gih get sys, dyn
            {
                if (args.Length != 3)
                {
                    Console.WriteLine("Supply args!");
                    Console.WriteLine("get [ pack ] [ destination ]");
                    Console.WriteLine("   Locates the index file at the specified URL (pack) and downloads all files to destination.\n");
                    return;
                }

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
                    File.Move(line, args[2]);
                    Console.WriteLine("\"" + line + "\" processed\n");
                }
                Console.WriteLine("Packages processed");
                return;
            }

            if (args[0] == "del") // legacy del, dyn
            {
                if (args.Length != 3)
                {
                    Console.WriteLine("Supply args!");
                    Console.WriteLine("del [ pack ] [ destination ]");
                    Console.WriteLine("   Locates the index file at the specified URL (pack) and removes all local files included in the index at destination.\n");
                    return;
                }

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
                    File.Delete(args[2] + "\\" + line);
                    Console.WriteLine("\"" + line + "\" removed\n");
                }
                Console.WriteLine("Packages removed");
                return;
            }

            if (args[0] == "index") // legacy index for gih sys, dyn
            {
                if (args.Length != 3)
                {
                    Console.WriteLine("Supply args!");
                    Console.WriteLine("index [ path ] [ destination ]");
                    Console.WriteLine("   Indexes path, creating a new Graphite Installation Hive at destination\n");
                    return;
                }

                try // error prone, so try-catch
                {
                    string[] files = Directory.GetFiles(args[1]);
                    using (StreamWriter sw = new StreamWriter(args[2] + "\\graphite.gih"))
                    {
                        foreach (string file in files)
                        {
                            string fileName = Path.GetFileName(file);
                            sw.WriteLine(fileName);
                        }
                        Console.WriteLine("New static Graphite Installation Hive created at \"" + args[2] + "\\graphite.gih\"");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                return;
            }

            if (args[0] == "pack") // g2 packing, dir > zip, sta
            {
                if (args.Length != 4)
                {
                    Console.WriteLine("Supply args!");
                    Console.WriteLine("pack [ dir ] [ packname ] [ destination ]");
                    Console.WriteLine("   Creates a new static .gpkg file at destination with the package included.\n");
                    return;
                }

                ZipFile.CreateFromDirectory(args[1], args[2] + ".gpkg");
                File.Move(args[2], args[3]);
                Console.WriteLine("Created new .gpkg file at \"" + args[3] + ".gpkg" + "\"");
                return;
            }

            if (args[0] == "unpack") // g2 unpacking, zip > dir, sta
            {
                if (args.Length != 3)
                {
                    Console.WriteLine("Supply args!");
                    Console.WriteLine("unpack [ dir ] [ destination ]");
                    Console.WriteLine("   Unpacks the static .gpkg file at dir to the destination.\n");
                    return;
                }

                ZipFile.ExtractToDirectory(args[1], args[2]);
                Console.WriteLine("Unpacked \"" + args[1] + "\" to \"" + Environment.CurrentDirectory + "\\" + Path.GetFileNameWithoutExtension(args[1]) + "\"");
                return;
            }

            if (args[0] == "netpack") // g2 index, dyn
            {
                if (args.Length != 4)
                {
                    Console.WriteLine("Supply args!");
                    Console.WriteLine("netpack [ path ] [ packname ] [ destination ]");
                    Console.WriteLine("   Creates a new dynamic .npkg pack from path at destination that will always extract the latest copy.\n");
                    return;
                }

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
                File.Move(args[2] + ".npkg", args[3]);
                return;
            }

            if (args[0] == "netunpack") // g2 get, dyn
            {
                if (args.Length != 3)
                {
                    Console.WriteLine("Supply args!");
                    Console.WriteLine("netunpack [ path ] [ destination ]");
                    Console.WriteLine("   Extracts the dynamic .npkg from path to destination.\n");
                    return;
                }

                Console.WriteLine("Decompressing .npkg file...");
                f.DecompressFileGZip(args[1], Path.GetFileNameWithoutExtension(args[1]) + ".tmp");
                string url = File.ReadAllText(Path.GetFileNameWithoutExtension(args[1]) + ".tmp");
                Console.WriteLine("Decompressed");

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
                    File.Move(line, args[2]);
                }
                File.Delete(args[1] + ".tmp");
                Console.WriteLine("Packages processed");
                return;
            }

            if (args[0] == "netdel") // g2 del, dyn
            {
                if (args.Length != 3)
                {
                    Console.WriteLine("Supply args!");
                    Console.WriteLine("netdel [ path ] [ destination ]");
                    Console.WriteLine("   Removes all files at destination included in the .npkg located at path.\n");
                    return;
                }

                f.DecompressFileGZip(args[1], Environment.CurrentDirectory + "\\" + Path.GetFileNameWithoutExtension(args[1]) + ".tmp");

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
                    File.Delete(args[2] + "\\" + line);
                    Console.WriteLine("\"" + line + "\" removed\n");
                }
                Console.WriteLine("Packages removed");
                return;
            }

            if (args[0] == "update") // net update with Grpahite-WebUpdateService
            {
                Console.WriteLine("Getting Update Service...");
                wc.DownloadFile("https://github.com/Dismalitie/Graphite/raw/main/Graphite-WebUpdateService/bin/Debug/Graphite-WebUpdateService.exe", ".\\updateService.exe");
                Console.WriteLine("Starting Update Service...");
                System.Diagnostics.Process.Start(".\\updateService.exe");
                return;
            }

            if (args[0] == "info")
            {
                Console.Write("Graphite is an open source, basic package management tool that lets you get, create and remove static and dynamic packages.");
            }
        }
    }
}
