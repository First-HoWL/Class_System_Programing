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
        public static void Main(string[] args)
        {
            /*
            List<Process> processList = new List<Process>();
            Console.WriteLine("With what command do you want help?");
            string b = Console.ReadLine();
            ProcessStartInfo startInfo = new ProcessStartInfo 
            {
                FileName = "cmd.exe",
                Arguments = $"/c help {b}",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            Process a = Process.Start(startInfo);
            string text = a.StandardOutput.ReadToEnd();
            Console.WriteLine($"Process output: \n{text}");
            a.Close();
            */
            //for (int i = 0; i < 5; i++)
            //{
            //    processList.Add(Process.Start("notepad.exe"));
            //    Console.WriteLine($"Процес {processList.Last().ProcessName} запущенно!");
            //}
            //while (processList.Count > 0) { 
            //    foreach(Process process in processList.ToArray())
            //    {
            //        if (process.HasExited) { 
            //            Console.WriteLine($"Process {process.Id} is gone!");
            //            processList.Remove(process);
            //        }
            //    }
            //}

            List<Process> processList = new List<Process>();
            do
            {
                Console.Clear();
                foreach (Process process in Process.GetProcesses())
                {
                    //Console.WriteLine($"{process.Id} - {process.ProcessName}");
                    processList.Add(process);
                }
                bubble_sort(processList, processList.Count());
                foreach (Process process in processList)
                {
                    Console.WriteLine($"{process.Id} - {process.ProcessName}");
                    //processList.Add(process);
                }
                Thread.Sleep(3000);
                processList = new List<Process>();
            } while (true);

            //Console.WriteLine($"Всі процеси завершено!");
        }
    }
}
