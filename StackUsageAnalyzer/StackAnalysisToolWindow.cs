//------------------------------------------------------------------------------
// <copyright file="StackAnalysisToolWindow.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace StackUsageAnalyzer
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("ffbd67c0-1100-4fa5-8c20-cc10264c540b")]
    public class StackAnalysisToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StackAnalysisToolWindow"/> class.
        /// </summary>
        public StackAnalysisToolWindow() : base(null)
        {
            this.Caption = "StackAnalysisToolWindow";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new StackAnalysisToolWindowControl();
        }
    }
}
