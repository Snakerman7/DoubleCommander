using DoubleCommander.Resources;
using DoubleCommander.Common;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class OkButton : DrawableBox
    {
        private readonly string _text;

        public OkButton(Point position) : base(position, new Size(25, 100))
        {
            _text = "OK";
        }

        public override void Draw(ConsoleGraphics g)
        {
            g.FillRectangle(0xffD93A02, _position.X, _position.Y, _size.Width, _size.Height);
            g.DrawString(_text, StringResources.FontName, 0xff000000, _position.X + 35, _position.Y - 2, 16);
        }
    }
}
