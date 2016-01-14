namespace StackUsageAnalyzer
{
    public class LineInstance
    {
        public string FileName { get; set; }
        public string Line { get; set; }

        public override string ToString()
        {
            return $"{ Line } => ${ FileName }";
        }
    }
}