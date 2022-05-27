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
using Stateless;


namespace Lib.AST.Controllers
{
    public partial class ASTVisitorController<TLocation> where TLocation : Enum
    {

        public Dictionary<TLocation, string> LocationsContent { get; } = new();
        
        public TLocation CurrentLocation { get; private set; }
        
        public ASTPreparationController PreparationController { get; }

        public event EventHandler<TLocation> OnVisitorProcess;
        
        public VisitorState State { get; private set; }

        private VisitorState requestState;
        public VisitorState RequestState
        {
            set
            {
                if (requestState == value)
                    return;
                
                requestState = value;
                PreProcessVisit();
            }
        }

        private bool _processing;
        private int _temp;

        public ParserRuleContext CurrentContext { get; set; }
        private Delegate CurrentDelegate { get; set; }

        public ASTVisitorController(ASTPreparationController preparationController)
        {
            PreparationController = preparationController;
        }

        internal void ProcessVisitError(IErrorNode node, Delegate visit)
        {
            Log.Error($"ERROR: Visitor cannot parse node {node}, stopped visiting.");
            
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

            CurrentDelegate = visit;
            
            PostProcessVisit(location);
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
            
            CurrentDelegate = visit;
            
            PostProcessVisit(location, context);
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
            
            CurrentDelegate = visit;
            
            PostProcessVisit(location, context);
        }

        internal void ProcessVisit([NotNull] ParserRuleContext context, TLocation location, Delegate visit)
        {
            var contextText = GetText(context);
            
            LocationsContent[location] = contextText;
            
            CurrentDelegate = visit;
            
            PostProcessVisit(location, context);
        }

        private void PreProcessVisit()
        {
            State = requestState;
            
            switch (State)
            {
                case VisitorState.Stop:
                    ShapingOperationsManager.ActiveShapingOperation.Stopwatch.Stop();
                    break;
                
                case VisitorState.Play:
                    ShapingOperationsManager.ActiveShapingOperation.Stopwatch.Start();
                    break;

                case VisitorState.Pause:
                    ShapingOperationsManager.ActiveShapingOperation.Stopwatch.Stop();
                    break;
                
                case VisitorState.Previous:
                case VisitorState.Next:
                default:
                    throw new NotImplementedException();
            }
            
            switch (State)
            {
                case VisitorState.Stop:
                    Log.Information("Stopped visiting");
                    break;
                
                case VisitorState.Play:
                    Log.Information("Played visiting");
                    break;
                
                case VisitorState.Pause:
                    Log.Information("Paused visiting");
                    break;

                case VisitorState.Previous:
                case VisitorState.Next:
                default:
                    throw new NotImplementedException();
            }
            
            ProcessVisit();
        }

        private void ProcessVisit()
        {
            if (_processing)
                return;
            
            if (State != VisitorState.Play)
                return;

            _processing = true;
            
            CurrentContext ??= PreparationController.ASTSet.GetRootContext();
            CurrentDelegate ??= new Func<IParseTree, object>(PreparationController.ASTSet.Visitor.Visit);
            
            Log.Information($"Visiting '{CurrentLocation}' location");
            var result = CurrentDelegate.DynamicInvoke(CurrentContext);
            _temp += 1;
            
            if (_temp >= 109)
                _processing = false;

            
            OnVisitorProcess?.Invoke(this, CurrentLocation);
        }

        private void PostProcessVisit(TLocation location, ParserRuleContext context)
        {
            if (PreparationController.ASTSet.GetRootContext() == context)
                _processing = false;
            
            CurrentContext = context;
            
            PostProcessVisit(location);
        }
        
        private void PostProcessVisit(TLocation location)
        {
            _processing = false;
            
            CurrentLocation = location;

            if (SettingsManager.VisitorSettings.PauseOnVisit)
                State = VisitorState.Pause;

            requestState = State;
            
            ProcessVisit();
        }
    }

    // TODO: Attempt at making a simpler FSM instead of current proccesing above
    public partial class ASTVisitorController<TLocation>
    {

        private void MakeFiniteStateMachine()
        {
            var ModuleVisit = new StateMachine<VisitorState, VisitorState>(State);

            ModuleVisit.Configure(VisitorState.Play).Permit(VisitorState.Pause, VisitorState.Play);
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
