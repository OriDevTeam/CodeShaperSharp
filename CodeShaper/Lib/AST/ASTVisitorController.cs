// System Namespaces
using System;
using System.Collections.Generic;


// Application Namespaces


// Library Namespaces
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;



namespace Lib.AST
{
    public class ASTVisitorController<TLocation> where TLocation : Enum
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

        public ASTVisitorController(ASTPreparationController preparationController)
        {
            PreparationController = preparationController;
        }
        
        
        internal object ProcessCustomVisit([NotNull] IParseTree tree, string text, TLocation location, Delegate visit)
        {
            return ProcessVisit(text, location);
        }
        
        internal object ProcessCustomVisit([NotNull] ParserRuleContext context, string text, TLocation location, Delegate visit)
        {
            return ProcessVisit(context, location);
        }
        
        internal void ProcessCustomVisit(string visitText, TLocation location)
        {
            LocationsContent[location] = visitText;

            ProcessVisit(visitText, location);
        }

        internal object ProcessVisit([NotNull] IParseTree tree, TLocation location, Delegate visit)
        {
            return new object();
        }
        
        internal object ProcessVisit([NotNull] ParserRuleContext context, TLocation location, Delegate visit)
        {
            return new object();
        }
        
        private object ProcessVisit([NotNull] ParserRuleContext context, TLocation location)
        {
            CurrentContext = context;
            
            var contextText = GetText(context);

            return ProcessVisit(contextText, location);
        }

        private object ProcessVisit(string visit, TLocation location)
        {
            LocationsContent[location] = visit;
            
            return ProcessVisitorState(location);
        }
        
        private object ProcessVisitorState(TLocation location)
        {
            CurrentLocation = location;
            
            OnVisitorProcess?.Invoke(this, location);

            switch (State)
            {
                case VisitorState.Stop:
                    break;
                
                case VisitorState.Play:
                    return PreparationController.Visitor.Visit(CurrentContext);

                case VisitorState.Pause:
                    break;
                
                case VisitorState.Previous:
                    throw new Exception("Cannot go back on a visitor, not implemented");
                    
                case VisitorState.Next:
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return null;
        }

        private void ProcessStateChange()
        {
            if (CurrentContext == null)
                CurrentContext = PreparationController.MakeRootContext();

            PreparationController.Visitor.Visit(CurrentContext);
        }
        
        internal string GetText(ParserRuleContext context)
        {
            var start = context.Start.StartIndex;
            var stop = -1;
            
            if (context.Stop == null)
                stop = context.Start.StopIndex;
            else
                stop = context.Stop.StopIndex;
            
            if (start > stop)
                return "";

            var interval = new Interval(start, stop);
            return PreparationController.InputStream.GetText(interval);
        }

        internal string AllText()
        {
            return PreparationController.InputStream.GetText(new Interval(0, PreparationController.InputStream.Size));
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
