using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StackUsageAnalyzer
{
    class ParserHandler
    {
        public ParserHandler(string rootFolder)
        {
            Folder = rootFolder;
        }
        
        public StackAnalysisViewModel Model { get; internal set; }
        
        public IEnumerable<FunctionStackInfo> Parse()
        {
            var files = ListFiles();
            var info = SuFileParser.Parse(files.SelectMany(f => File.ReadLines(f).Select(l => new LineInstance() { Line = l, FileName = f })));

            Model?.Items.Clear();
            foreach (var functionInfo in info.OrderBy(f => f.Bytes))
                Model?.Items.Add(functionInfo);

            return info;
        }

        public string Folder { get; internal set; }

        private IEnumerable<string> ListFiles()
        {
            return Directory.EnumerateFiles(Folder, @"*.su", SearchOption.AllDirectories);
        }
    }
}
