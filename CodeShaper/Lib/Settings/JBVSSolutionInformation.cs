// System Namesapces
using System;


// Application Namespaces


// Library Namespaces
using JetBrains.ProjectModel.SolutionFileParser;
using JetBrains.Util;


namespace DevLibrary.Settings
{
    internal class JBVSSolutionInformation
    {
        public ISlnFile solutionFile;

        public JBVSSolutionInformation(string solutionPath)
        {
            var solutionFileText = FileSystemPath.Parse(solutionPath);

            solutionFile = SolutionFileParser.ParseFile(solutionFileText);
        }

        public string[] GetAdditionalIncludesFromVCXProject()
        {
            string[] externalIncludes = Array.Empty<string>();



            return externalIncludes;
        }
    }
}
