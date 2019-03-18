using Battleship.Enums;
using Battleship.GameModels;
using Battleship.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Battleship.DisplayElements
{
    public class ShipTypeSelector : DisplayElement, IDisposable
    {
        private readonly ShipType[] _shipTypes = Enum.GetValues(typeof(ShipType)).OfType<ShipType>().ToArray();
        private ShipType? _selectedShipType = null;
        private readonly ObservableCollection<Ship> _shipsOnBattlefield;

        public ShipTypeSelector(int width, int height, ObservableCollection<Ship> shipsOnBattlefield) : base(width, height)
        {
            _shipsOnBattlefield = shipsOnBattlefield;
            _shipsOnBattlefield.CollectionChanged += ShipsOnBattlefield_CollectionChanged;
            ShipsOnBattlefield_CollectionChanged(null, null);

            FillBuffer();
        }

        private void ShipsOnBattlefield_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectedShipType = null;

            if (e != null)
            {
                foreach (var ship in e.NewItems.OfType<Ship>())
                {
                    ship.Damaged += Ship_Damaged;
                }

                foreach (var ship in e.OldItems.OfType<Ship>())
                {
                    ship.Damaged -= Ship_Damaged;
                }
            }

            foreach (var shipType in _shipTypes.Except(_shipsOnBattlefield.Select(ship => ship.ShipType)))
                for (int column = 0; column < Width; column++)
                {
                    Buffer[(int)shipType][column].Attributes &= ~CharAttributes.FOREGROUND_WHITE;
                    Buffer[(int)shipType][column].Attributes |= CharAttributes.FOREGROUND_GREEN;
                }

            foreach (var ship in _shipsOnBattlefield)
            {
                Ship_Damaged(ship);
            }

            Redraw();
        }

        public ShipType? SelectedShipType
        {
            get => _selectedShipType;
            private set
            {
                if (_selectedShipType != value)
                {
                    UnHighlightSelectedType();
                    if (_shipsOnBattlefield.Any(ship => ship.ShipType == value))
                    {
                        _selectedShipType = null;
                    }
                    else
                    {
                        _selectedShipType = value;
                    }
                    HighlightSelectedType();
                }
            }
        }

        public void SelectShipType(short x, short y)
        {
            var row = y - Top;
            if (row < _shipTypes.Length)
            {
                SelectedShipType = _shipTypes[row];
            }
        }

        public void SelectShipType(string input)
        {
            var types = _shipTypes.Where(shipType => shipType.ToString().StartsWith(input, StringComparison.OrdinalIgnoreCase)).ToList();

            if (types.Any())
            {
                SelectedShipType = types.First();
            }
        }

        public void DeselectShipType()
        {
            SelectedShipType = null;
        }

        private void FillBuffer()
        {
            for (int row = 0; row < Height; row++)
            {
                string shipString = "";
                if (row < _shipTypes.Length)
                {
                    var ship = new Ship(_shipTypes[row], 0, 0, Orientation.Horizontal);
                    shipString = ship.ToString();
                }
                for (int column = 0; column < Width; column++)
                {
                    if (column < shipString.Length)
                    {
                        Buffer[row][column].Character = column < shipString.Length ? shipString[column] : ' ';
                    }
                    Buffer[row][column].Attributes &= ~CharAttributes.FOREGROUND_INTENSITY;
                }
            }
        }

        private void Ship_Damaged(Ship ship)
        {
            int row = (int)ship.ShipType;

            int startColoredColumns = ship.ToString().IndexOf(ship.Name);
            int coloredColumns = ship.Name.Length;
            int whiteColumns = (ship.Health * coloredColumns) / ship.Size;

            for (int column = startColoredColumns; column < coloredColumns + startColoredColumns; column++)
            {
                if (column < whiteColumns + startColoredColumns)
                {
                    //white
                    Buffer[row][column].Attributes |= CharAttributes.FOREGROUND_WHITE;
                }
                else
                {
                    //red
                    Buffer[row][column].Attributes &= ~CharAttributes.FOREGROUND_WHITE;
                    Buffer[row][column].Attributes |= CharAttributes.FOREGROUND_RED;
                }
            }

            Redraw();
        }

        private void UnHighlightSelectedType()
        {
            if (SelectedShipType == null)
                return;

            for (int column = 0; column < Width; column++)
                Buffer[(int)SelectedShipType][column].Attributes &= ~CharAttributes.FOREGROUND_INTENSITY;

            Redraw();
        }

        private void HighlightSelectedType()
        {
            if (SelectedShipType == null)
                return;

            for (int column = 0; column < Width; column++)
                Buffer[(int)SelectedShipType][column].Attributes |= CharAttributes.FOREGROUND_INTENSITY;

            Redraw();
        }

        public void Dispose()
        {
            _shipsOnBattlefield.CollectionChanged -= ShipsOnBattlefield_CollectionChanged;
        }
    }
}