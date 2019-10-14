using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleCommander.Common;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class ProgressBar : DrawableBox
    {
        private int _progress = 0;
        private int _progressWidth = 0;

        public int Progress
        {
            get => _progress;
            set
            {
                if(value < 0)
                {
                    value = 0;
                }
                else if(value > 100)
                {
                    value = 100;
                }
                _progress = value;
                _progressWidth = (int)((_size.Width / 100.0) * _progress);
            }
        }

        public ProgressBar(Point position, Size size)
            : base(position, size)
        {
        }

        public override void Draw(ConsoleGraphics g)
        {
            g.FillRectangle(0xff00a5a5, _position.X, _position.Y, _progressWidth, _size.Height);
            g.DrawRectangle(0xffc7c7c7, _position.X, _position.Y, _size.Width, _size.Height);
        }
    }
}
