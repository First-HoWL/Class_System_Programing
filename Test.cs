using System.Text;
using System.Threading;
using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Game;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Game
{ 
    class BackupDirectory
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
        public BackupMode Mode { get; set; }
        public FileSystemWatcher Watcher { get; set; }
        public Timer Timer { get; set; }
        public int IntervalMs { get; set; }
    }

    enum BackupMode
    {
        Regular,
        OnChange
    }

    class SmartBackup
    {
        private List<BackupDirectory> directories = new();
        private SemaphoreSlim copySemaphore;
        private int maxConcurrentCopies = 2;

        public SmartBackup()
        {
            copySemaphore = new SemaphoreSlim(maxConcurrentCopies);
        }

        public void AddDirectory(string source, string destination, BackupMode mode, int intervalSeconds = 60)
        {
            var dir = new BackupDirectory
            {
                SourcePath = source,
                DestinationPath = destination,
                Mode = mode,
                IntervalMs = intervalSeconds * 1000
            };

            if (mode == BackupMode.OnChange)
            {
                var watcher = new FileSystemWatcher(source);
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;
                watcher.Changed += (s, e) => StartCopy(e.FullPath, destination);
                watcher.Created += (s, e) => StartCopy(e.FullPath, destination);
                watcher.Renamed += (s, e) => StartCopy(e.FullPath, destination);
                dir.Watcher = watcher;
            }
            else
            {
                Timer timer = new Timer(_ => StartDirectoryCopy(source, destination), null, 0, dir.IntervalMs);
                dir.Timer = timer;
            }

            directories.Add(dir);
        }

        public bool DeleteDirectory(int index)
        {
            if (index >= directories.Count() || index < 0)
                return false;

            directories.RemoveAt(index);
            return true;
        }

        private void StartCopy(string filePath, string destinationDir)
        {
            copySemaphore.Wait();
            try
            {
                if (!File.Exists(filePath)) return;
                string relativePath = Path.GetRelativePath(Path.GetDirectoryName(filePath), filePath);
                string destPath = Path.Combine(destinationDir, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                File.Copy(filePath, destPath, true);
                Console.WriteLine($"Copied: {filePath} -> {destPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying {filePath}: {ex.Message}");
            }
            copySemaphore.Release();
            
        }

        private void StartDirectoryCopy(string source, string destination)
        {
            foreach (var file in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
            {
                StartCopy(file, destination);
            }
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("=== SmartBackup Menu ===");
                Console.WriteLine("1. Додати директорію");
                Console.WriteLine("2. Переглянути директорії");
                Console.WriteLine("3. Видалити директорію");
                Console.WriteLine("4. Вийти");
                Console.Write("Оберіть опцію: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Write("Шлях до директорії: ");
                        string src = Console.ReadLine();
                        Console.Write("Куди зберігати копії: ");
                        string dst = Console.ReadLine();
                        Console.Write("Режим (1 - регулярний, 2 - при зміні): ");
                        int mode = int.Parse(Console.ReadLine());
                        int interval = 60;
                        if (mode == 1)
                        {
                            Console.Write("Інтервал у секундах: ");
                            interval = int.Parse(Console.ReadLine());
                        }
                        AddDirectory(src, dst, (BackupMode)(mode - 1), interval);
                        Console.Clear();
                        break;
                    case "2":
                        Console.Clear();
                        int i = 0;
                        foreach (var d in directories)
                        {
                            Console.WriteLine($"{i++}. {d.SourcePath} -> {d.DestinationPath} | Mode: {d.Mode}");
                        }
                        Console.WriteLine();
                        break;
                    case "3":
                        Console.WriteLine("Яку директорію");
                        Console.Write(">> ");
                        int numb = Convert.ToInt32(Console.ReadLine());
                        Console.Clear();
                        if (DeleteDirectory(numb) == false)
                            Console.WriteLine("Невірна опція.\n");
                        else
                            Console.WriteLine("Директорія видалена!\n");
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Невірна опція.");
                        break;
                }
            }
        }

        static void Main()
        {
            Console.InputEncoding = UTF8Encoding.UTF8;
            Console.OutputEncoding = UTF8Encoding.UTF8;
            SmartBackup app = new SmartBackup();
            app.ShowMenu();
        }
    }
}
