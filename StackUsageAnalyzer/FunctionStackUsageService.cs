using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace StackUsageAnalyzer
{
    [Guid("2E97FC2E-EFC4-4DE8-91CA-EC4A524B5CFA")]
    public interface SFunctionStackUsageService
    {
    }

    [Guid("D4034E02-323C-4660-8137-B690FFAB54C1")]
    [ComVisible(true)]
    public interface IFunctionStackUsageService
    {
        IFunctionStackInfo GetStackInfo(string functionName);
    }

    public interface IFunctionStackInfo
    {
        uint Bytes { get; }
        uint Column { get; }
        string Directory { get; }
        string File { get; }
        string FunctionName { get; }
        uint Line { get; }
        FunctionStackInfo.Qualifier Qualifiers { get; }
    }

    internal class FunctionStackUsageService : IFunctionStackUsageService
    {
        private StackUsageAnalyzerPackage stackUsageAnalyzerPackage;

        public FunctionStackUsageService(StackUsageAnalyzerPackage stackUsageAnalyzerPackage)
        {
            this.stackUsageAnalyzerPackage = stackUsageAnalyzerPackage;
        }

        IFunctionStackInfo IFunctionStackUsageService.GetStackInfo(string functionName)
        {
            return ParseResultRecorder.Instance.GetResult(functionName);
        }
    }
}