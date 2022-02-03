// System Namespaces


// Application Namespaces
using Lib.AST.Settings.Interfaces;


// Library Namespaces



namespace Lib.AST.Settings
{
    public class ASTVisitorSettings : IASTVisitorSettings
    {
        public bool PauseOnVisit { get; set; }
        public bool PauseOnAction { get; set; }
        public bool PauseOnFileChange { get; set; }
        
        public bool StopOnAction { get; set; }
        public bool StopOnFileChange { get; set; }
    }
}
