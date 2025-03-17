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

namespace SnakeGame
{

    class Program
    {
        const uint MB_OK = 0x00000000;
        const uint MB_OKCANCEL = 0x00000001;
        const uint MB_YESNO = 0x00000004;
        const uint MB_RETRYCANCEL = 0x00000005;
        const uint MB_YESNOCANCEL = 0x00000003;

        const uint MB_ICONERROR = 0x00000010;
        const uint MB_ICONQUESTION = 0x00000020;
        const uint MB_ICONWARNING = 0x00000030;
        const uint MB_ICONINFORMATION = 0x00000040;

        const int IDOK = 1;
        const int IDCANCEL = 2;
        const int IDYES = 6;
        const int IDNO = 7;
        const int IDRETRY = 4;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool MessageBeep(uint type);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool Beep(int frequency, int duration);


        public static void Main(string[] args)
        {
            int[] tones = new int[]{
              392, 392, 392, 311, 466, 392, 311, 466, 392,
              587, 587, 587, 622, 466, 369, 311, 466, 392,
              784, 392, 392, 784, 739, 698, 659, 622, 659,
              415, 554, 523, 493, 466, 440, 466,
              311, 369, 311, 466, 392
            };

            int[] durations = new int[]{
              350, 350, 350, 250, 100, 350, 250, 100, 700,
              350, 350, 350, 250, 100, 350, 250, 100, 700,
              350, 250, 100, 350, 250, 100, 100, 100, 450,
              150, 350, 250, 100, 100, 100, 450,
              150, 350, 250, 100, 750
            };

            for (int i = 0; i < tones.Length; i++)
            {
                Beep(tones[i], durations[i]);
            }
            //Beep(293, 500);
            //Beep(220, 400);
            //Beep(293, 400);
            //Beep(146, 400);
            //Beep(174, 400);
            //Beep(220, 400);
            //Beep(293, 400);
            //Beep(146, 400);

            

        }
    }
}
