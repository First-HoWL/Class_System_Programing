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


    class Car
    {
        public int id { get; set; }
        public int speed { get; set; }
        public int CurentSpeed { get; set; }
        public ConsoleColor color { get; set; }
        public int infelicity { get; set; }
        public double range { get; set; }

        public void NewSpeed()
        {
            Random random = new Random();
            if (random.Next(1, 3) == 1)
            {
                this.CurentSpeed = speed + random.Next(1, infelicity);
            }
            else
            {
                this.CurentSpeed = speed - random.Next(1, infelicity);
            }
        }

        public void Addrange()
        {
            this.range += ((this.CurentSpeed * 1000) / 7200);
        }

        public Car(int id, int speed, ConsoleColor color, int infelicity)
        {
            this.range = 0;
            this.color = color;
            this.infelicity = infelicity;
            this.id = id;
            this.speed = speed;
            this.CurentSpeed = 0;
        }


    }





    class Program
    {
        static readonly object lockObj = new object();
        static List<Car> cars = new List<Car>();
        static Random rnd = new Random();
        static List<Thread> list_threads = new List<Thread>();

        static void ProcessPeople(object Name)
        {
            Console.WriteLine($"Men {Name,4} | {"waiting before going on stadium", 35} | {semaphore.CurrentCount} places");
            semaphore.Wait();
            int a = rnd.Next(3000, 8000);
            Console.WriteLine($"Men {Name,4} | {$" | {a / 1000,7} s. | is on stadium", 35} | {semaphore.CurrentCount} places");
            Thread.Sleep(a);
            Console.Write($"Men {Name,4} | {"is going from stadium", 35} | ");
            semaphore.Release();
            Console.WriteLine($"{semaphore.CurrentCount} places");

        }




        static SemaphoreSlim semaphore = new SemaphoreSlim(1000, 1000);
        public static void Main(string[] args)
        {

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
            int i = 00000;
            Thread thread = new Thread(() => { 
                while (true) 
                { 
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{1000 - semaphore.CurrentCount} people");
                    Console.ResetColor();
                    Thread.Sleep(10000);
                } 
            });
            thread.Start();
            while (true) {
                for (int j = 0; j < rnd.Next(1, 30); j++) { 
                    Thread trainTread = new Thread(ProcessPeople);
                    trainTread.Start($"{i++ + 1}");
                }
                Thread.Sleep(rnd.Next(100, 4000));
            }
        }
    }
}
