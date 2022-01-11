// System Namespaces


// Application Namespaces
using AvaloniaUI.ViewModels.UserControls.HomeWindow;


// Library Namespaces
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;


namespace AvaloniaUI.Views.UserControls.HomeWindow
{
    public class ShaperProjectDetailedView : ReactiveUserControl<ShaperProjectDetailedViewModel>
    {
        public ShaperProjectDetailedView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
    }
}
