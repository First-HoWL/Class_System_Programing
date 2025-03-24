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

        public static void bubble_sort(List<Process> array, int len)
        {
            bool is_changed = true;
            Process a;
            while (is_changed)
            {
                is_changed = false;
                for (int i = 0; i < len - 1; i++)
                {
                    if (array[i].Id > array[i + 1].Id)
                    {
                        a = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = a;
                        is_changed = true;
                    }
                }
            }
        }

        public static void CountReversed(int a)
        {
            for (int i = a, j = 0; i > 0; i--, j++)
            {
                Console.WriteLine(i);
                Thread.Sleep(500);
            }
        }
        public static void Count(int a)
        {
            for (int i = 0; i < a; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(500);
            }
        }

        public static int Factorial(int a)
        {
            if (a == 1) return 1;
            var b = (a * Factorial(a - 1));
            Console.WriteLine(b);
            Thread.Sleep(300);
            return b;
        }

        public static void Main(string[] args)
        {
            

            Thread thread1 = new Thread(() => Factorial(5));
            Thread thread2 = new Thread(() => Factorial(6));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

        }
    }
}
