using DoubleCommander.Resources;
using DoubleCommander.Common;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class OkButton : DrawableBox
    {
        private readonly string _text;
        public bool Selected { get; set; } = false;

        public OkButton(Point position) : base(position, new Size(25, 100))
        {
            _text = StringResources.OkButtonText;
        }

        public override void Draw(ConsoleGraphics g)
        {
            uint color;
            if (Selected)
                color = ColorResources.SelectedButtonColor;
            else
                color = ColorResources.UnselectedButtonColor;
            g.FillRectangle(color, _position.X, _position.Y, _size.Width, _size.Height);
            g.DrawString(_text, StringResources.FontName, 0xff000000, _position.X + 35, _position.Y - 2, 16);
        }
    }
}
