// System Namespaces
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.AST;
using Lib.AST.ANTLR.CPP;
using Lib.AST.ANTLR.CPP14;
using Lib.AST.Interfaces;
using Lib.Settings;
using Lib.Shaping;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces


namespace Lib.Projects.VCXSolution
{
    public class VCXSolutionTarget<T> : IShapingTarget<IASTVisitor> where T: new()
    {
        private T Visitor = new();
        
        public ObservableCollection<IShapingTargetGroup> ShapingTargetGroups { get; } = new();

        public readonly MVCXSolution SolutionInformation;
        public readonly ShapeProject ShapeProject;
        public readonly ASTPreparationController<CPP14Lexer, CPP14Parser, CPPASTVisitor> PreparationController;

        public VCXSolutionTarget(string solutionPath, ShapeProject shapeProject)
        {
            ShapeProject = shapeProject;
            SolutionInformation = new MVCXSolution(solutionPath);
            PreparationController = new ASTPreparationController<CPP14Lexer, CPP14Parser, CPPASTVisitor>();
        }

        public void Load()
        {
            ShapingTargetGroups.Add(new VCXSolutionGroup(this));  
        }
        
        ShapeResult Parse(VCXModuleFile moduleFile)
        {
            return new CPPModuleAST(ShapeProject, moduleFile.FilePath).ParseAndProcessModule();
        }
    }
}
