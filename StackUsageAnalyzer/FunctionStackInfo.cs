namespace StackUsageAnalyzer
{
    using System;
    using System.Text.RegularExpressions;

    public class FunctionStackInfo
    {
        public string File { get; private set; }
        public uint Line { get; private set; }
        public uint Column { get; private set; }
        public string FunctionName { get; private set; }
        public uint Bytes { get; private set; }
        public Qualifier Qualifiers { get; private set; }

        public FunctionStackInfo()
        {
            
        }

        public static FunctionStackInfo Create(string file, string name, uint line, uint column, uint bytes, Qualifier qualifiers)
        {
            return new FunctionStackInfo()
                       {
                           Bytes = bytes,
                           Column = column,
                           File = file,
                           FunctionName = name,
                           Line = line,
                           Qualifiers = qualifiers
                       };
        }

        public static FunctionStackInfo Create(string suFileLine)
        {
            
            if (!LineRegex.IsMatch(suFileLine))
                throw new ArgumentException($"Line '{ suFileLine }' does not match SU file line", nameof(suFileLine));

            var matches = LineRegex.Match(suFileLine);

            var file = matches.Groups[1].Value;
            var line = uint.Parse(matches.Groups[2].Value);
            var column = uint.Parse(matches.Groups[3].Value);
            var name = matches.Groups[4].Value;
            var bytes = uint.Parse(matches.Groups[5].Value);
            var qualifier = ParseQualifier(matches.Groups[6].Value.Trim());

            return Create(file, name, line, column, bytes, qualifier);
        }
        
        public static Qualifier ParseQualifier(string q)
        {
            Qualifier qualifier = Qualifier.None;
            // Can be one of 'static', 'dynamic' and 'bounded', or combination separated by ',' ie dynamic,bounded
            var parts = q.Split(new[] { ',' });
            foreach (var part in parts)
            {
                if (part.Equals("static", StringComparison.InvariantCultureIgnoreCase))
                    qualifier |= Qualifier.Static;
                else if (part.Equals("dynamic", StringComparison.InvariantCultureIgnoreCase))
                    qualifier |= Qualifier.Dynamic;
                else if (part.Equals("bounded", StringComparison.InvariantCultureIgnoreCase))
                    qualifier |= Qualifier.Bounded;
            }

            return qualifier;
        }

        // events.c:70:10:_events_find_bit_position	0	static
        private static readonly Regex LineRegex = new Regex(@"(\S+):(\d+):(\d+):(\S+)\s(\d+)\s(.+)", RegexOptions.Compiled);

        [Flags]
        public enum Qualifier
        {
            None = (0 << 0),
            /// <summary>
            /// The qualifier static means that the function frame size is purely static. It usually means that all local 
            /// variables have a static size. In this case, the second field is a reliable measure of the function stack utilization.
            /// </summary>
            Static = (1 << 0),

            /// <summary>
            /// The qualifier dynamic means that the function frame size is not static. It happens mainly when some local 
            /// variables have a dynamic size. When this qualifier appears alone, the second field is not a reliable measure 
            /// of the function stack analysis. When it is qualified with bounded, it means that the second field is a reliable 
            /// maximum of the function stack utilization.
            /// </summary>
            Dynamic = (1 << 1),

            /// <summary>
            /// 
            /// </summary>
            Bounded = (1 << 2)
        }
    }
}