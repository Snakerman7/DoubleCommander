using DoubleCommander.Common;
using DoubleCommander.Resources;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class HelpTextBar : DrawableBox
    {
        private readonly string Text;

        public HelpTextBar(Point position, Size size)
            : base(position, size)
        {
            Text = StringResources.HelpText;
        }

        public override void Draw(ConsoleGraphics g)
        {
            g.DrawString(Text, StringResources.FontName, ColorResources.WindowTextColor, _position.X, _position.Y, 11);
        }
    }
}
