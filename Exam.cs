using System.Text;
using System.Threading;
using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

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
    class ConsoleWindow
    {
        public Point from;
        public Point to;
        public ConsoleColor ForegroundColor;
        public ConsoleColor BackgroundColor;
        List<string> text = new List<string>();
        public object lockObj;
        public object lockMessages;
        public bool Boards { get; set; }
        public ConsoleWindow(Point from, Point to, object lockObj, object lockMessages, ConsoleColor ForegroundColor = ConsoleColor.White, ConsoleColor BackgroundColor = ConsoleColor.Black)
        {
            this.from = from;
            this.to = to;
            this.ForegroundColor = ForegroundColor;
            this.BackgroundColor = BackgroundColor;
            this.lockObj = lockObj;
            this.lockMessages = lockMessages;
        }

        public void DrawBoards()
        {
            lock (lockObj)
            {
                for (int i = from.Y - 1; i < to.Y; i++)
                {
                    Console.SetCursorPosition(from.X - 1, i);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" ");
                    Console.ResetColor();
                }
                for (int i = from.Y - 1; i < to.Y + 0; i++)
                {
                    Console.SetCursorPosition(to.X + 1, i);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" ");
                    Console.ResetColor();
                }
                for (int j = from.X - 1; j < to.X + 1; j++)
                {
                    Console.SetCursorPosition(j, from.Y - 1);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" ");
                    Console.ResetColor();
                }
                for (int j = from.X - 1; j < to.X + 2; j++)
                {
                    Console.SetCursorPosition(j, to.Y);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" ");
                    Console.ResetColor();
                }
            }
        }

        public void Draw()
        {
            lock (lockObj)
                lock (lockMessages)
                {
                    int i = 0;
                    int j = 0;
                    if (Boards == true)
                        DrawBoards();
                    foreach (var messages in text)
                    {
                        Console.BackgroundColor = BackgroundColor;
                        Console.ForegroundColor = ForegroundColor;
                        for (int h = 0; h < messages.Length; h++)
                        {
                            if (i == to.X - from.X)
                            {
                                i = 0;
                                j++;
                            }
                            
                            if (messages[h] == '\n')
                            {
                                i = 0;
                                j++;
                            }
                            else
                            {
                                Console.SetCursorPosition(from.X + i++, from.Y + j);
                                Console.Write(messages[h]);
                            }
                        }
                        Console.ResetColor();
                    }
                }

        }

        public void DrawBackground()
        {
            lock (lockObj)
            {
                for (int i = from.Y; i < to.Y; i++)
                {
                    for (int j = from.X; j < to.X + 1; j++)
                    {
                        Console.SetCursorPosition(j, i);
                        Console.BackgroundColor = BackgroundColor;
                        Console.Write(" ");
                        Console.ResetColor();
                    }
                }
            }
        }

        public void Clear()
        {
            lock (lockObj)
                lock (lockMessages) {
                    text.Clear();
                    DrawBackground();
                }
        }

        public void WriteLine(string message)
        {
            lock (lockMessages)
            {
                if (text.Count() >= to.Y - from.Y - 1)
                {
                    text.Remove(text[0]);
                }
                while (message.Length > to.X - from.X)
                {
                    if (text.Count() >= to.Y - from.Y - 1)
                    {
                        text.Remove(text[0]);
                    }
                    string a = "";
                    for (int i = 0; i < to.X - from.X; i++)
                    {
                        a += message[0];
                        message = message.Remove(0, 1);
                    }
                    text.Add(a);
                    a = "";
                }

                text.Add(message + "\n");
            }
        }

        public void Write(string message)
        {
            lock (lockMessages)
            {
                
                while (message.Length > to.X - from.X)
                {
                    if (text.Count() > to.Y - from.Y)
                    {
                        text.Remove(text[0]);

                    }
                    string a = "";
                    for (int i = 0; i < to.X - from.X; i++)
                    {
                        a += message[0];
                        message = message.Remove(0, 1);
                    }

                    text.Add(a);
                    a = "";
                }
                if (text.Last().Length + message.Length < to.X - from.X && text.Last()[text.Last().Length - 1] != '\n')
                {
                    text[text.Count() - 1] = text.Last() + message;
                }
                else
                {
                    text.Add(message);
                }
            }
        }
    }
    class SmartBackup
    {
        private List<BackupDirectory> directories = new();
        private SemaphoreSlim copySemaphore;
        private int maxConcurrentCopies = 2;
        static readonly object lockObj = new object();
        static readonly object lockMessages = new object();
        static ConsoleWindow Window = new ConsoleWindow(new Point(40, 0), new Point(100, 20), lockObj, new object(), ConsoleColor.White, ConsoleColor.Green);
        static ConsoleWindow Window2 = new ConsoleWindow(new Point(0, 0), new Point(38, 20), lockObj, new object(), ConsoleColor.Black, ConsoleColor.Yellow);


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
            if (directories[index].Mode == BackupMode.OnChange)
                directories[index].Watcher.Dispose();
            else
                directories[index].Timer.Dispose();
            directories.RemoveAt(index);
            return true;
        }
        public bool EditDirectory(int index)
        {
            string str = "";
            string text = "";
            
            Window2.WriteLine("\nЩо ви хочете змінити?");
            Window2.WriteLine("1. Директорію з якої копіюєтся");
            Window2.WriteLine("2. Директорію в яку копіюєтся");
            Window2.WriteLine("3. Режим (1 - регулярний, 2 - при зміні)");

            Window2.Write(">> ");
            str = Console.ReadLine();
            

            Window2.Write("\n>> ");
            text = Console.ReadLine();
            var dir = directories[index];
            directories.RemoveAt(index);

            switch (str)
            {
                case "1":
                    {
                        AddDirectory(text, dir.DestinationPath, dir.Mode);
                        break;
                    }
                case "2":
                    {
                        AddDirectory(dir.SourcePath, text, dir.Mode);
                        break;
                    }
                case "3":
                    {
                        int interval = 60;
                        if (Convert.ToInt32(text) == 1)
                        {
                            Window2.Write("\nІнтервал у секундах: ");
                            interval = Convert.ToInt32(Console.ReadLine());
                        }
                        AddDirectory(dir.SourcePath, dir.DestinationPath, (BackupMode)(Convert.ToInt32(text) - 1), interval);
                        break;
                    }
            }
            Window2.Clear();
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
                Window.WriteLine($"Copied: {filePath} -> {destPath}");
            }
            catch (Exception ex)
            {
                Window.WriteLine($"Error copying {filePath}: {ex.Message}");
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
                string input;
                Window2.WriteLine("SmartBackup Menu");
                Window2.WriteLine("1. Додати директорію");
                Window2.WriteLine("2. Переглянути директорії");
                Window2.WriteLine("3. Видалити директорію");
                Window2.WriteLine("4. Змінити директорію");
                Window2.WriteLine("5. Вийти");
                Window2.Write("Оберіть опцію: ");
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        int mode = 0;
                        string src = "";
                        string dst = "";
                        Window2.Write("\nШлях до директорії: ");
                        src = Console.ReadLine();
                        Window2.Write("\nКуди зберігати копії: ");
                        dst = Console.ReadLine();
                        Window2.Write("\nРежим (1 - регулярний, 2 - при зміні): ");
                        mode = Convert.ToInt32(Console.ReadLine());
                        
                        int interval = 60;
                        if (mode == 1)
                        {
                            Window2.Write("\nІнтервал у секундах: ");
                            interval =Convert.ToInt32(Console.ReadLine());
                        }
                        AddDirectory(src, dst, (BackupMode)(mode - 1), interval);
                        Window2.Clear();
                        break;
                    case "2":
                        Window2.Clear();
                        int i = 0;
                        foreach (var d in directories)
                        {
                            Window2.WriteLine($"{i++}. {d.SourcePath} -> {d.DestinationPath} | Mode: {d.Mode}");
                        }
                        Window2.WriteLine("");
                        break;
                    case "3":
                        Window2.WriteLine("\nЯку директорію");
                        Window2.Write("\n>> ");
                        int numb = Convert.ToInt32(Console.ReadLine());
                        Window2.Clear();
                        
                        if (DeleteDirectory(numb) == false)
                            Window2.WriteLine("\nНевірна опція.\n");
                        else
                            Window2.WriteLine("Директорія видалена!\n");
                        break;
                    case "4":
                        Window2.WriteLine("\nЯку директорію");
                        Window2.Write("\n>> ");
                        int number = Convert.ToInt32(Console.ReadLine());
                        if (EditDirectory(number) == false)
                            Window2.WriteLine("\nНевірна опція.\n");
                        else
                            Window2.WriteLine("\nДиректорія змінена!\n");

                        break;
                    case "5":
                        return;
                    default:
                        Window2.WriteLine("\nНевірна опція.");
                        break;
                }
            }
        }

        static void Main()
        {
            Console.InputEncoding = UTF8Encoding.UTF8;
            Console.OutputEncoding = UTF8Encoding.UTF8;
            SmartBackup app = new SmartBackup();
            Parallel.Invoke(
                () =>
                {
                    Window.DrawBackground();
                    while (true)
                    {
                        Window.DrawBackground();
                        Window.Draw();
                        lock (lockObj)
                        {
                            Console.SetCursorPosition(0, 25);
                        }
                        Thread.Sleep(1400);
                    }
                },
                () =>
                {
                    Window2.DrawBackground();
                    while (true)
                    {
                        Window2.Draw(); 
                        lock (lockObj)
                        {
                            Console.SetCursorPosition(0, 25);
                        }
                        Thread.Sleep(700);
                    }
                },
                () =>
                {
                    app.ShowMenu();
                }
                );

            
        }
    }
}
