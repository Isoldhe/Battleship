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
        public ShipTypeSelector(int width, int height) : base(width, height)
        {
            //TODO logic for selecting a ship type to place
            //TODO hittesting for mouse input
            //TODO color change on ship type selection and when it has been placed
            //TODO? use same ships list as BattleField as backing store for which ships
            // have been placed and eventually later ship damage indication?

            FillBuffer();
        }

        private void FillBuffer()
        {
            var shipTypes = Enum.GetValues(typeof(ShipType)).OfType<ShipType>().ToArray();
            for (int row = 0; row < Height; row++)
            {
                string shipString = "";
                if (row < shipTypes.Length)
                {
                    var ship = new Ship(shipTypes[row], 0, 0, Orientation.Horizontal);
                    shipString = $"[{ship.Name[0]}] {shipTypes[row]} Size: {ship.Size}";
                }
                for (int column = 0; column < Width; column++)
                {
                    if (column < shipString.Length)
                    {
                        Buffer[row][column].Character = column < shipString.Length ? shipString[column] : ' ';
                    }
                }
            }
        }
    }
}