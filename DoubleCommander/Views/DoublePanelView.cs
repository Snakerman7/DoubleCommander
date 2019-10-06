using DoubleCommander.Common;
using DoubleCommander.Resources;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class DoublePanelView : View
    {
        private ListView _leftView;
        private ListView _rightView;

        public DoublePanelView(Point position, Size size, View parent = null)
            : base(position, size, parent)
        {
            _leftView = new ListView(new Point(0 + NumericConstants.MarginUpLeft, 0 + NumericConstants.MarginUpLeft),
               new Size(Size.Height - NumericConstants.MarginRightDown, Size.Width / 2 - NumericConstants.MarginRightDown), this);
            _rightView = new ListView(new Point(Size.Width / 2 + NumericConstants.MarginUpLeft, 0 + NumericConstants.MarginUpLeft),
                new Size(Size.Height - NumericConstants.MarginRightDown, Size.Width / 2 - NumericConstants.MarginRightDown),
                this)
            { Enabled = false };
            EventsSender.Subscribe(_leftView);
            EventsSender.Subscribe(_rightView);
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
