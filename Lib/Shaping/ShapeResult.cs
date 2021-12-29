// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.AST.ANTLR;
using Lib.Shapers.CPP;
using Lib.Shaping.Operations;


// Library Namespaces



namespace Lib.Shaping
{
    public partial class ShapeResult
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }

        public Dictionary<string, Builder> Builders { get; set; } = new();

        public List<Tuple<KeyValuePair<string, Replacement>, string, string>> Replacements { get; set; } = new();
        public List<Tuple<KeyValuePair<string, Addition>, string, string>> Additions { get; set; } = new();
        public List<Tuple<KeyValuePair<string, Subtraction>, string, string>> Subtractions { get; set; } = new();

        public ShapeProject ShapeProject { get; set; }


        public ShapeResult(ShapeProject shapeProject, string fileContent, string fileName)
        {
            ShapeProject = shapeProject;
            FileName = fileName;
            FileContent = fileContent;

            Builders = Building.GetTopBuilders(FileName, ShapeProject);
        }

        public void Process(CPPModule module, string context, Location location)
        {
            foreach (var builder in Builders)
            {
                if (Building.ProcessBuilder(builder, module, context, location))
                    ProcessBuilderReplacementsAdditionsSubtractions(builder, module, context, location);
            }

            ProcessReplacementsAdditionsSubtractions(module, context, location);
        }

        public void ProcessBuilderReplacementsAdditionsSubtractions(
            KeyValuePair<string, Builder> builder,
            CPPModule module, string context, Location location)
        {
            var activeBuilder = builder;

            if (builder.Value.ActiveBuilder.Value != null)
                activeBuilder = builder.Value.ActiveBuilder;

            if (activeBuilder.Value.Actions == null)
                return;



            throw new Exception();
        }

        private void ProcessReplacementsAdditionsSubtractions(CPPModule module, string context, Location location)
        {
            var newContext = context;

            foreach (var replacement in Replacing.ProcessReplacing(ref newContext, module, FileName, ShapeProject, location))
                Replacements.Add(new Tuple<KeyValuePair<string, Replacement>, string, string>(replacement, context, newContext));

            var addContext = newContext;

            foreach (var addition in Adding.ProcessAdding(ref addContext, module, FileName, ShapeProject, location))
                Additions.Add(new Tuple<KeyValuePair<string, Addition>, string, string>(addition, newContext, addContext));

            var subContext = addContext;

            foreach (var subtraction in Subtracting.ProcessSubtractions(ref subContext, module, FileName, ShapeProject, location))
                Subtractions.Add(new Tuple<KeyValuePair<string, Subtraction>, string, string>(subtraction, addContext, subContext));

            if (context == subContext)
                return;

            FileContent = FileContent.Replace(context, subContext);
        }

        public bool HasChanges()
        {
            if (Replacements.Count > 0)
                return true;

            if (Additions.Count > 0)
                return true;

            if (Subtractions.Count > 0)
                return true;

            return false;
        }
    }
}
