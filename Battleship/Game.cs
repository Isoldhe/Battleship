using Battleship.DisplayElements;
using Battleship.Enums;
using Battleship.GameModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Battleship
{
    public class Game
    {
        private BattleField _battleField;
        private Display _display;
        private StatusBar _statusBar;

        public Game()
        {
            _display = new Display();

            //TODO initialize battlefield according to player preferences
            _battleField = new BattleField()
            {
                Left = 1,
                Top = 1,
            };

            LoadTestData();

            _statusBar = new StatusBar(3, 50)
            {
                Left = 1,
                Top = _battleField.Top + _battleField.Height + 1,
                Status = "Press one of the arrow keys and be surprised by the result. If you dare, press Enter."
            };

            _display.AddElement(_battleField);
            _display.AddElement(_statusBar);
        }

        private void LoadTestData()
        {
            _battleField.AddShip(new Ship(ShipType.Destroyer, 0, 0, Orientation.Horizontal));
            _battleField.AddShip(new Ship(ShipType.AircraftCarrier, 9, 2, Orientation.Vertical));
            _battleField.AddShip(new Ship(ShipType.Submarine, 5, 3, Orientation.Horizontal));
            _battleField.AddShip(new Ship(ShipType.Cruiser, 3, 1, Orientation.Vertical));
            _battleField.AddShip(new Ship(ShipType.Battleship, 6, 7, Orientation.Vertical));
        }

        public void Run()
        {
            ConsoleKeyInfo keyStroke = new ConsoleKeyInfo();
            while (keyStroke.Key != ConsoleKey.Escape)
            {
                keyStroke = Console.ReadKey(true);
                switch (keyStroke.Key)
                {
                    //TODO implement cursor
                    //currently this feature shifts the field.. not very helpful but maybe funny
                    case ConsoleKey.LeftArrow:
                        _battleField.Left--;
                        break;
                    case ConsoleKey.UpArrow:
                        _battleField.Top--;
                        break;
                    case ConsoleKey.RightArrow:
                        _battleField.Left++;
                        break;
                    case ConsoleKey.DownArrow:
                        _battleField.Top++;
                        break;


                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        //TODO accept input
                        _statusBar.Status = "OMG YOU PRESSED THE WRONG BUTTON NOW EVERYTHING WILL EXPLODE .. " +
                            "3 .. 2 .. 1 .... .. . ... .. ............................";
                        Thread.Sleep(2500);
                        _statusBar.Status = "BOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOM!!!!";
                        Thread.Sleep(700);
                        break;
                    case ConsoleKey.Escape:
                        //quit
                        return;
                    default:
                        string key = keyStroke.KeyChar.ToString();
                        //do nothing
                        break;
                }
            }
        }

    }
}
