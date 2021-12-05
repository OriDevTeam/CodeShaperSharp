// System Namespaces
using System;
using System.Text.RegularExpressions;


// Application Namespaces
using Lib.Shapers;
using Lib.Shapers.CPP;


// Library Namespaces
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;

namespace Lib.AST.ANTLR
{
    public class CPPModuleVisitor : CPP14ParserBaseVisitor<object>
    {
        AntlrInputStream InputStream;
        CPPResult Result;

        CPPModule Module = new();

        bool invalid = false;

        public CPPModuleVisitor(ShapeProject shapeProject, CPPResult result, AntlrInputStream inputSteam, CommonTokenStream tokenStream)
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
            var reg = new Regex(@"#\s*?include\s*?[""<](.*?)["">]");

            var matches = reg.Matches(module);

            foreach (Match match in matches)
            {
                Module.Dictionary[Location.Include] = match.Value;

                Result.Process(Module, match.Value, Location.Include);
            }
        }

        public override object VisitFunctionDefinition([NotNull] CPP14Parser.FunctionDefinitionContext context)
        {
            if (invalid)
                return null;

            var con = GetOriginalText(context);
            var dcon = GetOriginalText(context.functionBody());

            Module.Dictionary[Location.FunctionDefinition] = con.Substring(0, con.Length - dcon.Length);

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

        /*
        private void Process(ParserRuleContext context, Location location)
        {
            var con = GetOriginalText(context);
            ShapedFile.Process(con, Location.FunctionDefinition);
        }
        */

        private string GetOriginalText(ParserRuleContext context)
        {
            var start = context.Start.StartIndex;

            var stop = -1;
            if (context.Stop == null)
                stop = context.Start.StopIndex;
            else
                stop = context.Stop.StopIndex;

            var interval = new Interval(start, stop);
            return InputStream.GetText(interval);
        }
    }

    public class CPPModule
    {
        public Dictionary<Location, string> Dictionary = new();
    }
}
