// System Namespaces


// Application Namespaces


// Library Namespaces
using AvaloniaUI.Models;
using AvalonStudio.Controls;


namespace AvaloniaUI.ViewModels.UserControls.ShapingWindow
{
    public class ActionToolBoxViewModel : DocumentTabViewModel
    {
        private ActionModel Action { get; }
        
        public ActionToolBoxViewModel()
        {
            Action = new ActionModel();
        }

        public ActionToolBoxViewModel(ActionModel action)
        {
            Action = action;
        }
    }
}
