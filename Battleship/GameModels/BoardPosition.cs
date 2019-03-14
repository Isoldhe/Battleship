using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.GameModels
{
    public class BoardPosition
    {
        public BoardPosition(int row, int column)
        {
            Coordinates = $"{(char)('A' + column)}{1 + row}";
            Row = row;
            Column = column;
            Value = ' ';
        }

        public string Coordinates { get; }
        public int Row { get; }
        public int Column { get; }
        public char Value { get; set; }

        public override string ToString()
        {
            return $"{Coordinates}: [{Value}]";
        }
    }
}
