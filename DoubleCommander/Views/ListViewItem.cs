using System;
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
            uint textColor = 0xffffffff;
            if (_parent.Enabled && Selected)
            {
                graphics.FillRectangle(0xff00bbbb, position.X, position.Y, size.Width, size.Height);
                textColor = 0xff000000;
            }

            graphics.DrawString(Text, "Consolas", textColor, position.X, position.Y, 10);
        }
    }
}
