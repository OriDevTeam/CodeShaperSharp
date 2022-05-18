// System Namespaces
using System;


// Application Namespaces
using Lib.Managers;
using Lib.Shaping;
using Lib.Shaping.Target.Interfaces;
using Lib.Miscelaneous;
using CLI.Utility.Extensions;


// Library Namespaces
using EasyConsole;


namespace CLI.Menu.Shaping.Making
{
    internal class ShapingUnusedPage : MenuPage
    {
        private static ShapingOperation ShapingOperation => ShapingOperationsManager.ActiveShapingOperation;
        private static IShapingTarget ShapingTarget => ShapingOperationsManager.ActiveShapingOperation.ShapingTarget;
        private static ShapeProject ShapeProject => ShapingOperationsManager.ActiveShapingOperation.ShapeProject;

        
        public ShapingUnusedPage(Program program) : base("Unused patches", program)
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
            DisplayUnused();
        }

        private static void DisplayUnused()
        {
            Console.WriteLine("");
            ConsoleExtensions.Write(ConsoleColor.Red, "Not applied: " + "\n");

            var replacements = ShapingTarget.UnusedReplacements();

            if (replacements.Count > 0)
            {
                ConsoleExtensions.Write(ConsoleColor.Yellow, " - Replacements: \n");
                foreach (var replacement in replacements)
                    ConsoleExtensions.Write(ConsoleColor.DarkYellow, "   - " + "replacement.Key" + "\n");

            }

            var additions = ShapingTarget.UnusedAdditions();

            if (additions.Count > 0)
            {
                ConsoleExtensions.Write(ConsoleColor.Green, " - Additions: \n");
                foreach (var addition in additions)
                    ConsoleExtensions.Write(ConsoleColor.DarkYellow, "   - " + "addition.Key" + "\n");
            }

            var subtractions = ShapingTarget.UnusedSubtractions();

            if (subtractions.Count <= 0)
                return;
            
            ConsoleExtensions.Write(ConsoleColor.Red, " - Subtractions: \n");
            
            foreach (var subtraction in subtractions)
                ConsoleExtensions.Write(ConsoleColor.DarkYellow, "   - " + "subtraction.Key" + "\n");
        }
    }
}
