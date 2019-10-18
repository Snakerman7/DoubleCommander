using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.FileSystem;
using DoubleCommander.Resources;
using NConsoleGraphics;
using System;
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
        private readonly Button _okButton;
        private readonly Button _cancelButton;
        private readonly string _sourcePath;
        private readonly string _destPath;

        public OperationView(OperationType type, string source, string dest, Point position, View parent = null)
            : base(position, new Size(NumericConstants.OperationViewHeight, NumericConstants.OperationViewWidth), parent)
        {
            _type = type;
            _sourcePath = source;
            _destPath = dest;
            if (Parent != null)
                parent.Enabled = false;
            _progressBar = new ProgressBar(new Point(Position.X + 10, Position.Y + 60), new Size(30, Size.Width - 20));
            _okButton = new Button(StringResources.OkButtonText, NumericConstants.ButtonOkTextAlign, 
                new Point(Position.X + 50, Position.Y + 150)) { Selected = true };
            _cancelButton = new Button(StringResources.CancelButtonText, NumericConstants.ButtonCancelTextAlign, 
                new Point(Position.X + Size.Width - 150, Position.Y + 150));
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Keys.RETURN)
            {
                if (_okButton.Selected)
                    Action();
                Close();
            }
            if (e.Key == Keys.LEFT || e.Key == Keys.RIGHT)
            {
                ChangeSelectedButton();
            }
            if (e.Key == Keys.ESCAPE)
            {
                Close();
            }
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            g.FillRectangle(ColorResources.WindowBackgroundColor, Position.X, Position.Y, Size.Width, Size.Height);
            g.DrawRectangle(ColorResources.WindowBorderColor, Position.X + NumericConstants.MarginUpLeft,
                Position.Y + NumericConstants.MarginUpLeft, Size.Width - NumericConstants.MarginRightDown,
                Size.Height - NumericConstants.MarginRightDown, NumericConstants.WindowBorderThikness);
            _progressBar.Draw(g);
            string name = Path.GetFileName(_sourcePath);
            name = name.Length > 40 ? name.Substring(0, 36).Insert(36, StringResources.LongFileNameEnd) : name;
            string text = string.Empty;
            switch (_type)
            {
                case OperationType.CopyFile:
                    text = $"Copy file:\n{name}";
                    break;
                case OperationType.MoveFile:
                    text = $"Move file:\n{name}";
                    break;
                case OperationType.CopyDirectory:
                    text = $"Copy directory:\n{name}";
                    break;
                case OperationType.MoveDirectory:
                    text = $"Move directory:\n{name}";
                    break;
            }
            g.DrawString(text, StringResources.FontName, 0xff000000,
                Position.X + 10, Position.Y + 20, 10);
            _okButton.Draw(g);
            _cancelButton.Draw(g);
        }

        private void Action()
        {
            switch (_type)
            {
                case OperationType.CopyFile:
                    CopyFile();
                    break;
                case OperationType.MoveFile:
                    MoveFile();
                    break;
                case OperationType.CopyDirectory:
                    CopyDirectory();
                    break;
                case OperationType.MoveDirectory:
                    MoveDirectory();
                    break;
            }
            EventsSender.SendUpdateEvent();
        }

        private void MoveDirectory()
        {
            try
            {
                if (Path.GetPathRoot(_sourcePath) == Path.GetPathRoot(_destPath))
                {
                    Directory.Move(_sourcePath, _destPath);
                    UpdateProgress(100);
                }
                else
                {
                    CopyDirectory();
                    Directory.Delete(_sourcePath, true);
                }
            }
            catch (Exception ex)
            {
                _ = new MessageView(ex.Message, Parent);
                Parent = null;
            }
        }

        private void CopyDirectory()
        {
            try
            {
                DirectoryInfo sourceDir = new DirectoryInfo(_sourcePath);
                sourceDir.CopyTo(_destPath, UpdateProgress);
            }
            catch (Exception ex)
            {
                _ = new MessageView(ex.Message, Parent);
                Parent = null;
            }
        }

        private void CopyFile()
        {
            try
            {
                if (!File.Exists(_destPath))
                {
                    FileInfo sourceFile = new FileInfo(_sourcePath);
                    sourceFile.CopyTo(_destPath, UpdateProgress);
                }
            }
            catch (Exception ex)
            {
                _ = new MessageView(ex.Message, Parent);
                Parent = null;
            }
        }

        private void MoveFile()
        {
            try
            {
                if (Path.GetPathRoot(_sourcePath) == Path.GetPathRoot(_destPath))
                {
                    if (!File.Exists(_destPath))
                    {
                        File.Move(_sourcePath, _destPath);
                        UpdateProgress(100);
                    }
                }
                else
                {
                    CopyFile();
                    File.Delete(_sourcePath);
                }
            }
            catch(Exception ex)
            {
                _ = new MessageView(ex.Message, Parent);
                Parent = null;
            }
        }

        private void ChangeSelectedButton()
        {
            _okButton.Selected = !_okButton.Selected;
            _cancelButton.Selected = !_cancelButton.Selected;
        }

        private void UpdateProgress(int progress)
        {
            _progressBar.Progress = progress;
        }

        public override void Close()
        {
            base.Close();
            if (Parent != null)
                Parent.Enabled = true;
        }
    }
}
