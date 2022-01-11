// System Namespaces
using System.Diagnostics;
using Lib.AST.ANTLR;
using Lib.AST.ANTLR.CPP;

// Application Namespaces
using Lib.Configurations;
using Lib.Projects.VCXSolution;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces
using Serilog;


namespace Lib.Shaping
{
    public class ShapingOperation
    {
        public ShapeProject ShapeProject { get; }
        public IShapingTarget ShapingTarget { get; }
        
        private ShapingConfiguration ShapingConfiguration { get; }

        public Stopwatch Stopwatch { get; } = new();

        public ShapingOperation(ShapingConfiguration shapingConfiguration, ShapeProject shapeProject)
        {
            ShapingConfiguration = shapingConfiguration;

            ShapeProject = shapeProject;
            ShapingTarget = new VCXSolutionTarget<CPPASTVisitor>(shapingConfiguration.SourceDirectory, shapeProject);
        }
        
        public ShapingOperation(ShapingConfiguration shapingConfiguration, IShapingTarget shapingTarget)
        {
            ShapingConfiguration = shapingConfiguration;
            
            ShapeProject = new ShapeProject(shapingConfiguration);
            ShapingTarget = shapingTarget;
        }

        public void Start()
        {
            Log.Information("Started Shaping {0}", ShapeProject.Configuration.Configuration.Name);
            Stopwatch.Start();

            ShapeProject.Load();
            ShapingTarget.Load();
        }

        private void Stop()
        {
            Stopwatch.Stop();
        }
    }
}
