// System Namespaces
using System;


// Application Namespaces
using Lib.Shapers;
using Lib.Shapers.Patch;
using Lib.Shaping.Result;


// Library Namespaces


namespace Lib.Shaping.Target.Interfaces
{
    public interface IShapingTargetFile
    {
        public string Name { get; }
        
        public ShapePatchFile ShapePatchFile { get; }
        
        public IShapingTargetGroup Parent { get; }
        public string FileContent { get; set; }
        
        public ShapeResult Result { get; }

        public event EventHandler<IShapingTargetFile> OnShapingTargetFileLoad;

        public void LoadFile();
    }
}