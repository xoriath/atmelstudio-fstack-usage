//------------------------------------------------------------------------------
// <copyright file="StackAnalysisToolWindowControl.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace StackUsageAnalyzer
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for StackAnalysisToolWindowControl.
    /// </summary>
    public partial class StackAnalysisToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StackAnalysisToolWindowControl"/> class.
        /// </summary>
        public StackAnalysisToolWindowControl()
        {
            this.InitializeComponent();

            this.DataContext = new StackAnalysisViewModel();
        }
    }
}