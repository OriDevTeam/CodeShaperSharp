// System Namespaces

using System;
using System.Collections.ObjectModel;


// Application Namespaces


// Library Namespaces


namespace Lib.Shaping.Target.Interfaces
{
    public interface IShapingTarget
    {
        public string Name { get; }
        public ObservableCollection<IShapingTargetGroup> ShapingTargetGroups { get; }
        
        public void Load();
        public void AddGroupsLoadEvent(EventHandler<IShapingTargetGroup> onLoadingShapingTargetGroup);
        public void AddFilesLoadEvent(EventHandler<IShapingTargetFile> onLoadingShapingTargetFile);
    }
}

