using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.Resources;
using NConsoleGraphics;
using System;
using System.Linq;

namespace DoubleCommander.Views
{
    public abstract class EditTextView : View
    {
        private readonly string _title;
        protected readonly TextBox _textBox;
        protected readonly Button _okButton;
        protected readonly Button _cancelButton;

        public EditTextView(string title, Point position, View parent = null) 
            : base(position, new Size(NumericConstants.WindowHeight, NumericConstants.WindowWidth), parent)
        {
            _title = title;
            _textBox = new TextBox(new Point(Position.X + 10, Position.Y + 40), new Size(20, Size.Width - 20));
            _okButton = new Button(StringResources.OkButtonText, NumericConstants.ButtonOkTextAlign,
                new Point(Position.X + 25, Position.Y + 100))
            { Selected = true };
            _cancelButton = new Button(StringResources.CancelButtonText, NumericConstants.ButtonCancelTextAlign,
                new Point(Position.X + Size.Width - 125, Position.Y + 100));
            if (Parent != null)
                Parent.Enabled = false;
        }

        public override void OnKeyDown(KeyDownEventArgs e)
        {
            if (e.Key == Keys.RETURN)
            {
                Action();
            }
            else if (e.Key == Keys.ESCAPE)
            {
                Close();
            }
            else if (e.Key == Keys.BACK)
            {
                _textBox.DeleteLetter();
            }
            else if (EventsSender.LettersKeys.Contains(e.Key))
            {
                _textBox.AddLetter(e.Key, e.IsShiftDown);
            }
            else if (e.Key == Keys.LEFT || e.Key == Keys.RIGHT)
            {
                ChangeSelectedButton();
            }
        }

        public override void OnPaint(PaintEventArgs e)
        {
            ConsoleGraphics g = e.Graphics;
            g.FillRectangle(ColorResources.WindowBackgroundColor, Position.X, Position.Y, Size.Width, Size.Height);
            g.DrawRectangle(ColorResources.WindowBorderColor, Position.X + NumericConstants.MarginUpLeft,
                Position.Y + NumericConstants.MarginUpLeft, Size.Width - NumericConstants.MarginRightDown,
                Size.Height - NumericConstants.MarginRightDown, NumericConstants.WindowBorderThikness);
            g.DrawString(_title, StringResources.FontName, ColorResources.WindowTextColor,
                Position.X + 20, Position.Y + 10, 12);
            _textBox.Draw(g);
            _okButton.Draw(g);
            _cancelButton.Draw(g);
        }

        private void ChangeSelectedButton()
        {
            _okButton.Selected = !_okButton.Selected;
            _cancelButton.Selected = !_cancelButton.Selected;
        }

        protected abstract void Action();

        public override void Close()
        {
            base.Close();
            if (Parent != null)
                Parent.Enabled = true;
        }
    }
}
