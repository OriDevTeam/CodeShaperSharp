// System Namespaces
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;


// Application Namespaces
using Lib.Projects;
using Lib.Configurations;


// Library Namespaces


namespace Lib.Shapers
{
    public class ShapeProject
    {
        public ShapeProjectConfiguration Configuration;
        public List<ShapePatch> Patches = new();

        public event EventHandler<string> SavingShapedFile;

        public ShapeProject(string projectDirectory)
        {
            Patches = ParsePatches(projectDirectory);
        }

        public List<ShapePatch> ParsePatches(string projectDirectory)
        {
            var patches = new List<ShapePatch>();

            if (!Directory.Exists(projectDirectory))
                return patches;

            var configurationPath = projectDirectory + @"\_settings\default.hjcfg";
            Configuration = new(configurationPath);

            var projectsPath = projectDirectory + @"\projects\";

            foreach (string file in Directory.EnumerateFiles(projectDirectory, "*.hjson", SearchOption.AllDirectories))
            {
                var shapePath = new ShapePatch(file);

                patches.Add(shapePath);
            }

            return patches;
        }

        public List<CPPResult> Shape(VCXSolution vcxProject, ShapingConfiguration config)
        {
            var results = new List<CPPResult>();

            foreach (var module in vcxProject.Modules)
            {
                SavingShapedFile?.Invoke(this, module.FilePath);
                results.Add(module.Result);
                module.Result.Save(module.FilePath, config);
            }

            return results;
        }


        internal bool ShouldLoad(string module)
        {
            foreach (var patch in Patches)
            {
                bool match = Regex.IsMatch(module, patch.Patch.FileSearch);

                if (match)
                    return true;
            }

            return false;
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
