// System Namespaces
using System;
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Shaping.Target.Interfaces;


// Library Namespaces


namespace Lib.Projects.CSSolution
{
    public class CSProjectGroup : IShapingTargetGroup
    {
        public string Name { get; }
        public ObservableCollection<IShapingTargetGroup> ShapingTargetGroups { get; }
        public ObservableCollection<IShapingTargetFile> ShapingTargetFiles { get; }
        public IShapingTargetGroup ParentGroup { get; set; }
        public event EventHandler<IShapingTargetGroup> OnShapingGroupLoad;
        public event EventHandler<IShapingTargetFile> OnShapingTargetFileLoad;

        public CSProjectGroup()
        {
                
        }
        
        public void LoadGroups()
        {
            throw new NotImplementedException();
        }
    }
}
