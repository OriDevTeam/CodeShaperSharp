// System Namespaces
using System;
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Shaping;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces


namespace Lib.Projects.VCXSolution
{
    public class VCXSolutionGroup : IShapingTargetGroup
    {
        public string Name { get; }

        public ObservableCollection<IShapingTargetGroup> ShapingTargetGroups { get; } = new();
        public ObservableCollection<IShapingTargetFile> ShapingTargetFiles { get; } = new();
        public IShapingTargetGroup ParentGroup { get; set; }

        public event EventHandler<IShapingTargetGroup> OnShapingGroupLoad;
        public event EventHandler<IShapingTargetFile> OnShapingTargetFileLoad;

        private VCXSolutionTarget SolutionTarget { get; }
        
        private ShapingOperation ShapingOperation { get; }

        public VCXSolutionGroup(ShapingOperation shapingOperation, VCXSolutionTarget solutionTarget, IShapingTargetGroup parent)
        {
            ShapingOperation = shapingOperation;
            
            SolutionTarget = solutionTarget;

            Name = SolutionTarget.SolutionInformation.Name;
            
            LoadGroups();
        }
        
        public void LoadGroups()
        {
            foreach (var project in SolutionTarget.SolutionInformation.projects)
            {
                var vcxProject = new VCXProjectGroup(ShapingOperation, SolutionTarget, project, this);
                
                OnShapingGroupLoad?.Invoke(this, vcxProject);
                
                ShapingTargetGroups.Add(vcxProject);
            }
        }
    }
}
