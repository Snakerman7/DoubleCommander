using DoubleCommander.Common;
using DoubleCommander.Core;
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
            g.DrawRectangle(ColorResources.TextBoxBorderColor, _position.X, _position.Y, _size.Width, _size.Height, 
                NumericConstants.WindowBorderThikness);
            g.FillRectangle(ColorResources.TextBoxBackgroundColor, _position.X, _position.Y, _size.Width, _size.Height);
            g.DrawString(Text, StringResources.FontName, ColorResources.ListItemSelectedTextColor, _position.X,
                _position.Y, NumericConstants.TextBoxFontSize);
        }

        public void AddLetter(Keys key, bool IsShiftDown)
        {
            if (Text.Length < NumericConstants.NewNameMaxLength)
            {
                char letter = EventsSender.GetLetterKeyChar(key);
                if (IsShiftDown)
                    letter = char.ToUpper(letter);
                Text += letter;
            }
        }

        public void DeleteLetter()
        {
            if (Text.Length > 0)
            {
                Text = Text.Remove(Text.Length - 1, 1);
            }
        }
    }
}
