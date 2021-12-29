// System Namespaces
using System;
using System.Collections.Generic;
using System.Reflection;


// Application Namespaces
using CLI.Menu;
using CLI.Menu.Shaping;
using CLI.Menu.Shaping.Making;
using CLI.Utility.Extensions;
using Lib.Configurations;


// Library Namespaces
using EasyConsole;


namespace CLI
{
    class ConsoleProgram : Program
    {
        public ConsoleProgram() : base("CodeShaper", breadcrumbHeader: true)
        {
            var pages = new List<MenuPage>()
            {
                new HomePage(this),
                new ShapePage(this),
                new ShapeProjectPage(this),
                new ShapingApplyPage(this),
                new ShapingResultPage(this),
                new ShapingAppliedPage(this),
                new ShapingUnappliedPage(this),
                new ShapingResultDetailed(this),
                new RunShapingConfiguration(this)
            };

            foreach (var page in pages)
                AddPage(page);

            SetPage<HomePage>();
        }

        public void Display()
        {
            // Get the assembly version
            var version = Assembly.GetExecutingAssembly().GetName().Version;

            // Display CLI header information
            Output.WriteLine(ConsoleColor.Green, ConsoleExtensions.WindowFill('-'));
            Output.WriteLine(ConsoleColor.Gray, "Code Shaper".PadSides(Console.WindowWidth));
            Output.WriteLine(ConsoleColor.Gray, "v{0}".Format(version.ToString()).PadSides(Console.WindowWidth));
            Output.WriteLine(ConsoleColor.Green, ConsoleExtensions.WindowFill('-'));
            Output.WriteLine("");
        }

        public void Make(ShapingConfiguration shapingConfiguration)
        {
            ShapingApplyPage.ShapingConfiguration = shapingConfiguration;
            NavigateTo<ShapingApplyPage>();
        }
    }
}
