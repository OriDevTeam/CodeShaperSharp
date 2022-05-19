// System Namespaces
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;


// Application Namespaces
using Lib.Configurations;
using Lib.Shapers.Interfaces;
using Lib.Shapers.Patch;
using Lib.Shapers.Patches;
using Lib.Shaping.Interfaces;


// Library Namespaces
using PCRE;


namespace Lib.Shaping
{
    public class ShapeProject : IShapeProject
    {
        public Type Patch => typeof(CPP14Patch);
        
        public ShapeProjectConfiguration Configuration;
        public readonly List<ShapePatchFile> Patches;

        public event EventHandler<string> SavingShapedFile;

        public ShapeProject(ShapingConfiguration shapingConfiguration)
        {
            Patches = ParsePatches(shapingConfiguration.ShapeProjectDirectory);
        }

        private List<ShapePatchFile> ParsePatches(string projectDirectory)
        {
            var patches = new List<ShapePatchFile>();

            if (!Directory.Exists(projectDirectory))
                throw new Exception();

            var configurationPath = projectDirectory + @"\settings.hjson";
            Configuration = new ShapeProjectConfiguration(configurationPath);

            foreach (var file in Directory.EnumerateFiles(projectDirectory, "projects/*.hjson",
                         SearchOption.AllDirectories))
            {
                patches.Add((ShapePatchFile)Activator.CreateInstance(Patch, file));
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

        internal ShapePatchFile MatchingShapePatch(string fileName)
        {
            return (from patch in Patches
                let match = PcreRegex.IsMatch(fileName, patch.Patch.FileSearch)
                where match
                select patch).FirstOrDefault();
        }
    }

    public static class ShapeProjectExtensions
    {
        public static int TotalReplacementCount(this ShapeProject shapeProject)
        {
            var count = 0;

            foreach (var patch in shapeProject.Patches)
                if (patch.Patch.Actions.Replacers != null)
                    count += patch.Patch.Actions.Replacers.Count;

            return count;
        }

        public static int TotalAdditionsCount(this ShapeProject shapeProject)
        {
            var count = 0;

            foreach (var patch in shapeProject.Patches)
                if (patch.Patch.Actions.Adders != null)
                    count += patch.Patch.Actions.Adders.Count;

            return count;
        }

        public static int TotalSubtractionsCount(this ShapeProject shapeProject)
        {
            var count = 0;

            foreach (var patch in shapeProject.Patches)
                if (patch.Patch.Actions.Subtracters != null)
                    count += patch.Patch.Actions.Subtracters.Count;

            return count;
        }
    }
}
