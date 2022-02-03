// System Namespaces
using System;
using System.Collections.ObjectModel;


// Application Namespaces


// Library Namespaces
using ReactiveUI;


namespace AvaloniaUI.ViewModels.UserControls.ShapingWindow
{
    public class ActionToolBoxViewModel : ViewModelBase
    {
        public static ActionToolBoxViewModel Instance { get; private set; }

        public ActionToolBoxViewModel()
        {
            Instance = this;
        }

        
        private ObservableCollection<Tuple<string, string>> fields = new();
        public ObservableCollection<Tuple<string, string>> Fields {
            get => fields;

            set
            {
                this.RaiseAndSetIfChanged(ref fields, value);
                fields = value;
            } 
        }
    }
}
