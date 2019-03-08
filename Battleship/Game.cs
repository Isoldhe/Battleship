using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Game
    {
        public static void Main(string[] args)
        {
            BattleField battleField = new BattleField();
            battleField.CreateShips();
            battleField.CreateFirstRow();
            battleField.CreateBattlefield();
        }

        
    }
}
