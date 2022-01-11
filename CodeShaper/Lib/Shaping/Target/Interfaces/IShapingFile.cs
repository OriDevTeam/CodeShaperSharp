// System Namespaces
using System;


// Application Namespaces


// Library Namespaces


namespace Lib.Shaping.Target.Interfaces
{
    public interface IShapingTargetFile
    {
        public string Name { get; }
        
        public event EventHandler<IShapingTargetFile> OnShapingTargetFileLoad;

        public void LoadFile();
    }
}