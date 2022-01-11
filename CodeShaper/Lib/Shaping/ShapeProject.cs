// System Namespaces
using System;
using System.IO;
using System.Collections.Generic;


// Application Namespaces
using Lib.Configurations;


// Library Namespaces
using PCRE;


namespace Lib.Shaping
{
    public class ShapeProject
    {
        public ShapeProjectConfiguration Configuration;
        public readonly List<ShapePatch> Patches;
        
        public event EventHandler<string> SavingShapedFile;

        public ShapeProject(ShapingConfiguration shapingConfiguration)
        {
            Patches = ParsePatches(shapingConfiguration.ShapeProjectDirectory);
        }

        private List<ShapePatch> ParsePatches(string projectDirectory)
        {
            var patches = new List<ShapePatch>();

            if (!Directory.Exists(projectDirectory))
                throw new Exception();

            var configurationPath = projectDirectory + @"\settings.hjson";
            Configuration = new(configurationPath);

            var projectsPath = projectDirectory + @"\projects\";

            foreach (string file in Directory.EnumerateFiles(projectDirectory, "projects/*.hjson", SearchOption.AllDirectories))
            {
                var shapePath = new ShapePatch(file);

                patches.Add(shapePath);
            }

            return patches;
        }
        
        /*
        public List<ShapeResult> Shape(VCXSolution vcxProject, ShapingConfiguration config)
        {
            var results = new List<ShapeResult>();

            foreach (var module in vcxProject.Modules)
            {
                SavingShapedFile?.Invoke(this, module.FilePath);
                results.Add(module.Result);
                module.Result.Save(module.FilePath, config);
            }

            return results;
        }
        */

        internal bool ShouldLoad(string module)
        {
            foreach (var patch in Patches)
            {
                var match = PcreRegex.IsMatch(module, patch.Patch.FileSearch);

                if (match)
                    return true;
            }

            return false;
        }

        public void Load()
        {
        }
    }

    public static class ShapeProjectExtensions
    {
        public static int TotalReplacementCount(this ShapeProject shapeProject)
        {
            int count = 0;

            foreach (var patch in shapeProject.Patches)
                count += patch.Patch.Actions.Replacements.Count;

            return count;
        }

        public static int TotalAdditionsCount(this ShapeProject shapeProject)
        {
            int count = 0;

            foreach (var patch in shapeProject.Patches)
                count += patch.Patch.Actions.Additions.Count;

            return count;
        }

        public static int TotalSubtractionsCount(this ShapeProject shapeProject)
        {
            int count = 0;

            foreach (var patch in shapeProject.Patches)
                count += patch.Patch.Actions.Subtractions.Count;

            return count;
        }
    }
}
