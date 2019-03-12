using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class BoardPosition
    {
        public Dictionary<string, char> Dictionary { get; }

        public BoardPosition()
        {
            Dictionary = new Dictionary<string, char>(StringComparer.OrdinalIgnoreCase)
            {
                { "a", '0' },
                { "b", '1' },
                { "c", '2' },
                { "d", '3' },
                { "e", '4' },
                { "f", '5' },
                { "g", '6' },
                { "h", '7' },
                { "i", '8' },
                { "j", '9' }
            };
        }
    }
}
