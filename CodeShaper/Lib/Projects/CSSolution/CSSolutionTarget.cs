// System Namespaces
using System;
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Shaping;
using Lib.Settings.Target;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces



namespace Lib.Projects.CSSolution
{
    public class CSSolutionTarget : IShapingTarget
    {
        public string Name => SolutionInformation.Name;
        public ObservableCollection<IShapingTargetGroup> ShapingTargetGroups { get; }

        private readonly MVCXSolution SolutionInformation;
        private readonly ShapingOperation ShapingOperation;
        
        public CSSolutionTarget(ShapingOperation shapingOperation)
        {
            ShapingOperation = shapingOperation;
            SolutionInformation = new MVCXSolution(ShapingOperation.ShapingConfiguration.SourceDirectory);
        }
        
        public void Load()
        {
            throw new NotImplementedException();
        }

        public void AddGroupsLoadEvent(EventHandler<IShapingTargetGroup> onLoadingShapingTargetGroup)
        {
            throw new NotImplementedException();
        }

        public void AddFilesLoadEvent(EventHandler<IShapingTargetFile> onLoadingShapingTargetFile)
        {
            throw new NotImplementedException();
        }
    }
}
