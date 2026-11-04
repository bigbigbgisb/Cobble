using Cobble.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Cobble.Parsers
{
    public class MemoParser
    {
        public string Serialize(Dictionary<string, ObservableCollection<MemoInfo>> source)
        {
            string result = "";
            foreach (var pair in source)
            {
                result += $"{pair.Key}{Environment.NewLine}";
                foreach (var item in pair.Value)
                {
                    result += $"    {item.Memo}{Environment.NewLine}";
                }
            }
            return result;
        }
        public Dictionary<string, ObservableCollection<MemoInfo>> Deserialize(string source)
        {
            Dictionary<string, ObservableCollection<MemoInfo>> parsed = [];
            string[] sourceLines = source.Split(Environment.NewLine);

            string key = "其他";
            foreach (var line in sourceLines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.Length < 4) key = line;
                else if (line[0..4] != "    ") key = line;
                if (!parsed.ContainsKey(key))
                    parsed[key] = [];
                if (key != line)
                    parsed[key].Add(new MemoInfo(line[4..], key));
            }

            return parsed;
        }
    }
}
