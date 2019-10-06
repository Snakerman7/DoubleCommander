using DoubleCommander.Common;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public abstract class View
    {
        public View Parent { get; set; }
        public Size Size { get; } = new Size();
        public Point Position { get; } = new Point();
        public bool Enabled { get; set; } = true;

        public View(Point position, Size size, View parent = null)
        {
            Position = position;
            Size = size;
            Parent = parent;
        }

        public abstract void OnPaint(ConsoleGraphics g);
        public abstract void OnKeyDown(Keys key);
    }
}
