// System Namespaces


// Application Namespaces
using CLI.Menu.Shaping;


// Library Namespaces
using EasyConsole;


namespace CLI.Menu
{
    internal class ShapePage : MenuPage
    {
        public ShapePage(ConsoleProgram program) : base("Shape", program,
            new Option("Shape a Project", () => program.NavigateTo<ShapeProjectPage>()),
            new Option("Run a Rapid Configuration", () => program.NavigateTo<RunShapingConfiguration>())
            )
        {
            Program = program;
        }

        public override void Display()
        {
            ((ConsoleProgram)Program).Display();
            base.Display();
        }
    }
}
