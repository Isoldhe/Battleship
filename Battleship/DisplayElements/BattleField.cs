using Battleship.Enums;
using Battleship.GameModels;
using Battleship.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Battleship.DisplayElements
{
    public class BattleField : DisplayElement
    {

        private static readonly int PLAYFIELD_LEFT = 2;
        private static readonly int PLAYFIELD_TOP = 1;
        private static readonly int PLAYFIELD_HORIZONTAL_SPACING = 2;
        private static readonly int PLAYFIELD_VERTICAL_SPACING = 1;

        private static readonly int MIN_WIDTH = 5;
        private static readonly int MAX_WIDTH = 26; //only 26 characters in the alphabet
        private static readonly int MIN_HEIGHT = 5;
        private static readonly int MAX_HEIGHT = 30; //perhaps it can be bigger.. depending on the monitor

        private static readonly int DEFAULT_WIDTH = 10;
        private static readonly int DEFAULT_HEIGHT = 10;

        private readonly BoardPosition[][] _playField;
        private readonly List<Ship> _ships = new List<Ship>();

        public BattleField() : this(DEFAULT_WIDTH, DEFAULT_HEIGHT)
        { }

        public BattleField(int width, int height)
        {
            if (width < MIN_WIDTH) width = MIN_WIDTH;
            if (width > MAX_WIDTH) width = MAX_WIDTH;
            if (height < MIN_HEIGHT) height = MIN_HEIGHT;
            if (height > MAX_HEIGHT) height = MAX_HEIGHT;

            _playField = new BoardPosition[height][];
            for (int row = 0; row < height; row++)
            {
                _playField[row] = new BoardPosition[width];
                for (int column = 0; column < width; column++)
                {
                    var position = new BoardPosition(row, column);
                    _playField[row][column] = position;
                    BoardPositions[position.Coordinates] = position;
                }
            }

            _width =
                _playField[0].Length * (PLAYFIELD_HORIZONTAL_SPACING + 1)
                + PLAYFIELD_HORIZONTAL_SPACING
                + PLAYFIELD_LEFT;
            _height =
                _playField.Length * (PLAYFIELD_VERTICAL_SPACING + 1)
                + PLAYFIELD_VERTICAL_SPACING
                + PLAYFIELD_TOP;
        }

        public Dictionary<string, BoardPosition> BoardPositions { get; } = new Dictionary<string, BoardPosition>(StringComparer.OrdinalIgnoreCase);
        public BoardPosition SelectedPosition { get; private set; }

        public override void Redraw()
        {
            WriteRowIndex();
            WriteColumnIndex();
            for (int x = 0; x < _playField[0].Length; x++) //colIndex
            {
                for (int y = 0; y < _playField.Length; y++) //rowIndex
                {
                    SetCursorToRowColumn(y, x);
                    char spot = _playField[y][x].Value;
                    if (char.IsLetter(spot))
                    {
                        Console.Write(spot);
                    }
                    else
                    {
                        Console.Write('~');
                    }
                }
            }
        }

        public bool AddShip(Ship ship)
        {
            if (_ships.Any(s => s.ShipType == ship.ShipType))
            {
                //cannot add another ship of the same type
                return false;
            }

            for (int i = 0; i < ship.Size; i++)
            {
                if (ship.Position == Orientation.Horizontal)
                {
                    _playField[ship.YLocation][ship.XLocation + i].Value = ship.Name[0];
                }
                else
                {
                    _playField[ship.YLocation + i][ship.XLocation].Value = ship.Name[0];
                }
            }
            _ships.Add(ship);

            return true;
        }

        public void SelectPosition(string input)
        {
            if (BoardPositions.ContainsKey(input))
            {
                if (SelectedPosition != null)
                {
                    DeselectPosition();
                }

                SelectedPosition = BoardPositions[input];

                DrawSelectedPosition();
            }
        }

        public void DeselectPosition()
        {
            ClearSelectedPosition();
            SelectedPosition = null;
        }

        private void ClearSelectedPosition()
        {
            SetCursorToRowColumn(SelectedPosition.Row, SelectedPosition.Column);
            Console.CursorLeft--;
            Console.Write(' ');
            Console.CursorLeft++;
            Console.Write(' ');
        }

        private void DrawSelectedPosition()
        {
            SetCursorToRowColumn(SelectedPosition.Row, SelectedPosition.Column);
            Console.CursorLeft--;
            Console.Write('>');
            Console.CursorLeft++;
            Console.Write('<');
        }

        private void SetCursorToRowColumn(int row, int column)
        {
            Console.SetCursorPosition(
                ColumnToCursorPosition(column),
                RowToCursorPosition(row));
        }

        private int RowToCursorPosition(int row)
        {
            return 
                row * (PLAYFIELD_VERTICAL_SPACING + 1) 
                + PLAYFIELD_VERTICAL_SPACING
                + PLAYFIELD_TOP
                + Top;
        }

        private int ColumnToCursorPosition(int column)
        {
            return 
                column * (PLAYFIELD_HORIZONTAL_SPACING + 1)
                + PLAYFIELD_HORIZONTAL_SPACING
                + PLAYFIELD_LEFT
                + Left;
        }

        private void WriteRowIndex()
        {
            int verticalCoordinates = 1;

            Console.SetCursorPosition(Left, PLAYFIELD_TOP);

            for (int row = 0; row < _playField.Length; row++)
            {
                Console.CursorTop = RowToCursorPosition(row);
                Console.CursorLeft = Left;
                Console.Write(verticalCoordinates++);
            }
        }

        private void WriteColumnIndex()
        {
            char horizontalCoordinates = 'A';
            Console.SetCursorPosition(PLAYFIELD_LEFT, Top);

            for (int column = 0; column < _playField[0].Length; column++)
            {
                Console.CursorLeft = ColumnToCursorPosition(column);
                Console.Write(horizontalCoordinates++);
            }
        }
    }
}
