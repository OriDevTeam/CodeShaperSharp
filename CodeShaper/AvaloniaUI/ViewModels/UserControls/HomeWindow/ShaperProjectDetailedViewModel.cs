// System Namespaces
using System;
using System.Windows.Input;


// Application Namespaces


// Library Namespaces
using Avalonia.Controls;
using AvaloniaUI.Views;
using ReactiveUI;


namespace AvaloniaUI.ViewModels.UserControls.HomeWindow
{
    public class ShaperProjectDetailedViewModel : ViewModelBase
    {
        public ShaperProjectDetailedViewModel()
        {
            MakeCommands();
        }

        private void MakeCommands()
        {
            StartShapingCommand = ReactiveCommand.Create(async () =>
            {
                var dialog = new OpenFolderDialog
                {
                    Title = "Select Shaper Project folder",
                    Directory = Environment.CurrentDirectory
                };

                var result = await dialog.ShowAsync(HomePage.Instance);

                return result;
            });
        }
    
        public ICommand StartShapingCommand { get; private set; }
    }
}
