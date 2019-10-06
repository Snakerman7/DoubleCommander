using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public abstract class View
    {
        public View Parent { get; set; }
        public Size Size { get; set; } = new Size();
        public Point Position { get; set; } = new Point();
        public bool Enabled { get; set; } = true;

        public abstract void OnPaint(ConsoleGraphics g);
        public abstract void OnKeyDown(Keys key);
    }
}
