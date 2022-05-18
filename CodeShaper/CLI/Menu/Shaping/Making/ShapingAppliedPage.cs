// System Namespaces
using System;


// Application Namespaces
using Lib.Miscelaneous;
using CLI.Utility.Extensions;


// Library Namespaces
using EasyConsole;
using Lib.Managers;


namespace CLI.Menu.Shaping.Making
{
    internal class ShapingAppliedPage : MenuPage
    {
        public ShapingAppliedPage(Program program) : base("Applied patches", program)
        {

        }

        public override void Display()
        {
            DisplayProgram();
            DisplayResult();
            Console.WriteLine();
            this.InputOptions(Menu);
        }

        private void DisplayProgram()
        {
            ConsoleProgram.Display();

            var ptr = typeof(Page).GetMethod("Display")!.MethodHandle.GetFunctionPointer();
            var baseDisplay = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
            baseDisplay?.Invoke();
        }

        private static void DisplayResult()
        {
            DisplayApplied();
        }

        private static void DisplayApplied()
        {
            var currentShapingOperation = ShapingOperationsManager.ActiveShapingOperation;
            
            Console.WriteLine("");
            ConsoleExtensions.Write(ConsoleColor.Red, "Applied: " + "\n");
            
            ConsoleExtensions.Write(ConsoleColor.Yellow, " - Replacements: \n");
            foreach (var replacement in currentShapingOperation.ShapingTarget.AppliedReplacements())
                ConsoleExtensions.Write(ConsoleColor.DarkYellow, "   - " + "replacement" + "\n");

            ConsoleExtensions.Write(ConsoleColor.Green, " - Additions: \n");
            foreach (var addition in currentShapingOperation.ShapingTarget.AppliedAdditions())
                ConsoleExtensions.Write(ConsoleColor.DarkYellow, "   - " + "addition" + "\n");

            ConsoleExtensions.Write(ConsoleColor.Red, " - Subtractions: \n");
            foreach (var subtraction in currentShapingOperation.ShapingTarget.AppliedSubtractions())
                ConsoleExtensions.Write(ConsoleColor.DarkYellow, "   - " + "subtraction" + "\n");
        }
    }
}
