// System Namespaces
using System.Windows.Input;


// Application Namespaces
using AvaloniaUI.Views.UserControls.ShapingWindow;
using Lib.AST;


// Library Namespaces
using ReactiveUI;


namespace AvaloniaUI.ViewModels.UserControls.ShapingWindow
{
    public class VisitorControllerViewModel : ViewModelBase
    {
        
        
        public VisitorControllerViewModel()
        {
            CreateCommands();
        }
        
        private void CreateCommands()
        {
            PlayPauseCommand = ReactiveCommand.Create(async () =>
            {
                var state = VisitorControllerView.Instance?.GetState();

                VisitorControllerView.Instance?.SetState(state == VisitorState.Play
                    ? VisitorState.Pause
                    : VisitorState.Play);
            });
            
            StopCommand = ReactiveCommand.Create(async () =>
            {
                VisitorControllerView.Instance?.SetState(VisitorState.Stop);
            });
        }

        private ICommand? PlayPauseCommand { get; set; }
        private ICommand? StopCommand { get; set; }
    }
}