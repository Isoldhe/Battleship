using Battleship.Enums;
using Battleship.GameModels;
using Battleship.Utilities;
using System;
using System.Linq;
using System.Text;

namespace Battleship.DisplayElements
{
    public class ShipTypeSelector : DisplayElement
    {
        private readonly ShipType[] _shipTypes = Enum.GetValues(typeof(ShipType)).OfType<ShipType>().ToArray();
        private ShipType? _selectedShipType = null;

        public ShipTypeSelector(int width, int height) : base(width, height)
        {
            //TODO color change when ship type has been placed
            //TODO? use same ships list as BattleField as backing store for which ships
            // have been placed and eventually later ship damage indication?

            FillBuffer();
        }

        public ShipType? SelectedShipType
        {
            get => _selectedShipType;
            private set
            {
                UnHighlightSelectedType();
                _selectedShipType = value;
                HighlightSelectedType();
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

        private void FillBuffer()
        {
            for (int row = 0; row < Height; row++)
            {
                string shipString = "";
                if (row < _shipTypes.Length)
                {
                    var ship = new Ship(_shipTypes[row], 0, 0, Orientation.Horizontal);
                    shipString = $"[{ship.Name[0]}] {_shipTypes[row]} Size: {ship.Size}";
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

        private void UnHighlightSelectedType()
        {
            if (SelectedShipType == null)
                return;

            for (int column = 0; column < Width; column++)
                Buffer[(int)SelectedShipType][column].Attributes &= ~CharAttributes.FOREGROUND_INTENSITY;
        }

        private void HighlightSelectedType()
        {
            if (SelectedShipType == null)
                return;

            for (int column = 0; column < Width; column++)
                Buffer[(int)SelectedShipType][column].Attributes |= CharAttributes.FOREGROUND_INTENSITY;
        }
    }
}