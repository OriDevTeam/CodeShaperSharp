// System Namespaces
using System;
using System.IO;


// Application Namespaces
using Lib.Shaping;
using Lib.Shaping.Target.Interfaces;
using Lib.Shapers.Interfaces;


// Library Namespaces


namespace Lib.Projects.VCXSolution
{
    public class VCXTargetFile : IShapingTargetFile
    {
        public string Name { get; }
        
        public IShapePatch ShapePatch { get; }
        public IShapingTargetGroup Parent { get; }

        public event EventHandler<IShapingTargetFile> OnShapingTargetFileLoad;

        public string FilePath;
        public string TargetFilePath;
        
        public string FileContent { get; set; }
        
        public ShapeResult Result;
        

        private readonly ShapingOperation ShapingOperation;
        
        public VCXTargetFile(ShapingOperation shapingOperation, IShapePatch shapePatch, string filePath, IShapingTargetGroup parentGroup)
        {
            Name = Path.GetFileName(filePath);
            FilePath = filePath;

            FileContent = File.ReadAllText(filePath);
            
            Parent = parentGroup;
            
            ShapingOperation = shapingOperation;
            ShapePatch = shapePatch;

            Result = new ShapeResult(ShapingOperation.ShapeProject, FilePath);
            
            LoadFile();
        }
        
        public void LoadFile()
        {
            ShapePatch.PreparationController.Prepare(Result.FileContent);
            ShapePatch.PreparationController.Visitor.VisitorController.OnVisitorProcess += Result.VisitorProcess;
        }
    }
}
