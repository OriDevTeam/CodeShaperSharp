// System Namespaces
using System.Runtime.Serialization;


// Application Namespaces
using Lib.AST.ANTLR.CPP14;
using Lib.AST.Controllers;
using Lib.Shapers.Loaders;


// Library Namespaces


namespace Lib.Shapers.Patches
{
    public class CPP14Patch : ShapePatchJSON<CPP14Location>
    {
        public CPP14Patch(string filePath) : base(filePath)
        {
            PreparationController = new ASTPreparationController(new CPPASTSet());
        }
    }

    public enum CPP14Location
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
