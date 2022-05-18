// System Namespaces
using System;
using System.Runtime.InteropServices;


// Application Namespaces
using Lib.Managers;


// Library Namespaces


namespace CLI
{
    internal static class NativeMethods
    {
        [DllImport("kernel32.dll")]
        internal static extern bool AllocConsole();
    }

    class EntryPoint
    {
        private static readonly ConsoleProgram Program = new();

        [STAThread]
        private static void Main(string[] args)
        {
            NativeMethods.AllocConsole();

            LibManager.Initialize(true);

            Program.Run();
        }
    }
}
