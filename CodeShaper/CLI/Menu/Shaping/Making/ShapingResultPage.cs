// System Namespaces
using System;


// Application Namespaces
using CLI.Utility.Extensions;


// Library Namespaces
using EasyConsole;


namespace CLI.Menu.Shaping.Making
{
    internal class ShapingResultPage : MenuPage
    {
        public ShapingResultPage(Program program) : base("Shaping Result", program,
                new Option("See Applied Patches", () => program.NavigateTo<ShapingAppliedPage>()),
                new Option("See Unused Patches", () => program.NavigateTo<ShapingUnusedPage>()),
                new Option("See Changes Detailed", () => program.NavigateTo<ShapingResultDetailed>())
            )
        {
        }

        public override void Display()
        {
            DisplayProgram();
            DisplayBase();
            DisplayInformation();
            this.InputOptions(Menu);
        }

        private static void DisplayProgram()
        {
            ConsoleProgram.Display();
        }

        private void DisplayBase()
        {
            var ptr = typeof(Page).GetMethod("Display")!.MethodHandle.GetFunctionPointer();
            var baseDisplay = (Action)Activator.CreateInstance(typeof(Action), this, ptr);

            baseDisplay?.Invoke();
        }

        private static void DisplayInformation()
        {
            DisplayResult();
            Console.WriteLine();
        }

        private static void DisplayResult()
        {
            var unparsedModules = 0;
            var modules = 0;

            // var t = TimeWatch.Elapsed;
            // var time = $"{t.Minutes:D2} minutes, {t.Seconds:D2} seconds, {t.Milliseconds:D3} milliseconds";

            Console.WriteLine();
            // Output.WriteLine(ConsoleColor.Cyan, "Time Elapsed: {0}", time);

            Console.WriteLine();
            ConsoleExtensions.Write(ConsoleColor.Green, $"Parsed Modules: {modules}\n");
            ConsoleExtensions.Write(ConsoleColor.Red, $"Invalid Modules: {unparsedModules}\n");
        }
    }
}