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
    public class ASTVisitorController<T> where T: Enum
    {
        private AntlrInputStream InputStream { get; }
        public Dictionary<T, string> LocationsContent { get; private set; } = new();

        public event EventHandler<T> OnVisitorProcess;
        
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

        private IParseTreeVisitor<object> Visitor { get; }
        private ParserRuleContext CurrentContext { get; set; }

        public ASTVisitorController(AntlrInputStream inputStream)
        {
            InputStream = inputStream;
        }
        
        public ASTVisitorController(IParseTreeVisitor<object> visitor)
        {
            Visitor = visitor;
        }
        
        internal object ProcessCustomVisit([NotNull] IParseTree tree, string text, T location, Delegate visit)
        {
            return ProcessVisit(tree, location);
        }
        
        internal object ProcessCustomVisit([NotNull] ParserRuleContext context, string text, T location, Delegate visit)
        {
            return ProcessVisit(context, location);
        }
        
        internal void ProcessCustomVisit(string visitText, T location)
        {
            LocationsContent[location] = visitText;
        }

        internal object ProcessVisit([NotNull] IParseTree tree, T location, Delegate visit)
        {
            return new object();
        }
        
        internal object ProcessVisit([NotNull] ParserRuleContext context, T location, Delegate visit)
        {
            return new object();
        }

        internal object ProcessVisit([NotNull] IParseTree tree, T location)
        {
            return new object();
        }
        
        internal object ProcessVisit([NotNull] ParserRuleContext context, T location)
        {
            CurrentContext = context;
            
            var contextText = GetText(context);

            LocationsContent[location] = contextText;
            
            OnVisitorProcess?.Invoke(this, location);

            return ProcessVisitorState();
        }
        
        private object ProcessVisitorState()
        {
            switch (State)
            {
                case VisitorState.Stop:
                    break;
                
                case VisitorState.Play:
                    return Visitor.Visit(CurrentContext);

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
            Visitor.Visit(CurrentContext);
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
            return InputStream.GetText(interval);
        }

        internal string AllText()
        {
            return InputStream.GetText(new Interval(0, InputStream.Size));
        }
    }
    
    public enum VisitorState
    {
        // Stops the visitor completely
        Stop,
        
        // Processes the visitor visit
        Play,
        
        // Pauses the visitor
        Pause,
        
        // Goes back one visit (not implemented)
        Previous,
        
        // Skips processing of the visit
        Next
    }
}
