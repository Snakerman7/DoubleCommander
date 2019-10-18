using System;
using NConsoleGraphics;

namespace DoubleCommander.Core
{
    public class PaintEventArgs : EventArgs
    {
        public ConsoleGraphics Graphics { get; }

        public PaintEventArgs(ConsoleGraphics graphics)
        {
            Graphics = graphics;
        }
    }
}
