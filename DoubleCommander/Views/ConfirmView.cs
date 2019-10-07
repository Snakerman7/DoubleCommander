using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;
using DoubleCommander.Common;

namespace DoubleCommander.Views
{
    public class ConfirmView : View
    {
        private readonly string _text;

        public ConfirmView(string messageText, Point position, Size size, View parent = null)
            : base(position, size, parent)
        {
            _text = messageText;
        }

        public override void OnKeyDown(Keys key)
        {
            throw new NotImplementedException();
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            throw new NotImplementedException();
        }
    }
}
