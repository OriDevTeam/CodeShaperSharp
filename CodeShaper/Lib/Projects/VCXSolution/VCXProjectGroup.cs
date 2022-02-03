// System Namespaces
using System;
using System.IO;
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Settings;
using Lib.Settings.Target;
using Lib.Shaping;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces


namespace Lib.Projects.VCXSolution
{
    public class VCXProjectGroup : IShapingTargetGroup
    {
        public string Name { get; }
        
        public ObservableCollection<IShapingTargetGroup> ShapingTargetGroups { get; } = new();
        public ObservableCollection<IShapingTargetFile> ShapingTargetFiles { get; } = new();
        public IShapingTargetGroup ParentGroup { get; set; }

        public event EventHandler<IShapingTargetGroup> OnShapingGroupLoad;
        public event EventHandler<IShapingTargetFile> OnShapingTargetFileLoad;

        private VCXSolutionTarget SolutionTarget { get; }
        private MVCXProject Project { get; }
        
        private ShapingOperation ShapingOperation { get; }
        
        public VCXProjectGroup(ShapingOperation shapingOperation, VCXSolutionTarget solutionTarget, MVCXProject project, IShapingTargetGroup parent)
        {
            Name = $"Project {project.Name}";

            ShapingOperation = shapingOperation;
            
            SolutionTarget = solutionTarget;
            Project = project;

            ParentGroup = parent;
            
            LoadFiles();
        }

        public void LoadGroups()
        {
            
        }

        private void LoadFiles()
        {
            foreach (var header in Project.includes)
            {
                var matchingPatch = ShapingOperation.ShapeProject.MatchingShapePatch(header);
                
                if (matchingPatch == null)
                    continue;

                var headerFilePath = Path.GetDirectoryName(Project.Path) + @"\" + header;

                var headerFile = new VCXTargetFile(ShapingOperation, matchingPatch, headerFilePath, this);
                
                ShapingTargetFiles.Add(headerFile);
                
                OnShapingTargetFileLoad?.Invoke(this, headerFile);
            }

            foreach (var module in Project.modules)
            {
                var matchingPatch = ShapingOperation.ShapeProject.MatchingShapePatch(module);
                
                if (matchingPatch == null)
                    continue;

                var moduleFilePath = Path.GetDirectoryName(Project.Path) + @"\" + module;
                
                var moduleFile = new VCXTargetFile(ShapingOperation, matchingPatch, moduleFilePath, this);
                
                ShapingTargetFiles.Add(moduleFile);
                
                OnShapingTargetFileLoad?.Invoke(this, moduleFile);
            }
        }
    }
}
