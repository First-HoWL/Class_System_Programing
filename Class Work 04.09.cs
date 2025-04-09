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
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Game
{

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
        public ConsoleWindow(Point from, Point to, object lockObj, object lockMessages,  ConsoleColor ForegroundColor = ConsoleColor.White, ConsoleColor BackgroundColor = ConsoleColor.Black)
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
            lock (lockObj) { 
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
                for (int j = from.X - 1; j < to.X +1; j++)
                {
                    Console.SetCursorPosition(j, from.Y - 1);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write(" ");
                    Console.ResetColor();
                }
                for (int j = from.X  - 1; j < to.X + 2; j++)
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
                        for (int h = 0; h < messages.Length; h++) { 
                            if (i + 2 == to.X)
                            {
                                i = 0;
                                j++;
                            }
                            if (messages[h] == '\n')
                            {
                                i = 0;
                                j++;
                            }
                            else { 
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
            lock (lockObj) { 
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

        public void WriteLine(string message)
        {
            lock (lockMessages)
            {
                if (text.Count() > to.Y - from.Y)
                {
                    text.Remove(text[0]);
                }
                while (message.Length > to.X - from.X) { 
                    string a = "";
                    for (int i = 0; i < to.X - from.X; i++)
                    {
                        a += message[i];
                        message.Remove(message[i]);
                    }
                    text.Add(a);
                }
                
                text.Add(message + "\n");
                
               

            }
        }
        public void Write(string message)
        {
            lock (lockMessages)
            {
                if (text.Count() > to.Y - from.Y)
                {
                    text.Remove(text[0]);

                }
                while (message.Length > to.X - from.X)
                {
                    string a = "";
                    for (int i = 0; i < to.X - from.X; i++)
                    {
                        a += message[0];
                        message = message.Remove(0, 1);
                    }
                    
                    text.Add(a);
                    a = "";
                }
                if (text.Last().Length + message.Length < to.X - from.X)
                {
                    text[text.Count() - 1] = text.Last() + message;
                }
                else { 
                    text.Add(message);
                }
            }
        }
    }

    class Program
    {
        static readonly object lockObj = new object();
        static readonly object lockMessages = new object();
        static Random rnd = new Random();
        static List<Thread> list_threads = new List<Thread>();
        static SemaphoreSlim semaphore = new SemaphoreSlim(3, 3);
        static List<string> TextToConsole = new List<string>();
        static List<ConsoleWindow> Windows = new List<ConsoleWindow>{
            new ConsoleWindow(new Point(3,3), new Point(20, 15), lockObj, lockMessages, ConsoleColor.Blue, ConsoleColor.Gray),
            new ConsoleWindow(new Point(25,3), new Point(40, 15), lockObj, lockMessages, ConsoleColor.Green, ConsoleColor.Yellow)

        };

        public static void Count(ConsoleWindow a, int b)
        {
            lock (lockObj)
            {
                for (int i = 0; i < b; i++)
                    a.WriteLine(i.ToString());
            }
        }
        public static void Main(string[] args)
        {

            Windows[0].WriteLine("aa");
            Windows[1].WriteLine("aa");

            Parallel.Invoke(
                () => {
                    Windows[0].DrawBackground();
                    while (true)
                    {
                        Windows[0].Draw();
                        Thread.Sleep(700);
                    }
                },
                () => {
                    Windows[1].DrawBackground();
                    while (true)
                    {
                        Windows[1].Draw();
                        Thread.Sleep(700);
                    }
                },
                () => 
                {   while (true) {
                        string a = "";
                        string b = "";
                        ConsoleKey key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.C)
                            {
                            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!");
                            while (key != ConsoleKey.Enter) 
                            {
                                key = Console.ReadKey(true).Key;
                                if (key == ConsoleKey.Enter)
                                    break;
                                Console.WriteLine("AAAAAAAAAAAAAAAA");
                                a += key.ToString().Last();
                                Console.WriteLine(a);
                            }
                            key = Console.ReadKey(true).Key;
                            b += key.ToString().Last();
                            Console.WriteLine(b);
                            Count(Windows[Convert.ToInt32(b)], Convert.ToInt32(a));
                        }
                    }
                    

                }
                );


        }
    }
}
