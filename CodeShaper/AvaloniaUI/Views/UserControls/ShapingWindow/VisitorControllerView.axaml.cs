// System Namespaces
using System;
using Avalonia;

// Application Namespaces
using Lib.AST;
using Lib.Managers;
using Lib.AST.Controllers;


// Library Namespaces
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Avalonia.Threading;


namespace AvaloniaUI.Views.UserControls.ShapingWindow
{
    public class VisitorControllerView : ReactiveUserControl<VisitorControllerView>
    {
        public static VisitorControllerView? Instance { get; set; }
        
        private DispatcherTimer UpdateTimer { get; set; }
        
        public VisitorControllerView()
        {
            Instance = this;
            
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            
            BindControls();

            UpdateTimer = new DispatcherTimer()
            {
                Interval = new TimeSpan(100),
            };
            
            UpdateTimer.Tick += UpdateEvent;
            UpdateTimer.Start();
        }

        private void BindControls()
        {
            VisitorLocationText = this.Find<TextBlock>("VisitorLocationText");
            TimeElapsedText = this.Find<TextBlock>("TimeElapsedText");
            FileNameText = this.Find<TextBlock>("FileNameText");
            VisitorNameText = this.Find<TextBlock>("VisitorNameText");
            
            PreviousButton = this.Find<Button>("PreviousButton");
            PlayButton = this.Find<Button>("PlayButton");
            PlayButtonText = this.Find<TextBlock>("PlayButtonText");
            PlayButtonIcon = this.Find<PathIcon>("PlayButtonIcon");
            StopButton = this.Find<Button>("StopButton");
            NextButton = this.Find<Button>("NextButton");
        }

        private void RefreshController()
        {
            VisitorTreeView.Instance?.Refresh();
            
            var currentTargetFile = ShapingOperationsManager.ActiveShapingOperation.OperationsController.CurrentTargetFile;
            var visitorState = ShapingOperationsManager.ActiveShapingOperation.OperationsController.VisitorState;
            
            if (currentTargetFile != null)
            {
                var currentLocation = currentTargetFile.ShapePatch.PreparationController.ASTSet.Visitor.VisitorController.CurrentLocation;

                var locationStr = "None";

                if (currentLocation != null)
                    locationStr = currentLocation.ToString();
                
                FileNameText.Text = currentTargetFile.Name;
                VisitorLocationText.Text = $"Visitor Location: {locationStr}";
                VisitorNameText.Text = currentTargetFile.ShapePatch.PreparationController.ASTSet.Visitor.Name;
            }

            App.Current.Styles.TryGetResource("PauseRegular", out var pauseIcon);
            App.Current.Styles.TryGetResource("PlayRegular", out var playIcon);
            
            switch (visitorState)
            {
                case VisitorState.Play:
                    PlayButtonText.Text = "Pause";
                    PlayButtonIcon.Data = (Geometry)pauseIcon!;
                    break;
                case VisitorState.Pause:
                    PlayButtonText.Text = "Play";
                    PlayButtonIcon.Data = (Geometry)playIcon!;
                    break;
                case VisitorState.Stop:
                    PlayButtonText.Text = "Play";
                    PlayButtonIcon.Data = (Geometry)playIcon!;
                    break;
                case VisitorState.Previous:
                    break;
                case VisitorState.Next:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }

        public VisitorState GetState()
        {
            return ShapingOperationsManager.ActiveShapingOperation.OperationsController.VisitorState;
        }
        
        public void SetState(VisitorState state)
        {
            ShapingOperationsManager.ActiveShapingOperation.OperationsController.VisitorState = state;
            RefreshController();
        }
        
        private void UpdateEvent(object? sender, EventArgs e)
        {
            TimeElapsedText.Text = $"Time Elapsed: {ShapingOperationsManager.ActiveShapingOperation.Stopwatch.Elapsed}";
        }

        private TextBlock VisitorLocationText { get; set; }
        private TextBlock TimeElapsedText { get; set; }
        private TextBlock FileNameText { get; set; }
        private TextBlock VisitorNameText { get; set; }

        private Button PreviousButton { get; set; }
        private Button PlayButton { get; set; }
        private TextBlock PlayButtonText { get; set; }
        private PathIcon PlayButtonIcon { get; set; }
        private Button StopButton { get; set; }
        private Button NextButton { get; set; }
        
    }
}