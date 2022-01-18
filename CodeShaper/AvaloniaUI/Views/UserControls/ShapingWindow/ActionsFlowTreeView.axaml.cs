using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvaloniaUI.Views.UserControls.ShapingWindow
{
    public class ActionsFlowTreeView : UserControl
    {
        public ActionsFlowTreeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}