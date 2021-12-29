// System Namespaces
using System;
using System.Diagnostics;


// Application Namespaces
using Lib.Shaping;
using Lib.Projects;
using CLI.Utility.Extensions;


// Library Namespaces
using EasyConsole;


namespace CLI.Menu.Shaping.Making
{
    internal class ShapingAppliedPage : MenuPage
    {
        public static VCXSolution VCXProject;
        public static ShapeProject ShapeProject;

        public static Stopwatch TimeWatch;

        public ShapingAppliedPage(ConsoleProgram program) : base("Applied patches", program)
        {

        }

        public override void Display()
        {
            DisplayProgram();
            DisplayResult();
            Console.WriteLine();
            this.InputOptions(Menu);
        }

        public void DisplayProgram()
        {
            ((ConsoleProgram)Program).Display();

            var ptr = typeof(Page).GetMethod("Display").MethodHandle.GetFunctionPointer();
            var basedisplay = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
            basedisplay();
        }

        public void DisplayResult()
        {
            DisplayApplied();
        }

        public void DisplayApplied()
        {
            Console.WriteLine("");
            ConsoleExtensions.Write(ConsoleColor.Red, "Applied: " + "\n");

            ConsoleExtensions.Write(ConsoleColor.Yellow, " - Replacements: \n");
            foreach (var replacement in VCXProject.AppliedReplacements())
                ConsoleExtensions.Write(ConsoleColor.DarkYellow, "   - " + replacement.Key + "\n");

            ConsoleExtensions.Write(ConsoleColor.Green, " - Additions: \n");
            foreach (var addition in VCXProject.AppliedAdditions())
                ConsoleExtensions.Write(ConsoleColor.DarkYellow, "   - " + addition.Key + "\n");

            ConsoleExtensions.Write(ConsoleColor.Red, " - Subtractions: \n");
            foreach (var subtraction in VCXProject.AppliedSubtractions())
                ConsoleExtensions.Write(ConsoleColor.DarkYellow, "   - " + subtraction.Key + "\n");
        }
    }
}
