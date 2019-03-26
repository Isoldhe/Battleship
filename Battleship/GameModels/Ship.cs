using Battleship.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.GameModels
{
    public class Ship
    {
        public string Name { get; private set; }
        public ShipType ShipType { get; }
        public int Size { get; set; }
        public int XLocation { get; set; }
        public int YLocation { get; set; }
        public Orientation Orientation { get; set; } // horizontal (H) or vertical (V)

        public Ship(ShipType shipType, int xLocation, int yLocation, Orientation orientation)
        {
            Name = shipType.ToString();
            ShipType = shipType;
            switch (shipType)
            {
                case ShipType.Destroyer:
                    Size = 2;
                    break;
                case ShipType.AircraftCarrier:
                    Size = 5;
                    break;
                case ShipType.Submarine:
                    Size = 4;
                    break;
                case ShipType.Battleship:
                case ShipType.Cruiser:
                    Size = 3;
                    break;
                default:
                    throw new ArgumentException($"Ship type [{shipType}] does not have a defined Size!", nameof(shipType));
            }
            XLocation = xLocation;
            YLocation = yLocation;
            Orientation = orientation;
        }
    }
}
