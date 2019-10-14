using DoubleCommander.Common;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public abstract class DrawableBox
    {
        protected readonly Point _position;
        protected readonly Size _size;

        public DrawableBox(Point position, Size size)
        {
            _position = position;
            _size = size;
        }

        public abstract void Draw(ConsoleGraphics g);
    }
}
