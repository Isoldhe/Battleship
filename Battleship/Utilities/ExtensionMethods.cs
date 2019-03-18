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

            foreach (var lessHugeLine in hugeLine.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                var words = lessHugeLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!words.Any() && lines.Count < maxHeight)
                {
                    lines.Add("");
                }

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
            }

            return lines;
        }

        /// <summary>
        /// Creates a multidimensional array.
        /// </summary>
        /// <param name="createNew">(row, column) => return new T;</param>
        public static T[][] CreateMultiDimensionalArray<T>(int width, int height, Func<int, int, T> createNew = null)
        {
            var multiDimensionalArray = new T[height][];
            for (int row = 0; row < height; row++)
            {
                multiDimensionalArray[row] = new T[width];
                for (int column = 0; column < width; column++)
                {
                    if (createNew != null)
                    {
                        multiDimensionalArray[row][column] = createNew(row, column);
                    }
                }
            }
            return multiDimensionalArray;
        }
    }
}
