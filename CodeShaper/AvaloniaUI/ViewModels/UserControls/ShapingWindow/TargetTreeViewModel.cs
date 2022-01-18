// System Namespaces
using System.Collections.ObjectModel;


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
        }
    }
}
