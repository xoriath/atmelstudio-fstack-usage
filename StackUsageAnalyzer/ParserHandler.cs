using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StackUsageAnalyzer
{
    class ParserHandler
    {
        public string Project { get; internal set; }

        public StackAnalysisViewModel Model { get; internal set; }

        // TODO: Use real path
        public string ProjectDir => @"C:\Users\Morten\Documents\Atmel Studio\7.0\LEDflasher0\LEDflasher0";

        // TODO: read conf from event?
        public string Configuration => @"Debug";
        public IEnumerable<FunctionStackInfo> Parse()
        {
            var files = ListFiles();
            var info = SuFileParser.Parse(files.SelectMany(f => File.ReadLines(f).Select(l => new LineInstance() { Line = l, FileName = f })));

            Model.Items.Clear();
            foreach (var functionInfo in info)
                Model.Items.Add(functionInfo);

            return info;
        }

        private IEnumerable<string> ListFiles()
        {
            return Directory.EnumerateFiles(Path.Combine(ProjectDir, Configuration), @"*.su", SearchOption.AllDirectories);
        }
    }
}
