using Battleship.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship.DisplayElements
{
    public class StatusBar : DisplayElement
    {
        private string _status;
        private Dictionary<string, string> _savedStatusses = new Dictionary<string, string>();

        public StatusBar(int width, int height) : base(width, height)
        { }

        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    FillBuffer();
                }
            }
        }

        private void FillBuffer()
        {
            var lines = Status?.WordWrap(Width, Height) ?? new List<string>();

            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    if (row < lines.Count && column < lines[row].Length)
                    {
                        Buffer[row][column].Character = lines[row][column];
                    }
                    else
                    {
                        Buffer[row][column].Character = ' ';
                    }
                }
            }

            Redraw();
        }

        public void SaveStatus(string key)
        {
            _savedStatusses[key] = Status;
        }

        public void LoadStatus(string key)
        {
            if (_savedStatusses.ContainsKey(key))
            {
                Status = _savedStatusses[key];
            }
        }

        public void DeleteStatus(string key)
        {
            _savedStatusses.Remove(key);
        }

        public bool StatusExists(string key)
        {
            return _savedStatusses.ContainsKey(key);
        }
    }
}
