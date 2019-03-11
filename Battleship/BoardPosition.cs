using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class BoardPosition
    {
        public Dictionary<string, int> Dictionary { get; }

        public BoardPosition()
        {
            Dictionary = new Dictionary<string, int>
            {
                { "A", 0 },
                { "B", 1 },
                { "C", 2 },
                { "D", 3 },
                { "E", 4 },
                { "F", 5 },
                { "G", 6 },
                { "H", 7 },
                { "I", 8 },
                { "J", 9 }
            };
        }
    }
}
