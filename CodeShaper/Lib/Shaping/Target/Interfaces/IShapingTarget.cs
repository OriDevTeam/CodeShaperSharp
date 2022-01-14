// System Namespaces
using System.Collections.ObjectModel;


// Application Namespaces


// Library Namespaces


namespace Lib.Shaping.Target.Interfaces
{
    public interface IShapingTarget
    {
        public string Name { get; }
        public ObservableCollection<IShapingTargetGroup> ShapingTargetGroups { get; }
        
        void Load();
    }
}

