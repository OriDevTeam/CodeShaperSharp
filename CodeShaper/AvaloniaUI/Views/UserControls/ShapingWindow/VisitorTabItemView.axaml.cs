using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaUI.Views.UserControls.ShapingWindow
{
    public class VisitorTabItemView : UserControl
    {
        public VisitorTabItemView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}