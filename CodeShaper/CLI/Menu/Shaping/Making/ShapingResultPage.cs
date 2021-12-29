// System Namespaces
using System;
using System.Diagnostics;


// Application Namespaces
using Lib.Configurations;
using Lib.Shaping;
using Lib.Projects;
using CLI.Utility.Extensions;


// Library Namespaces
using EasyConsole;


namespace CLI.Menu.Shaping.Making
{
    internal class ShapingResultPage : MenuPage
    {
        public static ShapingConfiguration ShapingConfiguration;

        public static VCXSolution VCXProject;
        public static ShapeProject ShapeProject;

        public static Stopwatch TimeWatch;

        public ShapingResultPage(ConsoleProgram program) : base("Shaping Result", program,
                new Option("See Applied Patches", () => program.NavigateTo<ShapingAppliedPage>()),
                new Option("See Unapplied Patches", () => program.NavigateTo<ShapingUnappliedPage>()),
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

        public void DisplayProgram()
        {
            ((ConsoleProgram)Program).Display();
        }
        
        public void DisplayBase()
        {
            var ptr = typeof(Page).GetMethod("Display").MethodHandle.GetFunctionPointer();
            var basedisplay = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
            basedisplay();
        }

        public void DisplayInformation()
        {
            DisplayResult();
            Console.WriteLine();
        }

        public void DisplayResult()
        {
            int unparsedModules = 0;
            int modules = 0;

            var t = TimeWatch.Elapsed;
            string time = string.Format("{0:D2} minutes, {1:D2} seconds, {2:D3} milliseconds",
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);

            Console.WriteLine();
            Output.WriteLine(ConsoleColor.Cyan, "Time Elapsed: {0}", time);

            Console.WriteLine();
            ConsoleExtensions.Write(ConsoleColor.Green, string.Format("Parsed Modules: {0}\n", modules));
            ConsoleExtensions.Write(ConsoleColor.Red, string.Format("Invalid Modules: {0}\n", unparsedModules));

        }
    }
}