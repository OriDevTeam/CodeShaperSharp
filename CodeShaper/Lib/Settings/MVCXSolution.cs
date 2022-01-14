﻿// System Namespaces
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;


// Application Namespaces


// Library Namespaces


namespace Lib.Settings
{

    public class MVCXSolution
    {
        public string Name { get;}
        public List<MVCXProject> projects;

        public MVCXSolution(string solutionPath)
        {
            Name = $"Solution {Path.GetFileName(Path.GetDirectoryName(solutionPath))}";
            
            var solutionFile = File.ReadAllText(solutionPath + "client.sln");
            projects = LoadProjectsRegex(solutionPath, solutionFile);
            
        }

        private List<MVCXProject> LoadProjectsRegex(string solutionPath, string solutionFile)
        {
            var projects = new List<MVCXProject>();

            var pattern = $@"Project\(""(.*?)""\)\s*=\s*""(.*?)""\s*,\s*""(.*?)""\s*,\s*""(.*?)\s?EndProject";
             
            Regex rg = new(pattern, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

            var projectsInformation = rg.Matches(solutionFile);

            for (var idx = 0; idx < projectsInformation.Count; idx++)
            {

                var name = projectsInformation[idx].Groups[2].Value;

                var strGuid = projectsInformation[idx].Groups[1].Value;
                var guidStr = strGuid.Substring(1, strGuid.Length - 2);
                var guid = Guid.Parse(guidStr);

                var otherStrGuid = projectsInformation[idx].Groups[4].Value;
                var guidOtherStr = otherStrGuid.Substring(1, otherStrGuid.Length - 4);
                var otherGuid = Guid.Parse(guidOtherStr);

                var dir = Path.GetDirectoryName(solutionPath) + "\\" + projectsInformation[idx].Groups[3].Value;
                projects.Add(new MVCXProject(name, dir, guid, otherGuid));
            }

            return projects;
        }
    }

    public class MVCXProject
    {
        public string Name { get; private set; }
        public string Path { get; private set; }
        public List<MVCXProjectConfiguration> configurations = new();
        public List<string> includes = new();
        public List<string> modules = new();

        public MVCXProject(string projectName, string projectFile, Guid guid, Guid otherGuid)
        {
            Name = projectName;
            Path = projectFile;
            var projectXml = XDocument.Load(projectFile);

            LoadProject(projectXml);
        }

        public void LoadProject(XDocument projectDocument)
        {
            configurations = LoadConfigurations(projectDocument);
            includes = LoadIncludes(projectDocument);
            modules = LoadModules(projectDocument);
        }

        List<MVCXProjectConfiguration> LoadConfigurations(XDocument projectDocument)
        {
            var configurations = new List<MVCXProjectConfiguration>();

            var configurationData = projectDocument.Descendants().Where(p => p.Name.LocalName == "ItemDefinitionGroup");

            foreach (var configuration in configurationData)
            {
                var config = new MVCXProjectConfiguration(configuration);
                configurations.Add(config);
            }

            return configurations;
        }

        List<string> LoadIncludes(XDocument projectDocument)
        {
            var includes = new List<string>();

            var includeData = projectDocument.Descendants().Where(p => p.Name.LocalName == "ClInclude");

            foreach (var include in includeData)
            {
                var attr = include.Attribute("Include");

                if (attr is not null)
                    includes.Add(attr.Value);
            }

            return includes;
        }

        List<string> LoadModules(XDocument projectDocument)
        {
            var modules = new List<string>();

            var includeData = projectDocument.Descendants().Where(p => p.Name.LocalName == "ClCompile");

            foreach (var include in includeData)
            {
                var attr = include.Attribute("Include");

                if (attr is not null)
                    modules.Add(attr.Value);
            }

            return modules;
        }
    }

    public class MVCXProjectConfiguration
    {
        public string AdditionalIncludeDirectories { get; private set; }

        public MVCXProjectConfiguration(XElement projectConfigurationElement)
        {

            AdditionalIncludeDirectories = GetAdditionalIncludesFromProject(projectConfigurationElement);
        }

        private static string GetAdditionalIncludesFromProject(XContainer projectConfigurationElement)
        {
            var additionalIncludes = "";

            var additionalIncludesData = projectConfigurationElement.Descendants().Where(p => p.Name.LocalName == "AdditionalIncludeDirectories");

            foreach (var additionalInclude in additionalIncludesData)
            {
                additionalIncludes = additionalInclude.Value;
            }

            return additionalIncludes;
        }
    }

}
