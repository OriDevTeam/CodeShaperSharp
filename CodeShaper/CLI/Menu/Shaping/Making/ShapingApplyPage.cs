// System Namespaces
using System;


// Application Namespaces
using Lib.Shaping.Target.Interfaces;
using Lib.Managers;
using Lib.Miscelaneous;
using Lib.Shaping;
using CLI.Utility.Extensions;


// Library Namespaces
using EasyConsole;


namespace CLI.Menu.Shaping.Making
{
    internal class ShapingApplyPage : MenuPage
    {
        private static ShapingOperation ShapingOperation => ShapingOperationsManager.ActiveShapingOperation;
        private static IShapingTarget ShapingTarget => ShapingOperationsManager.ActiveShapingOperation.ShapingTarget;
        private static ShapeProject ShapeProject => ShapingOperationsManager.ActiveShapingOperation.ShapeProject;
        
        public ShapingApplyPage(Program program) : base("Applying Shaping", program)
        {
        }

        public override void Display()
        {
            ProcessShapeProject();
            DisplayProgram();
            
            DisplayShapingInformation();
            ProcessShaping();

            Program.NavigateTo<ShapingResultPage>();
        }

        private void DisplayProgram()
        {
            ConsoleProgram.Display();

            var ptr = typeof(Page).GetMethod("Display")!.MethodHandle.GetFunctionPointer();
            var baseDisplay = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
            baseDisplay?.Invoke();
        }

        private static void DisplayShapingInformation()
        {
            Output.WriteLine(ConsoleColor.Green, ConsoleExtensions.WindowFill('-'));

            Output.WriteLine(ConsoleColor.Cyan, "Project: " + ShapeProject.Configuration.Configuration.Name);
            ConsoleExtensions.Write(ConsoleColor.Red, "Patches: " + ShapeProject.Patches.Count + "\n");
            
            ConsoleExtensions.Write(ConsoleColor.Red, " - Replacements: " + ShapeProject.TotalReplacementCount() + "\n");
            ConsoleExtensions.Write(ConsoleColor.Red, " - Additions: " + ShapeProject.TotalAdditionsCount() + "\n");
            ConsoleExtensions.Write(ConsoleColor.Red, " - Subtractions: " + ShapeProject.TotalSubtractionsCount() + "\n");

            Output.WriteLine("");
            Output.WriteLine(ConsoleColor.Green, ConsoleExtensions.WindowFill('-'));
        }

        private static void DisplayShapingProcess()
        {
            Output.WriteLine(ConsoleColor.Green, ConsoleExtensions.WindowFill('-'));

            Output.WriteLine(ConsoleColor.Cyan, "Project: " + ShapeProject.Configuration.Configuration.Name);
            Console.WriteLine("");
            // Output.WriteLine(ConsoleColor.Cyan, "Parsed Modules: {0}/{1}", VCXProject.Modules.Count, VCXProject.ModuleCount);
            
            // var time = $"{t.Minutes:D2} minutes, {t.Seconds:D2} seconds, {t.Milliseconds:D3} milliseconds";

            // Output.WriteLine(ConsoleColor.Cyan, "Time Elapsed: {0}", time);
            
            
            Console.WriteLine("");
            ConsoleExtensions.Write(ConsoleColor.Red, "Applied: " + "\n");
            ConsoleExtensions.Write(ConsoleColor.Red, " - Replacements: " + ShapingTarget.TotalProcessedReplacementCount() + "\n");
            ConsoleExtensions.Write(ConsoleColor.Red, " - Additions: " + ShapingTarget.TotalProcessedAdditionCount() + "\n");
            ConsoleExtensions.Write(ConsoleColor.Red, " - Subtractions: " + ShapingTarget.TotalProcessedSubtractionCount() +"\n");

            Output.WriteLine("");
            Output.WriteLine(ConsoleColor.Green, ConsoleExtensions.WindowFill('-'));
        }

        public static void DisplaySaving()
        {
            ConsoleExtensions.Write(ConsoleColor.Red, "Saving Shaped Files: " + "\n");
        }

        private void ProcessShapeProject()
        {
            ShapeProject.SavingShapedFile += OnSavingShapedFile;
            ShapingTarget.AddGroupsLoadEvent(OnLoadingShapingTargetGroup);
            ShapingTarget.AddFilesLoadEvent(OnLoadingShapingTargetFile);
        }

        private void ProcessShaping()
        {
            Console.Clear();
            DisplayProgram();

            ShapingOperation.Start();
        }

        private void OnLoadingShapingTargetGroup(object sender, IShapingTargetGroup targetGroup)
        {
            Console.Clear();
            DisplayProgram();

            DisplayShapingProcess();

            Output.WriteLine(ConsoleColor.Red, $"Parsing {targetGroup.GetType().Name} {targetGroup.Name}");
            Output.WriteLine(ConsoleColor.Red, ConsoleExtensions.WindowFill('-'));
        }

        private void OnLoadingShapingTargetFile(object sender, IShapingTargetFile targetFile)
        {
            Console.Clear();
            DisplayProgram();
            
            DisplayShapingProcess();

            Output.WriteLine(ConsoleColor.Green, $"Parsing {targetFile.GetType().Name} {targetFile.Name}");
            Output.WriteLine(ConsoleColor.Green, ConsoleExtensions.WindowFill('-'));
        }

        private static void OnSavingShapedFile(object sender, string filePath)
        {
            // Output.WriteLine(ConsoleColor.Green, $" {filePath}");
        }
    }
}
