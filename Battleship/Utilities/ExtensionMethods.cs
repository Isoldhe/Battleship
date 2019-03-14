using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.Utilities
{
    public static class ExtensionMethods
    {
        public static List<string> WordWrap(this string hugeLine, int maxWidth, int maxHeight)
        {
            var lines = new List<string>();
            var words = hugeLine.Split(' ').ToList();

            while (words.Any() && lines.Count < maxHeight)
            {
                int currentLength = 0;
                var line = words.TakeWhile(word =>
                {
                    bool lengthReached = (currentLength += word.Length) < maxWidth;
                    currentLength++; //add count for a space
                    return lengthReached;
                }).ToList();

                if (line.Any())
                {
                    lines.Add(string.Join(" ", line));
                    words.RemoveRange(0, line.Count);
                }
                else
                {
                    lines.Add(words[0].Substring(0, maxWidth));
                    words[0] = words[0].Remove(0, maxWidth);
                }
            }

            return lines;
        }
    }
}
