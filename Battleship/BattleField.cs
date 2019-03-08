using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class BattleField
    {
        private string[][] battleField = new string[10][];
        private List<Ship> ships = new List<Ship>();

        public void CreateFirstRow()
        {
            char horizontalCoordinates = 'A';
            Console.Write("   ");
            while (horizontalCoordinates < 'K')
            {
                Console.Write(horizontalCoordinates + " ");
                horizontalCoordinates++;
            }
            Console.WriteLine();
        }

        public void CreateBattlefield()
        {
            for (int rowIndex = 0; rowIndex < battleField.Length; rowIndex++)
            {
                battleField[rowIndex] = new string[10]; // Create a row of 10 columns/cells
            }

            CreateShips();
            foreach(Ship ship in ships)
            {
                AddShip(ship);
            }

            int verticalCoordinates = 1;
            for (int rowIndex = 0; rowIndex < battleField.Length; rowIndex++)
            {
                // First column with numbers 1-10
                Console.Write((rowIndex == 9) ? verticalCoordinates + " " : verticalCoordinates + "  ");

                // Battlefield board
                for (int colIndex = 0; colIndex < battleField[rowIndex].Length; colIndex++)
                {
                    if (battleField[rowIndex][colIndex] == null)
                    {
                        Console.Write("-");
                    }
                    Console.Write(battleField[rowIndex][colIndex] + " ");
                }
                Console.WriteLine();
                verticalCoordinates++;
            }
        }

        public void AddShip(Ship ship)
        {
            battleField[ship.XLocation][ship.YLocation] = ship.Name;
            for (int i = 0; i < ship.Size; i++)
            {
                if (ship.Position == "H")
                {
                    battleField[ship.XLocation][ship.YLocation + i] = ship.Name;
                }
                else
                {
                    battleField[ship.XLocation + i][ship.YLocation] = ship.Name;
                }
            }
        }

        public void CreateShips()
        {
            Ship destroyer = new Ship("D", 2, 0, 0, "H");
            Ship aircraftCarrier = new Ship("A", 5, 2, 9, "V");
            Ship submarine = new Ship("S", 4, 3, 5, "H");
            Ship cruiser = new Ship("C", 3, 1, 3, "V");
            Ship battleship = new Ship("B", 3, 7, 6, "V");

            ships.Add(destroyer);
            ships.Add(aircraftCarrier);
            ships.Add(submarine);
            ships.Add(cruiser);
            ships.Add(battleship);
        }
    }
}
