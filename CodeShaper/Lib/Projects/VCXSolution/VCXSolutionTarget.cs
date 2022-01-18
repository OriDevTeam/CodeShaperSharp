// System Namespaces
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Settings;
using Lib.Shaping;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces


namespace Lib.Projects.VCXSolution
{
    public partial class VCXSolutionTarget : IShapingTarget
    {
        public string Name => SolutionInformation.Name;
        
        public ObservableCollection<IShapingTargetGroup> ShapingTargetGroups { get; } = new();
        public IShapingTargetFile SelectedTargetFile { get; private set; }

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

        public void Shape(VCXTargetFile target)
        {
            SelectedTargetFile = target;
        }
    }
}
