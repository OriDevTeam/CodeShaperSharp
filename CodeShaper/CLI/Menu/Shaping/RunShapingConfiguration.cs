// System Namespaces
using System;


// Application Namespaces
using Lib.Managers;
using Lib.Shaping;


// Library Namespaces
using EasyConsole;


namespace CLI.Menu.Shaping
{
    internal class RunShapingConfiguration : MenuPage
    {
        public RunShapingConfiguration(Program program) : base("Run Shaping Configuration", program)
        {
        }

        public override void Display()
        {
            ConsoleProgram.Display();

            var ptr = typeof(Page).GetMethod("Display")!.MethodHandle.GetFunctionPointer();
            var baseDisplay = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
            baseDisplay?.Invoke();

            DisplayShapingConfigurations();
        }

        private void DisplayShapingConfigurations()
        {
            var configurations = ShapingConfigurationsManager.LocalShapingConfigurations;

            var idx = 0;
            foreach (var configuration in configurations)
            {
                idx++;
                Output.WriteLine(ConsoleColor.Yellow, idx + ". " + configuration.Item1.Name);
            }

            var (config, shapeProject) = configurations[Input.ReadInt("Which one to run?: ", 0, configurations.Count) - 1];
            
            var shapingOperation = new ShapingOperation(config, shapeProject);
                
            ShapingOperationsManager.AddShapingOperations(shapingOperation);
            ShapingOperationsManager.SetShapingOperations(shapingOperation);
            
            ((ConsoleProgram)Program).Make(config, shapeProject);
        }
    }
}
