// System Namespaces


// Application Namespaces


// Library Namespaces
using EasyConsole;


namespace CLI.Utility.Extensions
{
    public static class MenuPageExtensions
    {
        public static void InputOptions(this MenuPage menuPage, EasyConsole.Menu menu)
        {
            var page = (Page)menuPage;

            if (page.Program.NavigationEnabled && !menu.Contains("Go back"))
            {
                menu.Add("Go back", delegate
                {
                    page.Program.NavigateBack();
                });
            }

            menu.Display();
        }
    }
}
