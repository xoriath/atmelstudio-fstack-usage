
namespace StackUsageAnalyzer
{
    using System.Collections.Generic;
    using System.Linq;

    public class SuFileParser
    {
        public static IEnumerable<FunctionStackInfo> Parse(IEnumerable<string> input)
        {
            return input.Where(s => !string.IsNullOrEmpty(s)).Select(FunctionStackInfo.Create);
        }
    }
}
