using DoubleCommander.Common;
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
        private readonly string _sourcePath;
        private string _destPath;
        private bool _doWork = false;

        public OperationView(OperationType type, string source, string dest, Point position, View parent = null)
            : base(position, new Size(200, 400), parent)
        {
            _type = type;
            _sourcePath = source;
            _destPath = dest;
            parent.Enabled = false;
            _progressBar = new ProgressBar(new Point(Position.X + 10, Position.Y + 50), new Size(30, Size.Width - 20));
        }

        public override void OnKeyDown(Keys key)
        {
            if (key == Keys.RETURN)
            {
                if ((_type == OperationType.CopyFile || _type == OperationType.MoveFile) && _doWork == true)
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
                EventsSender.Unsubscribe(this);
                Parent.Enabled = true;
            }
            if (key == Keys.LEFT || key == Keys.RIGHT)
            {
                _doWork = !_doWork;
            }
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            g.FillRectangle(0xffe9d8b5, Position.X, Position.Y, Size.Width, Size.Height);
            g.DrawRectangle(0xffffffff, Position.X + NumericConstants.MarginUpLeft, Position.Y + NumericConstants.MarginUpLeft,
                Size.Width - NumericConstants.MarginRightDown, Size.Height - NumericConstants.MarginRightDown, 2);
            _progressBar.Draw(g);

            string name;
            switch (_type)
            {
                case OperationType.CopyFile:
                    name = Path.GetFileName(_sourcePath);
                    name = name.Length > 40 ? name.Substring(0, 36).Insert(36, StringResources.LongFileNameEnd) : name;
                    g.DrawString($"Copy file: {name}", StringResources.FontName, 0xff000000,
                        Position.X + 10, Position.Y + 20, 10);
                    break;
                case OperationType.MoveFile:
                    name = Path.GetFileName(_sourcePath);
                    name = name.Length > 40 ? name.Substring(0, 36).Insert(36, StringResources.LongFileNameEnd) : name;
                    g.DrawString($"Move file: {name}", StringResources.FontName, 0xff000000,
                        Position.X + 10, Position.Y + 20, 10);
                    break;
            }
            if (_doWork)
            {
                g.FillRectangle(0xff9E9A8F, Position.X + 30, Position.Y + 150, 100, 25);
            }
            else
            {
                g.FillRectangle(0xff9E9A8F, Position.X + Size.Width - 130, Position.Y + 150, 100, 25);
            }
            g.DrawString("Yes", StringResources.FontName, 0xff000000, Position.X + 55, Position.Y + 148, 16);
            g.DrawString("No", StringResources.FontName, 0xff000000, Position.X + Size.Width - 100, Position.Y + 148, 16);
        }

        private void UpdateProgress(int progress)
        {
            _progressBar.Progress = progress;
        }
    }
}
