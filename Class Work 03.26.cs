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

        public void NewSpeed() {
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
        
        static void goCar(Car car)
        {
            while (car.range < 10000)
            {
                Thread.Sleep(500);
                car.Addrange();
                car.NewSpeed();
            }

        }

        public static void Main(string[] args)
        {
            Console.ResetColor();
            Random rand = new Random();

            Console.Write("How many cars do you want? \n >");
            var a = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < a; i++)
            {
                cars.Add(new Car(i + 1, rand.Next(60, 73), (ConsoleColor)rand.Next(1, 16), rand.Next(3, 8)));
            }
            List<Thread> list_threads = new List<Thread>();
            foreach (var car in cars)
            {
                list_threads.Add(new Thread(() => goCar(car)));
            }
            foreach (var item in list_threads) { 
                item.Start();
            }

            Console.Clear();
            while (true)
            {
                foreach (var item in cars)
                {
                    Console.ForegroundColor = item.color;
                    Console.WriteLine($" {item.id, 3} | {item.CurentSpeed, 5}km/h | {Math.Round(item.range / 1000, 2), 7} / 10 km");
                    Console.ResetColor();
                }
                Console.SetCursorPosition(0, 0);
            }
        }
    }
}
