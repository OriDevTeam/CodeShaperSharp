// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shaping.Operations;
using Lib.AST.Interfaces;
using Lib.Shapers.Interfaces;


// Library Namespaces



namespace Lib.Shaping
{
    public partial class ShapeResult
    {
        public string FileName { get; set; }
        public string FileContent { get; set; }

        public List<IShapeActionsBuilder> Builders { get; set; } = new();

        public List<Tuple<IShapeActionsReplacer, string, string>> Replacements { get; set; } = new();
        public List<Tuple<IShapeActionsAdder, string, string>> Additions { get; set; } = new();
        public List<Tuple<IShapeActionsSubtracter, string, string>> Subtractions { get; set; } = new();

        public ShapeProject ShapeProject { get; set; }


        public ShapeResult(ShapeProject shapeProject, string fileContent, string fileName)
        {
            ShapeProject = shapeProject;
            FileName = fileName;
            FileContent = fileContent;

            Builders = Building.GetTopBuilders(FileName, ShapeProject.Patches);
        }

        public void VisitorProcess(object sender, Enum location)
        {
            var visitor = (IASTVisitor)sender;
            var context = visitor.VisitorController.LocationsContent[location];
            
            foreach (var builder in Builders)
            {
                if (builder.ProcessBuilder(visitor, location))
                    ProcessBuilderReplacementsAdditionsSubtractions(builder, visitor, context, location);
            }

            ProcessReplacementsAdditionsSubtractions(visitor, context, location);
        }

        private void ProcessBuilderReplacementsAdditionsSubtractions(
            IShapeActionsBuilder builder,
            IASTVisitor visitor, string context, Enum location)
        {
            var activeBuilder = builder;

            if (builder.ActiveBuilder != null)
                activeBuilder = builder.ActiveBuilder;

            if (activeBuilder.Actions == null)
                return;

            throw new Exception();
        }

        private void ProcessReplacementsAdditionsSubtractions(
            IASTVisitor visitor, string context, Enum location)
        {
            var patches = ShapeProject.Patches;
            
            var newContext = context;
            
            foreach (var replacer in Replacing.ProcessReplacing(ref newContext, visitor, FileName, patches, location))
                Replacements.Add(new Tuple<IShapeActionsReplacer, string, string>(replacer, context, newContext));

            var addContext = newContext;

            foreach (var addition in Adding.ProcessAdding(ref addContext, visitor, FileName, patches, location))
                Additions.Add(new Tuple<IShapeActionsAdder, string, string>(addition, newContext, addContext));

            var subContext = addContext;

            foreach (var subtracter in Subtracting.ProcessSubtractions(ref subContext, visitor, FileName, patches, location))
                Subtractions.Add(new Tuple<IShapeActionsSubtracter, string, string>(subtracter, addContext, subContext));

            if (context == subContext)
                return;

            FileContent = FileContent.Replace(context, subContext);
        }

        public bool HasChanges()
        {
            return Subtractions.Count > 0 || Replacements.Count > 0 || Additions.Count > 0;
        }
    }
}
