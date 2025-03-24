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

        public static void bubble_sort(List<int> array, int len)
        {
            int a;
            bool is_changed = true;
            while (is_changed)
            {
                is_changed = false;
                for (int i = 0; i < len - 1; i++)
                {
                    if (array[i] > array[i + 1])
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
            if (a <= 1) return 1;
            var b = (a * Factorial(a - 1));
            Console.WriteLine(b);
            Thread.Sleep(300);
            return b;
        }

        public static void QuickSortWithoutThread(List<int> list)
        {
            if (list.Count < 2)
                return;
            int pivot = list.Last();
            List<int> list_less = new List<int>();
            List<int> list_more = new List<int>();
            List<int> list_equal = new List<int>();
            for (int i = 0; i < list.Count(); i++)
            {
                if (pivot < list[i])
                {
                    list_less.Add(list[i]);
                }
                else if (pivot > list[i])
                {
                    list_more.Add(list[i]);
                }
                else
                {
                    list_equal.Add(list[i]);
                }
            }

            QuickSortWithoutThread(list_less);
            QuickSortWithoutThread(list_more);

            list.Clear();
            list.AddRange(list_less);
            list.AddRange(list_equal);
            list.AddRange(list_more);
        }

        public static void QuickSort(List<int> list)
        {
            if (list.Count < 2)
                return;
            int pivot = list.Last();
            List<int> list_less = new List<int>();
            List<int> list_more = new List<int>();
            List<int> list_equal = new List<int>();
            for (int i = 0; i < list.Count(); i++) { 
                if (pivot < list[i])
                {
                    list_less.Add(list[i]);
                }
                else if (pivot > list[i])
                {
                    list_more.Add(list[i]);
                }
                else
                {
                    list_equal.Add(list[i]);
                }
            }

            Thread thread1 = new Thread(() => QuickSort(list_less));
            Thread thread2 = new Thread(() => QuickSort(list_more));

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            
            
            list.Clear();
            list.AddRange(list_less);
            list.AddRange(list_equal);
            list.AddRange(list_more);
        }

        public static void Main(string[] args)
        {
            List<int> list = new List<int>();
            List<int> list2 = new List<int>();
            List<int> list3 = new List<int>();
            Random rnd = new Random();  
            for (int i = 0; i < 20000; i++) {
                int a = rnd.Next(1, 100);
                list.Add(a);
                list2.Add(a);
                list3.Add(a);
            }


            //for (int i = 0; i < list.Count; i++)
            //{
            //    Console.Write(list[i] + ", ");
            //}

            Console.WriteLine();

            Stopwatch sw = Stopwatch.StartNew();
            Thread thread1 = new Thread(() => QuickSort(list));
            thread1.Start();
            thread1.Join();
            sw.Stop();

            Stopwatch sw1 = Stopwatch.StartNew();
            bubble_sort(list2, list2.Count());
            sw1.Stop();

            Stopwatch sw2 = Stopwatch.StartNew();
            QuickSortWithoutThread(list3);
            sw2.Stop();

            //for (int i = 0; i < list.Count; i++)
            //{
            //    Console.Write(list[i] + ", ");
            //}
            Console.WriteLine();
            Console.WriteLine("Quick sort: " + sw);
            Console.WriteLine("Buble sort: " + sw1);
            Console.WriteLine("Quick sort without threading: " + sw2);
            

        }
    }
}
