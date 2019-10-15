using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.Resources;
using NConsoleGraphics;
using System.IO;
using System.Linq;
using System.Text;

namespace DoubleCommander.Views
{
    public class CreateFolderView : View
    {
        private readonly string _path;
        private readonly TextBox _textBox;
        private readonly OkButton _okButton;
        private readonly CancelButton _cancelButton;

        public CreateFolderView(string path, Point position, View parent = null)
            : base(position, new Size(150, 300), parent)
        {
            _path = path;
            _textBox = new TextBox(new Point(Position.X + 10, Position.Y + 20), new Size(20, Size.Width - 20));
            _okButton = new OkButton(new Point(Position.X + 25, Position.Y + 100)) { Selected = true };
            _cancelButton = new CancelButton(new Point(Position.X + Size.Width - 125, Position.Y + 100));
            EventsSender.Subscribe(this);
            Parent.Enabled = false;
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Keys.RETURN)
            {
                if (_okButton.Selected)
                {
                    string fullName = Path.Combine(_path, _textBox.Text.ToString());
                    if (!Directory.Exists(fullName))
                    {
                        Directory.CreateDirectory(fullName);
                        EventsSender.SendUpdateEvent();
                    }
                }
                EventsSender.Unsubscribe(this);
                Parent.Enabled = true;
            }
            else if(e.Key == Keys.ESCAPE)
            {
                EventsSender.Unsubscribe(this);
                Parent.Enabled = true;
            }
            else if(e.Key == Keys.BACK)
            {
                if(_textBox.Text.Length > 0)
                {
                    _textBox.Text = _textBox.Text.Remove(_textBox.Text.Length - 1, 1);
                }
            }
            else if(EventsSender.LettersKeys.Contains(e.Key))
            {
                if (_textBox.Text.Length < NumericConstants.NewNameMaxLength)
                {
                    char letter = EventsSender.GetLetterKeyChar(e.Key);
                    if (e.IsShiftDown)
                        letter = char.ToUpper(letter);
                    _textBox.Text += letter;
                }
            } else if(e.Key == Keys.LEFT || e.Key == Keys.RIGHT)
            {
                ChangeSelectedButton();
            }
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            g.FillRectangle(ColorResources.WindowBackgroundColor, Position.X, Position.Y, Size.Width, Size.Height);
            g.DrawRectangle(ColorResources.WindowBorderColor, Position.X + NumericConstants.MarginUpLeft, Position.Y + NumericConstants.MarginUpLeft,
                Size.Width - NumericConstants.MarginRightDown, Size.Height - NumericConstants.MarginRightDown, NumericConstants.WindowBorderThikness);
            _textBox.Draw(g);
            _okButton.Draw(g);
            _cancelButton.Draw(g);
        }

        private void ChangeSelectedButton()
        {
            _okButton.Selected = !_okButton.Selected;
            _cancelButton.Selected = !_cancelButton.Selected;
        }
    }
}
