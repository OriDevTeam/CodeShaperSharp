// System Namespaces


// Application Namespaces


// Library Namespaces
using Avalonia.Controls;
using Avalonia.Markup.Xaml;


namespace AvaloniaUI.Views.UserControls.HomeWindow
{
    public class ShaperView : UserControl
    {
        public ShaperView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
