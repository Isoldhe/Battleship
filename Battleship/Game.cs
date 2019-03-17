using Battleship.DisplayElements;
using Battleship.Enums;
using Battleship.GameModels;
using Battleship.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Battleship
{
    public class Game
    {
        private BattleField _battleField;
        private Display _display;
        private StatusBar _statusBar;
        private StringBuilder _input = new StringBuilder();
        private string _statusBeforeInput;
        private bool _windowInvalidated;
        private bool _quit;

        public Game()
        {
            _display = new Display();

            //TODO initialize battlefield according to player preferences
            _battleField = new BattleField()
            {
                Left = 1,
                Top = 1,
            };

            LoadTestData();

            _statusBar = new StatusBar(3, 50)
            {
                Left = 1,
                Top = _battleField.Top + _battleField.Height + 1,
                Status = "Welcome to Battleship. Start typing to enter a coordinate."
            };

            _display.AddElement(_battleField);
            _display.AddElement(_statusBar);
        }

        private void LoadTestData()
        {
            _battleField.AddShip(new Ship(ShipType.Destroyer, 0, 0, Orientation.Horizontal));
            _battleField.AddShip(new Ship(ShipType.AircraftCarrier, 9, 2, Orientation.Vertical));
            _battleField.AddShip(new Ship(ShipType.Submarine, 5, 3, Orientation.Horizontal));
            _battleField.AddShip(new Ship(ShipType.Cruiser, 3, 1, Orientation.Vertical));
            _battleField.AddShip(new Ship(ShipType.Battleship, 6, 7, Orientation.Vertical));
        }

        public void Run()
        {
            foreach (var consoleInput in LowLevelConsoleFunctions.ReadConsoleInput())
            {
                switch (consoleInput.EventType)
                {
                    case InputEventType.FOCUS_EVENT:
                        bool gotFocus = consoleInput.FocusEvent.bSetFocus != 0;
                        break;
                    case InputEventType.KEY_EVENT:
                        if (consoleInput.KeyEvent.bKeyDown)
                        {
                            for (int i = 0; i < consoleInput.KeyEvent.wRepeatCount; i++)
                            {
                                HandleKeyEvent(consoleInput.KeyEvent.ToConsoleKeyInfo());
                            }
                        }

                        break;
                    case InputEventType.MOUSE_EVENT:

                        //consoleInput.MouseEvent;
                        break;
                    case InputEventType.WINDOW_BUFFER_SIZE_EVENT:
                        if (_display.CheckSize())
                        {
                            //window size is correct
                            RevalidateWindow();
                        }
                        else
                        {
                            //window was resized
                            InvalidateWindow();
                        }
                        break;
                    default:
                        break;
                }

                if (_quit)
                {
                    break;
                }
            }
        }

        private void HandleKeyEvent(ConsoleKeyInfo keyStroke)
        {
            switch (keyStroke.Key)
            {
                //TODO implement cursor
                case ConsoleKey.LeftArrow:
                    if (_input.Length > 0)
                    {
                        CancelInput();
                    }
                    break;
                case ConsoleKey.UpArrow:
                    if (_input.Length > 0)
                    {
                        CancelInput();
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (_input.Length > 0)
                    {
                        CancelInput();
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (_input.Length > 0)
                    {
                        CancelInput();
                    }
                    break;

                case ConsoleKey.Backspace:
                    if (_input.Length > 0)
                    {
                        _input.Remove(_input.Length - 1, 1);
                        InputChanged();
                    }
                    break;
                default:
                    if (char.IsLetterOrDigit(keyStroke.KeyChar))
                    {
                        if (_input.Length < 20)
                        {
                            _input.Append(keyStroke.KeyChar);
                            InputChanged();
                        }
                    }
                    else
                    {
                        //do nothing
                    }
                    break;
                case ConsoleKey.Enter:
                case ConsoleKey.Spacebar:
                    if (_windowInvalidated)
                    {
                        RevalidateWindow();
                        break;
                    }

                    _battleField.SelectPosition(_input.ToString());
                    CancelInput();
                    break;


                case ConsoleKey.Escape:
                    if (_input.Length > 0)
                    {
                        CancelInput();
                        break;
                    }

                    if (_battleField.SelectedPosition != null)
                    {
                        _battleField.DeselectPosition();
                        break;
                    }

                    string previousStatus = _statusBar.Status;
                    if (PromptUser())
                    {
                        //quit
                        _quit = true;
                        break;
                    }
                    else
                    {
                        //don't quit
                        _statusBar.Status = previousStatus;
                        break;
                    }
            }
        }

        private void InvalidateWindow()
        {
            if (!_windowInvalidated)
            {
                _windowInvalidated = true;
                Console.SetCursorPosition(0, 0);
                Console.Clear();
                Console.Write("Window size invalid. Press enter to auto-resize window.");
            }
        }

        private void RevalidateWindow()
        {
            _display.RefreshDisplay();
            _windowInvalidated = false;
        }

        private void CancelInput()
        {
            _input.Clear();
            InputChanged();
        }

        private void InputChanged()
        {
            if (_input.Length > 0)
            {
                if (_statusBeforeInput == null)
                {
                    _statusBeforeInput = _statusBar.Status;
                }
                _statusBar.Status = $"Coordinate: {_input}";
            }
            else
            {
                if (_statusBeforeInput != null)
                {
                    _statusBar.Status = _statusBeforeInput;
                    _statusBeforeInput = null;
                }
            }
        }

        private bool PromptUser()
        {
            _statusBar.Status = "Are you sure you want to quit? (Y/N)   ";

            while (true)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Y:
                    case ConsoleKey.Enter:
                    case ConsoleKey.Spacebar:
                        return true;
                    case ConsoleKey.N:
                    case ConsoleKey.Escape:
                        return false;
                    default:
                        _statusBar.Status = "Invalid input. Are you sure you want to quit? (Y/N)   ";
                        break;
                }
            }
        }
    }
}
