// System Namespaces
using System;
using System.IO;
using System.Collections.Generic;


// Application Namespaces
using Lib.Shapers.CPP;
using Lib.Configurations;
using Lib.AST.ANTLR;


// Library Namespaces


namespace Lib.Shapers
{
    public class CPPResult
    {
        public string FileName;
        public string FileContent;

        public List<Tuple<KeyValuePair<string, Replacement>, string, string>> Replacements = new();
        public List<Tuple<KeyValuePair<string, Addition>, string, string>> Additions = new();
        public List<Tuple<KeyValuePair<string, Subtraction>, string, string>> Subtractions = new();

        public ShapeProject ShapeProject;

        public CPPResult(ShapeProject shapeProject, string fileContent, string fileName)
        {
            ShapeProject = shapeProject;
            FileName = fileName;
            FileContent = fileContent;
        }

        public void Process(CPPModule module, string context, Location location)
        {
            var newContext = context;

            foreach (var replacement in Replacing.ProcessReplacing(ref newContext, module, FileName, ShapeProject, location))
                Replacements.Add(new Tuple<KeyValuePair<string, Replacement>, string, string>(replacement, context, newContext));

            var addContext = newContext;

            foreach (var addition in Adding.ProcessAdding(ref addContext, module, FileName, ShapeProject, location))
                Additions.Add(new Tuple<KeyValuePair<string, Addition>, string, string>(addition, newContext, addContext));

            var subContext = addContext;

            foreach (var subtraction in Subtracting.ProcessSubtractions(ref subContext, module, FileName, ShapeProject, location))
                Subtractions.Add(new Tuple<KeyValuePair<string, Subtraction>, string, string>(subtraction, addContext, subContext));

            if (context == subContext)
                return;

            FileContent = FileContent.Replace(context, subContext);
        }

        private void SaveLogs(string filePath, ShapingConfiguration configuration)
        {
            var targetFilePath = "";
        }

        private void SaveLogs(string filePath)
        {
            var dir = Path.GetDirectoryName(filePath) + @"\" + Path.GetFileNameWithoutExtension(filePath); 

            for (int i = 0; i < Replacements.Count; i++)
            {
                var replacement = Replacements[i];

                Directory.CreateDirectory(dir);

                var path = string.Format(@"{0}\{1}_{2}.rep.log", dir, i, replacement.Item1.Key);
                var content = string.Format("From:\n{0}\n\n\nTo:\n{1}\n",
                                             replacement.Item2, replacement.Item3);

                File.WriteAllText(path, content);
            }

            for (int i = 0; i < Additions.Count; i++)
            {
                var addition = Additions[i];

                Directory.CreateDirectory(dir);

                var path = string.Format(@"{0}\{1}_{2}.add.log", dir, i, addition.Item1.Key);
                var content = string.Format("From:\n{0}\n\n\nTo:\n{1}\n",
                                             addition.Item2, addition.Item3);

                File.WriteAllText(path, content);
            }

            for (int i = 0; i < Subtractions.Count; i++)
            {
                var subtraction = Subtractions[i];

                Directory.CreateDirectory(dir);

                var path = string.Format(@"{0}\{1}_{2}.sub.log", dir, i, subtraction.Item1.Key);
                var content = string.Format("From:\n{0}\n\n\nTo:\n{1}\n",
                                             subtraction.Item2, subtraction.Item3);

                File.WriteAllText(path, content);
            }
        }

        public void Save(string filePath, ShapingConfiguration configuration)
        {
            var targetFilePath = "";

            switch (configuration.ResultOptions)
            {
                case ResultOptions.ReplaceOriginal:
                    if (File.Exists(targetFilePath))
                        File.Delete(targetFilePath);

                    targetFilePath = filePath;

                    break;
                
                case ResultOptions.BackupAndReplaceMoveOriginal:
                    if (File.Exists(filePath))
                        File.Move(filePath, configuration.BackupDirectory);

                    targetFilePath = filePath;
                    break;
                
                case ResultOptions.CreateNew:
                    var amount = filePath.Length - configuration.SourceDirectory.Length;
                    var sub = filePath.Substring(configuration.SourceDirectory.Length, amount);

                    targetFilePath = configuration.TargetDirectory + sub;

                    if (File.Exists(targetFilePath))
                        File.Delete(targetFilePath);

                    break;
                default:
                    throw new NotImplementedException("Result Options not implemented");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));

            File.WriteAllText(targetFilePath, FileContent);
            SaveLogs(targetFilePath);
        }

        public bool HasChanges()
        {
            if (Replacements.Count > 0)
                return true;

            if (Additions.Count > 0)
                return true;

            if (Subtractions.Count > 0)
                return true;

            return false;
        }
    }
}
