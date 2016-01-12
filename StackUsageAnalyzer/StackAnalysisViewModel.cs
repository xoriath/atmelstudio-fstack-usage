using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace StackUsageAnalyzer
{
    public class StackAnalysisViewModel : DependencyObject
    {
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register(nameof(IsActive), typeof(bool), typeof(StackAnalysisToolWindowControl), 
                new FrameworkPropertyMetadata(new PropertyChangedCallback(OnIsActiveChanged)));

        private static void OnIsActiveChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var isActive = (bool?)e.NewValue;

            if (isActive.HasValue && isActive.Value == true)
                SetToolchainOption();
            else
                UnsetToolchainOption();

        }

        private static void UnsetToolchainOption()
        {
            //throw new NotImplementedException();
        }

        private static void SetToolchainOption()
        {
            //throw new NotImplementedException();
        }

        public ObservableCollection<FunctionStackInfo> Items { get; private set; } = new ObservableCollection<FunctionStackInfo>();

        private readonly SolutionEventsListener solutionListener;
        public StackAnalysisViewModel()
        {
            solutionListener = SolutionEventsListener.Instance;
            solutionListener.BuildFinishedEvent += BuildFinisedEvent;
            
        }

        private void BuildFinisedEvent(SolutionEventsListener sender, System.EventArgs e)
        {
            if (!IsActive)
                return;

            var args = e as SolutionBuildFinishedEvent;
            var handler = new ParserHandler() { Model = this, Project = args.ProjectName };
            handler.Parse();
        }
    }
}