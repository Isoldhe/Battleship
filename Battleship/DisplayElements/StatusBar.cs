﻿using Battleship.Utilities;
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

        public StatusBar(int height, int width)
        {
            _height = height;
            _width = width;
        }

        public string Status
        {
            get => _status;
            set
            {
                Clear();
                _status = value;
                Redraw();
            }
        }

        public override void Redraw()
        {
            if (string.IsNullOrWhiteSpace(Status)) return;

            var lines = Status.WordWrap(Width, Height);

            Console.SetCursorPosition(Left, Top);
            for (int i = 0; i < lines.Count; i++)
            {
                Console.CursorLeft = Left;
                Console.CursorTop = Top + i;
                Console.Write(lines[i]);
            }
        }
    }
}
