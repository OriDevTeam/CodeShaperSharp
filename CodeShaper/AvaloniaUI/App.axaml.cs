// System Namespaces


// Application Namespaces
using AvaloniaUI.ViewModels;
using AvaloniaUI.Views;
using Lib.Managers;


// Library Namespaces
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;


namespace AvaloniaUI
{
    public class App : Application
    {
        public override void Initialize()
        {
            InitializeLib();
            
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new HomePage()
                {
                    DataContext = new HomePageViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private static void InitializeLib()
        {
            LibManager.Initialize();
        }
    }
}