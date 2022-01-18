// System Namespaces
using System;


// Application Namespaces
using Lib.Shapers.Interfaces;


// Library Namespaces


namespace Lib.Shaping.Target.Interfaces
{
    public interface IShapingTargetFile
    {
        public string Name { get; }
        
        public IShapePatch ShapePatch { get; }
        
        public IShapingTargetGroup Parent { get; }
        public string FileContent { get; set; }

        public event EventHandler<IShapingTargetFile> OnShapingTargetFileLoad;

        public void LoadFile();
    }
}