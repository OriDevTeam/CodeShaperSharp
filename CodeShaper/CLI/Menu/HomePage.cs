// System Namespaces


// Application Namespaces
using CLI.Menu.Shaping;


// Library Namespaces
using EasyConsole;


namespace CLI.Menu
{
    internal class HomePage : MenuPage
    {
        public HomePage(Program program) : base("Home", program,
            new Option("Shaper", () => program.NavigateTo<ShapePage>()))
        {
        }

        public override void Display()
        {
            ConsoleProgram.Display();
            base.Display();
        }
    }
}
