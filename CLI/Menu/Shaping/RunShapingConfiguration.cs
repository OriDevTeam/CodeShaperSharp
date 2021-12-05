// System Namespaces
using System;


// Application Namespaces


// Library Namespaces
using EasyConsole;


namespace CLI.Menu.Shaping
{
    internal class RunShapingConfiguration : MenuPage
    {
        public RunShapingConfiguration(ConsoleProgram program) : base("Run Shaping Configuration", program)
        {
        }

        public override void Display()
        {
            ((ConsoleProgram)Program).Display();

            var ptr = typeof(Page).GetMethod("Display").MethodHandle.GetFunctionPointer();
            var basedisplay = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
            basedisplay();

            DisplaySome();
        }

        public void DisplaySome()
        {
            var configurations = MakerTest.GetRapidConfigurations();

            var idx = 0;
            foreach (var configuration in configurations)
            {
                idx++;
                Output.WriteLine(ConsoleColor.Yellow, idx + ". - " + configuration.Name);
            }

            var config = configurations[Input.ReadInt("Which one to run?: ", 0, configurations.Count) - 1];

            MakerTest.RunShapingConfiguration((ConsoleProgram)Program, config);
;        }
    }
}
