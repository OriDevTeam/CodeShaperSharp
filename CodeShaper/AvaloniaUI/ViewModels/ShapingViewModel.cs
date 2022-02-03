// System Namespaces


// Application Namespaces
using Lib.Managers;


// Library Namespaces


namespace AvaloniaUI.ViewModels
{
    public class ShapingViewModel : ViewModelBase
    {
        public ShapingViewModel()
        {
            ShapingOperationsManager.ActiveShapingOperation.Start();
        }
    }
}
