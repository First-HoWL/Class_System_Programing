using System;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    const uint MB_OK = 0x00000000;

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern bool Beep(int frequency, int duration);

    static void Main()
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
        
        MessageBox(IntPtr.Zero, "Мене звати Денис Ященко.", "Program", MB_OK);
        Thread musicThread = new Thread(() =>
        {
            for (int i = 0; i < tones.Length; i++)
            {
                Beep(tones[i], durations[i]);
            }
        });
        musicThread.Start();
        MessageBox(IntPtr.Zero, "Я навчаюся програмуванню.", "Program", MB_OK);
        musicThread.Join();
        MessageBox(IntPtr.Zero, "Це приклад програми\nз використанням MessageBox та Beep.", "Program", MB_OK);
    }
}
