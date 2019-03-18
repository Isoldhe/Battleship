using Battleship.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.GameModels
{
    public delegate void DamagedEvent(Ship sender);

    public class Ship
    {
        public event DamagedEvent Damaged;

        private int _health;

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
            _health = Size;
        }

        public string Name { get; }
        public ShipType ShipType { get; }
        public int Size { get; set; }
        public int XLocation { get; set; }
        public int YLocation { get; set; }
        public Orientation Orientation { get; set; } // horizontal (H) or vertical (V)

        public int Health
        {
            get => _health;
            set
            {
                _health = value;
                Damaged?.Invoke(this);
            }
        }

        public override string ToString()
        {
            return $"[{Name[0]}] {Name} Size: {Size}";
        }
    }
}
