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

        public static void Main(string[] args)
        {

            var result = MessageBox(IntPtr.Zero, "Does it move?", "Message", MB_YESNO | MB_ICONQUESTION);
            Console.WriteLine(result);
            if (result == IDYES) { 
                result = MessageBox(IntPtr.Zero, "Should it?", "Message", MB_YESNO | MB_ICONQUESTION);
                if (result == IDYES)
                {
                    Environment.Exit(0);
                }
                if (result == IDNO)
                {
                    MessageBox(IntPtr.Zero, "Problem!", "Message", MB_OK | MB_ICONWARNING);
                }
            }
            else if (result == IDNO)
            {
                result = MessageBox(IntPtr.Zero, "Should it?", "Message", MB_YESNO | MB_ICONQUESTION);
                if (result == IDYES)
                {
                    MessageBox(IntPtr.Zero, "Problem!", "Message", MB_OK | MB_ICONWARNING);
                }
                if (result == IDNO)
                {
                    Environment.Exit(0);
                }
            }

        }
    }
}
