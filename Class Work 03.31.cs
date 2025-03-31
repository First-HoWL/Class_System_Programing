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


    class info
    {
        public int ThreadId { get; set; }
        public int Money { get; set; }

        public info(int ThreadId, int Money) 
        {
            this.Money = Money;
            this.ThreadId = ThreadId;
        }
    }


    class Program
    {
        static readonly object lockObj = new object();
        static List<Car> cars = new List<Car>();
        static List<info> Info = new List<info>();
        static Random rnd = new Random();

        static void goCar(Car car)
        {
            while (car.range < 200)
            {
                Thread.Sleep(500);
                car.Addrange();
                car.NewSpeed();
            }
            car.CurentSpeed = 0;
        }

        static void AddBalance()
        {
            lock (lockObj)
            {
                Info.Add(new info(Thread.CurrentThread.ManagedThreadId, rnd.Next(1, 10) * 100));
            }

        }

        static void ReduseBalance()
        {
            lock (lockObj)
            {
                Info.Add(new info(Thread.CurrentThread.ManagedThreadId, rnd.Next(1, 10) * 100 * -1));
            }
        }

        static void WhatYouChoose(Thread last)
        {
            int a = rnd.Next(1, 3);
            if (a == 1)
            {
                AddBalance();
            }
            else{
                ReduseBalance();
            }
            Thread.Sleep(2000);

            
            
        }
        static List<Thread> list_threads = new List<Thread>();

        static void Threads()
        {
            while (true)
            {
                if (list_threads.Count() < 5)
                {
                    lock (lockObj)
                    {
                        list_threads.Add(new Thread(() => WhatYouChoose(list_threads.Last())));
                        list_threads.Last().Start();

                    }
                }
                foreach(Thread thread in list_threads.ToArray())
                {
                    if(list_threads.Last().ThreadState == System.Threading.ThreadState.Stopped)
                    {
                        lock (lockObj)
                        {
                            list_threads.Remove(thread);
                        }
                    }
                }
            }
        }

        static void ProcessCar(object CarName)
        {
            Console.WriteLine($"Car {CarName,7} | waiting before going on gas station");
            semaphore.WaitOne();
            int a = rnd.Next(3000, 8000);
            Console.WriteLine($"Car {CarName, 7} | {rnd.Next(3000, 7000) / 1000, 7} s. | is on gas station");
            Thread.Sleep(a);
            Console.WriteLine($"Car {CarName, 7} | is going from gas station");
            semaphore.Release();
            
        }


        static Semaphore semaphore = new Semaphore(3, 3);
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
            
            for (int i = 0; i < 10; i++)
            {
                Thread trainTread = new Thread(ProcessCar);
                trainTread.Start($"N{i + 1}");
            }


        }
    }
}
