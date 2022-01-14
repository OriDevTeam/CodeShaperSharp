// System Namespaces
using System.Runtime.Serialization;


// Application Namespaces
using Lib.Shapers.Loaders;


// Library Namespaces


namespace Lib.Shapers.CPP
{
    public class CPPPatch : ShapePatchJSON<Location>
    {
        public CPPPatch(string filePath) : base(filePath)
        {
        }
    }

    public enum Location
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
