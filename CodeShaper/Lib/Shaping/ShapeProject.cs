﻿// System Namespaces
using System;
using System.IO;
using System.Collections.Generic;


// Application Namespaces
using Lib.Configurations;
using Lib.Shapers.CPP;
using Lib.Shapers.Interfaces;
using Lib.Shaping.Interfaces;


// Library Namespaces
using PCRE;


namespace Lib.Shaping
{
    public class ShapeProject : IShapeProject
    {
        public Type Patch => typeof(CPPPatch);
        
        public ShapeProjectConfiguration Configuration;
        public readonly List<IShapePatch<Enum>> Patches;

        public event EventHandler<string> SavingShapedFile;

        public ShapeProject(ShapingConfiguration shapingConfiguration)
        {
            Patches = ParsePatches(shapingConfiguration.ShapeProjectDirectory);
        }

        private List<IShapePatch<Enum>> ParsePatches(string projectDirectory)
        {
            var patches = new List<IShapePatch<Enum>>();

            if (!Directory.Exists(projectDirectory))
                throw new Exception();

            var configurationPath = projectDirectory + @"\settings.hjson";
            Configuration = new ShapeProjectConfiguration(configurationPath);

            var projectsPath = projectDirectory + @"\projects\";

            foreach (var file in Directory.EnumerateFiles(projectDirectory, "projects/*.hjson", SearchOption.AllDirectories))
            {
                var shapePath = Activator.CreateInstance(Patch, file) as IShapePatch<Enum>;

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
                var match = PcreRegex.IsMatch(module, patch.Header.FileSearch);

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
                count += patch.Header.Actions.Replacers.Count;

            return count;
        }

        public static int TotalAdditionsCount(this ShapeProject shapeProject)
        {
            int count = 0;

            foreach (var patch in shapeProject.Patches)
                count += patch.Header.Actions.Adders.Count;

            return count;
        }

        public static int TotalSubtractionsCount(this ShapeProject shapeProject)
        {
            int count = 0;

            foreach (var patch in shapeProject.Patches)
                count += patch.Header.Actions.Subtracters.Count;

            return count;
        }
    }
}
