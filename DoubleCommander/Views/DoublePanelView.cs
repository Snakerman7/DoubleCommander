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
        private enum Operation { Copy, Move, None }

        private readonly ListView _leftView;
        private readonly ListView _rightView;
        private readonly HelpTextBar _helpBar;
        private Operation _operation = Operation.None;
        private FileSystemItem _operationSourceItem;

        public DoublePanelView(Point position, Size size, View parent = null)
            : base(position, size, parent)
        {
            _leftView = new ListView(new Point(0 + NumericConstants.MarginUpLeft, 0 + NumericConstants.MarginUpLeft),
               new Size(Size.Height - NumericConstants.MarginRightDown - 20, Size.Width / 2 - NumericConstants.MarginRightDown),
               this);
            _rightView = new ListView(new Point(Size.Width / 2 + NumericConstants.MarginUpLeft, 0 + NumericConstants.MarginUpLeft),
                new Size(Size.Height - NumericConstants.MarginRightDown - 20, Size.Width / 2 - NumericConstants.MarginRightDown),
                this)
            { Enabled = false };
            _helpBar = new HelpTextBar(new Point(Position.X + 5, Size.Height - NumericConstants.MarginRightDown - 10),
                                       new Size(20, Size.Width - 10));
        }

        public override void OnKeyDown(KeyDownEventArgs e)
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
                    _operationSourceItem = GetSourceItem();
                    if (_operationSourceItem != null)
                        _operation = Operation.Copy;
                }
                if (e.Key == Keys.F2)
                {
                    _operationSourceItem = GetSourceItem();
                    if (_operationSourceItem != null)
                        _operation = Operation.Move;
                }
                if(e.Key == Keys.F3)
                {
                    switch (_operation)
                    {
                        case Operation.Copy:
                            StartOperation();
                            break;
                        case Operation.Move:
                            StartOperation();
                            _operation = Operation.None;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public override void OnPaint(PaintEventArgs e)
        {
            _helpBar.Draw(e.Graphics);
        }

        private void StartOperation()
        {
            string destPath = GetDestinationPath();
            if (destPath == string.Empty)
            {
                return;
            }
            View activeView = _leftView.Enabled ? _leftView : _rightView;
            Point viewPosition = new Point(Size.Width / 2 - NumericConstants.OperationViewWidth / 2,
                                           Size.Height / 2 - NumericConstants.OperationViewHeight / 2);
            switch (_operationSourceItem)
            {
                case FileItem file:
                    OperationView.OperationType fileOperationType = _operation == Operation.Copy ?
                        OperationView.OperationType.CopyFile : OperationView.OperationType.MoveFile;
                    _ = new OperationView(fileOperationType, file.FullName,
                        Path.Combine(destPath, file.NameWithExtension), viewPosition, activeView);
                    break;
                case DirectoryItem dir:
                    OperationView.OperationType dirOperationType = _operation == Operation.Copy ?
                        OperationView.OperationType.CopyDirectory : OperationView.OperationType.MoveDirectory;
                    _ = new OperationView(dirOperationType, dir.FullName,
                        Path.Combine(destPath, dir.Name), viewPosition, activeView);
                    break;
            }
        }

        private FileSystemItem GetSourceItem()
        {
            if (_leftView.Enabled)
            {
                return CheckSourceItem(_leftView.SelectedItem);
            }
            else
            {
                return CheckSourceItem(_rightView.SelectedItem);
            }
        }

        private string GetDestinationPath()
        {
            if (_leftView.Enabled)
            {
                return _leftView.FSViewer.CurrentPath;
            }
            else
            {
                return _rightView.FSViewer.CurrentPath;
            }
        }

        private static FileSystemItem CheckSourceItem(FileSystemItem item)
        {
            if(item is FileItem || item is DirectoryItem)
            {
                return item;
            }
            else
            {
                return null;
            }
        }

        public override void Update()
        {
            base.Update();
            _leftView.Update();
            _rightView.Update();
        }
    }
}
