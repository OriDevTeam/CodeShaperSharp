// System Namespaces


// Application Namespaces


// Library Namespaces


namespace Lib.AST.Settings.Interfaces
{
    public interface IASTVisitorSettings
    {
        public bool PauseOnVisit { get; set; }
        public bool PauseOnAction { get; set; }
        public bool PauseOnFileChange { get; set; }
        
        public bool StopOnAction { get; set; }
        
        public bool StopOnFileChange { get; set; }
    }
}
