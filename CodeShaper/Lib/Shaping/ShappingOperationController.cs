﻿// System Namespaces
using System.Linq;


// Application Namespaces
using Lib.AST.Controllers;
using Lib.Shaping.Target.Interfaces;
using Lib.Utility.Extensions;


// Library Namespaces



namespace Lib.Shaping
{
    public class ShapingOperationsController
    {
        public ShapingOperation ShapingOperation { get; }
        
        public IShapingTargetFile CurrentTargetFile { get; set; }

        public ShapingOperationsController(ShapingOperation shapingOperation)
        {
            ShapingOperation = shapingOperation;
        }


        private IShapingTargetFile Next()
        {
            if (CurrentTargetFile == null)
                return NextFile(ShapingOperation.ShapingTarget.ShapingTargetGroups.FirstOrDefault());

            return NextFile(null);
        }
        
        private IShapingTargetFile NextFile(IShapingTargetGroup group)
        {
            if (group.ShapingTargetFiles.Count < 1)
                return NextFile(NextGroup(group));

            return group.ShapingTargetFiles.FirstOrDefault();
        }

        private IShapingTargetGroup NextGroup(IShapingTargetGroup group)
        {
            if (group.ParentGroup == null)
                return group.ShapingTargetGroups.First();

            var grp = group.ParentGroup.ShapingTargetGroups.Next(x => x == group);
            return grp;
        }
        
        public VisitorState VisitorState 
        {
            get
            {
                return CurrentTargetFile == null ?
                    VisitorState.Stop :
                    CurrentTargetFile.ShapePatchFile.PreparationController.ASTSet.Visitor.VisitorController.State;
            }
            set
            {
                if (CurrentTargetFile == null)
                {
                    var nextTargetFile = CurrentTargetFile = Next();

                    if (nextTargetFile == null)
                        return;

                    CurrentTargetFile = nextTargetFile;
                }

                var visitorController = CurrentTargetFile.ShapePatchFile.PreparationController.ASTSet.Visitor.VisitorController;

                visitorController.RequestState = value;
            }
        }
        
    }
}
