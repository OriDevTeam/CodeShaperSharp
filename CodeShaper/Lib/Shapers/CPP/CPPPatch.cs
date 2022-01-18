// System Namespaces
using System;
using System.Runtime.Serialization;


// Application Namespaces
using Lib.Shapers.Loaders;
using Lib.AST;
using Lib.AST.ANTLR.CPP;
using Lib.AST.Interfaces;


// Library Namespaces
using Antlr4.Runtime;


namespace Lib.Shapers.CPP
{
    public class CPPPatch : ShapePatchJSON<Location>
    {
        public CPPPatch(string filePath) : base(filePath)
        {
            // var type = typeof(ASTPreparationController<CPP14Lexer, CPP14Parser, CPPASTVisitor>);
            // PreparationController = (ASTPreparationController<Lexer, Parser, IASTVisitor>)Activator.CreateInstance(type);
            PreparationController = new ASTPreparationController(typeof(CPP14Lexer), typeof(CPP14Parser), typeof(CPPASTVisitor));
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
