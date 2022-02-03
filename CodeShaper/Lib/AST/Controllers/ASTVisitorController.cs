// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces
using Lib.Managers;


// Library Namespaces
using Serilog;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;


namespace Lib.AST.Controllers
{
    public partial class ASTVisitorController<TLocation> where TLocation : Enum
    {

        public Dictionary<TLocation, string> LocationsContent { get; private set; } = new();
        
        public TLocation CurrentLocation { get; private set; }
        
        public ASTPreparationController PreparationController { get; }

        public event EventHandler<TLocation> OnVisitorProcess;
        
        private VisitorState state;
        public VisitorState State
        {
            get => state;
            set
            {
                ProcessStateChange();
                state = value;
            }
        }
        
        private ParserRuleContext CurrentContext { get; set; }
        private Delegate CurrentDelegate { get; set; }

        public ASTVisitorController(ASTPreparationController preparationController)
        {
            PreparationController = preparationController;
        }

        internal void ProcessVisitError(IErrorNode node, Delegate visit)
        {
            Log.Error($"ERROR: Visitor cannot parse node {node}, stopping visits.");
            
        }
        
        internal void ProcessCustomVisit([NotNull] IParseTree tree, 
            string text, TLocation location, 
            IEnumerable<Tuple<TLocation, string>> customVisits,
            Delegate visit
        )
        {
            LocationsContent[location] = text;

            foreach (var (item1, item2) in customVisits)
                LocationsContent[item1] = item2;

            // CurrentContext = tree;
            
            CurrentDelegate = visit;
            
            ProcessVisitorState(location);
        }
        
        internal void ProcessCustomVisit([NotNull] ParserRuleContext context, 
            TLocation location, 
            IEnumerable<Tuple<TLocation, string>> customVisits,
            Delegate visit
        )
        {
            LocationsContent[location] = GetText(context);

            foreach (var (item1, item2) in customVisits)
                LocationsContent[item1] = item2;

            CurrentContext = context;
            
            CurrentDelegate = visit;
            
            ProcessVisitorState(location);
        }
        
        internal void ProcessCustomVisit([NotNull] ParserRuleContext context, 
            string text, TLocation location, 
            IEnumerable<Tuple<TLocation, string>> customVisits,
            Delegate visit
        )
        {
            LocationsContent[location] = text;

            if (customVisits != null)
                foreach (var (item1, item2) in customVisits)
                    LocationsContent[item1] = item2;

            CurrentContext = context;
            
            CurrentDelegate = visit;
            
            ProcessVisitorState(location);
        }

        internal void ProcessVisit([NotNull] ParserRuleContext context, TLocation location, Delegate visit)
        {
            var contextText = GetText(context);
            
            LocationsContent[location] = contextText;
            
            CurrentContext = context;

            CurrentDelegate = visit;
            
            ProcessVisitorState(location);
        }
        
        private void ProcessVisitorState(TLocation location)
        {
            Log.Information($"Visiting '{location}' location");
            
            CurrentLocation = location;

            OnVisitorProcess?.Invoke(this, location);
            
            ProcessVisitorSettings();
            
            switch (State)
            {
                case VisitorState.Stop:
                    ShapingOperationsManager.ActiveShapingOperation.Stopwatch.Stop();
                    break;
                
                case VisitorState.Play:
                    ShapingOperationsManager.ActiveShapingOperation.Stopwatch.Start();
                    CurrentDelegate.DynamicInvoke(CurrentContext);
                    break;

                case VisitorState.Pause:
                    ShapingOperationsManager.ActiveShapingOperation.Stopwatch.Stop();
                    break;
                
                case VisitorState.Previous:
                    throw new Exception("Cannot go back on a visitor, not implemented");
                    
                case VisitorState.Next:
                    throw new Exception("Cannot go back on a visitor, not implemented");
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessVisitorSettings()
        {
            if (SettingsManager.VisitorSettings.PauseOnVisit)
                state = VisitorState.Pause;
        }
        
        private void ProcessStateChange()
        {
            CurrentContext ??= PreparationController.ASTSet.GetRootContext();

            PreparationController.ASTSet.Visitor.Visit(CurrentContext);
        }
    }

    public partial class ASTVisitorController<TLocation>
    {
        internal string GetText(ParserRuleContext context)
        {
            var start = context.Start.StartIndex;

            var stop = context.Stop?.StopIndex ?? context.Start.StopIndex;
            
            if (start > stop)
                return "";

            var interval = new Interval(start, stop);
            return PreparationController.ASTSet.InputStream.GetText(interval);
        }

        internal string AllText()
        {
            return PreparationController.ASTSet.InputStream.GetText(new Interval(0, PreparationController.ASTSet.InputStream.Size));
        }
    }
    
    public enum VisitorState
    {
        // Stops the visitor completely
        Stop,
        
        // Processes the visitor visits
        Play,
        
        // Pauses the visitor
        Pause,
        
        // Goes back one visit (not implemented)
        Previous,
        
        // Skips processing of the visit
        Next
    }
}
