// System Namespaces
using System;
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Settings.Target;
using Lib.Shaping;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces


namespace Lib.Projects.VCXSolution
{
    public class VCXSolutionTarget : IShapingTarget
    {
        public string Name => SolutionInformation.Name;
        
        public ObservableCollection<IShapingTargetGroup> ShapingTargetGroups { get; } = new();
        private IShapingTargetFile SelectedTargetFile { get; set; }

        public readonly MVCXSolution SolutionInformation;

        private readonly ShapingOperation ShapingOperation;
        
        public readonly ShapeProject ShapeProject;

        public VCXSolutionTarget(ShapingOperation shapingOperation)
        {
            ShapingOperation = shapingOperation;
            
            SolutionInformation = new MVCXSolution(ShapingOperation.ShapingConfiguration.SourceDirectory);
        }

        public void Load()
        {
            ShapingTargetGroups.Add(new VCXSolutionGroup(ShapingOperation, this, null));
        }

        public void AddGroupsLoadEvent(EventHandler<IShapingTargetGroup> onLoadingShapingTargetGroup)
        {
            foreach (var group in ShapingTargetGroups) 
                AddGroupsLoadEvent(group, onLoadingShapingTargetGroup);
        }

        private static void AddGroupsLoadEvent(IShapingTargetGroup group, EventHandler<IShapingTargetGroup> onLoadingShapingTargetGroup)
        {
            foreach (var childrenGroup in group.ShapingTargetGroups)
            {
                group.OnShapingGroupLoad += onLoadingShapingTargetGroup;
                AddGroupsLoadEvent(childrenGroup, onLoadingShapingTargetGroup);
            }

            group.OnShapingGroupLoad += onLoadingShapingTargetGroup;
        }
        
        public void AddFilesLoadEvent(EventHandler<IShapingTargetFile> onLoadingShapingTargetFile)
        {
            foreach (var group in ShapingTargetGroups) 
                AddFilesLoadEvent(group, onLoadingShapingTargetFile);
        }

        private void AddFilesLoadEvent(IShapingTargetGroup group, EventHandler<IShapingTargetFile> onLoadingShapingTargetFile)
        {
            foreach (var childrenGroup in group.ShapingTargetGroups)
            {
                AddFilesLoadEvent(childrenGroup, onLoadingShapingTargetFile);
            }

            foreach (var file in group.ShapingTargetFiles)
                file.OnShapingTargetFileLoad += onLoadingShapingTargetFile;
        }
    }
}
