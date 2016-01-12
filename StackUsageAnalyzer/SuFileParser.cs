
namespace StackUsageAnalyzer
{
    using System.Collections.Generic;
    using System.Linq;

    public class SuFileParser
    {
        public IEnumerable<FunctionStackInfo> Parse(IEnumerable<string> input)
        {
            return input.Select(FunctionStackInfo.Create);
        }

    }
}
