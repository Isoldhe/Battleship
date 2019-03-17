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
        private readonly BattleField _battleField;
        private readonly Display _display;
        private readonly StatusBar _statusBar;
        private readonly StatusBar _inputStatus;
        private readonly StringBuilder _input = new StringBuilder();
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

        public static bool WindowInvalidated { get; private set; }

        public void Run()
        {
            var previousMouseButtonState = MouseButtonState.ALL_RELEASED;

            foreach (var consoleInput in LowLevelConsoleFunctions.ReadConsoleInput())
            {
                switch (consoleInput.EventType)
                {
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
                        if (consoleInput.MouseEvent.dwEventFlags.HasFlag(MouseEventFlags.MOUSE_MOVED)
                            && consoleInput.MouseEvent.dwButtonState == MouseButtonState.FROM_LEFT_1ST_BUTTON_PRESSED)
                        {
                            var elementHit = _display.HitTest(consoleInput.MouseEvent.dwMousePosition.X, consoleInput.MouseEvent.dwMousePosition.Y);
                            if (elementHit == _battleField)
                            {
                                _battleField.SelectPositionNear(consoleInput.MouseEvent.dwMousePosition.X, consoleInput.MouseEvent.dwMousePosition.Y);
                            }
                        }

                        if (consoleInput.MouseEvent.dwEventFlags.HasFlag(MouseEventFlags.MOUSE_BUTTON_STATE_CHANGED))
                        {
                            if (consoleInput.MouseEvent.dwButtonState == MouseButtonState.ALL_RELEASED 
                                && previousMouseButtonState == MouseButtonState.FROM_LEFT_1ST_BUTTON_PRESSED)
                            {
                                //left button clicked
                                var elementHit = _display.HitTest(consoleInput.MouseEvent.dwMousePosition.X, consoleInput.MouseEvent.dwMousePosition.Y);
                                if (elementHit == _battleField)
                                {
                                    _battleField.SelectPositionNear(consoleInput.MouseEvent.dwMousePosition.X, consoleInput.MouseEvent.dwMousePosition.Y);
                                    if (_battleField.SelectedPosition != null)
                                    {
                                        ConfirmSelectedPosition();
                                    }
                                }
                            }
                            previousMouseButtonState = consoleInput.MouseEvent.dwButtonState;
                        }
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
                case ConsoleKey.LeftArrow:
                    if (_input.Length > 0)
                    {
                        CancelInput();
                    }

                    if (_battleField.SelectedPosition == null)
                    {
                        _battleField.SelectPosition(0, 0);
                    }
                    else
                    {
                        _battleField.SelectPosition(
                            _battleField.SelectedPosition.Row, 
                            _battleField.SelectedPosition.Column - 1);
                    }

                    break;
                case ConsoleKey.UpArrow:
                    if (_input.Length > 0)
                    {
                        CancelInput();
                    }

                    if (_battleField.SelectedPosition == null)
                    {
                        _battleField.SelectPosition(0, 0);
                    }
                    else
                    {
                        _battleField.SelectPosition(
                            _battleField.SelectedPosition.Row - 1,
                            _battleField.SelectedPosition.Column);
                    }

                    break;
                case ConsoleKey.RightArrow:
                    if (_input.Length > 0)
                    {
                        CancelInput();
                    }

                    if (_battleField.SelectedPosition == null)
                    {
                        _battleField.SelectPosition(0, 0);
                    }
                    else
                    {
                        _battleField.SelectPosition(
                            _battleField.SelectedPosition.Row,
                            _battleField.SelectedPosition.Column + 1);
                    }

                    break;
                case ConsoleKey.DownArrow:
                    if (_input.Length > 0)
                    {
                        CancelInput();
                    }

                    if (_battleField.SelectedPosition == null)
                    {
                        _battleField.SelectPosition(0, 0);
                    }
                    else
                    {
                        _battleField.SelectPosition(
                            _battleField.SelectedPosition.Row + 1,
                            _battleField.SelectedPosition.Column);
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
                    if (WindowInvalidated)
                    {
                        RevalidateWindow();
                        break;
                    }

                    if (_input.Length > 0)
                    {
                        _battleField.SelectPosition(_input.ToString());
                        CancelInput();
                    }

                    if (_battleField.SelectedPosition != null)
                    {
                        ConfirmSelectedPosition();
                        break;
                    }
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

                    _statusBar.SaveStatus("quit");
                    if (PromptUser())
                    {
                        //quit
                        _quit = true;
                        break;
                    }

                    //don't quit
                    _statusBar.LoadStatus("quit");
                    break;
            }
        }

        private void ConfirmSelectedPosition()
        {
            //TODO: do something with confirmed position
            //_battleField.DeselectPosition();
            _inputStatus.Status = "Position confirmed.";
        }

        private void InvalidateWindow()
        {
            if (!WindowInvalidated)
            {
                WindowInvalidated = true;
                Console.SetCursorPosition(0, 0);
                Console.Clear();
                Console.Write("Window size invalid. Press enter to auto-resize window.");
            }
        }

        private void RevalidateWindow()
        {
            _display.RefreshDisplay();
            WindowInvalidated = false;
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
                if (!_statusBar.StatusExists("input"))
                {
                    _statusBar.SaveStatus("input");
                }
                _statusBar.Status = $"Coordinate: {_input}";
            }
            else
            {
                if (_statusBar.StatusExists("input"))
                {
                    _statusBar.LoadStatus("input");
                    _statusBar.DeleteStatus("input");
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
