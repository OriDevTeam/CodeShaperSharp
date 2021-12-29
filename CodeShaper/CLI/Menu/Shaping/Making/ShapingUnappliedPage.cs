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
    internal class ShapingUnappliedPage : MenuPage
    {
        public static VCXSolution VCXProject;
        public static ShapeProject ShapeProject;

        public static Stopwatch TimeWatch;

        public ShapingUnappliedPage(ConsoleProgram program) : base("Unapplied patches", program)
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
            DisplayUnaplied();
        }

        public void DisplayUnaplied()
        {
            Console.WriteLine("");
            ConsoleExtensions.Write(ConsoleColor.Red, "Not applied: " + "\n");

            var replacements = VCXProject.UnapliedReplacements();

            if (replacements.Count > 0)
            {
                ConsoleExtensions.Write(ConsoleColor.Yellow, " - Replacements: \n");
                foreach (var replacement in replacements)
                    ConsoleExtensions.Write(ConsoleColor.DarkYellow, "   - " + replacement.Key + "\n");

            }

            var additions = VCXProject.UnapliedAdditions();

            if (additions.Count > 0)
            {
                ConsoleExtensions.Write(ConsoleColor.Green, " - Additions: \n");
                foreach (var addition in additions)
                    ConsoleExtensions.Write(ConsoleColor.DarkYellow, "   - " + addition.Key + "\n");
            }

            var subtractions = VCXProject.UnapliedSubtractions();

            if (subtractions.Count > 0)
            {
                ConsoleExtensions.Write(ConsoleColor.Red, " - Subtractions: \n");
                foreach (var subtraction in subtractions)
                    ConsoleExtensions.Write(ConsoleColor.DarkYellow, "   - " + subtraction.Key + "\n");
            }
        }
    }
}
