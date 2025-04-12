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
        public static int sum1;
        public static int sum2;

        public static int Sum(List<int> ints)
        {
            int a = 0;

            for (int i = 0; i < ints.Count; i++)
            {
                a += ints[i];
            }
            return a;

        }

        public static void Main(string[] args)
        {
            Console.InputEncoding = UTF8Encoding.UTF8;
            Console.OutputEncoding = UTF8Encoding.UTF8;

            List<int> ints = new List<int>();
            List<int> ints1 = new List<int>();
            List<int> ints2 = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                ints.Add(rnd.Next(1, 100));
            }
            for (int i = 0; i < ints.Count(); i++)
            {
                if (i % 2 == 0)
                    ints1.Add(ints[i]);
                else
                    ints2.Add(ints[i]);
            }


            Parallel.Invoke(
                () => {
                    sum1 = Sum(ints1);
                    Console.WriteLine($"[Потік {Thread.CurrentThread.ManagedThreadId}] Сума першої частини: {sum1} ");
                },
                () => {
                    sum2 = Sum(ints2);
                    Console.WriteLine($"[Потік {Thread.CurrentThread.ManagedThreadId}] Сума другої частини: {sum2} ");
                }
                );
            Console.WriteLine($"Загальна сума масиву: {sum1 + sum2}");

        }
    }
}
