using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.FileSystem;
using DoubleCommander.Resources;
using NConsoleGraphics;
using System.Linq;
using System.IO;
using System;

namespace DoubleCommander.Views
{
    public class RenameView : View
    {
        private readonly TextBox _textBox;
        private readonly OkButton _okButton;
        private readonly CancelButton _cancelButton;
        private readonly FileSystemItem _fsItem;

        public RenameView(FileSystemItem fsItem, Point position, View parent = null)
            : base(position, new Size(150, 300), parent)
        {
            _fsItem = fsItem;
            _textBox = new TextBox(new Point(Position.X + 10, Position.Y + 40), new Size(20, Size.Width - 20));
            _okButton = new OkButton(new Point(Position.X + 25, Position.Y + 100)) { Selected = true };
            _cancelButton = new CancelButton(new Point(Position.X + Size.Width - 125, Position.Y + 100));
            if (Parent != null)
                Parent.Enabled = false;
            _textBox.Text = _fsItem.Name;
        }

        public override void OnKeyDown(KeyEventArgs e)
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

        public override void OnPaint(ConsoleGraphics g)
        {
            g.FillRectangle(ColorResources.WindowBackgroundColor, Position.X, Position.Y, Size.Width, Size.Height);
            g.DrawRectangle(ColorResources.WindowBorderColor, Position.X + NumericConstants.MarginUpLeft, Position.Y + NumericConstants.MarginUpLeft,
                Size.Width - NumericConstants.MarginRightDown, Size.Height - NumericConstants.MarginRightDown, NumericConstants.WindowBorderThikness);
            g.DrawString($"Rename: {_fsItem.Name}", StringResources.FontName, 0xff000000, Position.X + 20, Position.Y + 10, 12);
            _textBox.Draw(g);
            _okButton.Draw(g);
            _cancelButton.Draw(g);
        }

        public override void Close()
        {
            base.Close();
            if (Parent != null)
                Parent.Enabled = true;
        }

        private void ChangeSelectedButton()
        {
            _okButton.Selected = !_okButton.Selected;
            _cancelButton.Selected = !_cancelButton.Selected;
        }

        private void Action()
        {
            if (_okButton.Selected)
            {
                try
                {
                    switch (_fsItem)
                    {
                        case FileItem file:
                            FileInfo fileInfo = new FileInfo(file.FullName);
                            fileInfo.Rename(_textBox.Text + "." + file.Extension);
                            break;
                        case DirectoryItem dir:
                            DirectoryInfo dirInfo = new DirectoryInfo(dir.FullName);
                            dirInfo.Rename(_textBox.Text);
                            break;
                    }
                }
                catch(Exception ex)
                {
                    _ = new MessageView(ex.Message, Parent);
                }
                EventsSender.SendUpdateEvent();
            }
            Close();
        }
    }
}
