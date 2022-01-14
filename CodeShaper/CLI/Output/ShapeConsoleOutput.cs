// System Namespacs


// Application Namespaces


// Library Namespaces


namespace Lib.Output
{
    internal interface ShapeReplacementOutput
    {

    }

    internal class ShapeConsoleOutput : ShapeReplacementOutput
    {
        public string From { get; private set; }
        public string To { get; private set; }

        public ShapeConsoleOutput()
        {

        }

        public string Output()
        {
            return To;
        }
    }
}
