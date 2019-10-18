using NConsoleGraphics;
using System;

namespace DoubleCommander.Core
{
    public class KeyDownEventArgs : EventArgs
    {
        public Keys Key { get; }
        public bool IsShiftDown { get; }

        public KeyDownEventArgs(Keys key, bool isShiftDown = false)
        {
            Key = key;
            IsShiftDown = isShiftDown;
        }
    }
}
