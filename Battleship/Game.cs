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
        private bool _paused;

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
            //TODO use following snippet for input to include mouse and window resizing events
            //foreach (var consoleInput in LowLevelConsoleFunctions.ReadConsoleInput())
            //{
            //    switch (consoleInput.EventType)
            //    {
            //        case InputEventType.FOCUS_EVENT:
            //            bool gotFocus = consoleInput.FocusEvent.bSetFocus != 0;
            //            break;
            //        case InputEventType.KEY_EVENT:
            //            var keyStroke = consoleInput.KeyEvent.ToConsoleKeyInfo();

            //            break;
            //        case InputEventType.MOUSE_EVENT:

            //            //consoleInput.MouseEvent;
            //            break;
            //        case InputEventType.WINDOW_BUFFER_SIZE_EVENT:
            //            //window was resized
            //            PauseGame();
            //            break;
            //        default:
            //            break;
            //    }
            //}

            var keyStroke = new ConsoleKeyInfo();

            bool keepPlaying = true;
            while (keepPlaying)
            {
                keyStroke = Console.ReadKey(true);
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
                        _input.Remove(_input.Length - 1, 1);
                        InputChanged();
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

                        //quit
                        keepPlaying = PromptUser();
                        // TODO: if user types Y to keep playing, show previous status bar
                        _statusBar.Status = "";
                        break;
                }
            }
        }

        private void PauseGame()
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Game paused. Press enter to continue.");
            _paused = true;
        }

        private void ResumeGame()
        {
            _display.RefreshDisplay();
            _paused = false;
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
                _statusBar.Status = _statusBeforeInput;
                _statusBeforeInput = null;
            }
        }

        private bool PromptUser()
        {
            _statusBar.Status = "Are you sure you want to quit? (Y/N)   ";

            while (true)
            {
                var answer = Console.ReadLine();

                if (answer.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                else if (answer.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else
                {
                    _statusBar.Status = "Invalid input. Are you sure you want to quit? (Y/N)   ";
                }
            }
        }
    }
}
