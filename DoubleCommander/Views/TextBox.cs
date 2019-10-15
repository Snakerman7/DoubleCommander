using DoubleCommander.Common;
using DoubleCommander.Resources;
using NConsoleGraphics;
using System.Text;

namespace DoubleCommander.Views
{
    public class TextBox : DrawableBox
    {
        public string Text { get; set; } = string.Empty;

        public TextBox(Point position, Size size) : base(position, size)
        {
        }

        public override void Draw(ConsoleGraphics g)
        {
            g.DrawRectangle(0xff000000, _position.X, _position.Y, _size.Width, _size.Height, 2);
            g.FillRectangle(0xffffffff, _position.X, _position.Y, _size.Width, _size.Height);
            g.DrawString(Text, StringResources.FontName, ColorResources.ListItemSelectedTextColor, _position.X,
                _position.Y, 12);
        }
    }
}
