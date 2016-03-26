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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ToolchainHelper.Instance.SetCommonOption("-fstack-usage");
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ToolchainHelper.Instance.RemoveCommonOption("-fstack-usage");
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            TryNavigateFromRow(sender, e);
        }
        
        private void TryNavigateFromRow(object sender, RoutedEventArgs e)
        {
            var row = ItemsControl.ContainerFromElement(sender as DataGrid, e.OriginalSource as DependencyObject) as DataGridRow;

            if (row == null)
                return;

            var function = row.Item as FunctionStackInfo;
            if (function == null)
                return;


            Navigate(function);
        }

        private static void Navigate(FunctionStackInfo function)
        {
            if (function == null)
                return;

            // The SU file uses line 1 indexed line and column, Atmel Studio uses 0 index
            Atmel.Studio.Framework.FileOpenHelper.OpenFile(function.FullPath,
                            function.Line == 0 ? 0 : function.Line - 1,
                            function.Column == 0 ? 0 : function.Column - 1);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Navigate((sender as DataGrid)?.SelectedItem as FunctionStackInfo);

        }
    }
}