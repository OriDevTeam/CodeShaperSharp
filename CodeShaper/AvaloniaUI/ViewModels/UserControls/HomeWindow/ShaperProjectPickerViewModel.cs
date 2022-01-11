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
    public class ShaperProjectPickerViewModel : ViewModelBase
    {
        public ShaperProjectPickerViewModel()
        {
            MakeCommands();
        }

        private void MakeCommands()
        {
            OpenShapeProjectCommand = ReactiveCommand.Create(async () =>
            {
                var dialog = new OpenFolderDialog
                {
                    Title = "Select Shaper Project folder",
                    Directory = Environment.CurrentDirectory
                };

                var result = await dialog.ShowAsync(HomePage.Instance);

                return result;
            });

            CreateNewShapeProjectCommand = ReactiveCommand.Create(async () =>
            {
                
            });
        }
        
        
        public ICommand OpenDialogCommand { get; private set; }
        public ICommand OpenShapeProjectCommand { get; private set; }
        public ICommand CreateNewShapeProjectCommand { get; private set; }
    }
}
