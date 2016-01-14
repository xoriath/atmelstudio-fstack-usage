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
            //var options = GetCommonOptions(args.ProjectName);
            //SetCommonOption(args.ProjectName, options + " -fstack-usage");
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

        public string SetCommonOption(string projectName, string option)
        {
            var project = FindProject(projectName);
            if (project == null)
                return null;

            var properties = project.Properties;
            var toolchainoptions = properties.Item("ToolchainOptions").Value as Atmel.Studio.Extensibility.Toolchain.ProjectToolchainOptions;

            if (toolchainoptions == null)
                return null;

            if (toolchainoptions.CCompiler != null)
                return toolchainoptions.CCompiler.MiscellaneousSettings += $" { option }";
            else if (toolchainoptions.CppCompiler != null)
                return toolchainoptions.CppCompiler.MiscellaneousSettings += $" { option }";
            else
                return null;
        }


            public string GetCommonOptions(string projectName)
        {
            var project = FindProject(projectName);
            if (project == null)
                return null;

            var properties = project.Properties;
            var toolchainoptions = properties.Item("ToolchainOptions").Value as Atmel.Studio.Extensibility.Toolchain.ProjectToolchainOptions;

            if (toolchainoptions == null)
                return null;

            if (toolchainoptions.CCompiler != null)
                return toolchainoptions.CCompiler.MiscellaneousSettings;
            else if (toolchainoptions.CppCompiler != null)
                return toolchainoptions.CppCompiler.MiscellaneousSettings;
            else
                return null;
        }
    }
}