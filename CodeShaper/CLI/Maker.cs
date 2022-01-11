// System Namespaces
using System.Collections.Generic;
using System.IO;


// Application Namespaces
using Lib.Shapers;
using Lib.Configurations;
using Lib.Projects;
using Lib.Shaping;



// Library Namespaces


namespace CLI
{
    internal static class MakerTest
    {
        public static void RunShapingConfiguration(ConsoleProgram program, ShapingConfiguration configuration)
        {
            program.Make(configuration);
        }
    }
}
