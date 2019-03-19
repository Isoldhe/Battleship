using Battleship.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Battleship";

            LowLevelConsoleFunctions.SetInputMode(
                ConsoleModeInput.ENABLE_WINDOW_INPUT | 
                ConsoleModeInput.ENABLE_MOUSE_INPUT | 
                ConsoleModeInput.ENABLE_EXTENDED_FLAGS);

            LowLevelConsoleFunctions.SetOutputMode(
                ConsoleModeOutput.DISABLE_NEWLINE_AUTO_RETURN);

            var game = new Game();
            game.Run();
        }
    }
}
