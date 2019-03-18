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

        public BattleField(int width, int height) : base(
            width * (PLAYFIELD_HORIZONTAL_SPACING + 1)
                + PLAYFIELD_HORIZONTAL_SPACING
                + PLAYFIELD_LEFT,
            height * (PLAYFIELD_VERTICAL_SPACING + 1)
                + PLAYFIELD_VERTICAL_SPACING
                + PLAYFIELD_TOP)
        {
            if (width < MIN_WIDTH) width = MIN_WIDTH;
            if (width > MAX_WIDTH) width = MAX_WIDTH;
            if (height < MIN_HEIGHT) height = MIN_HEIGHT;
            if (height > MAX_HEIGHT) height = MAX_HEIGHT;

            _playField = ExtensionMethods.CreateMultiDimensionalArray(
                width,
                height,
                (row, column) =>
                {
                    var position = new BoardPosition(row, column);
                    BoardPositions[position.Coordinates] = position;
                    return position;
                });

            FillBuffer();
        }

        protected void FillBuffer()
        {
            WriteRowIndex();
            WriteColumnIndex();
            for (int x = 0; x < _playField[0].Length; x++) //colIndex
            {
                for (int y = 0; y < _playField.Length; y++) //rowIndex
                {
                    char spot = _playField[y][x].Value;
                    var row = RowToBufferPosition(y);
                    var column = ColumnToBufferPosition(x);
                    Buffer[row][column].Character = char.IsLetter(spot) ? spot : ' ';
                }
            }

            for (int row = PLAYFIELD_TOP; row < Height; row++)
            {
                for (int column = PLAYFIELD_LEFT; column < Width; column++)
                {
                    Buffer[row][column].Attributes = CharAttributes.BACKGROUND_BLUE | CharAttributes.FOREGROUND_WHITE | CharAttributes.FOREGROUND_INTENSITY;
                }
            }
        }

        public Dictionary<string, BoardPosition> BoardPositions { get; } = new Dictionary<string, BoardPosition>(StringComparer.OrdinalIgnoreCase);
        public BoardPosition SelectedPosition { get; private set; }

        public bool AddShip(Ship ship)
        {
            if (_ships.Any(s => s.ShipType == ship.ShipType))
            {
                //cannot add another ship of the same type
                return false;
            }

            for (int i = 0; i < ship.Size; i++)
            {
                if (ship.Orientation == Orientation.Horizontal)
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

        public void SelectPosition(int row, int column)
        {
            if (row >= 0 && row < _playField.Length &&
                column >= 0 && column < _playField[0].Length)
            {
                if (SelectedPosition != null)
                {
                    DeselectPosition();
                }

                SelectedPosition = _playField[row][column];

                DrawSelectedPosition();
            }
        }

        public void SelectPositionNear(short x, short y)
        {
            var column = CursorPositionToColumn(x);
            var row = CursorPositionToRow(y);
            SelectPosition(row, column);
        }

        public void DeselectPosition()
        {
            ClearSelectedPosition();
            SelectedPosition = null;
        }

        private void ClearSelectedPosition()
        {
            var row = RowToBufferPosition(SelectedPosition.Row);
            var column = ColumnToBufferPosition(SelectedPosition.Column);
            Buffer[row][column - 1].Character = ' ';
            Buffer[row][column + 1].Character = ' ';
            Redraw();
        }

        private void DrawSelectedPosition()
        {
            var row = RowToBufferPosition(SelectedPosition.Row);
            var column = ColumnToBufferPosition(SelectedPosition.Column);
            Buffer[row][column - 1].Character = '>';
            Buffer[row][column + 1].Character = '<';
            Redraw();
        }

        private int RowToBufferPosition(int row)
        {
            return 
                row * (PLAYFIELD_VERTICAL_SPACING + 1) 
                + PLAYFIELD_VERTICAL_SPACING
                + PLAYFIELD_TOP;
        }

        private int ColumnToBufferPosition(int column)
        {
            return 
                column * (PLAYFIELD_HORIZONTAL_SPACING + 1)
                + PLAYFIELD_HORIZONTAL_SPACING
                + PLAYFIELD_LEFT;
        }

        private int CursorPositionToRow(int y)
        {
            return
                (y
                - PLAYFIELD_VERTICAL_SPACING
                - PLAYFIELD_TOP
                - Top) / (PLAYFIELD_VERTICAL_SPACING + 1);
        }

        private int CursorPositionToColumn(int x)
        {
            return
                (x + 1
                - PLAYFIELD_HORIZONTAL_SPACING
                - PLAYFIELD_LEFT
                - Left) / (PLAYFIELD_HORIZONTAL_SPACING + 1);
        }

        private void WriteRowIndex()
        {
            int verticalCoordinates = 1;

            for (int row = 0; row < _playField.Length; row++)
            {
                var coords = verticalCoordinates++.ToString().PadLeft(2);
                for (int column = 0; column < coords.Length; column++)
                {
                    Buffer[RowToBufferPosition(row)][column].Character = coords[column];
                }
            }
        }

        private void WriteColumnIndex()
        {
            char horizontalCoordinates = 'A';

            for (int column = 0; column < _playField[0].Length; column++)
            {
                Buffer[0][ColumnToBufferPosition(column)].Character = horizontalCoordinates++;
            }
        }
    }
}
