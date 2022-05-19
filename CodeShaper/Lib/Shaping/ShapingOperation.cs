// System Namespaces
using System.Diagnostics;


// Application Namespaces
using Lib.Configurations;
using Lib.Projects.VCXSolution;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces
using Serilog;


namespace Lib.Shaping
{
    public partial class ShapingOperation
    {
        public ShapeProject ShapeProject { get; }
        public IShapingTarget ShapingTarget { get; }
        
        public ShapingConfiguration ShapingConfiguration { get; }
    
        public ShapingOperationsController OperationsController { get; }
        
        public Stopwatch Stopwatch { get; } = new();

        public ShapingOperation(ShapingConfiguration shapingConfiguration, ShapeProject shapeProject)
        {
            ShapingConfiguration = shapingConfiguration;

            ShapeProject = shapeProject;
            ShapingTarget = new VCXSolutionTarget(this);
            
            OperationsController = new ShapingOperationsController(this);
            
            Log.Information($"Ready to start shapping");
        }
        
        public void Start()
        {
            Log.Information($"Started Shaping {ShapeProject.Configuration.Configuration.Name}");
            ShapingTarget.Load();
            
            Log.Information($"Loaded Shape Project and Shaping Target");
        }
        
        private void Stop()
        {
            Log.Information($"Stopped Shaping {ShapeProject.Configuration.Configuration.Name}");
            Stopwatch.Stop();
        }
        
    }
}
