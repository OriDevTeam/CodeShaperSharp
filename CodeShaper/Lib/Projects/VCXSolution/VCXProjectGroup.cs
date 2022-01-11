// System Namespaces
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Settings;
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
        
        public event EventHandler<IShapingTargetGroup> OnShapingGroupLoad;
        public event EventHandler<IShapingTargetFile> OnShapingTargetFileLoad;
        
        public VCXSolutionTarget SolutionTarget { get; }
        public MVCXProject Project { get; }

        public VCXProjectGroup(VCXSolutionTarget solutionTarget, MVCXProject project)
        {
            Name = $"Project {project.Name}";

            SolutionTarget = solutionTarget;
            Project = project;

            LoadFiles();
        }

        public void LoadGroups()
        {
            
        }
        
        public void LoadFiles()
        {
            foreach (var header in Project.includes)
            {
                if (!SolutionTarget.ShapeProject.ShouldLoad(header))
                    continue;
                
                var headerFilePath = Path.GetDirectoryName(Project.Path) + @"\" + header;

                var headerFile = new VCXModuleFile(SolutionTarget.ShapeProject, headerFilePath);
                
                ShapingTargetFiles.Add(headerFile);
                
                OnShapingTargetFileLoad?.Invoke(this, headerFile);
            }

            foreach (var module in Project.modules)
            {
                if (!SolutionTarget.ShapeProject.ShouldLoad(module))
                    continue;

                var moduleFilePath = Path.GetDirectoryName(Project.Path) + @"\" + module;
                
                var moduleFile = new VCXModuleFile(SolutionTarget.ShapeProject, moduleFilePath);
                
                ShapingTargetFiles.Add(moduleFile);
                
                OnShapingTargetFileLoad?.Invoke(this, moduleFile);
            }
        }
    }
}
