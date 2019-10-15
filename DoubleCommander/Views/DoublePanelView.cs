using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.FileSystem;
using DoubleCommander.Resources;
using NConsoleGraphics;
using System.IO;

namespace DoubleCommander.Views
{
    public class DoublePanelView : View
    {
        private readonly ListView _leftView;
        private readonly ListView _rightView;
        private readonly HelpTextBar _helpBar;

        public DoublePanelView(Point position, Size size, View parent = null)
            : base(position, size, parent)
        {
            _leftView = new ListView(new Point(0 + NumericConstants.MarginUpLeft, 0 + NumericConstants.MarginUpLeft),
               new Size(Size.Height - NumericConstants.MarginRightDown - 20, Size.Width / 2 - NumericConstants.MarginRightDown), this);
            _rightView = new ListView(new Point(Size.Width / 2 + NumericConstants.MarginUpLeft, 0 + NumericConstants.MarginUpLeft),
                new Size(Size.Height - NumericConstants.MarginRightDown - 20, Size.Width / 2 - NumericConstants.MarginRightDown), this)
            { Enabled = false };
            _helpBar = new HelpTextBar(new Point(Position.X + 5, Size.Height - NumericConstants.MarginRightDown - 10),
                                    new Size(20, Size.Width-10));
            EventsSender.Subscribe(this);
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            if (Enabled && (_leftView.Enabled || _rightView.Enabled))
            {
                if (e.Key == Keys.TAB)
                {
                    _leftView.Enabled = !_leftView.Enabled;
                    _rightView.Enabled = !_rightView.Enabled;
                }
                if (e.Key == Keys.F1)
                {
                    CopyOperation();
                }
                if (e.Key == Keys.F2)
                {
                    //var (source, dest, name, type) = GetOperationParameters();
                    //if (dest == string.Empty || source == string.Empty)
                    //{
                    //    return;
                    //}
                    //if (type == FileSystemItemType.File)
                    //{
                    //    View activeView = _leftView.Enabled ? _leftView : _rightView;
                    //    Point viewPosition = new Point(Size.Width / 2 - 200, Size.Height / 2 - 100);
                    //    OperationView view = new OperationView(OperationType.MoveFile, Path.Combine(source, name),
                    //        Path.Combine(dest, name), viewPosition, activeView);
                    //}
                }
            }
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            _helpBar.Draw(g);
        }

        private void CopyOperation()
        {
            var (source, destPath) = GetOperationParameters();
            if (destPath == string.Empty)
            {
                return;
            }
            switch (source)
            {
                case FileItem file:
                    View activeView = _leftView.Enabled ? _leftView : _rightView;
                    Point viewPosition = new Point(Size.Width / 2 - 200, Size.Height / 2 - 100);
                    OperationView view = new OperationView(OperationType.CopyFile, file.FullName,
                        Path.Combine(destPath, file.NameWithExtension), viewPosition, activeView);
                    break;
                case DirectoryItem dir:

                    break;
            }
        }

        private (FileSystemItem source, string destPath) GetOperationParameters()
        {
            if (_leftView.Enabled)
            {
                return (_leftView.SelectedItem, _rightView.FSViewer.CurrentPath);
            }
            else
            {
                return (_rightView.SelectedItem, _leftView.FSViewer.CurrentPath);
            }
        }
    }
}
