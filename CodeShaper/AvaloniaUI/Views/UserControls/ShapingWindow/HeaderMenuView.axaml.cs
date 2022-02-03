// System Namespaces


// Application Namespaces


// Library Namespaces
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;


namespace AvaloniaUI.Views.UserControls.ShapingWindow
{
    public class HeaderMenuView : ReactiveUserControl<HeaderMenuView>
    {
        public static HeaderMenuView? Instance { get; private set; }
        
        public HeaderMenuView()
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
            PauseOnVisitCheckBox = this.Find<CheckBox>("PauseOnVisitCheckBox");
            PauseOnActionCheckBox = this.Find<CheckBox>("PauseOnActionCheckBox");
            PauseOnFileChangeCheckBox = this.Find<CheckBox>("PauseOnFileChangeCheckBox");
        }
        
        public CheckBox? PauseOnVisitCheckBox { get; private set; }
        public CheckBox? PauseOnActionCheckBox { get; private set; }
        public CheckBox? PauseOnFileChangeCheckBox { get; private set; }
    }
}
