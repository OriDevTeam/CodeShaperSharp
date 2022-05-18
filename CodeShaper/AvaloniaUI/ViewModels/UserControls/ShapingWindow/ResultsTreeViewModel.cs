// System Namespaces
using System.Collections.ObjectModel;


// Application Namespaces
using Lib.Managers;
using Lib.Shaping.Result;


// Library Namespaces


namespace AvaloniaUI.ViewModels.UserControls.ShapingWindow
{
    public class ResultsTreeViewModel : ViewModelBase
    {
        public ResultsTreeViewModel()
        {
        }

        internal void Refresh()
        {
            Result = ShapingOperationsManager.ActiveShapingOperation.OperationsController
                .CurrentTargetFile.Result;
        }
        
        public ObservableCollection<ShapeResult> Results { get; set; } = new();

        public ShapeResult? Result { get; private set; }
    }
}
