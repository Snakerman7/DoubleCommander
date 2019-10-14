using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.FileSystem;
using DoubleCommander.Resources;
using NConsoleGraphics;
using System.IO;

namespace DoubleCommander.Views
{
    public enum OperationType
    {
        CopyFile,
        MoveFile,
        CopyDirectory,
        MoveDirectory
    }

    public class OperationView : View
    {
        private readonly ProgressBar _progressBar;
        private readonly OperationType _type;
        private readonly OkButton _okButton;
        private readonly string _sourcePath;
        private string _destPath;

        public OperationView(OperationType type, string source, string dest, Point position, View parent = null)
            : base(position, new Size(200, 400), parent)
        {
            _type = type;
            _sourcePath = source;
            _destPath = dest;
            parent.Enabled = false;
            _progressBar = new ProgressBar(new Point(Position.X + 10, Position.Y + 50), new Size(30, Size.Width - 20));
            _okButton = new OkButton(new Point(Position.X + Size.Width / 2 - 50, Position.Y + 150));
            EventsSender.Subscribe(this);
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Keys.RETURN)
            {
                if ((_type == OperationType.CopyFile || _type == OperationType.MoveFile))
                {
                    _destPath = FileSystemViewer.CheckFile(_destPath);
                    FileInfo sourceFile = new FileInfo(_sourcePath);
                    FileInfo destFile = new FileInfo(_destPath);
                    sourceFile.CopyTo(destFile, UpdateProgress);
                    if (_type == OperationType.MoveFile)
                    {
                        sourceFile.Delete();
                    }
                    EventsSender.SendUpdateEvent();
                }
                else if((_type == OperationType.CopyDirectory))
                {

                }

                EventsSender.Unsubscribe(this);
                Parent.Enabled = true;
            }
            if (e.Key == Keys.ESCAPE)
            {
                EventsSender.Unsubscribe(this);
                Parent.Enabled = true;
            }
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            g.FillRectangle(0xffCCB5A2, Position.X, Position.Y, Size.Width, Size.Height);
            g.DrawRectangle(0xffffffff, Position.X + NumericConstants.MarginUpLeft, Position.Y + NumericConstants.MarginUpLeft,
                Size.Width - NumericConstants.MarginRightDown, Size.Height - NumericConstants.MarginRightDown, 2);
            _progressBar.Draw(g);

            string name = Path.GetFileName(_sourcePath);
            name = name.Length > 40 ? name.Substring(0, 36).Insert(36, StringResources.LongFileNameEnd) : name;
            string text = string.Empty;
            switch (_type)
            {
                case OperationType.CopyFile:
                    text = $"Copy file: {name}";
                    break;
                case OperationType.MoveFile:
                    text = $"Move file: {name}";
                    break;
            }
            g.DrawString(text, StringResources.FontName, 0xff000000,
                Position.X + 10, Position.Y + 20, 10);

            _okButton.Draw(g);
        }

        private void UpdateProgress(int progress)
        {
            _progressBar.Progress = progress;
        }
    }
}
