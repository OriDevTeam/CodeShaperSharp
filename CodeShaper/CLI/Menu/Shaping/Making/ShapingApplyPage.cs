// System Namespaces
using System;
using System.Threading;
using System.Diagnostics;

// Application Namespaces
using Lib.Configurations;
using Lib.Shaping;
using Lib.Projects;
using Lib.Settings;
using CLI.Utility.Extensions;


// Library Namespaces
using EasyConsole;


namespace CLI.Menu.Shaping.Making
{
    internal class ShapingApplyPage : MenuPage
    {
        public static ShapingConfiguration ShapingConfiguration;

        public ShapingApplyPage(ConsoleProgram program) : base("Applying Shaping", program)
        {
        }

        public override void Display()
        {
            _stopWatch = Stopwatch.StartNew();
            _stopWatch.Start();

            ShapingResultPage.TimeWatch = _stopWatch;
            ShapingAppliedPage.TimeWatch = _stopWatch;
            ShapingUnappliedPage.TimeWatch = _stopWatch;

            ProcessShapeProject();
            DisplayProgram();

            if (ShapeProject != null)
                DisplayShapingInformation();

            Thread.Sleep(3000);

            ProcessVCXSolution();
            ProcessShaping();

            Program.NavigateTo<ShapingResultPage>();
        }

        public void DisplayProgram()
        {
            ((ConsoleProgram)Program).Display();

            var ptr = typeof(Page).GetMethod("Display").MethodHandle.GetFunctionPointer();
            var basedisplay = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
            basedisplay();
        }

        public void DisplayShapingInformation()
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

        public void DisplayShapingProcess()
        {
            Output.WriteLine(ConsoleColor.Green, ConsoleExtensions.WindowFill('-'));

            Output.WriteLine(ConsoleColor.Cyan, "Project: " + ShapeProject.Configuration.Configuration.Name);
            Console.WriteLine("");
            Output.WriteLine(ConsoleColor.Cyan, "Parsed Modules: {0}/{1}", VCXProject.Modules.Count, VCXProject.ModuleCount);

            var t = _stopWatch.Elapsed;
            string time = string.Format("{0:D2} minutes, {1:D2} seconds, {2:D3} milliseconds",
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);

            Output.WriteLine(ConsoleColor.Cyan, "Time Elapsed: {0}", time);

            Console.WriteLine("");
            ConsoleExtensions.Write(ConsoleColor.Red, "Applied: " + "\n");
            ConsoleExtensions.Write(ConsoleColor.Red, " - Replacements: " + VCXProject.TotalProcessedReplacementCount() + "\n");
            ConsoleExtensions.Write(ConsoleColor.Red, " - Additions: " + VCXProject.TotalProcessedAdditionCount() + "\n");
            ConsoleExtensions.Write(ConsoleColor.Red, " - Subtractions: " + VCXProject.TotalProcessedSubtractionCount() +"\n");

            Output.WriteLine("");
            Output.WriteLine(ConsoleColor.Green, ConsoleExtensions.WindowFill('-'));
        }

        public void DisplaySaving()
        {
            ConsoleExtensions.Write(ConsoleColor.Red, "Saving Shaped Files: " + "\n");
        }

        public void ProcessShapeProject()
        {
            if (ShapeProject == null)
            {
                ShapeProject = new(ShapingConfiguration);
                ShapeProject.SavingShapedFile += OnSavingShapedFile;

                ShapingResultPage.ShapeProject = ShapeProject;
                ShapingAppliedPage.ShapeProject = ShapeProject;
                ShapingUnappliedPage.ShapeProject = ShapeProject;
                ShapingResultDetailed.ShapeProject = ShapeProject;
            }
        }

        public void ProcessVCXSolution()
        {
            VCXProject = new VCXSolution(ShapingConfiguration.SourceDirectory, ShapeProject);
            VCXProject.LoadingVCXSolution += OnLoadingVCXSolution;
            VCXProject.LoadingVCXProject += OnLoadingVCXProject;
            VCXProject.LoadingVCXModule += OnLoadingVCXModule;

            ShapingResultPage.VCXProject = VCXProject;
            ShapingAppliedPage.VCXProject = VCXProject;
            ShapingUnappliedPage.VCXProject = VCXProject;
            ShapingResultDetailed.VCXProject = VCXProject;

            VCXProject.Load();
        }

        public void ProcessShaping()
        {
            Console.Clear();
            DisplayProgram();

            ShapeProject.Shape(VCXProject, ShapingConfiguration);
        }

        void OnLoadingVCXSolution(object sender, EventArgs e)
        {
            Console.Clear();
            DisplayProgram();

            if (ShapeProject != null)
                DisplayShapingProcess();

            Output.WriteLine(ConsoleColor.Red, "Parsing VCX Solution");
            Output.WriteLine(ConsoleColor.Red, ConsoleExtensions.WindowFill('-'));
        }

        void OnLoadingVCXProject(object sender, MVCXProject mvcxProject)
        {
            Console.Clear();
            DisplayProgram();

            if (ShapeProject != null)
                DisplayShapingProcess();

            Output.WriteLine(ConsoleColor.Green, "Parsing VCX Project {0}", mvcxProject.Name);

            Output.WriteLine(ConsoleColor.Green, ConsoleExtensions.WindowFill('-'));
        }

        void OnLoadingVCXModule(object sender, VCXModule vcxModule)
        {
            Console.Clear();
            DisplayProgram();

            if (ShapeProject != null)
                DisplayShapingProcess();

            Output.WriteLine(ConsoleColor.Green, "Parsing VCX Module {0}", vcxModule.Name);

            Output.WriteLine(ConsoleColor.Green, ConsoleExtensions.WindowFill('-'));
        }

        void OnSavingShapedFile(object sender, string filePath)
        {
            return;

            Output.WriteLine(ConsoleColor.Green, " {0}", filePath);
        }
    }
}
