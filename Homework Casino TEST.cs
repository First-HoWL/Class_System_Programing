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
using System.Data.SqlTypes;




namespace Game
{

    class player
    {
        public int money;
        public bool ready;
        public bool newPlayer;
        public int chosen;
        public int call;
        public player(int money) 
        { 
            this.money = money;
            this.newPlayer = true;
        }

        public void SetSector(int sector)
        {
            if (sector == chosen)
            {
                money += call * 2;
                newPlayer = false;
                ready = false;
                chosen = -1;
                call = 0;
            }
            else
            {
                newPlayer = false;
                ready = false;
                chosen = -1;
                call = 0;
            }
        }



    }

    

    class Program
    {
        static readonly object lockObj = new object();
        static Random rnd = new Random();
        public static int sum1;
        public static int sum2;
        static SemaphoreSlim semaphore = new SemaphoreSlim(5, 5);
        public static int sector;
        public static bool sectorNew;





        public void Player(player players)
        {
            semaphore.Wait();

            while (players.money != 0)
            {
                players.chosen = rnd.Next(1, 38);
                players.call = rnd.Next(players.money / 2, players.money);
                players.money -= players.call;
                players.ready = true;
                while (true)
                {
                    if (sectorNew == true)
                    {
                        players.SetSector(sector);

                        break;
                    }
                }

            }

            semaphore.Release();
        }



        public static void Main(string[] args)
        {
            Console.InputEncoding = UTF8Encoding.UTF8;
            Console.OutputEncoding = UTF8Encoding.UTF8;

            /*

            Створіть додаток, що імітує роботу столу казино протягом дня. За столом одночасно можуть сидіти п'ять осіб (п'ять потоків). 
            Кожен з них має фіксовану суму грошей. Кожен гравець може поставити певну суму на число (сума та число обираються випадково).
            Якщо кулька рулетки потрапила на число гравця, його сума подвоюється. Якщо ставка не зіграла, гравець втрачає всю поставлену суму.
            Якщо у гравця закінчилися гроші, він звільняє стіл і його місце займає новий гравець (новий потік). 
            Загальна кількість потенційних гравців за день обирається випадково, але має знаходитися в діапазоні від 20 до 100.
            День закінчується, коли усі потенційні гравці побувають за столом і зіграють хоча б один раунд. 
            Підсумком дня є звіт про усіх гравців наступного формату:
            Гравець1 [початкова сума] [кінцева сума]
            Гравець2 [початкова сума] [кінцева сума]
            Гравець3 [початкова сума] [кінцева сума]


            Використовуйте механізми багатопотоковості та синхронізації (Semaphore).


            */

        }
    }
}
