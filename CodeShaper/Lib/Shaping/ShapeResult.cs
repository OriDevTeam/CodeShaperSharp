// System Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using Lib.AST;

// Application Namespaces
using Lib.Shaping.Operations;
using Lib.AST.Interfaces;
using Lib.Shapers.Interfaces;


// Library Namespaces



namespace Lib.Shaping
{
    public partial class ShapeResult
    {
        public string FilePath { get; }
        public string FileName { get; set; }
        public string FileContent { get; set; }
        
        private ShapeProject ShapeProject { get; }

        public List<IShapeActionsBuilder> Builders { get; set; } = new();

        public List<Tuple<IShapeActionsReplacer, string, string>> Replacements { get; set; } = new();
        public List<Tuple<IShapeActionsAdder, string, string>> Additions { get; set; } = new();
        public List<Tuple<IShapeActionsSubtracter, string, string>> Subtractions { get; set; } = new();
        


        public ShapeResult(ShapeProject shapeProject, string filePath)
        {
            ShapeProject = shapeProject;

            FilePath = filePath;
            FileName = Path.GetFileName(filePath);;
            FileContent = File.ReadAllText(filePath);

            Builders = Building.GetTopBuilders(FileName, ShapeProject.Patches);
        }

        public void VisitorProcess(object sender, Enum location)
        {
            var visitorController = (ASTVisitorController<Enum>)sender;
            var context = visitorController.LocationsContent[location];
            
            foreach (var builder in Builders)
            {
                if (builder.ProcessBuilder(visitorController.PreparationController.Visitor, location))
                    ProcessBuilderReplacementsAdditionsSubtractions(builder, visitorController.PreparationController.Visitor, location);
            }

            ProcessReplacementsAdditionsSubtractions(visitorController.PreparationController.Visitor, context, location);
        }


        private void ProcessBuilderReplacementsAdditionsSubtractions(
            IShapeActionsBuilder builder, IASTVisitor visitor, Enum location)
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
