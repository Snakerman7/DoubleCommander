using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.FileSystem;
using DoubleCommander.Resources;
using NConsoleGraphics;
using System;
using System.IO;

namespace DoubleCommander.Views
{
    public class PropertiesView : View
    {
        private readonly string _labels;
        private readonly string _infoText;
        private readonly FileSystemItem _fsItem;
        private readonly Button _okButton;

        public PropertiesView(FileSystemItem fsItem, Point position, View parent = null)
            : base(position, new Size(NumericConstants.PropertiesViewHeight, NumericConstants.PropertiesViewWidth), parent)
        {
            _fsItem = fsItem;
            if (Parent != null)
            {
                Parent.Enabled = false;
            }
            _okButton = new Button(StringResources.OkButtonText, 35, 
                new Point(Position.X + Size.Width / 2 - NumericConstants.ButtonWidth / 2, Position.Y + Size.Height - 40))
            { Selected = true };
            _labels = LoadLabels();
            _infoText = LoadInfo();
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
            g.DrawString(_labels, StringResources.FontName, ColorResources.WindowTextColor, Position.X + 10, Position.Y + 20, 10);
            g.DrawString(_infoText, StringResources.FontName, ColorResources.WindowTextColor,
                Position.X + 150, Position.Y + 20, 10);
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

        private string LoadLabels()
        {
            switch (_fsItem)
            {
                case FileItem _:
                    return StringResources.FilePropertiesLabels;
                case DirectoryItem _:
                    return StringResources.DirectoryPropertiesLabels;
                default:
                    return string.Empty;
            }
        }

        private string LoadInfo()
        {
            try
            {
                switch (_fsItem)
                {
                    case FileItem file:
                        FileInfo fileInfo = new FileInfo(file.FullName);
                        return $"{file.Name}\n{Path.GetDirectoryName(file.FullName)}\n{Path.GetPathRoot(file.FullName)}" +
                            $"\n{fileInfo.IsReadOnly}\n{fileInfo.LastAccessTime}\n{fileInfo.LastWriteTime}" +
                            $"\n{FileSystemViewer.BytesToString(file.Size)}";
                    case DirectoryItem dir:
                        DirectoryInfo dirInfo = new DirectoryInfo(dir.FullName);
                        var (filesCount, dirsCount) = dirInfo.GetContentCount();
                        return $"{dir.Name}\n{Path.GetDirectoryName(dir.FullName)}\n{Path.GetPathRoot(dir.FullName)}" +
                            $"\n{dirInfo.LastAccessTime}\n{dirInfo.LastWriteTime}" +
                            $"\n{FileSystemViewer.BytesToString(dirInfo.GetDirectorySize())}\n{filesCount}\n{dirsCount}";
                }
            }
            catch (Exception ex)
            {
                Close();
                _ = new MessageView(ex.Message, Parent);
            }
            return string.Empty;
        }
    }
}
