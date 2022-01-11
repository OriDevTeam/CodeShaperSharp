// System Namespaces
using System;
using System.IO;


// Application Namespaces
using Lib.Shaping;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces


namespace Lib.Projects.VCXSolution
{
    public class VCXModuleFile : IShapingTargetFile
    {
        public string Name { get; }
        
        public event EventHandler<IShapingTargetFile> OnShapingTargetFileLoad;

        public string FilePath;
        public string TargetFilePath;

        ShapeProject ShapeProject;
        public ShapeResult Result;

        public VCXModuleFile(ShapeProject shapeProject, string filePath)
        {
            Name = Path.GetFileName(filePath);
            FilePath = filePath;
            ShapeProject = shapeProject;
        }
        
        public void LoadFile()
        {
        }
    }
}
