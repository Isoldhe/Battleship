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

        public Game()
        {
            //TODO initialize battlefield according to player preferences
            _battleField = new BattleField()
            {
                BattleFieldLeft = 1,
                BattleFieldTop = 1,
            };
            _battleField.LoadTestData();

            ResetView(_battleField.BattleFieldWidthInChars + 1, _battleField.BattleFieldHeightInChars + 1);

            _battleField.RefreshField();
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
                        _battleField.BattleFieldLeft--;
                        break;
                    case ConsoleKey.UpArrow:
                        _battleField.BattleFieldTop--;
                        break;
                    case ConsoleKey.RightArrow:
                        _battleField.BattleFieldLeft++;
                        break;
                    case ConsoleKey.DownArrow:
                        _battleField.BattleFieldTop++;
                        break;


                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        //TODO accept input
                        break;
                    case ConsoleKey.Escape:
                        //quit
                        return;
                    case ConsoleKey.R:
                        ResetView(_battleField.BattleFieldWidthInChars + 10, _battleField.BattleFieldHeightInChars + 10);
                        _battleField.RefreshField();
                        break;
                    default:
                        string key = keyStroke.KeyChar.ToString();
                        //do nothing
                        break;
                }
            }
        }

        public void ResetView(int totalWidth, int totalHeight)
        {
            Console.CursorVisible = false;
            Console.Clear();

            Console.SetWindowSize(totalWidth, totalHeight);
            Console.SetBufferSize(totalWidth, totalHeight);

            //set size again after setting buffer to clear the scrollbar area
            Console.SetWindowSize(totalWidth, totalHeight);
        }
    }
}
