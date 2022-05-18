// System Namespaces


// Application Namespaces


// Library Namespaces
using EasyConsole;


namespace CLI.Menu.Shaping
{
    internal class ShapePage : MenuPage
    {
        public ShapePage(Program program) : base("Shape", program,
            new Option("Shape a Project", () => program.NavigateTo<ShapeProjectPage>()),
            new Option("Run Local Shaping Configuration", () => program.NavigateTo<RunShapingConfiguration>())
            )
        {
            Program = program;
        }

        public override void Display()
        {
            ConsoleProgram.Display();
            base.Display();
        }
    }
}
