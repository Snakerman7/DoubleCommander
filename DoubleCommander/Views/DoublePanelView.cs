using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class DoublePanelView : View
    {
        private ListView _leftView;
        private ListView _rightView;

        public ListView LeftView {
            get => _leftView;
            set
            {
                _leftView = value;
                _leftView.Parent = this;
                _leftView.Position = new Point(0 + 5, 0 + 5);
                _leftView.Size = new Size(Size.Height - 10, Size.Width / 2 - 10);
            }
        }
        public ListView RightView {
            get => _rightView; 
            set
            {
                _rightView = value;
                _rightView.Parent = this;
                _rightView.Position = new Point(Size.Width/2 + 5, 0 + 5);
                _rightView.Size = new Size(Size.Height - 10, Size.Width / 2 - 10);
            }
        }

        public DoublePanelView(Point position, Size size)
        {
            Position = position;
            Size = size;
        }

        public override void OnKeyDown(Keys key)
        {
            if(key == Keys.TAB)
            {
                _leftView.Enabled = !_leftView.Enabled;
                _rightView.Enabled = !_rightView.Enabled;
            }
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            
        }
    }
}
