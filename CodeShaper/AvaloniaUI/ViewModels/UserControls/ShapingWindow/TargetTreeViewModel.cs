// System Namespaces
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;

// Application Namespaces
using Lib.Managers;
using Lib.Shaping.Target.Interfaces;


// Library Namespaces


namespace AvaloniaUI.ViewModels.UserControls.ShapingWindow
{
    public class TargetTreeViewModel : ViewModelBase
    {
        public static ObservableCollection<IShapingTargetGroup> Groups =>
            ShapingOperationsManager.ActiveShapingOperation.ShapingTarget.ShapingTargetGroups;

        public TargetTreeViewModel()
        {
            Source = new HierarchicalTreeDataGridSource<IShapingTargetGroup>(Groups)
            {
                Columns =
                {
                    new HierarchicalExpanderColumn<IShapingTargetGroup>(
                        new TextColumn<IShapingTargetGroup, string>("Group", x => x.Name),
                        x => x.ShapingTargetGroups),
                }
            };
        }
        
        public HierarchicalTreeDataGridSource<IShapingTargetGroup> Source { get; }
    }
}
