// System Namespaces
using System;
using System.Linq;


// Application Namespaces


// Library Namespaces



namespace CLI.Utility.Extensions
{
    public static class ConsoleExtensions
    {
        public static void Write(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ResetColor();
        }

        public static string WindowFill(char chr)
        {
            return string.Concat(Enumerable.Repeat(chr, Console.WindowWidth));
        }
    }
}
