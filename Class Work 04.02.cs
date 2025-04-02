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


    
    class Program
    {
        static readonly object lockObj = new object();
        static Random rnd = new Random();
        static List<Thread> list_threads = new List<Thread>();
        static SemaphoreSlim semaphore = new SemaphoreSlim(1000, 1000);

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

        public static void Main(string[] args)
        {

            //Parallel.For(1, 11, i => code(i));
            //for(int i = 1; i < 11; i++) {code(i);}

            List<int> ints = new List<int>();

            for (int i = 0; i < 100000000; i++)
            {
                ints.Add(rnd.Next(1, 10000));
            }


            Parallel.Invoke(() => {
                Stopwatch sw = Stopwatch.StartNew();
                int a = sum(ints);
                sw.Stop();
                Console.WriteLine($"sum: {a, 20} | {sw.ElapsedMilliseconds}");
            },
            () => {
                Stopwatch sw1 = Stopwatch.StartNew();
                double b = avg(ints);
                sw1.Stop();
                Console.WriteLine($"avg: {b,20} | {sw1.ElapsedMilliseconds}");
            },
            () => {
                Stopwatch sw2 = Stopwatch.StartNew();
                int c = min(ints);
                sw2.Stop();
                Console.WriteLine($"min: {c,20} | {sw2.ElapsedMilliseconds}");
            },
            () => {
                Stopwatch sw3 = Stopwatch.StartNew();
                int d = max(ints);
                sw3.Stop();
                Console.WriteLine($"max: {d,20} | {sw3.ElapsedMilliseconds}");
            }
            );


            //Parallel.For(1, 11, a => { Parallel.For(1, 11, i => { Console.WriteLine($"[{a}] {a} * {i} = {a * i}"); }); });
            /*int Balance = 1000;

            Thread a = new Thread(() => Threads());
            a.Start();
            
            while (true)
            {
                foreach (var item in Info.ToArray())
                {
                    if (item != null) { 
                        if (item.Money >= 0)
                        {
                            Balance += item.Money;
                            Console.WriteLine($"Thread: {item.ThreadId,10} | {item.Money,8} | Balance: {Balance,8}");

                        }
                        else if (item.Money < 0 && Balance + item.Money >= 0)
                        {
                            Balance += item.Money;
                            Console.WriteLine($"Thread: {item.ThreadId,10} | {item.Money,8} | Balance: {Balance,8}");

                        }
                        else
                        {
                            Console.WriteLine($"Thread: {item.ThreadId,10} | {item.Money,8} | Not enought money!");

                        }
                        lock (lockObj)
                        {
                            Info.Remove(item);
                        }
                    }
                }
            }*/
            //int i = 00000;
            //Thread thread = new Thread(() => {
            //    while (true)
            //    {
            //        Console.ForegroundColor = ConsoleColor.Green;
            //        Console.WriteLine($"{1000 - semaphore.CurrentCount} people");
            //        Console.ResetColor();
            //        Thread.Sleep(10000);
            //    }
            //});

            //thread.Start();
            //while (true)
            //{
            //    /*for (int j = 0; j < rnd.Next(1, 30); j++)
            //    {
            //        Thread trainTread = new Thread(ProcessPeople);
            //        trainTread.Start($"{i++ + 1}");
            //    }*/
            //    Parallel.For(1, rnd.Next(1, 30), i => ProcessPeople($"{i + 1}"));
            //    Thread.Sleep(rnd.Next(100, 4000));
            //}
        }
    }
}
