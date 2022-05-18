// System Namespaces
using System.Runtime.Serialization;


// Application Namespaces
using Lib.Shapers.Loaders;
using Lib.AST.ANTLR.CSharp6;
using Lib.AST.Controllers;


// Library Namespaces



namespace Lib.Shapers.Patches
{
    public class CSharp6Patch : ShapePatchJSON<CSharp6Locations>
    {
        public CSharp6Patch(string filePath) : base(filePath)
        {
            PreparationController = new ASTPreparationController(new CSharpASTSet());
        }
    }
    
    public enum CSharp6Locations
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "module")]
        Module,

        [EnumMember(Value = "include")]
        Include,

        [EnumMember(Value = "var")]
        ModuleVariable,

        [EnumMember(Value = "var.def")]
        ModuleVariableDefinition,

        [EnumMember(Value = "declaration")]
        Declaration,

        [EnumMember(Value = "declaration.statement")]
        DeclarationStatement,

        [EnumMember(Value = "method")]
        Function,

        [EnumMember(Value = "method.def")]
        FunctionDefinition,

        [EnumMember(Value = "method.body")]
        FunctionBody,

        [EnumMember(Value = "method.condition")]
        MethodCondition,
    }
}
