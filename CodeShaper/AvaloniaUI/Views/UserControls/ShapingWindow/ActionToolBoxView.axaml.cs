// System Namespaces


// Application Namespaces


// Library Namespaces
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaUI.Models;


namespace AvaloniaUI.Views.UserControls.ShapingWindow
{
    public class ActionToolBox : ReactiveUserControl<ActionModel>
    {
        public ActionModel? Action;

        public ActionToolBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            CreateComponents();
        }

        private void CreateComponents()
        {
            NameTextBox = new TextBox
            {
                Text = Action?.Name,
                IsVisible = true
            };

            ActionTextBox = new TextBox
            {
                Text = Action?.Action,
                IsVisible = true
            };
        }

        private TextBox NameTextBox { get; set; }
        private TextBox ActionTextBox { get; set; }
    }
}
