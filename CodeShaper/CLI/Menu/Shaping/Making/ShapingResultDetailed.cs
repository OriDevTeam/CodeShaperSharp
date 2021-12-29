// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shaping;
using Lib.Projects;
using CLI.Utility.Extensions;


// Library Namespaces
using EasyConsole;


namespace CLI.Menu.Shaping.Making
{
    internal class ShapingResultDetailed : MenuPage
    {
        public static VCXSolution VCXProject;
        public static ShapeProject ShapeProject;

        public ShapingResultDetailed(ConsoleProgram program) : base("Changes detailed", program)
        {

        }

        public override void Display()
        {
            DisplayProgram();
            DisplayResult();
            Console.WriteLine();
            this.InputOptions(Menu);
        }

        public void DisplayProgram()
        {
            ((ConsoleProgram)Program).Display();

            var ptr = typeof(Page).GetMethod("Display").MethodHandle.GetFunctionPointer();
            var basedisplay = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
            basedisplay();
        }

        public void DisplayResult()
        {
            DisplayModules();
        }

        public void DisplayModules()
        {
            int idx = 0;

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

                    ConsoleExtensions.Write(ConsoleColor.Gray, "  " + idx + ". - " + mod.Name + "\n");
                    modules.Add(mod);
                }
            }

            Console.WriteLine();
            Input.ReadInt("Type the number referent to module", 0, VCXProject.Modules.Count);
        }
    }
}
