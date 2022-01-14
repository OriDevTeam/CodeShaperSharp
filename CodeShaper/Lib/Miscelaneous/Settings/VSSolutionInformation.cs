// System Namesapces
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;
using Lib.Settings;


// Application Namespaces


// Library Namespaces
using Microsoft.Build.Construction;



namespace DevLibrary.Settings
{
    internal class VSSolutionInformation
    {
        SolutionFile solutionFile;

        public VSSolutionInformation(string solutionPath)
        {
            // solutionFile = SolutionFile.Parse(solutionPath + "client.sln");

            // var projectInSolution = FindProject("UserInterface");
            
            // var projectVcxjProjFile = File.ReadAllText(projectInSolution.AbsolutePath);

            // var dd = VCXProjectFile.DeserializeToObject<VCXProjectFile>(projectInSolution.AbsolutePath);

            // var vcProj = VCXProj.DeserializeToObject<VCXProj>(projectInSolution.AbsolutePath);
        }

        public ProjectInSolution FindProject(string projectName)
        {
            foreach (var project in solutionFile.ProjectsInOrder)
            {
                if (project.ProjectName == projectName)
                    return project;
            }

            return null;
        }

    }

}
