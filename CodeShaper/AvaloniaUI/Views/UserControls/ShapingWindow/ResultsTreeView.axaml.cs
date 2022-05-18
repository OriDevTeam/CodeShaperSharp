// System Namespaces


// Application Namespaces
using AvaloniaUI.ViewModels.UserControls.ShapingWindow;


// Library Namespaces
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;


namespace AvaloniaUI.Views.UserControls.ShapingWindow
{
    public class ResultsTreeView : ReactiveUserControl<ResultsTreeViewModel>
    {
        public static ResultsTreeView? Instance { get; private set; }
        
        public ResultsTreeView()
        {
            Instance = this;
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            BindControls();
        }

        private void BindControls()
        {
            
        }

        internal void Refresh()
        {
            ViewModel ??= (ResultsTreeViewModel?)Content;
            ViewModel?.Refresh();
        }
    }
}
