// System Namespaces
using System;


// Application Namespaces
using Lib.Shaping.Interfaces;
using Lib.Shaping.Target.Interfaces;
using CLI.Utility.Extensions;


// Library Namespaces
using EasyConsole;


namespace CLI.Menu.Shaping.Making
{
    internal class ShapingResultDetailed : MenuPage
    {
        public ShapingResultDetailed(Program program) : base("Changes detailed", program)
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

        private void DisplayResult()
        {
            DisplayModules();
        }

        private static void DisplayModules()
        {
            var idx = 0;
            
            /*
            var modules = new List<VCXModule>();

            foreach (var project in VCXProject.Projects)
            {
                var mods = new List<VCXModule>();

                if (project.Modules.Count < 1)
                    continue;
                
                foreach (var module in project.Modules)
                {
                    if (module.Result.HasChanges())
                        mods.Add(module);
                }

                if (mods.Count < 1)
                    continue;

                ConsoleExtensions.Write(ConsoleColor.Cyan, "\n" + project.Project.Name + "\n");

                foreach (var mod in mods)
                {
                    idx++;
                    ConsoleExtensions.Write(ConsoleColor.Gray, $"  {idx}. - {mod.Name}\n");
                    modules.Add(mod);
                }
            }
            */
            
            Console.WriteLine();
            // Input.ReadInt("Type the number referent to module", 0, ShapingTarget.Modules.Count);
        }
    }
}
