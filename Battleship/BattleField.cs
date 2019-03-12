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

        private readonly char[][] _playField;
        private readonly List<Ship> _ships = new List<Ship>();

        private int _battleFieldLeft;
        private int _battleFieldTop;

        public BattleField() : this(DEFAULT_WIDTH, DEFAULT_HEIGHT)
        { }

        public BattleField(int width, int height)
        {
            if (width < MIN_WIDTH) width = MIN_WIDTH;
            if (width > MAX_WIDTH) width = MAX_WIDTH;
            if (height < MIN_HEIGHT) height = MIN_HEIGHT;
            if (height > MAX_HEIGHT) height = MAX_HEIGHT;

            _playField = new char[height][];
            for (int row = 0; row < height; row++)
            {
                _playField[row] = new char[width];
            }

            BattleFieldWidthInChars =
                _playField[0].Length * (PLAYFIELD_HORIZONTAL_SPACING + 1)
                + PLAYFIELD_HORIZONTAL_SPACING
                + PLAYFIELD_LEFT;
            BattleFieldHeightInChars =
                _playField.Length * (PLAYFIELD_VERTICAL_SPACING + 1)
                + PLAYFIELD_VERTICAL_SPACING
                + PLAYFIELD_TOP;

            RefreshField();
        }

        public int BattleFieldWidthInChars { get; }

        public int BattleFieldHeightInChars { get; }

        public int BattleFieldLeft
        {
            get => _battleFieldLeft;
            set
            {
                ClearField();
                _battleFieldLeft = value;
                RefreshField();
            }
        }

        public int BattleFieldTop
        {
            get => _battleFieldTop;
            set
            {
                ClearField();
                _battleFieldTop = value;
                RefreshField();
            }
        }

        private void ClearField()
        {
            Console.CursorLeft = BattleFieldLeft;
            Console.CursorTop = BattleFieldTop;
            string whiteSpace = string.Empty.PadRight(BattleFieldWidthInChars - 1);
            for (int i = 0; ; i++)
            {
                Console.Write(whiteSpace);

                if (i >= BattleFieldHeightInChars - 1)
                {
                    break;
                }

                Console.CursorLeft = BattleFieldLeft;
                Console.CursorTop++;
            }
        }

        public void RefreshField()
        {
            WriteRowIndex();
            WriteColumnIndex();
            for (int x = 0; x < _playField[0].Length; x++) //colIndex
            {
                for (int y = 0; y < _playField.Length; y++) //rowIndex
                {
                    SetCursorToRowColumn(y, x);
                    char spot = _playField[y][x];
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

        public void LoadTestData()
        {
            AddShip(new Ship(ShipType.Destroyer, 0, 0, Orientation.Horizontal));
            AddShip(new Ship(ShipType.AircraftCarrier, 9, 2, Orientation.Vertical));
            AddShip(new Ship(ShipType.Submarine, 5, 3, Orientation.Horizontal));
            AddShip(new Ship(ShipType.Cruiser, 3, 1, Orientation.Vertical));
            AddShip(new Ship(ShipType.Battleship, 6, 7, Orientation.Vertical));
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
                    _playField[ship.YLocation][ship.XLocation + i] = ship.Name[0];
                }
                else
                {
                    _playField[ship.YLocation + i][ship.XLocation] = ship.Name[0];
                }
            }
            _ships.Add(ship);

            return true;
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
                + _battleFieldTop;
        }

        private int ColumnToCursorPosition(int column)
        {
            return 
                column * (PLAYFIELD_HORIZONTAL_SPACING + 1)
                + PLAYFIELD_HORIZONTAL_SPACING
                + PLAYFIELD_LEFT
                + _battleFieldLeft;
        }

        private void WriteRowIndex()
        {
            int verticalCoordinates = 1;

            Console.SetCursorPosition(BattleFieldLeft, PLAYFIELD_TOP);

            for (int row = 0; row < _playField.Length; row++)
            {
                Console.CursorTop = RowToCursorPosition(row);
                Console.CursorLeft = BattleFieldLeft;
                Console.Write(verticalCoordinates++);
            }
        }

        private void WriteColumnIndex()
        {
            char horizontalCoordinates = 'A';
            Console.SetCursorPosition(PLAYFIELD_LEFT, BattleFieldTop);

            for (int column = 0; column < _playField[0].Length; column++)
            {
                Console.CursorLeft = ColumnToCursorPosition(column);
                Console.Write(horizontalCoordinates++);
            }
        }
    }
}
