// System Namespaces
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

        public void Shape()
        {
            
        }
        
        /*
        private IShapingTargetFile Next()
        {
            if (CurrentTargetFile == null)
                return Next(ShapingOperation.ShapingTarget.ShapingTargetGroups[0]);

            var next = CurrentTargetFile.Parent.ShapingTargetFiles.Next(x => false);

            return next ?? Next(CurrentTargetFile.Parent);
        }

        private IShapingTargetFile NextGroup(IShapingTargetGroup group)
        {
            while (true)
            {
                if (group.ParentGroup == null)
                    @group = @group?.ShapingTargetGroups.FirstOrDefault();
                    
                if (@group is { ShapingTargetFiles: { } })
                    return Next(group.ParentGroup);
                
            }
        }
        */
        /*
        
        private IShapingTargetFile Next()
        {
            IShapingTargetGroup group = null;
            IShapingTargetFile targetFile = null;
            
            if (CurrentTargetFile == null)
            {
                group = NextGroup(null);
                targetFile = group.ShapingTargetFiles.FirstOrDefault();
            }
            
            return targetFile;
        }
        
        
        private IShapingTargetFile NextFile(IShapingTargetFile file)
        {
            
        }
        
        private IShapingTargetGroup NextGroup()
        {
            return NextGroup(ShapingOperation.ShapingTarget.ShapingTargetGroups.FirstOrDefault());
        }
        
        private IShapingTargetGroup NextGroup(IShapingTargetGroup group)
        {
            IShapingTargetGroup grp;
            
            grp = CurrentTargetFile.Parent.ShapingTargetGroups.Next(x => x == CurrentTargetFile.Parent);
            
            if (grp != null && grp.ShapingTargetFiles.Count < 1)
                return NextGroup(grp);

            return grp;
        }
        
        */


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
                if (CurrentTargetFile == null)
                    return VisitorState.Stop;
                
                return CurrentTargetFile.ShapePatch.PreparationController.ASTSet.Visitor.VisitorController.State;
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

                var visitorController = CurrentTargetFile.ShapePatch.PreparationController.ASTSet.Visitor.VisitorController;

                visitorController.State = value;
            }
        }
        
    }
}