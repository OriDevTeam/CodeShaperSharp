// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.AST.Interfaces;
using Lib.Shapers.Interfaces;
using Lib.Shaping.Operations;


// Library Namespaces



namespace Lib.Shaping
{
    public class NewShapeResult<TLocation> where TLocation : Enum
    {
        private string FilePath { get; }
        private ShapeProject ShapeProject { get; }

        private List<IShapeActionsBuilder> Builders { get; } = new();
        
        public List<Tuple<IShapeActionsReplacer, string, string>> Replacements { get; } = new();
        public List<Tuple<IShapeActionsAdder, string, string>> Additions { get; } = new();
        public List<Tuple<IShapeActionsSubtracter, string, string>> Subtractions { get; } = new();

        
        public NewShapeResult(ShapeProject shapeProject, string filePath)
        {
            FilePath = filePath;
            ShapeProject = shapeProject;
        }
        
        public void VisitorProcess(object sender, TLocation location)
        {
            var visitor = (IASTVisitor)sender;
            var context = visitor.VisitorController.LocationsContent[location];
            
            foreach (var builder in Builders)
            {
                if (builder.ProcessBuilder(visitor, location))
                    ProcessBuilderReplacementsAdditionsSubtractions(builder, visitor, location);
            }

            // ProcessReplacementsAdditionsSubtractions(visitor, context, location);
        }

        private static void ProcessBuilderReplacementsAdditionsSubtractions(
            IShapeActionsBuilder builder,
            IASTVisitor visitor,
            TLocation location)
        {
            var activeBuilder = builder;

            if (builder.ActiveBuilder != null)
                activeBuilder = builder.ActiveBuilder;

            if (activeBuilder.Actions == null)
                return;

            throw new Exception();
        }
    }
}
