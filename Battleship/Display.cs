using Battleship.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Display
    {
        private int _totalWidth = 30;
        private int _totalHeight = 20;
        private List<DisplayElement> _elements = new List<DisplayElement>();

        
        public bool RemoveElement(DisplayElement element)
        {
            if (_elements.Remove(element))
            {
                element.DisplayElementChanged -= DisplayElementChanged;
                RefreshDisplay();
                return true;
            }
            return false;
        }

        public void AddElement(DisplayElement element)
        {
            _elements.Add(element);
            RefreshDisplay();
            element.DisplayElementChanged += DisplayElementChanged;
        }

        public DisplayElement HitTest(short x, short y)
        {
            return _elements.FirstOrDefault(element =>
            x >= element.Left && x < element.Left + element.Width
            && y >= element.Top && y < element.Top + element.Height);
        }

        private void DisplayElementChanged()
        {
            RefreshDisplay();
        }

        public void RefreshDisplay()
        {
            Console.Clear();
            if (!_elements.Any()) return;

            _totalWidth = _elements.Max(element => element.Left + element.Width);
            _totalHeight = _elements.Max(element => element.Top + element.Height);

            ResetView();

            foreach (var element in _elements)
            {
                element.Redraw();
            }
        }

        public bool CheckSize()
        {
            return Console.WindowWidth == _totalWidth && Console.WindowHeight == _totalHeight
                && Console.BufferWidth == _totalWidth && Console.BufferHeight == _totalHeight;
        }
        
        private void ResetView()
        {
            Console.CursorVisible = false;

            if (!CheckSize())
            {
                Console.SetWindowSize(_totalWidth, _totalHeight);
                Console.SetBufferSize(_totalWidth, _totalHeight);

                //set size again after setting buffer to clear the scrollbar area
                Console.SetWindowSize(_totalWidth, _totalHeight);
            }
        }
    }
}
