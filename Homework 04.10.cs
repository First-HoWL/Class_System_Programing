using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

class PlayerResult
{
    public string Name { get; set; }
    public int StartMoney { get; set; }
    public int EndMoney { get; set; }
}

class Program
{
    static Semaphore tableSemaphore = new Semaphore(5, 5);
    static object lockobj = new object();
    static List<PlayerResult> results = new List<PlayerResult>();
    static int playerNumber = 1;
    static Random random = new Random();
    static int totalPlayers;

    static void Main()
    {
        Console.OutputEncoding = UTF8Encoding.UTF8;
        Console.InputEncoding = UTF8Encoding.UTF8;
        totalPlayers = random.Next(20, 101);
        Console.WriteLine($"Сьогодні до казино завітає {totalPlayers} гравців.\n");

        List<Thread> threads = new List<Thread>();


        for (int i = 0; i < totalPlayers; i++)
        {
            Thread t = new Thread(PlayerThread);
            threads.Add(t);
            t.Start();
        }


        foreach (Thread t in threads)
        {
            t.Join();
        }


        Console.WriteLine("\nЗвіт про результати дня");
        foreach (var result in results)
        {
            Console.WriteLine($"{result.Name} [початкова сума: {result.StartMoney}] [кінцева сума: {result.EndMoney}]");
        }

        Console.WriteLine("\nДень у казино завершено.");
    }

    static void PlayerThread()
    {

        int number;
        lock (lockobj)
        {
            number = playerNumber++;
        }

        string playerName = $"Гравець {number}";
        int money = random.Next(500, 2001);
        int startMoney = money;


        tableSemaphore.WaitOne();

        Console.WriteLine($"{playerName} сів за стіл зі {money} грн.");

        while (money > 0)
        {
            int bet = random.Next(Math.Min(50, money), Math.Min(501, money + 1));
            int chosenNumber = random.Next(0, 37);
            int ballNumber = random.Next(0, 37);

            Console.WriteLine($"{playerName} ставить {bet} грн на {chosenNumber}. Кулька випала на {ballNumber}.");

            if (chosenNumber == ballNumber)
            {
                money += bet;
                Console.WriteLine($"Ура! {playerName} виграв! Нова сума: {money} грн.");
            }
            else
            {
                money -= bet;
                Console.WriteLine($"{playerName} програв. Залишилось: {money} грн.");
            }

            Thread.Sleep(300);
        }

        Console.WriteLine($"{playerName} залишає стіл.");


        lock (lockobj)
        {
            results.Add(new PlayerResult
            {
                Name = playerName,
                StartMoney = startMoney,
                EndMoney = money
            });
        }


        tableSemaphore.Release();
    }
}
