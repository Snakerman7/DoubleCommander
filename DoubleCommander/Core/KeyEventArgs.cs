using System;
using NConsoleGraphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCommander.Core
{
    public class KeyEventArgs : EventArgs
    {
        public Keys Key { get; }
        public bool IsShiftDown { get; }

        public KeyEventArgs(Keys key, bool isShiftDown)
        {
            Key = key;
            IsShiftDown = isShiftDown;
        }
    }
}
