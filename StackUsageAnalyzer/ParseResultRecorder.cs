using System;
using System.Collections.Generic;
using System.Linq;

namespace StackUsageAnalyzer
{
    internal class ParseResultRecorder
    {
        private static StackUsageAnalyzerPackage package;
        private StackUsageAnalyzerPackage stackUsageAnalyzerPackage;

        private ParseResultRecorder(StackUsageAnalyzerPackage stackUsageAnalyzerPackage)
        {
            this.stackUsageAnalyzerPackage = stackUsageAnalyzerPackage;
        }

        internal static void Initialize(StackUsageAnalyzerPackage stackUsageAnalyzerPackage)
        {
            Instance = new ParseResultRecorder(stackUsageAnalyzerPackage);
        }

        internal static ParseResultRecorder Instance { get; private set; }

        public Dictionary<string, IFunctionStackInfo> Result { get; } = new Dictionary<string, IFunctionStackInfo>();
        internal void RecordResult(IEnumerable<IFunctionStackInfo> result)
        {
            foreach(var r in result)
            {
                Result[r.FunctionName] = r;
            }
        }

        internal IFunctionStackInfo GetResult(string functionName)
        {
            IFunctionStackInfo result;
            if (!Result.TryGetValue(functionName, out result))
                return null;
            return result;
        }
    }
}