using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackUsageAnalyzer
{
    class SolutionBuildStartedEvent : EventArgs
    {
        public string ProjectName { get; set; }

        public string ProjectFolder { get; set; }
    }

    class SolutionBuildFinishedEvent : EventArgs
    {
        public string ProjectName { get; set; }

        public string ProjectFolder { get; set; }
    }
}
