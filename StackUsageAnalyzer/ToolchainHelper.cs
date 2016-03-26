using Atmel.Studio.Services;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;

namespace StackUsageAnalyzer
{
    internal class ToolchainHelper
    {
        private Package package;

        private IServiceProvider serivceProvider => this.package;

        private DTE dte;
        public DTE Dte { get { return dte ?? (dte = serivceProvider.GetService(typeof(DTE)) as DTE); } }

        public ToolchainHelper(Package package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));

            this.package = package;
            this.listener = SolutionEventsListener.Instance;

            listener.BuildStartedEvent += Listener_BuildStartedEvent;
            listener.BuildFinishedEvent += Listener_BuildFinishedEvent;
        }

        private void Listener_BuildFinishedEvent(SolutionEventsListener sender, EventArgs e)
        {
            var args = e as SolutionBuildFinishedEvent;
            //var options = GetCommonOptions(args.ProjectName);
        }

        private void Listener_BuildStartedEvent(SolutionEventsListener sender, EventArgs e)
        {
            var args = e as SolutionBuildStartedEvent;
        }

        private SolutionEventsListener listener;

        public static ToolchainHelper Instance { get; private set; }

        internal static void Initialize(Package package)
        {
            Instance = new ToolchainHelper(package);
        }

        public Project FindProject(string projectName)
        {
            if (Dte != null)
                for (int projectIndex = 1; projectIndex <= Dte.Solution.Projects.Count; projectIndex++)
                    if (Dte.Solution.Projects.Item(projectIndex).Name == projectName)
                        return Dte.Solution.Projects.Item(projectIndex);

            return null;
        }

        private string GetActiveProjectName()
        {
            if (Dte == null)
                return string.Empty;

            var projects = Dte.ActiveSolutionProjects as Array;
            if (projects == null || projects.Length == 0)
                return string.Empty;

            return (projects.GetValue(0) as Project)?.Name;
        }
        
        public void SetCommonOption(string option)     
        {
            SetCommonOption(GetActiveProjectName(), option);
        }

        public void SetCommonOption(string projectName, string option)
        {
            var project = FindProject(projectName);
            if (project == null)
                return;

            var properties = project.Properties;
            var toolchainoptions = properties.Item("ToolchainOptions").Value as Atmel.Studio.Extensibility.Toolchain.ProjectToolchainOptions;
            
            if (toolchainoptions == null)
                return;

            if (toolchainoptions.CCompiler != null && !toolchainoptions.CCompiler.MiscellaneousSettings.Contains(option))
                toolchainoptions.CCompiler.MiscellaneousSettings += $" { option }";
            else if (toolchainoptions.CppCompiler != null && !toolchainoptions.CppCompiler.MiscellaneousSettings.Contains(option))
                toolchainoptions.CppCompiler.MiscellaneousSettings += $" { option }";

            (project.Object as IProjectHandle).SetPropertyForAllConfiguration(ProjectPropertyConstants.ToolchainSettings, toolchainoptions.ToString());

            project.Save();
        }

        public void RemoveCommonOption(string option)
        {
            RemoveCommonOption(GetActiveProjectName(), option);
        }

        public void RemoveCommonOption(string projectName, string option)
        {
            var project = FindProject(projectName);
            if (project == null)
                return;

            var properties = project.Properties;
            var toolchainoptions = properties.Item("ToolchainOptions").Value as Atmel.Studio.Extensibility.Toolchain.ProjectToolchainOptions;

            if (toolchainoptions == null)
                return;

            if (toolchainoptions.CCompiler != null && !toolchainoptions.CCompiler.MiscellaneousSettings.Contains(option))
                toolchainoptions.CCompiler.MiscellaneousSettings.Replace(option, string.Empty);
            else if (toolchainoptions.CppCompiler != null && !toolchainoptions.CppCompiler.MiscellaneousSettings.Contains(option))
                toolchainoptions.CppCompiler.MiscellaneousSettings.Replace(option, string.Empty);

            (project.Object as IProjectHandle).SetPropertyForAllConfiguration(ProjectPropertyConstants.ToolchainSettings, toolchainoptions.ToString());

            project.Save();
        }
    }
}