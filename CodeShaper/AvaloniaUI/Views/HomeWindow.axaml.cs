// System Namespaces
using System.Threading.Tasks;


// Application Namespaces
using AvaloniaUI.ViewModels;
using AvaloniaUI.Views.UserControls.HomeWindow;


// Library Namespaces
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;



namespace AvaloniaUI.Views
{
    public class HomePage : ReactiveWindow<HomePageViewModel>
    {
        public static HomePage Instance { get; private set; }
        public HomePage()
        {
            Instance = this;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
            ProjectPickerView = this.FindControl<ShaperProjectPickerView>("PickerView");
            ProjectDetailedView = this.FindControl<ShaperProjectDetailedView>("DetailedView");
        }

        public void ToggleUserArea(bool toggle)
        {
            ProjectPickerView.IsVisible = !toggle;
            ProjectDetailedView.IsVisible = toggle;
        }
        
        private ShaperProjectPickerView ProjectPickerView { get; set; }
        private ShaperProjectDetailedView ProjectDetailedView { get; set; }
    }
}
