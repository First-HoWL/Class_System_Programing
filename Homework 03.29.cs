using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

class Program
{
    static bool stopRequested = false;

    public static void ShowProcessesWithInterval(int seconds)
    {
        stopRequested = false;

        Thread worker = new Thread(() =>
        {
            while (!stopRequested)
            {
                Console.Clear();
                List<Process> processes = Process.GetProcesses().OrderBy(p => p.ProcessName).ToList();
                Console.WriteLine("List of processes:\n");
                foreach (var process in processes)
                {
                    Console.WriteLine($"{process.ProcessName,-30} ID: {process.Id}");
                }
                Console.WriteLine("Press Enter to exit");

                for (int i = 0; i < seconds * 10; i++)
                {
                    if (stopRequested) return;
                    Thread.Sleep(100);
                }
            }
        });

        worker.Start();

        Console.ReadLine();
        stopRequested = true;
        worker.Join();
    }

    public static void ShowProcessDetails()
    {
        Console.Write("Enter the process ID: ");
        string input = Console.ReadLine();
        int a;
        if (int.TryParse(input, out a))
        {
            try
            {
                Process proc = Process.GetProcessById(a);
                int count = Process.GetProcessesByName(proc.ProcessName).Length;
                Console.WriteLine();
                Console.WriteLine($"ID: {proc.Id}");
                Console.WriteLine($"Start time: {proc.StartTime}");
                Console.WriteLine($"Processor time: {proc.TotalProcessorTime}");
                Console.WriteLine($"Count of threads: {proc.Threads.Count}");
                Console.WriteLine($"Count of copies: {count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erorr: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Incorrect ID.");
        }
    }

    public static void KillProcess()
    {
        Console.Write("Enter the process ID for break: ");
        string input = Console.ReadLine();
        int pid;
        if (int.TryParse(input, out pid))
        {
            try
            {
                Process.GetProcessById(pid).Kill();
                Console.WriteLine("Process killed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Incorrect ID.");
        }
    }

    public static void StartApplication()
    {
        Console.WriteLine("Choose program:");
        Console.WriteLine("1. Notepad");
        Console.WriteLine("2. Calculator");
        Console.WriteLine("3. Paint");
        Console.WriteLine("4. Other program");
        Console.Write("Your choice: ");
        string choice = Console.ReadLine();

        try
        {
            switch (choice)
            {
                case "1":
                    Process.Start("notepad.exe");
                    break;
                case "2":
                    Process.Start("calc.exe");
                    break;
                case "3":
                    Process.Start("mspaint.exe");
                    break;
                case "4":
                    Console.Write("Enter the full path: ");
                    string path = Console.ReadLine();
                    Process.Start(path);
                    break;
                default:
                    Console.WriteLine("Incorrect choice.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Menu");
            Console.WriteLine("1. Show process list");
            Console.WriteLine("2. Show process details");
            Console.WriteLine("3. Break the process");
            Console.WriteLine("4. Start program");
            Console.WriteLine("0. Exit");
            Console.Write("Your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter the update interval (sec): ");
                    if (int.TryParse(Console.ReadLine(), out int interval))
                        ShowProcessesWithInterval(interval);
                    else
                        Console.WriteLine("Incorrect");
                    break;

                case "2":
                    ShowProcessDetails();
                    break;

                case "3":
                    KillProcess();
                    break;

                case "4":
                    StartApplication();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Something went wrong");
                    break;
            }

            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
