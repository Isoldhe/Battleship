using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Battleship.Utilities
{
    public static class LowLevelConsoleFunctions
    {
        #region Handles

        /// <summary>
        /// Provides access to the console window handle.
        /// </summary>
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// Provides access to the console input, output and error handles.
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(
            IOHandles nStdHandle);

        #endregion

        #region ConsoleModes

        [DllImport("kernel32.dll")]
        static extern bool GetConsoleMode(
            IntPtr hConsoleHandle,
            out uint lpMode);

        public static ConsoleModeInput GetInputMode()
        {
            GetConsoleMode(GetStdHandle(IOHandles.STD_INPUT_HANDLE), out uint mode);
            return (ConsoleModeInput)mode;
        }

        public static ConsoleModeOutput GetOutputMode()
        {
            GetConsoleMode(GetStdHandle(IOHandles.STD_OUTPUT_HANDLE), out uint mode);
            return (ConsoleModeOutput)mode;
        }

        [DllImport("kernel32.dll")]
        static extern bool SetConsoleMode(
            IntPtr hConsoleHandle,
            uint dwMode);

        public static void SetInputMode(ConsoleModeInput mode)
        {
            SetConsoleMode(GetStdHandle(IOHandles.STD_INPUT_HANDLE), (uint)mode);
        }

        public static void SetOutputMode(ConsoleModeOutput mode)
        {
            SetConsoleMode(GetStdHandle(IOHandles.STD_OUTPUT_HANDLE), (uint)mode);
        }

        #endregion

        #region ConsoleIO

        /// <summary>
        /// If the number of records requested in the nLength parameter exceeds the number of records available in the buffer,
        /// the number available is read. The function does not return until at least one input record has been read.
        /// </summary>
        [DllImport("kernel32.dll", EntryPoint = "ReadConsoleInputW", CharSet = CharSet.Unicode)]
        static extern bool ReadConsoleInput(
            IntPtr hConsoleInput,
            [Out] INPUT_RECORD[] lpBuffer,
            int nLength,
            out int lpNumberOfEventsRead);

        public static IEnumerable<INPUT_RECORD> ReadConsoleInput()
        {
            var buffer = new INPUT_RECORD[100];

            while (true)
            {
                ReadConsoleInput(
                    GetStdHandle(IOHandles.STD_INPUT_HANDLE),
                    buffer,
                    buffer.Length,
                    out int eventsRead);

                foreach (var record in buffer.Take(eventsRead))
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Cells in the destination rectangle whose corresponding source location are outside the boundaries of the source
        /// buffer rectangle are left unaffected by the write operation. In other words, these are the cells for which no data
        /// is available to be written. WriteConsoleOutput has no effect on the cursor position.
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
            IntPtr hConsoleOutput,
            CHAR_INFO[] lpBuffer,
            COORD dwBufferSize,
            COORD dwBufferCoord,
            ref SMALL_RECT lpWriteRegion);

        public static void WriteConsoleOutput(CHAR_INFO[][] buffer, int top, int left)
        {
            var height = (short)buffer.Length;
            var width = (short)buffer[0].Length;
            var region = new SMALL_RECT()
            {
                Left = (short)left,
                Top = (short)top,
                Right = (short)(left + width - 1),
                Bottom = (short)(top + height - 1)
            };

            var newBuffer = new CHAR_INFO[width * height];

            for (int row = 0; row < height; row++)
            {
                buffer[row].CopyTo(newBuffer, row * width);
            }

            WriteConsoleOutput(
                GetStdHandle(IOHandles.STD_OUTPUT_HANDLE),
                newBuffer,
                new COORD() { X = width, Y = height },
                new COORD(),
                ref region);
        }

        #endregion
    }

    public enum IOHandles : int
    {
        STD_INPUT_HANDLE = -10,
        STD_OUTPUT_HANDLE = -11,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        public short X;
        public short Y;
    }

    public struct SMALL_RECT
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT_RECORD
    {
        [FieldOffset(0)]
        public InputEventType EventType;
        [FieldOffset(4)]
        public KEY_EVENT_RECORD KeyEvent;
        [FieldOffset(4)]
        public MOUSE_EVENT_RECORD MouseEvent;
        //[FieldOffset(4)]
        //public WINDOW_BUFFER_SIZE_RECORD WindowBufferSizeEvent;
        //[FieldOffset(4)]
        //public MENU_EVENT_RECORD MenuEvent;
        //[FieldOffset(4)]
        //public FOCUS_EVENT_RECORD FocusEvent;
    };

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public struct KEY_EVENT_RECORD
    {
        [FieldOffset(0), MarshalAs(UnmanagedType.Bool)]
        public bool KeyDown;
        [FieldOffset(4), MarshalAs(UnmanagedType.U2)]
        public ushort RepeatCount;
        [FieldOffset(6), MarshalAs(UnmanagedType.U2)]
        public ushort VirtualKeyCode;
        [FieldOffset(8), MarshalAs(UnmanagedType.U2)]
        public ushort VirtualScanCode;
        [FieldOffset(10)]
        public char UnicodeChar;
        [FieldOffset(12), MarshalAs(UnmanagedType.U4)]
        public ControlKeyState controlKeyState;

        public ConsoleKeyInfo ToConsoleKeyInfo()
        {
            return new ConsoleKeyInfo(
                UnicodeChar,
                (ConsoleKey)VirtualKeyCode,
                controlKeyState.HasFlag(ControlKeyState.SHIFT_PRESSED),
                controlKeyState.HasFlag(ControlKeyState.LEFT_ALT_PRESSED)
                || controlKeyState.HasFlag(ControlKeyState.RIGHT_ALT_PRESSED),
                controlKeyState.HasFlag(ControlKeyState.LEFT_CTRL_PRESSED)
                || controlKeyState.HasFlag(ControlKeyState.RIGHT_CTRL_PRESSED));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSE_EVENT_RECORD
    {
        public COORD MousePosition;
        public MouseButtonState ButtonState;
        public ControlKeyState ControlKeyState;
        public MouseEventFlags EventFlags;
    }

    //CHAR_INFO struct, which was a union in the old days
    // so we want to use LayoutKind.Explicit to mimic it as closely
    // as we can
    [StructLayout(LayoutKind.Explicit)]
    public struct CHAR_INFO
    {
        [FieldOffset(0)]
        public char Character;
        [FieldOffset(2)] //2 bytes seems to work properly
        public CharAttributes Attributes;
    }

    // Enumerated type for the control messages sent to the handler routine
    enum CtrlTypes : uint
    {
        CTRL_C_EVENT = 0,
        CTRL_BREAK_EVENT,
        CTRL_CLOSE_EVENT,
        CTRL_LOGOFF_EVENT = 5,
        CTRL_SHUTDOWN_EVENT
    }

    [Flags]
    public enum CharAttributes : ushort
    {
        /// <summary>
        /// None.
        /// </summary>
        None = 0x0000,

        /// <summary>
        /// Text color contains blue.
        /// </summary>
        FOREGROUND_BLUE = 0x0001,

        /// <summary>
        /// Text color contains green.
        /// </summary>
        FOREGROUND_GREEN = 0x0002,

        /// <summary>
        /// Text color contains red.
        /// </summary>
        FOREGROUND_RED = 0x0004,

        /// <summary>
        /// Text color is intensified.
        /// </summary>
        FOREGROUND_INTENSITY = 0x0008,

        /// <summary>
        /// Background color contains blue.
        /// </summary>
        BACKGROUND_BLUE = 0x0010,

        /// <summary>
        /// Background color contains green.
        /// </summary>
        BACKGROUND_GREEN = 0x0020,

        /// <summary>
        /// Background color contains red.
        /// </summary>
        BACKGROUND_RED = 0x0040,

        /// <summary>
        /// Background color is intensified.
        /// </summary>
        BACKGROUND_INTENSITY = 0x0080,

        /// <summary>
        /// Leading byte.
        /// </summary>
        COMMON_LVB_LEADING_BYTE = 0x0100,

        /// <summary>
        /// Trailing byte.
        /// </summary>
        COMMON_LVB_TRAILING_BYTE = 0x0200,

        /// <summary>
        /// Top horizontal
        /// </summary>
        COMMON_LVB_GRID_HORIZONTAL = 0x0400,

        /// <summary>
        /// Left vertical.
        /// </summary>
        COMMON_LVB_GRID_LVERTICAL = 0x0800,

        /// <summary>
        /// Right vertical.
        /// </summary>
        COMMON_LVB_GRID_RVERTICAL = 0x1000,

        /// <summary>
        /// Reverse foreground and background attribute.
        /// </summary>
        COMMON_LVB_REVERSE_VIDEO = 0x4000,

        /// <summary>
        /// Underscore.
        /// </summary>
        COMMON_LVB_UNDERSCORE = 0x8000,

        FOREGROUND_WHITE = FOREGROUND_BLUE | FOREGROUND_GREEN | FOREGROUND_RED,
    }

    [Flags]
    public enum DisplayMode : uint
    {
        NORMAL = 0,
        CONSOLE_FULLSCREEN = 1,
        CONSOLE_FULLSCREEN_HARDWARE = 2
    }

    [Flags]
    public enum ConsoleModeInput : uint
    {
        NONE = 0,

        /// <summary>
        /// CTRL+C is processed by the system and is not placed in the input buffer. If the input buffer is being read by ReadFile or ReadConsole, other control keys are processed by the system and are not returned in the ReadFile or ReadConsole buffer. If the ENABLE_LINE_INPUT mode is also enabled, backspace, carriage return, and line feed characters are handled by the system.
        /// </summary>
        ENABLE_PROCESSED_INPUT = 0x0001,

        /// <summary>
        /// The ReadFile or ReadConsole function returns only when a carriage return character is read. If this mode is disabled, the functions return when one or more characters are available.
        /// </summary>
        ENABLE_LINE_INPUT = 0x0002,

        /// <summary>
        /// Characters read by the ReadFile or ReadConsole function are written to the active screen buffer as they are read. This mode can be used only if the ENABLE_LINE_INPUT mode is also enabled.
        /// </summary>
        ENABLE_ECHO_INPUT = 0x0004,

        /// <summary>
        /// User interactions that change the size of the console screen buffer are reported in the console's input buffer. Information about these events can be read from the input buffer by applications using the ReadConsoleInput function, but not by those using ReadFile or ReadConsole.
        /// </summary>
        ENABLE_WINDOW_INPUT = 0x0008,

        /// <summary>
        /// If the mouse pointer is within the borders of the console window and the window has the keyboard focus, mouse events generated by mouse movement and button presses are placed in the input buffer. These events are discarded by ReadFile or ReadConsole, even when this mode is enabled.
        /// </summary>
        ENABLE_MOUSE_INPUT = 0x0010,

        /// <summary>
        /// When enabled, text entered in a console window will be inserted at the current cursor location and all text following that location will not be overwritten. When disabled, all following text will be overwritten.
        /// </summary>
        ENABLE_INSERT_MODE = 0x0020,

        /// <summary>
        /// This flag enables the user to use the mouse to select and edit text.
        /// To enable this mode, use ENABLE_QUICK_EDIT_MODE | ENABLE_EXTENDED_FLAGS. To disable this mode, use ENABLE_EXTENDED_FLAGS without this flag.
        /// </summary>
        ENABLE_QUICK_EDIT_MODE = 0x0040,

        /// <summary>
        /// Required to enable or disable extended flags. See ENABLE_INSERT_MODE and ENABLE_QUICK_EDIT_MODE.
        /// </summary>
        ENABLE_EXTENDED_FLAGS = 0x0080,

        /// <summary>
        /// ??? no one has a clue what this one does
        /// </summary>
        ENABLE_AUTO_POSITION = 0x0100,

        /// <summary>
        /// Setting this flag directs the Virtual Terminal processing engine to convert user input received by the console window into Console Virtual Terminal Sequences that can be retrieved by a supporting application through WriteFile or WriteConsole functions.
        /// The typical usage of this flag is intended in conjunction with ENABLE_VIRTUAL_TERMINAL_PROCESSING on the output handle to connect to an application that communicates exclusively via virtual terminal sequences.
        /// </summary>
        ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200,
    }

    [Flags]
    public enum ConsoleModeOutput : uint
    {
        NONE = 0,

        /// <summary>
        /// Characters written by the WriteFile or WriteConsole function or echoed by the ReadFile or ReadConsole function are parsed for ASCII control sequences, and the correct action is performed. Backspace, tab, bell, carriage return, and line feed characters are processed.
        /// </summary>
        ENABLE_PROCESSED_OUTPUT = 0x0001,

        /// <summary>
        /// When writing with WriteFile or WriteConsole or echoing with ReadFile or ReadConsole, the cursor moves to the beginning of the next row when it reaches the end of the current row. This causes the rows displayed in the console window to scroll up automatically when the cursor advances beyond the last row in the window. It also causes the contents of the console screen buffer to scroll up (discarding the top row of the console screen buffer) when the cursor advances beyond the last row in the console screen buffer. If this mode is disabled, the last character in the row is overwritten with any subsequent characters.
        /// </summary>
        ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002,

        /// <summary>
        /// When writing with WriteFile or WriteConsole, characters are parsed for VT100 and similar control character sequences that control cursor movement, color/font mode, and other operations that can also be performed via the existing Console APIs. For more information, see Console Virtual Terminal Sequences.
        /// </summary>
        ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004,

        /// <summary>
        /// When writing with WriteFile or WriteConsole, this adds an additional state to end-of-line wrapping that can delay the cursor move and buffer scroll operations.
        /// Normally when ENABLE_WRAP_AT_EOL_OUTPUT is set and text reaches the end of the line, the cursor will immediately move to the next line and the contents of the buffer will scroll up by one line. In contrast with this flag set, the scroll operation and cursor move is delayed until the next character arrives. The written character will be printed in the final position on the line and the cursor will remain above this character as if ENABLE_WRAP_AT_EOL_OUTPUT was off, but the next printable character will be printed as if ENABLE_WRAP_AT_EOL_OUTPUT is on. No overwrite will occur. Specifically, the cursor quickly advances down to the following line, a scroll is performed if necessary, the character is printed, and the cursor advances one more position.
        /// The typical usage of this flag is intended in conjunction with setting ENABLE_VIRTUAL_TERMINAL_PROCESSING to better emulate a terminal emulator where writing the final character on the screen (in the bottom right corner) without triggering an immediate scroll is the desired behavior.
        /// </summary>
        DISABLE_NEWLINE_AUTO_RETURN = 0x0008,

        /// <summary>
        /// The APIs for writing character attributes including WriteConsoleOutput and WriteConsoleOutputAttribute allow the usage of flags from character attributes to adjust the color of the foreground and background of text. Additionally, a range of DBCS flags was specified with the COMMON_LVB prefix. Historically, these flags only functioned in DBCS code pages for Chinese, Japanese, and Korean languages. With exception of the leading byte and trailing byte flags, the remaining flags describing line drawing and reverse video (swap foreground and background colors) can be useful for other languages to emphasize portions of output.
        /// With exception of the leading byte and trailing byte flags, the remaining flags describing line drawing and reverse video (swap foreground and background colors) can be useful for other languages to emphasize portions of output.
        /// Setting this console mode flag will allow these attributes to be used in every code page on every language.
        /// It is off by default to maintain compatibility with known applications that have historically taken advantage of the console ignoring these flags on non-CJK machines to store bits in these fields for their own purposes or by accident.
        /// Note that using the ENABLE_VIRTUAL_TERMINAL_PROCESSING mode can result in LVB grid and reverse video flags being set while this flag is still off if the attached application requests underlining or inverse video via Console Virtual Terminal Sequences.
        /// </summary>
        ENABLE_LVB_GRID_WORLDWIDE = 0x0010,
    }

    public enum InputEventType : ushort
    {
        NONE = 0,

        /// <summary>
        /// The Event member contains a FOCUS_EVENT_RECORD structure. These events are used internally and should be ignored.
        /// </summary>
        FOCUS_EVENT = 0x0010,

        /// <summary>
        /// The Event member contains a KEY_EVENT_RECORD structure with information about a keyboard event.
        /// </summary>
        KEY_EVENT = 0x0001,

        /// <summary>
        /// The Event member contains a MENU_EVENT_RECORD structure. These events are used internally and should be ignored.
        /// </summary>
        MENU_EVENT = 0x0008,

        /// <summary>
        /// The Event member contains a MOUSE_EVENT_RECORD structure with information about a mouse movement or button press event.
        /// </summary>
        MOUSE_EVENT = 0x0002,

        /// <summary>
        /// The Event member contains a WINDOW_BUFFER_SIZE_RECORD structure with information about the new size of the console screen buffer.
        /// </summary>
        WINDOW_BUFFER_SIZE_EVENT = 0x0004,
    }

    [Flags]
    public enum ControlKeyState : uint
    {
        /// <summary>
        /// The right ALT key is pressed.
        /// </summary>
        RIGHT_ALT_PRESSED = 0x0001,

        /// <summary>
        /// The left ALT key is pressed.
        /// </summary>
        LEFT_ALT_PRESSED = 0x0002,

        /// <summary>
        /// The right CTRL key is pressed.
        /// </summary>
        RIGHT_CTRL_PRESSED = 0x0004,

        /// <summary>
        /// The left CTRL key is pressed.
        /// </summary>
        LEFT_CTRL_PRESSED = 0x0008,

        /// <summary>
        /// The SHIFT key is pressed.
        /// </summary>
        SHIFT_PRESSED = 0x0010,

        /// <summary>
        /// The NUM LOCK light is on.
        /// </summary>
        NUMLOCK_ON = 0x0020,

        /// <summary>
        /// The SCROLL LOCK light is on.
        /// </summary>
        SCROLLLOCK_ON = 0x0040,

        /// <summary>
        /// The CAPS LOCK light is on.
        /// </summary>
        CAPSLOCK_ON = 0x0080,

        /// <summary>
        /// The key is enhanced.
        /// </summary>
        ENHANCED_KEY = 0x0100,
    }

    [Flags]
    public enum MouseButtonState : uint
    {
        ALL_RELEASED = 0,

        /// <summary>
        /// The leftmost mouse button.
        /// </summary>
        FROM_LEFT_1ST_BUTTON_PRESSED = 0x0001,

        /// <summary>
        /// The rightmost mouse button.
        /// </summary>
        RIGHTMOST_BUTTON_PRESSED = 0x0002,

        /// <summary>
        /// The second button from the left.
        /// </summary>
        FROM_LEFT_2ND_BUTTON_PRESSED = 0x0004,

        /// <summary>
        /// The third button from the left.
        /// </summary>
        FROM_LEFT_3RD_BUTTON_PRESSED = 0x0008,

        /// <summary>
        /// The fourth button from the left.
        /// </summary>
        FROM_LEFT_4TH_BUTTON_PRESSED = 0x0010,
    }

    [Flags]
    public enum MouseEventFlags : uint
    {
        /// <summary>
        /// A button was pressed or released.
        /// </summary>
        MOUSE_BUTTON_STATE_CHANGED = 0,

        /// <summary>
        /// A change in mouse position occurred.
        /// </summary>
        MOUSE_MOVED = 0x0001,

        /// <summary>
        /// The second click (button press) of a double-click occurred. The first click is returned as a regular button-press event.
        /// </summary>
        DOUBLE_CLICK = 0x0002,

        /// <summary>
        /// The vertical mouse wheel was moved.
        /// If the high word of the dwButtonState member contains a positive value, the wheel was rotated forward, away from the user. Otherwise, the wheel was rotated backward, toward the user.    }
        /// </summary>
        MOUSE_WHEELED = 0x0004,

        /// <summary>
        /// The horizontal mouse wheel was moved.
        /// If the high word of the dwButtonState member contains a positive value, the wheel was rotated to the right. Otherwise, the wheel was rotated to the left.
        /// </summary>
        MOUSE_HWHEELED = 0x0008,
    }
}
