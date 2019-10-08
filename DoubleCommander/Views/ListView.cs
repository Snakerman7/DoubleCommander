using DoubleCommander.Common;
using DoubleCommander.Resources;
using GenericCollections;
using NConsoleGraphics;
using System.Linq;

namespace DoubleCommander.Views
{
    public enum MoveDirection
    {
        Up = -1,
        Down = 1
    }

    public class ListView : View
    {
        private readonly List<ListViewItem> _items = new List<ListViewItem>();
        private int _visibleItemsFirstIndex = 0;
        private readonly int _visibleItemsCount;
        public int SelectedIndex { get; private set; } = 0;
        private readonly object _locker = new object();
        private Point _itemsStartPosition;
        public FileSystemViewer FSViewer { get; } = new FileSystemViewer();

        public ListView(Point position, Size size, View parent)
            : base(position, size, parent)
        {
            _itemsStartPosition = new Point(position.X, Position.Y + NumericConstants.ListViewItemHeight);
            _visibleItemsCount = (Size.Height - NumericConstants.ListViewItemHeight * 3) / NumericConstants.ListViewItemHeight;
            var names = FSViewer.Items.ToArray();
            int count = names.Length > _visibleItemsCount ? _visibleItemsCount : names.Length;
            for (int i = 0; i < names.Length; i++)
            {
                _items.Add(new ListViewItem(this, names[i].Name));
            }
            _items[SelectedIndex].Selected = true;
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            g.FillRectangle(ColorResources.ListViewBackgroundColor, Position.X, Position.Y, Size.Width, Size.Height);
            ListViewItem[] items = null;
            lock (_locker)
            {
                items = _items.ToArray();
            }
            int lastIndex = _visibleItemsFirstIndex + _visibleItemsCount > items.Length ?
                items.Length : _visibleItemsFirstIndex + _visibleItemsCount;
            for (int i = _visibleItemsFirstIndex; i < lastIndex; i++)
            {
                items[i].Draw(g,
                    new Point(_itemsStartPosition.X + NumericConstants.MarginUpLeft, 
                    _itemsStartPosition.Y + (i - _visibleItemsFirstIndex) *
                                NumericConstants.ListViewItemHeight + NumericConstants.MarginUpLeft),
                    new Size(NumericConstants.ListViewItemHeight, Size.Width - NumericConstants.MarginRightDown));
            }
            g.DrawLine(ColorResources.AppBackground, Position.X, Position.Y + NumericConstants.ListViewItemHeight,
                                    Position.X + Size.Width, Position.Y + NumericConstants.ListViewItemHeight, 2);
            g.DrawLine(ColorResources.AppBackground, Position.X, Size.Height - NumericConstants.ListViewItemHeight,
                                    Position.X + Size.Width, Size.Height - NumericConstants.ListViewItemHeight, 2);
            g.DrawString(FSViewer.CurrentPath, StringResources.FontName, ColorResources.ListItemTextColor,
                Position.X, Position.Y, NumericConstants.FontSize);
            g.DrawString($"{SelectedIndex + 1}/{_items.Count}", StringResources.FontName, ColorResources.ListItemTextColor,
                Position.X + 22, Size.Height - NumericConstants.ListViewItemHeight + 3, NumericConstants.FontSize);
        }

        public override void OnKeyDown(Keys key)
        {
            if (Enabled)
            {
                if (key == Keys.DOWN && SelectedIndex < _items.Count - 1)
                {
                    Move(MoveDirection.Down);
                }
                else if (key == Keys.UP && SelectedIndex > 0)
                {
                    Move(MoveDirection.Up);
                }
                if (key == Keys.RETURN)
                {
                    if (FSViewer.Items[SelectedIndex].Type != FileSystemItemType.File)
                    {
                        string name = FSViewer.Items[SelectedIndex].Name;
                        FSViewer.GoToFolder(name);
                        Update();
                    }
                }
                if (key == Keys.BACK)
                {
                    if (FSViewer.CurrentPath != string.Empty)
                    {
                        string name = StringResources.BackPath;
                        FSViewer.GoToFolder(name);
                        Update();
                    }
                }
            }
        }

        public override void OnUpdate()
        {
            UpdateItems();
        }

        private void Move(MoveDirection direction)
        {
            _items[SelectedIndex].Selected = !_items[SelectedIndex].Selected;
            SelectedIndex += (int)direction;
            _items[SelectedIndex].Selected = !_items[SelectedIndex].Selected;
            if (direction == MoveDirection.Down && (SelectedIndex == _visibleItemsFirstIndex + _visibleItemsCount &&
                    _visibleItemsFirstIndex + _visibleItemsCount < _items.Count))
            {
                _visibleItemsFirstIndex++;
            }
            else if (SelectedIndex == _visibleItemsFirstIndex &&
                        _visibleItemsFirstIndex > 0)
            {
                _visibleItemsFirstIndex--;
            }
        }

        public void UpdateItems()
        {
            FSViewer.UpdateItems();
            Update();
        }

        private void Update()
        {
            lock (_locker)
            {
                _items.Clear();
                SelectedIndex = 0;
                _visibleItemsFirstIndex = 0;
                var names = FSViewer.Items.ToArray();
                for (int i = 0; i < names.Length; i++)
                {
                    string shortName = names[i].Name;
                    if (shortName.Length > NumericConstants.MaxFileNameLength)
                    {
                        shortName = shortName.Substring(0, NumericConstants.MaxFileNameLength - 4)
                            .Insert(NumericConstants.MaxFileNameLength - 4, StringResources.LongFileNameEnd);
                    }
                    _items.Add(new ListViewItem(this, $"{shortName,-46}{names[i].Extension,-6}{names[i].Size,8}"));
                }
                _items[SelectedIndex].Selected = true;
            }
        }
    }
}
