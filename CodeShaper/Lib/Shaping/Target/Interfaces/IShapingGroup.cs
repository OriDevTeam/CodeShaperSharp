// System Namespaces
using System;
using System.Collections.ObjectModel;


// Application Namespaces


// Library Namespaces



namespace Lib.Shaping.Target.Interfaces
{
    public interface IShapingTargetGroup
    {
        public string Name { get; }
        
        public ObservableCollection<IShapingTargetGroup> ShapingTargetGroups { get; }
        public ObservableCollection<IShapingTargetFile> ShapingTargetFiles { get; }
        IShapingTargetGroup ParentGroup { get; set; }

        public event EventHandler<IShapingTargetGroup> OnShapingGroupLoad;
        public event EventHandler<IShapingTargetFile> OnShapingTargetFileLoad;

        public void LoadGroups();
    }
}
