using System;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Atmel.Studio.Services;
using System.ComponentModel.Composition;
using System.IO;

namespace StackUsageAnalyzer
{
    class SolutionEventsListener : ATProjectListener, IVsUpdateSolutionEvents2, IDisposable
    {
        private SolutionEventsListener(Package package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));

            this.package = package;

        }

        private readonly Package package;
        private IServiceProvider ServiceProvider => this.package;

        public event SolutionEventHandler BuildFinishedEvent;
        public event SolutionEventHandler BuildStartedEvent;
        public delegate void SolutionEventHandler(SolutionEventsListener sender, EventArgs e);

        public static void Initialize(Package package)
        {
            if (Instance != null)
                Instance.Dispose(false);

            Instance = new SolutionEventsListener(package);

            Instance.AdivseSolutionUpdates();
        }

        private void AdivseSolutionUpdates()
        {
            solutionBuildManager = ServiceProvider.GetService(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager2;

            solutionBuildManager?.AdviseUpdateSolutionEvents(this, out updateSolutionEventsCookie);
        }

        public static SolutionEventsListener Instance { get; private set; }

        int IVsUpdateSolutionEvents.OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateProjectCfg_Begin(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, ref int pfCancel)
        {
            var name = GetNameProperty(pHierProj);
            var folder = GetProjectDirectoryProperty(pHierProj);

            if (!string.IsNullOrEmpty(name))
                BuildStartedEvent(this, new SolutionBuildStartedEvent() { ProjectName = name, ProjectFolder = folder });

            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateProjectCfg_Done(IVsHierarchy pHierProj, IVsCfg pCfgProj, IVsCfg pCfgSln, uint dwAction, int fSuccess, int fCancel)
        {
            var name = GetNameProperty(pHierProj);
            var folder = GetProjectDirectoryProperty(pHierProj);
                
            if (!string.IsNullOrEmpty(name))
                BuildFinishedEvent(this, new SolutionBuildFinishedEvent() { ProjectName = name, ProjectFolder = folder });

            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_Cancel()
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateSolution_Cancel()
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents.UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        int IVsUpdateSolutionEvents2.UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return VSConstants.S_OK;
        }

        private string GetNameProperty(IVsHierarchy pHierProj)
        {
            object o;
            if (pHierProj.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_Name, out o) != VSConstants.S_OK)
                return string.Empty;
            return o as string;
        }

        private string GetProjectDirectoryProperty(IVsHierarchy pHierProj)
        {
            object o;
            if (pHierProj.GetProperty((uint)VSConstants.VSITEMID.Root, (int)__VSHPROPID.VSHPROPID_ProjectDir, out o) != VSConstants.S_OK)
                return string.Empty;
            return o as string;
        }


        private IVsSolutionBuildManager2 solutionBuildManager;
        private uint updateSolutionEventsCookie;

        public bool HasAdvised => updateSolutionEventsCookie != 0;

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                if (solutionBuildManager != null && updateSolutionEventsCookie != 0)
                    solutionBuildManager.UnadviseUpdateSolutionEvents(updateSolutionEventsCookie);

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SolutionEventsListener() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


        public override void OnProjectCreated(ProjectCreatedEventArgs args)
        {
            base.OnProjectCreated(args);
        }
    }
}
