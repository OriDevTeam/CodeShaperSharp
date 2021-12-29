// System Namespaces
using System;
using System.Runtime.InteropServices;


// Application Namespaces


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
        static ConsoleProgram program = new();

        [STAThread]
        static void Main(string[] args)
        {
            NativeMethods.AllocConsole();

            program.Run();
        }
    }
}
