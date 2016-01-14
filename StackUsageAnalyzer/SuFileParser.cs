
namespace StackUsageAnalyzer
{
    using System.Collections.Generic;
    using System.Linq;

    public class SuFileParser
    {
        public static IEnumerable<FunctionStackInfo> Parse(IEnumerable<LineInstance> input)
        {
            return input.Where(l => !string.IsNullOrEmpty(l.Line)).Select(FunctionStackInfo.Create);
        }

        public static IEnumerable<FunctionStackInfo> Parse(IEnumerable<string> input)
        {
            return input.Where(l => !string.IsNullOrEmpty(l)).Select(FunctionStackInfo.Create);
        }
    }
}
