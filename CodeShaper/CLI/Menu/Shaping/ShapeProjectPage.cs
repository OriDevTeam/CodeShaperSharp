// System Namespaces
using System;
using System.Runtime.Serialization;


// Application Namespaces
using CLI.Utility.Dialogs;
using CLI.Utility.Extensions;
using Lib.Configurations;
using Lib.Utility.Extensions;


// Library Namespaces
using EasyConsole;


namespace CLI.Menu.Shaping
{
    internal class ShapeProjectPage : MenuPage
    {
        private readonly ShapingConfiguration shapingConfiguration;

        public ShapeProjectPage(Program program) : base("Shape Project", program)
        {
            shapingConfiguration = new ShapingConfiguration();
        }

        public override void Display()
        {
            ConsoleProgram.Display();

            var ptr = typeof(Page).GetMethod("Display")!.MethodHandle.GetFunctionPointer();
            var baseDisplay = (Action)Activator.CreateInstance(typeof(Action), this, ptr);
            baseDisplay?.Invoke();

            DisplayOptions();

            if (DisplayActions())
                Program.NavigateTo<ShapeProjectPage>();
        }

        private void DisplayOptions()
        {
            Output.WriteLine("");
            Output.WriteLine(ConsoleColor.Green, "Paths:");

            ConsoleExtensions.Write(ConsoleColor.Cyan, " - Source Files Directory: ");
            ConsoleExtensions.Write(ConsoleColor.Magenta, string.IsNullOrEmpty(shapingConfiguration.SourceDirectory) ?
                "None" : shapingConfiguration.SourceDirectory);
            Console.WriteLine();

            switch (shapingConfiguration.ResultOptions)
            {
                case ResultOptions.ReplaceOriginal:
                    break;
                case ResultOptions.BackupAndReplaceMoveOriginal:
                    ConsoleExtensions.Write(ConsoleColor.Cyan, " - Backup Directory: ");
                    ConsoleExtensions.Write(ConsoleColor.Magenta, string.IsNullOrEmpty(shapingConfiguration.TargetDirectory) ?
                        "None" : shapingConfiguration.TargetDirectory);
                    Console.WriteLine();
                    break;
                case ResultOptions.CreateNew:
                    ConsoleExtensions.Write(ConsoleColor.Cyan, " - Shaped Files Directory: ");
                    ConsoleExtensions.Write(ConsoleColor.Magenta, string.IsNullOrEmpty(shapingConfiguration.BackupDirectory) ?
                        "None" : shapingConfiguration.BackupDirectory);
                    Console.WriteLine();
                    break;
            }
            if (shapingConfiguration.ResultOptions != ResultOptions.ReplaceOriginal)
            {
            }

            Output.WriteLine("");
            Output.WriteLine(ConsoleColor.Green, "Result Options:");

            ConsoleExtensions.Write(ConsoleColor.Cyan, " - On shaping files: ");
            ConsoleExtensions.Write(ConsoleColor.Magenta, shapingConfiguration.ResultOptions.ToEnumString());
        }


        private bool DisplayActions()
        {
            Output.WriteLine("\n");

            Output.WriteLine(ConsoleColor.Yellow, "What to do?: ");
            var input = InputExtension.ReadEnumAttr<UserAction>("");

            switch (input)
            {
                case UserAction.SelectSourceProjectDirectory:
                    shapingConfiguration.SourceDirectory = FolderDialog.SelectDirectory("Select source project directory");
                    break;
                case UserAction.SelectTargetProjectDirectory:
                    var dir = FolderDialog.SelectDirectory("Select target/backup project directory");
                    shapingConfiguration.TargetDirectory = dir;
                    shapingConfiguration.BackupDirectory = dir;
                    break;
                case UserAction.ChangeShapingResult:
                    shapingConfiguration.ResultOptions = InputResult();
                    break;
                case UserAction.ShapeSourceFiles:
                    ((ConsoleProgram)Program).Make(shapingConfiguration);
                    break;
                case UserAction.GoBack:
                    Program.NavigateBack();
                    return false;
                default:
                    throw new NotImplementedException($"Option not implemented: {input}");
            }

            return true;
        }

        private ResultOptions InputResult()
        {
            Console.Clear();
            ConsoleProgram.Display();

            Output.WriteLine("");
            Output.WriteLine(ConsoleColor.Yellow, "What should be the result of shaping?");
            var input = InputExtension.ReadEnumAttr<ResultOptions>("");

            return input;
        }

        private enum UserAction
        {
            [EnumMember(Value = "Select source project directory")]
            SelectSourceProjectDirectory,

            [EnumMember(Value = "Select target/backup project directory")]
            SelectTargetProjectDirectory,

            [EnumMember(Value = "Change shaping result")]
            ChangeShapingResult,

            [EnumMember(Value = "Shape source files")]
            ShapeSourceFiles,

            [EnumMember(Value = "Go back")]
            GoBack
        }
    }
}
