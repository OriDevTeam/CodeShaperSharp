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
        public static List<ShapingConfiguration> GetLocalShapingConfigurations()
        {
            var configsDir = @"configs\";

            var configurations = new List<ShapingConfiguration>();

            foreach (string file in Directory.EnumerateFiles(configsDir, "*.hjson", SearchOption.AllDirectories))
                configurations.Add(ShapingConfiguration.Load(file));

            return configurations;
        }

        public static void RunShapingConfiguration(ConsoleProgram program, ShapingConfiguration configuration)
        {
            program.Make(configuration);
        }
    }
}
