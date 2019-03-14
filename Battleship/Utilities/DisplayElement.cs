using System;

namespace Battleship.Utilities
{
    public delegate void DisplayElementChangedEvent();

    public abstract class DisplayElement
    {
        public event DisplayElementChangedEvent DisplayElementChanged;

        protected int _top;
        protected int _left;
        protected int _width;
        protected int _height;

        protected void Clear()
        {
            string whitespace = string.Empty.PadRight(Width);
            bool preventFinalCharacterWrite = Left + Width == Console.WindowWidth;
            for (int i = 0; i < Height; i++)
            {
                Console.CursorLeft = Left;
                Console.CursorTop = Top + i;

                if (preventFinalCharacterWrite && Top + i + 1 == Console.WindowHeight)
                    whitespace = whitespace.Remove(0, 1);

                Console.Write(whitespace);
            }
        }

        public abstract void Redraw();

        public virtual int Top
        {
            get => _top;
            set
            {
                if (value < 0 || value + Height > Console.LargestWindowHeight) return;
                _top = value;
                TriggerDisplayElementChangedEvent();
            }
        }

        public virtual int Left
        {
            get => _left;
            set
            {
                if (value < 0 || value + Width > Console.LargestWindowWidth) return;
                _left = value;
                TriggerDisplayElementChangedEvent();
            }
        }

        public virtual int Width
        {
            get => _width;
            protected set
            {
                _width = value;
                TriggerDisplayElementChangedEvent();
            }
        }

        public virtual int Height
        {
            get => _height;
            protected set
            {
                _height = value;
                TriggerDisplayElementChangedEvent();
            }
        }

        protected void TriggerDisplayElementChangedEvent()
        {
            DisplayElementChanged?.Invoke();
        }
    }
}