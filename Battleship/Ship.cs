using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Ship
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public int XLocation { get; set; }
        public int YLocation { get; set; }
        public string Position { get; set; } // horizontal (H) or vertical (V)

        public Ship(string name, int size, int xLocation, int yLocation, string position)
        {
            Name = name;
            Size = size;
            XLocation = xLocation;
            YLocation = yLocation;
            Position = position;
        }
    }
}
