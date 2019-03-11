using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            Console.WriteLine("Add your Destroyer to the battlefield. It's size 2. Where do you want to locate it?  ");
            string input = Console.ReadLine();
            string xPos = "";
            string yPos = "";

            string pattern = "^[a-jA-J](?:[1-9]|0[1-9]|10)$";
            Regex rgx = new Regex(pattern);
            bool validInput = rgx.IsMatch(input);
            if (input != null && validInput)
            {
                xPos = input.Substring(0, 1);
                yPos = input.Remove(0, 1);
                Console.WriteLine("xPos = " + xPos);

                Console.WriteLine("Place it horizontally (H) or vertically (V)?  ");
                string position = Console.ReadLine();
                if (position.ToUpper() == "H" || position.ToUpper() == "V")
                {
                    Console.WriteLine("The position is " + position.ToUpper());

                    BoardPosition bp = new BoardPosition();
                    Dictionary<string, int> hoi = bp.Dictionary;
                    int test;
                    if (hoi.TryGetValue(xPos.ToUpper(), out test))
                    {
                        Console.WriteLine(test.ToString());
                    }

                    // A, B, C, ... = rows / XLocation 0, 1, 2, ...
                    // 1, 2, 3, ... = columns/ YLocation of number - 1
                    // parse int...
                    
                }
            }

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
