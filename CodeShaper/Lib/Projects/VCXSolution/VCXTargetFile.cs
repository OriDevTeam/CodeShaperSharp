// System Namespaces
using System;
using System.IO;


// Application Namespaces
using Lib.Shaping;
using Lib.Shapers;
using Lib.Shapers.Patch;
using Lib.Shaping.Result;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces


namespace Lib.Projects.VCXSolution
{
    public class VCXTargetFile : IShapingTargetFile
    {
        public string Name { get; }
        
        public ShapePatchFile ShapePatchFile { get; }
        public IShapingTargetGroup Parent { get; }

        public event EventHandler<IShapingTargetFile> OnShapingTargetFileLoad;

        public string FilePath;
        public string TargetFilePath;
        
        public string FileContent { get; set; }
        
        public ShapeResult Result { get; }
        

        private readonly ShapingOperation ShapingOperation;
        
        public VCXTargetFile(ShapingOperation shapingOperation, ShapePatchFile shapePatch, string filePath, IShapingTargetGroup parentGroup)
        {
            Name = Path.GetFileName(filePath);
            FilePath = filePath;

            FileContent = File.ReadAllText(filePath);
            
            Parent = parentGroup;
            
            ShapingOperation = shapingOperation;
            ShapePatchFile = shapePatch;

            Result = new ShapeResult(ShapingOperation.ShapeProject, FilePath);
            
            LoadFile();
        }
        
        public void LoadFile()
        {
            ShapePatchFile.PreparationController.Prepare(Result.FileContent);
            ShapePatchFile.PreparationController.ASTSet.Visitor.VisitorController.OnVisitorProcess += Result.VisitorProcess;
        }
    }
}
