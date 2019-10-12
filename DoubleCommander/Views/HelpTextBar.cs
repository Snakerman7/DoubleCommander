using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleCommander.Common;
using NConsoleGraphics;
using DoubleCommander.Resources;

namespace DoubleCommander.Views
{
    public class HelpTextBar
    {
        private readonly Point _position;
        private readonly Size _size;
        private readonly string Text;

        public HelpTextBar(Point position, Size size)
        {
            _position = position;
            _size = size;
            Text = "F1 : Copy  F2 : Move";
        }

        public void Draw(ConsoleGraphics g)
        {
            g.DrawString(Text, "Consolas", 0xff000000, _position.X, _position.Y, 11);
        }
    }
}
