using Battleship.DisplayElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Game
    {
        private BattleField _battleField;
        private Display _display;

        public Game()
        {
            _display = new Display();

            //TODO initialize battlefield according to player preferences
            _battleField = new BattleField()
            {
                Left = 1,
                Top = 1,
            };
            _battleField.LoadTestData();

            _display.AddElement(_battleField);
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
