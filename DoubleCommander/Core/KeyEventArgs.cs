using NConsoleGraphics;
using System;

namespace DoubleCommander.Core
{
    public class KeyEventArgs : EventArgs
    {
        public Keys Key { get; }
        public bool IsShiftDown { get; }

        public KeyEventArgs(Keys key, bool isShiftDown = false)
        {
            Key = key;
            IsShiftDown = isShiftDown;
        }
    }
}
