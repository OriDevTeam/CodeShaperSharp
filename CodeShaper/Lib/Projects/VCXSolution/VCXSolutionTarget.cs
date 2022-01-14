// System Namespaces
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.AST;
using Lib.AST.ANTLR.CPP;
using Lib.Settings;
using Lib.Shaping;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces


namespace Lib.Projects.VCXSolution
{
    public class VCXSolutionTarget : IShapingTarget
    {
        public string Name => SolutionInformation.Name;
        
        public ObservableCollection<IShapingTargetGroup> ShapingTargetGroups { get; } = new();

        public readonly MVCXSolution SolutionInformation;
        public readonly ShapeProject ShapeProject;
        private readonly ASTPreparationController<CPP14Lexer, CPP14Parser, CPPASTVisitor> preparationController;

        public VCXSolutionTarget(string solutionPath, ShapeProject shapeProject)
        {
            ShapeProject = shapeProject;
            SolutionInformation = new MVCXSolution(solutionPath);
            preparationController = new ASTPreparationController<CPP14Lexer, CPP14Parser, CPPASTVisitor>();
        }

        public void Load()
        {
            ShapingTargetGroups.Add(new VCXSolutionGroup(this));
            
        }
        
        public void Shape(VCXModuleFile moduleFile)
        {
            preparationController.Prepare(moduleFile.Result.FileContent);
            preparationController.Visitor.VisitorController.OnVisitorProcess += moduleFile.Result.VisitorProcess;
        }
    }
}
