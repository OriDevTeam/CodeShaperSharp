// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.Shaping;


// Library Namespaces
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using PCRE;


namespace Lib.AST.ANTLR.CPP14
{
    public class CPPModuleVisitor : CPP14ParserBaseVisitor<object>
    {
        AntlrInputStream InputStream;
        ShapeResult Result;

        CPPModule Module = new();

        bool invalid = false;

        public CPPModuleVisitor(ShapeProject shapeProject, ShapeResult result, AntlrInputStream inputSteam, CommonTokenStream tokenStream)
        {
            Result = result;
            InputStream = inputSteam;
        }

        public override object VisitErrorNode(IErrorNode node)
        {
            invalid = true;
            Console.WriteLine("ERROR: Module cannot be parsed");

            return base.VisitErrorNode(node);
        }

        public override object Visit(IParseTree tree)
        {
            var con = InputStream.GetText(new Interval(0, InputStream.Size));

            Module.Dictionary[Location.Module] = con;

            Result.Process(Module, con, Location.Module);

            CustomVisitInclude(con);

            return base.Visit(tree);
        }

        public override object VisitTranslationUnit([NotNull] CPP14Parser.TranslationUnitContext context)
        {
            if (invalid)
                return null;

            return base.VisitTranslationUnit(context);
        }

        public void CustomVisitInclude(string module)
        {
            var reg = new PcreRegex(@"#\s*+include\s*+[""<](.*?)["">]");

            var matches = reg.Matches(module);

            foreach (var match in matches)
            {
                Module.Dictionary[Location.Include] = match.Value;

                Result.Process(Module, match.Value, Location.Include);
            }
        }
        
        public override object VisitDeclaration([NotNull] CPP14Parser.DeclarationContext context)
        {
            var con = GetOriginalText(context);  

            Module.Dictionary[Location.Declaration] = con;

            Result.Process(Module, con, Location.Declaration);
        
            return base.VisitDeclaration(context);
        }

        public override object VisitDeclarationStatement([NotNull] CPP14Parser.DeclarationStatementContext context)
        {
            var con = GetOriginalText(context);

            Module.Dictionary[Location.DeclarationStatement] = con;

            Result.Process(Module, con, Location.DeclarationStatement);

            return base.VisitDeclarationStatement(context);
        }
        public override object VisitFunctionDefinition([NotNull] CPP14Parser.FunctionDefinitionContext context)
        {
            if (invalid)
                return null;

            var con = GetOriginalText(context);
            var dcon = GetOriginalText(context.functionBody());

            Module.Dictionary[Location.Function] = con;
            Module.Dictionary[Location.FunctionDefinition] = con.Substring(0, con.Length - dcon.Length);

            Result.Process(Module, con, Location.Function);
            Result.Process(Module, con, Location.FunctionDefinition);

            return base.VisitFunctionDefinition(context);
        }

        public override object VisitFunctionBody([NotNull] CPP14Parser.FunctionBodyContext context)
        {
            if (invalid)
                return null;

            var cone = context.GetText();
            var con = GetOriginalText(context);

            var body = con.Substring(1, con.Length - 2);
            Module.Dictionary[Location.FunctionBody] = body;

            Result.Process(Module, body, Location.FunctionBody);

            return base.VisitFunctionBody(context);
        }
        public override object VisitCondition([NotNull] CPP14Parser.ConditionContext context)
        {
            var con = GetOriginalText(context);

            Module.Dictionary[Location.MethodCondition] = con;

            Result.Process(Module, con, Location.MethodCondition);

            return base.VisitCondition(context);
        }

        public override object VisitConditionalExpression([NotNull] CPP14Parser.ConditionalExpressionContext context)
        {
            return base.VisitConditionalExpression(context);
        }

        public override object VisitAbstractDeclarator([NotNull] CPP14Parser.AbstractDeclaratorContext context)
        {
            var con = context.GetText();

            return base.VisitAbstractDeclarator(context);
        }

        public override object VisitDeclarator([NotNull] CPP14Parser.DeclaratorContext context)
        {
            var con = context.GetText();

            return base.VisitDeclarator(context);
        }

        private string GetOriginalText(ParserRuleContext context)
        {
            var start = context.Start.StartIndex;

            var stop = -1;
            if (context.Stop == null)
                stop = context.Start.StopIndex;
            else
                stop = context.Stop.StopIndex;


            if (start > stop)
                return "";

            var interval = new Interval(start, stop);
            return InputStream.GetText(interval);
        }
    }

    public class CPPModule
    {
        public Dictionary<Location, string> Dictionary = new();
    }
}
