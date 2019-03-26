using System;

namespace Battleship.Utilities
{
    public delegate void DisplayElementChangedEvent();

    public abstract class DisplayElement
    {
        public event DisplayElementChangedEvent DisplayElementChanged;

        private int _top;
        private int _left;

        public DisplayElement(int width, int height)
        {
            Width = width;
            Height = height;

            Buffer = ExtensionMethods.CreateMultiDimensionalArray(
                width, 
                height, 
                (row, column) => new CHAR_INFO() { Attributes = CharAttributes.FOREGROUND_GREEN | CharAttributes.FOREGROUND_INTENSITY });
        }

        protected CHAR_INFO[][] Buffer { get; private set; }

        public void Redraw()
        {
            LowLevelConsoleFunctions.WriteConsoleOutput(Buffer, _top, _left);
        }

        public int Width { get; }

        public int Height { get; }

        public virtual int Top
        {
            get => _top;
            set
            {
                if (value < 0 || value + Height > Console.LargestWindowHeight) return;
                _top = value;
                OnDisplayElementChanged();
            }
        }

        public virtual int Left
        {
            get => _left;
            set
            {
                if (value < 0 || value + Width > Console.LargestWindowWidth) return;
                _left = value;
                OnDisplayElementChanged();
            }
        }

        protected void OnDisplayElementChanged()
        {
            DisplayElementChanged?.Invoke();
        }
    }
}