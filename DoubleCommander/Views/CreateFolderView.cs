using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.Resources;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class CreateFolderView : View
    {
        private StringBuilder _name = new StringBuilder();


        public CreateFolderView(Point position, Size size, View parent = null)
            : base(position, size, parent)
        {
            EventsSender.Subscribe(this);
            Parent.Enabled = false;
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Keys.RETURN)
            {
                EventsSender.Unsubscribe(this);
                Parent.Enabled = true;
            }
            else if(e.Key == Keys.BACK)
            {
                if(_name.Length > 0)
                {
                    _name.Remove(_name.Length - 1, 1);
                }
            }
            else if(NumericConstants.LettersKeys.Contains(e.Key))
            {
                char letter = EventsSender.GetLetterKeyChar(e.Key);
                if (e.IsShiftDown)
                    letter = char.ToUpper(letter);
                _name.Append(letter);
            }
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            g.FillRectangle(0xffffffff, Position.X, Position.Y, Size.Width, Size.Height);
            g.DrawString(_name.ToString(), StringResources.FontName, ColorResources.ListItemSelectedTextColor, Position.X,
                Position.Y, 10);
        }
    }
}
