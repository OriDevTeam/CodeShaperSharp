// System Namespaces


// Application Namespaces


// Library Namespaces
using EasyConsole;


namespace CLI.Menu
{
    internal class HomePage : MenuPage
    {
        public HomePage(ConsoleProgram program) : base("Home", program,
            new Option("Shaper", () => program.NavigateTo<ShapePage>()))
        {
        }

        public override void Display()
        {
            ((ConsoleProgram)Program).Display();
            base.Display();
        }
    }
}
