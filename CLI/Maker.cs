// System Namespaces
using System.Collections.Generic;
using System.IO;


// Application Namespaces
using Lib.Shapers;
using Lib.Configurations;
using Lib.Projects;



// Library Namespaces


namespace CLI
{
    internal static class MakerTest
    {
        public static List<RapidConfiguration> GetRapidConfigurations()
        {
            var rapidsDir = @"rapids\";

            var configurations = new List<RapidConfiguration>();

            foreach (string file in Directory.EnumerateFiles(rapidsDir, "*.hjson", SearchOption.AllDirectories))
                configurations.Add(RapidConfiguration.Load(file));

            return configurations;
        }

        public static void RunShapingConfiguration(ConsoleProgram program, RapidConfiguration configuration)
        {
            var shapingConfiguration = new ShapingConfiguration()
            {
                ResultOptions = ResultOptions.CreateNew,
                ShapeProjectDirectory = configuration.ShapeProjectPath,
                SourceDirectory = configuration.SourcePath,
                TargetDirectory = configuration.Target,
            };

            program.Make(shapingConfiguration);
        }
    }

    internal static class Maker
    {
        public static void ShapeSource(ShapingConfiguration shapingConfiguration)
        {
            var shapeProject = new ShapeProject(shapingConfiguration.ShapeProjectDirectory);
            var vcxSolution = new VCXSolution(shapingConfiguration.SourceDirectory, shapeProject);
        }

        public static void ShapeSource(ShapeProject shapeProject, VCXSolution vcxSolution)
        {
        }
    }
}
