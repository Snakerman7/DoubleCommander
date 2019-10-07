using DoubleCommander.Common;
using DoubleCommander.Resources;
using NConsoleGraphics;
using System.IO;

namespace DoubleCommander.Views
{
    public class DoublePanelView : View
    {
        private readonly ListView _leftView;
        private readonly ListView _rightView;
        private OperationView _operationView = null;

        public DoublePanelView(Point position, Size size, View parent = null)
            : base(position, size, parent)
        {
            _leftView = new ListView(new Point(0 + NumericConstants.MarginUpLeft, 0 + NumericConstants.MarginUpLeft),
               new Size(Size.Height - NumericConstants.MarginRightDown, Size.Width / 2 - NumericConstants.MarginRightDown), this);
            _rightView = new ListView(new Point(Size.Width / 2 + NumericConstants.MarginUpLeft, 0 + NumericConstants.MarginUpLeft),
                new Size(Size.Height - NumericConstants.MarginRightDown, Size.Width / 2 - NumericConstants.MarginRightDown), this)
            { Enabled = false };
            EventsSender.Subscribe(_leftView);
            EventsSender.Subscribe(_rightView);
            EventsSender.SubscribeToUpdateEvent(_leftView);
            EventsSender.SubscribeToUpdateEvent(_rightView);
        }

        public override void OnKeyDown(Keys key)
        {
            if (Enabled && (_leftView.Enabled || _rightView.Enabled))
            {
                if (key == Keys.TAB)
                {
                    _leftView.Enabled = !_leftView.Enabled;
                    _rightView.Enabled = !_rightView.Enabled;
                }
                if (key == Keys.F1)
                {
                    var (source, dest, name, type) = GetOperationParameters();
                    if (dest == string.Empty || source == string.Empty)
                    {
                        return;
                    }
                    if (type == FileSystemItemType.File)
                    {
                        View activeView = _leftView.Enabled ? _leftView : _rightView;
                        Point viewPosition = new Point(Size.Width / 2 - 200, Size.Height / 2 - 100);
                        _operationView = new OperationView(OperationType.CopyFile, Path.Combine(source, name),
                            Path.Combine(dest, name), viewPosition, activeView);
                        EventsSender.Subscribe(_operationView);
                    }
                }
                if (key == Keys.F2)
                {
                    var (source, dest, name, type) = GetOperationParameters();
                    if (dest == string.Empty || source == string.Empty)
                    {
                        return;
                    }
                    if (type == FileSystemItemType.File)
                    {
                        FileSystemViewer.MoveFile(source, dest, name);
                    }
                    EventsSender.SendUpdateEvent();
                }
            }
        }

        public override void OnPaint(ConsoleGraphics g)
        {

        }

        private (string source, string dest, string name, FileSystemItemType type) GetOperationParameters()
        {
            FileSystemItemType type;
            string sourcePath;
            string destPath;
            string name;
            if (_leftView.Enabled)
            {
                sourcePath = _leftView.FSViewer.CurrentPath;
                destPath = _rightView.FSViewer.CurrentPath;
                name = string.Join(".", _leftView.FSViewer.Items[_leftView.SelectedIndex].Name,
                                        _leftView.FSViewer.Items[_leftView.SelectedIndex].Extension);
                type = _leftView.FSViewer.Items[_leftView.SelectedIndex].Type;
            }
            else
            {
                sourcePath = _rightView.FSViewer.CurrentPath;
                destPath = _leftView.FSViewer.CurrentPath;
                name = string.Join(".", _rightView.FSViewer.Items[_rightView.SelectedIndex].Name,
                                        _rightView.FSViewer.Items[_rightView.SelectedIndex].Extension);
                type = _rightView.FSViewer.Items[_rightView.SelectedIndex].Type;
            }
            return (sourcePath, destPath, name, type);
        }
    }
}
