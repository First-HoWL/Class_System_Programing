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
using Org.BouncyCastle.Asn1.Cms;

namespace Game
{


    
    class Program
    {
        static readonly object lockObj = new object();
        static readonly object lockMessages = new object();
        static Random rnd = new Random();
        static List<Thread> list_threads = new List<Thread>();
        static SemaphoreSlim semaphore = new SemaphoreSlim(3, 3);
        static List<string> TextToConsole = new List<string>();

        static void ProcessPeople(object Name)
        {
            Console.WriteLine($"Men {Name,4} | {"waiting before going on stadium",35} | {semaphore.CurrentCount} places");
            semaphore.Wait();
            int a = rnd.Next(3000, 8000);
            Console.WriteLine($"Men {Name,4} | {$" | {a / 1000,7} s. | is on stadium",35} | {semaphore.CurrentCount} places");
            Thread.Sleep(a);
            Console.Write($"Men {Name,4} | {"is going from stadium",35} | ");
            semaphore.Release();
            Console.WriteLine($"{semaphore.CurrentCount} places");

        }

        static void code(int a)
        {
            string b = "";
            for (int i = 1; i < 11; i++)
            {
                b += $"{i * a,3} | ";
            }
            b += "\b\b ";
            Console.WriteLine(b);
        }

        static int sum(List<int> list)
        {
            int a = 0;
            for(int i = 0; i < list.Count(); i++)
            {
                a += list[i];
            }
            return a;
        }

        static double avg(List<int> list)
        {
            double a = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                a += list[i];
            }
            return a / list.Count();
        }
        static int min(List<int> list)
        {
            int a = list[0];
            for (int i = 0; i < list.Count(); i++)
            {
                if (a > list[i])
                {
                    a = list[i];
                } 
            }
            return a;
        }
        static int max(List<int> list)
        {
            int a = list[0];
            for (int i = 0; i < list.Count(); i++)
            {
                if (a < list[i])
                {
                    a = list[i];
                }
            }
            return a;
        }

        public static void Count()
        {
            lock (lockObj)
            {
                Console.SetCursorPosition(40, 0);
                Console.Write(rnd.Next(1, 10));
            }
        }

        public static void OutputWindow()
        {
            lock(lockObj)
                lock (lockMessages)
                {
                    


                    int i = 0;
                    foreach(var messages in TextToConsole)
                    {
                        Console.SetCursorPosition(55, i++);
                        Console.WriteLine(messages);
                    }
                }
        }

        public static uint Menu(IEnumerable<string> Action)
        {
            uint active = 0;
            while (true)
            {
                lock (lockObj)
                {
                    Console.SetCursorPosition(0, 0);
                    for (int i = 0; i < Action.Count(); i++)
                    {
                     
                        if (i == active)
                            Console.WriteLine($" > {Action.ElementAt(i)}");
                        else
                            Console.WriteLine($"   {Action.ElementAt(i)}");
                    }
                }

                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (active > 0 && (key == ConsoleKey.UpArrow || key == ConsoleKey.W))
                        active--;
                    else if ((key == ConsoleKey.DownArrow || key == ConsoleKey.S) && active < Action.Count() - 1)
                        active++;
                    else if (key == ConsoleKey.Enter)
                    {
                        //Console.Clear();
                        return active;
                    }
                }
            }
        }

        public static void AddMessage(string message)
        {
            lock (lockMessages)
            {
                if (TextToConsole.Count() > 19)
                {
                    TextToConsole.Remove(TextToConsole[0]);
                    Console.Clear();
                    
                }
                TextToConsole.Add(message);
            }
        }

        static void ProcessCar(object CarName)
        {
            AddMessage($"Car {CarName,7} | waiting before going on gas station");
            semaphore.Wait();
            int a = rnd.Next(3000, 8000);
            AddMessage($"Car {CarName,7} | {a / 1000,7} s. | is on gas station");
            Thread.Sleep(a);
            AddMessage($"Car {CarName,7} | is going from gas station");
            semaphore.Release();

        }
        static int numb = 1;

        static void start(int a)
        {
            Parallel.For(0, a, i => ProcessCar($"N{numb++}"));
            
        }

        public static void Main(string[] args)
        {
            Console.CursorVisible = false;
            List<string> Action = new List<string> { 
                "Print \"Hello\"",
                "Add 1 car",
                "Add 5 car",
                "Add 10 car",
                "Exit"
            };
            Parallel.Invoke(
                () => {
                    while (true)
                    {
                        switch (Menu(Action))
                        {
                            case 0: AddMessage("Hello!"); break;
                            case 1:
                                Thread a = new Thread(() => start(1)); a.Start(); break;
                            case 2:
                                Thread b = new Thread(() => start(5)); b.Start(); break;
                            case 3:
                                Thread c = new Thread(() => start(10)); c.Start(); break;
                            case 4: Environment.Exit(0); break;
                            // Menu(new List<string> { "Back", "Another back" })
                        }
                    }
                },
                () => {
                    while (true) { Count(); }
                },
                () =>{
                    while (true) OutputWindow();
                },
                () =>
                {
                    Parallel.For(0, 5, i => ProcessCar($"N{numb++}"));
                }
            );
        }
    }
}
