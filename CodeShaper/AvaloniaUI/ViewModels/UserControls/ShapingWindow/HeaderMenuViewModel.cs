// System Namespaces
using System.Windows.Input;


// Application Namespaces
using AvaloniaUI.Views.UserControls.ShapingWindow;
using Lib.Managers;


// Library Namespaces
using ReactiveUI;


namespace AvaloniaUI.ViewModels.UserControls.ShapingWindow
{
    public class HeaderMenuViewModel : ViewModelBase
    {
        public HeaderMenuViewModel()
        {
            CreateCommands();
        }
        
        private void CreateCommands()
        {
            PauseOnVisitCommand = ReactiveCommand.Create(async () =>
            {
                var check = (bool)HeaderMenuView.Instance.PauseOnVisitCheckBox.IsChecked;
                
                SettingsManager.VisitorSettings.PauseOnVisit = check;
            });
            
            PauseOnActionCommand = ReactiveCommand.Create(async () =>
            {
                var check = (bool)HeaderMenuView.Instance.PauseOnVisitCheckBox.IsChecked;
                
                SettingsManager.VisitorSettings.PauseOnVisit = check;
            });
            
            PauseOnActionCommand = ReactiveCommand.Create(async () =>
            {
                var check = (bool)HeaderMenuView.Instance.PauseOnActionCheckBox.IsChecked;
                
                SettingsManager.VisitorSettings.PauseOnAction = check;
            });
            
            PauseOnFileChange = ReactiveCommand.Create(async () =>
            {
                var check = (bool)HeaderMenuView.Instance.PauseOnFileChangeCheckBox.IsChecked;
                
                SettingsManager.VisitorSettings.PauseOnFileChange = check;
            });
        }
        
        private ICommand? PauseOnVisitCommand { get; set; }
        private ICommand? PauseOnActionCommand { get; set; }
        private ICommand? PauseOnFileChange { get; set; }
    }
}
