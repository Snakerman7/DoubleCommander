using DoubleCommander.Common;
using DoubleCommander.Resources;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class ListViewItem
    {
        public ListView _parent;
        public string Text { get; set; }
        public bool Selected { get; set; } = false;

        public ListViewItem(ListView parent, string text)
        {
            Text = text;
            _parent = parent;
        }

        public void Draw(ConsoleGraphics graphics, Point position, Size size)
        {
            uint textColor = ColorResources.ListItemTextColor;
            if (_parent.Enabled && Selected)
            {
                graphics.FillRectangle(ColorResources.ListItemBackgroundColor, position.X, position.Y, size.Width, size.Height);
                textColor = ColorResources.ListItemSelectedTextColor;
            }
            graphics.DrawString(Text, StringResources.FontName, textColor, position.X, position.Y, NumericConstants.FontSize);
        }
    }
}
