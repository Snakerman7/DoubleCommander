using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.FileSystem;
using DoubleCommander.Resources;
using GenericCollections;
using NConsoleGraphics;
using System;
using System.Diagnostics;

namespace DoubleCommander.Views
{
    public enum MoveDirection
    {
        Up = -1,
        Down = 1
    }

    public class ListView : View
    {
        private List<ListViewItem> _items = new List<ListViewItem>();
        private int _visibleItemsFirstIndex = 0;
        private readonly int _visibleItemsCount;
        public int SelectedIndex { get; private set; } = 0;
        private readonly Point _itemsStartPosition;
        public FileSystemViewer FSViewer { get; } = new FileSystemViewer();
        public FileSystemItem SelectedItem => FSViewer.Items[SelectedIndex];

        public ListView(Point position, Size size, View parent)
            : base(position, size, parent)
        {
            _itemsStartPosition = new Point(position.X, Position.Y + NumericConstants.ListViewItemHeight);
            _visibleItemsCount = (Size.Height - NumericConstants.ListViewItemHeight * 3) / NumericConstants.ListViewItemHeight;
            var names = FSViewer.Items.ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                _items.Add(new ListViewItem(this, names[i].Name));
            }
            _items[SelectedIndex].Selected = true;
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            var items = _items;
            g.FillRectangle(ColorResources.ListViewBackgroundColor, Position.X, Position.Y, Size.Width, Size.Height);
            int lastIndex = _visibleItemsFirstIndex + _visibleItemsCount > items.Count ?
                items.Count : _visibleItemsFirstIndex + _visibleItemsCount;
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
            g.DrawString($"{SelectedIndex + 1}/{this._items.Count}", StringResources.FontName, ColorResources.ListItemTextColor,
                Position.X + 22, Size.Height - NumericConstants.ListViewItemHeight + 3, NumericConstants.FontSize);
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            if (Enabled)
            {
                if (e.Key == Keys.DOWN && SelectedIndex < _items.Count - 1)
                {
                    Move(MoveDirection.Down);
                }
                else if (e.Key == Keys.UP && SelectedIndex > 0)
                {
                    Move(MoveDirection.Up);
                }
                if (e.Key == Keys.RETURN)
                {
                    ChooseCurrentItem();
                }
                if (e.Key == Keys.BACK)
                {
                    Back();
                }
                if (e.Key == Keys.F3)
                {
                    FSViewer.GoToFolder(string.Empty);
                    Update(true);
                }
                if (e.Key == Keys.F4)
                {
                    OpenPropertiesView();
                }
                if (e.Key == Keys.F5)
                {
                    OpenRenameView();
                }
                if (e.Key == Keys.F6)
                {
                    OpenCreateFolderView();
                }
            }
        }

        private void Back()
        {
            if (FSViewer.CurrentPath != string.Empty)
            {
                FSViewer.GoToFolder(StringResources.BackPath);
                Update(true);
            }
        }

        private void ChooseCurrentItem()
        {
            try
            {
                switch (SelectedItem)
                {
                    case DirectoryItem dir:
                        FSViewer.GoToFolder(dir.Name);
                        Update(true);
                        break;
                    case FileItem file:
                        Process.Start(file.FullName);
                        break;
                    case FileSystemItem item:
                        FSViewer.GoToFolder(item.Name);
                        Update(true);
                        break;
                }
            }
            catch(Exception ex)
            {
                _ = new MessageView(ex.Message, this);
                FSViewer.GoToFolder(StringResources.BackPath);
            }
        }

        private void OpenPropertiesView()
        {
            if (FSViewer.CurrentPath != string.Empty && SelectedItem.Name != StringResources.BackPath)
            {
                _ = new PropertiesView(SelectedItem,
                    new Point(Parent.Position.X + Parent.Size.Width / 2 - 250,
                              Parent.Position.Y + Parent.Size.Height / 2 - 75),
                    this);
            }
        }

        private void OpenRenameView()
        {
            if (FSViewer.CurrentPath != string.Empty && SelectedItem.Name != StringResources.BackPath)
            {
                _ = new RenameView(SelectedItem,
                    new Point(Position.X + Size.Width / 2 - 150, Position.Y + Size.Height / 2 - 100), this);
            }
        }

        private void OpenCreateFolderView()
        {
            if (FSViewer.CurrentPath != string.Empty)
            {
                _ = new CreateFolderView(FSViewer.CurrentPath,
                    new Point(Position.X + Size.Width / 2 - 150, Position.Y + Size.Height / 2 - 100), this);
            }
        }

        public override void OnUpdate()
        {
            FSViewer.UpdateItems();
            Update(false);
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

        private void Update(bool resetSelectedIndex)
        {
            List<ListViewItem> newItems = new List<ListViewItem>();
            if (resetSelectedIndex)
            {
                SelectedIndex = 0;
                _visibleItemsFirstIndex = 0;
            }
            for (int i = 0; i < FSViewer.Items.Count; i++)
            {
                newItems.Add(new ListViewItem(this, FSViewer.Items[i].ToString()));
            }
            newItems[SelectedIndex].Selected = true;
            _items = newItems;
        }
    }
}
