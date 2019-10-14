using DoubleCommander.Common;
using DoubleCommander.Core;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public abstract class View
    {
        public View Parent { get; }
        public Size Size { get; }
        public Point Position { get; }
        public bool Enabled { get; set; } = true;

        public View(Point position, Size size, View parent = null)
        {
            Position = position;
            Size = size;
            Parent = parent;
        }

        public abstract void OnPaint(ConsoleGraphics g);
        public abstract void OnKeyDown(KeyEventArgs e);
        public virtual void OnUpdate()
        {
            // Override if necessary
        }
    }
}
