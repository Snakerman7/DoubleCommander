using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.Resources;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class MessageView : View
    {
        private readonly Button _okButton;
        private readonly string _message;

        public MessageView(string message, View parent = null)
            : base(new Point(EventsSender.Graphics.ClientWidth / 2 - NumericConstants.OperationViewWidth / 2,
                   EventsSender.Graphics.ClientHeight / 2 - NumericConstants.OperationViewHeight / 2),
                  new Size(NumericConstants.OperationViewHeight, NumericConstants.OperationViewWidth), parent)
        {
            _message = message;
            int newLineCount = message.Length / 50;
            for (int i = 0; i <= newLineCount; i++)
            {
                _message = _message.Insert(50 * i + i, "\n");
            }
            _okButton = new Button(StringResources.OkButtonText, NumericConstants.ButtonOkTextAlign, 
                new Point(Position.X + Size.Width / 2 - NumericConstants.ButtonWidth / 2, Position.Y + Size.Height - 40))
            { Selected = true };
            if (Parent != null)
            {
                Parent.Enabled = false;
            }
        }

        public override void OnKeyDown(KeyDownEventArgs e)
        {
            if (e.Key == Keys.RETURN || e.Key == Keys.ESCAPE)
            {
                Close();
            }
        }

        public override void OnPaint(PaintEventArgs e)
        {
            ConsoleGraphics g = e.Graphics;
            g.FillRectangle(ColorResources.WindowBackgroundColor, Position.X, Position.Y, Size.Width, Size.Height);
            g.DrawRectangle(ColorResources.WindowBorderColor, Position.X + NumericConstants.MarginUpLeft,
                Position.Y + NumericConstants.MarginUpLeft, Size.Width - NumericConstants.MarginRightDown,
                Size.Height - NumericConstants.MarginRightDown, NumericConstants.WindowBorderThikness);
            g.DrawString(_message, StringResources.FontName, 0xff000000,
                Position.X + 10, Position.Y + 10, 10);
            _okButton.Draw(g);
        }

        public override void Close()
        {
            base.Close();
            if (Parent != null)
            {
                Parent.Enabled = true;
            }
        }
    }
}
