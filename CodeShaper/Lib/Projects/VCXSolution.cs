// System Namespaces
using System;
using System.IO;
using System.Collections.Generic;


// Application Namespaces
using Lib.Settings;
using Lib.AST.ANTLR;
using Lib.Shapers;
using Lib.Shapers.CPP;
using Lib.Shaping;



// Library Namespaces



namespace Lib.Projects
{
    public class VCXSolution
    {
        public string SolutionPath;
        public MVCXSolution SolutionInformation;
        public ShapeProject ShapeProject;

        public List<VCXModule> Modules = new();
        public List<VCXProject> Projects = new();

        public event EventHandler LoadingVCXSolution;
        public event EventHandler<MVCXProject> LoadingVCXProject;
        public event EventHandler<VCXModule> LoadingVCXModule;

        public int ModuleCount = 0;

        public VCXSolution(string solutionPath, ShapeProject shapeProject)
        {
            SolutionPath = solutionPath;
            ShapeProject = shapeProject;
            SolutionInformation = new MVCXSolution(solutionPath);
        }

        public void Load()
        {
            ParseSolution();
        }

        void ParseSolution()
        {
            LoadingVCXSolution?.Invoke(this, new EventArgs());

            foreach (var project in SolutionInformation.projects)
                Projects.Add(ParseProject(SolutionPath, project));
        }

        VCXProject ParseProject(string solutionPath, MVCXProject project)
        {
            var vcxproject = new VCXProject(project);

            LoadingVCXProject?.Invoke(this, project);

            var toLoad = new List<string>();

            foreach (var header in project.includes)
            {
                if (!ShapeProject.ShouldLoad(header))
                    continue;

                ModuleCount++;
                toLoad.Add(header);
            }

            foreach (var module in project.modules)
            {
                if (!ShapeProject.ShouldLoad(module))
                    continue;

                ModuleCount++;
                toLoad.Add(module);
            }

            foreach (var load in toLoad)
            {
                var modulePath = Path.GetDirectoryName(project.Path) + @"\" + load;

                var mod = new VCXModule(ShapeProject, modulePath, project.configurations[0]);

                LoadingVCXModule?.Invoke(this, mod);

                Modules.Add(mod);

                vcxproject.Modules.Add(mod);

                // Thread.Sleep(500);
            }

            return vcxproject;
        }
    }

    public class VCXProject
    {
        public MVCXProject Project;
        public List<VCXModule> Modules = new();

        public event EventHandler<VCXModule> LoadingVCXModule;

        public VCXProject(MVCXProject project)
        {
            Project = project;
        }
    }

    public class VCXModule
    {
        public string Name;
        public string FilePath;
        public string TargetFilePath;

        ShapeProject ShapeProject;
        public ShapeResult Result;

        public CPPModuleAST ModuleAST;

        public VCXModule(ShapeProject shapeProject, string filePath, MVCXProjectConfiguration projectConfiguration)
        {
            Name = Path.GetFileName(filePath);
            FilePath = filePath;
            ShapeProject = shapeProject;

            Result = Parse();
        }

        ShapeResult Parse()
        {
            ModuleAST = new CPPModuleAST(ShapeProject, FilePath);
            return ModuleAST.ParseAndProcessModule();
        }

    }

    public static class ReportProcessedExtensions
    {
        public static int TotalProcessedReplacementCount(this VCXSolution vcxProject)
        {
            int count = 0;

            foreach (var module in vcxProject.Modules)
                count += module.Result.Replacements.Count;

            return count;
        }

        public static int TotalProcessedAdditionCount(this VCXSolution vcxProject)
        {
            int count = 0;

            foreach (var module in vcxProject.Modules)
                count += module.Result.Additions.Count;

            return count;
        }

        public static int TotalProcessedSubtractionCount(this VCXSolution vcxProject)
        {
            int count = 0;

            foreach (var module in vcxProject.Modules)
                count += module.Result.Subtractions.Count;

            return count;
        }

        public static List<KeyValuePair<string, Replacement>> AppliedReplacements(this VCXSolution vcxProject)
        {
            var replacements = new List<KeyValuePair<string, Replacement>>();

            foreach (var module in vcxProject.Modules)
                foreach (var replacement in module.Result.Replacements)
                    if (!replacements.Contains(replacement.Item1))
                        replacements.Add(replacement.Item1);

            return replacements;
        }

        public static List<KeyValuePair<string, Addition>> AppliedAdditions(this VCXSolution vcxProject)
        {
            var additions = new List<KeyValuePair<string, Addition>>();

            foreach (var module in vcxProject.Modules)
                foreach (var addition in module.Result.Additions)
                    if (!additions.Contains(addition.Item1))
                        additions.Add(addition.Item1);

            return additions;
        }

        public static List<KeyValuePair<string, Subtraction>> AppliedSubtractions(this VCXSolution vcxProject)
        {
            var subtractions = new List<KeyValuePair<string, Subtraction>>();

            foreach (var module in vcxProject.Modules)
                foreach (var subtraction in module.Result.Subtractions)
                    if (!subtractions.Contains(subtraction.Item1))
                        subtractions.Add(subtraction.Item1);

            return subtractions;
        }
    }

    public static class ReportUnprocessedExtensions
    {
        public static List<KeyValuePair<string, Replacement>> UnapliedReplacements(this VCXSolution vcxProject)
        {
            var applied = vcxProject.AppliedReplacements();

            var replacements = new List<KeyValuePair<string, Replacement>>();

            foreach (var module in vcxProject.Modules)
                foreach (var patch in vcxProject.ShapeProject.Patches)
                {
                    foreach (var replacement in patch.Patch.Actions.Replacements)
                    {
                        if(!applied.Contains(replacement))
                            if (!replacements.Contains(replacement))
                                replacements.Add(replacement);
                    }
                }

            return replacements;
        }

        public static List<KeyValuePair<string, Addition>> UnapliedAdditions(this VCXSolution vcxProject)
        {
            var applied = vcxProject.AppliedAdditions();

            var additions = new List<KeyValuePair<string, Addition>>();

            foreach (var module in vcxProject.Modules)
                foreach (var patch in vcxProject.ShapeProject.Patches)
                {
                    foreach (var addition in patch.Patch.Actions.Additions)
                    {
                        if (!applied.Contains(addition))
                            if (!additions.Contains(addition))
                                additions.Add(addition);
                    }
                }

            return additions;
        }

        public static List<KeyValuePair<string, Subtraction>> UnapliedSubtractions(this VCXSolution vcxProject)
        {
            var applied = vcxProject.AppliedSubtractions();

            var subtractions = new List<KeyValuePair<string, Subtraction>>();

            foreach (var module in vcxProject.Modules)
                foreach (var patch in vcxProject.ShapeProject.Patches)
                {
                    foreach (var subtraction in patch.Patch.Actions.Subtractions)
                    {
                        if (!applied.Contains(subtraction))
                            if (!subtractions.Contains(subtraction))
                                subtractions.Add(subtraction);
                    }
                }

            return subtractions;
        }
    }
}
