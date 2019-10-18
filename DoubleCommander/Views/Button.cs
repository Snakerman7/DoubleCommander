using DoubleCommander.Common;
using DoubleCommander.Resources;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class Button : DrawableBox
    {
        private readonly string _text;
        private readonly int _textAlign;
        public bool Selected { get; set; } = false;

        public Button(string Text, int textAlign, Point position)
            : base(position, new Size(NumericConstants.ButtonHeight, NumericConstants.ButtonWidth))
        {
            _text = Text;
            _textAlign = textAlign;
        }

        public override void Draw(ConsoleGraphics g)
        {
            uint color;
            if (Selected)
                color = ColorResources.SelectedButtonColor;
            else
                color = ColorResources.UnselectedButtonColor;
            g.FillRectangle(color, _position.X, _position.Y, _size.Width, _size.Height);
            g.DrawString(_text, StringResources.FontName, ColorResources.ButtonTextColor,
                _position.X + _textAlign, _position.Y - 2, NumericConstants.ButtonFontSize);
        }
    }
}
