using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battleship.DisplayElements;
using Battleship.Enums;

namespace Battleship.GameModels
{
    class Bot
    {
        public string Name { get; set; }
        public BattleField BotBattleField { get; set; }

        public Bot()
        {
            // TODO: create random names
            Name = "Trump";

            BotBattleField = new BattleField()
            {
                Left = 50,
                Top = 1,
            };

            PlaceShips();
        }

        // Places ships at random positions
        private void PlaceShips()
        {
            Random random = new Random();

            // For each shiptype, create random position
            foreach (ShipType shipType in (ShipType[]) Enum.GetValues(typeof(ShipType)))
            {
                Orientation orientation;
                int maxValue = 10;
                int xLocation = 0;
                int yLocation = 0;

                switch (shipType)
                {
                    case ShipType.Destroyer:
                        maxValue -= 2;
                        break;
                    case ShipType.AircraftCarrier:
                        maxValue -= 5;
                        break;
                    case ShipType.Submarine:
                        maxValue -= 4;
                        break;
                    case ShipType.Battleship:
                    case ShipType.Cruiser:
                        maxValue -= 3;
                        break;
                    default:
                        maxValue = 10;
                        break;
                }

                int orient = random.Next(0, 2);
                if (orient < 1)
                {
                    orientation = Orientation.Horizontal;
                    // min value is inclusive, max value is exclusive
                    xLocation = random.Next(0, maxValue);
                    yLocation = random.Next(0, 9);
                }
                else
                {
                    orientation = Orientation.Vertical;
                    xLocation = random.Next(0, 9);
                    yLocation = random.Next(0, maxValue);
                }
                
                BotBattleField.AddShip(new Ship(shipType, xLocation, yLocation, orientation));
            }

            BotBattleField.AddShip(new Ship(ShipType.Destroyer, 0, 0, Orientation.Horizontal));
            BotBattleField.AddShip(new Ship(ShipType.AircraftCarrier, 9, 2, Orientation.Vertical));
            BotBattleField.AddShip(new Ship(ShipType.Submarine, 5, 3, Orientation.Horizontal));
            BotBattleField.AddShip(new Ship(ShipType.Cruiser, 3, 1, Orientation.Vertical));
            BotBattleField.AddShip(new Ship(ShipType.Battleship, 6, 7, Orientation.Vertical));
        }
    }
}
