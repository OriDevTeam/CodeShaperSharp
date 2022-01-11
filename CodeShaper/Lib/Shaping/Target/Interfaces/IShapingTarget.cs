// System Namespaces
using System.Collections.ObjectModel;
using Lib.AST.Interfaces;


// Application Namespaces


// Library Namespaces


namespace Lib.Shaping.Target.Interfaces
{
    public interface IShapingTarget<T> where T: IASTVisitor
    {
        public ObservableCollection<IShapingTargetGroup> ShapingTargetGroups { get; }
        void Load();
    }
}

