// System Namespaces
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;


// Application Namespaces



// Library Namespaces


namespace CLI.Utility.Dialogs
{
    public static class FolderDialog
    {
        public static string SelectDirectory(string title)
        {
            FolderBrowserDialog folderBrowser = new();

            folderBrowser.Description = title;
            folderBrowser.UseDescriptionForTitle = true;
            folderBrowser.SelectedPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\\";
            folderBrowser.RootFolder = Environment.SpecialFolder.Desktop;

            var result = folderBrowser.ShowDialog();

            if (result == DialogResult.OK)
                return folderBrowser.SelectedPath + @"\\";

            return string.Empty;
        }
    }

}
