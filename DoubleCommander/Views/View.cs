using DoubleCommander.Common;
using DoubleCommander.Core;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public abstract class View
    {
        public View Parent { get; set; }
        public Size Size { get; }
        public Point Position { get; }
        public bool Enabled { get; set; } = true;

        public View(Point position, Size size, View parent = null)
        {
            Position = position;
            Size = size;
            Parent = parent;
            EventsSender.Subscribe(this);
        }

        public abstract void OnPaint(PaintEventArgs e);
        public abstract void OnKeyDown(KeyDownEventArgs e);
        public virtual void Update()
        {
            // Override if necessary
        }

        public virtual void Close()
        {
            EventsSender.Unsubscribe(this);
        }
    }
}
