using DoubleCommander.Common;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class HelpTextBar : DrawableBox
    {
        private readonly string Text;

        public HelpTextBar(Point position, Size size)
            : base(position, size)
        {
            Text = "F1 : Copy  F2 : Move";
        }

        public override void Draw(ConsoleGraphics g)
        {
            g.DrawString(Text, "Consolas", 0xff000000, _position.X, _position.Y, 11);
        }
    }
}
