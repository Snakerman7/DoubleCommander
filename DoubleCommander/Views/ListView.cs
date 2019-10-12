﻿using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.FileSystem;
using DoubleCommander.Resources;
using GenericCollections;
using NConsoleGraphics;
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
        private readonly object _locker = new object();
        private Point _itemsStartPosition;
        public FileSystemViewer FSViewer { get; } = new FileSystemViewer();
        public FileSystemItem SelectedItem => FSViewer.Items[SelectedIndex];

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
            EventsSender.Subscribe(this);
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            g.FillRectangle(ColorResources.ListViewBackgroundColor, Position.X, Position.Y, Size.Width, Size.Height);
            lock (_locker)
            {
                int lastIndex = _visibleItemsFirstIndex + _visibleItemsCount > _items.Count ?
                    _items.Count : _visibleItemsFirstIndex + _visibleItemsCount;
                for (int i = _visibleItemsFirstIndex; i < lastIndex; i++)
                {
                    _items[i].Draw(g,
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
                    switch (FSViewer.Items[SelectedIndex])
                    {
                        case DirectoryItem dir:
                            FSViewer.GoToFolder(dir.Name);
                            Update();
                            break;
                        case FileItem file:
                            Process.Start(file.FullName);
                            break;
                        case FileSystemItem item:
                            FSViewer.GoToFolder(item.Name);
                            Update();
                            break;
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
            List<ListViewItem> newItems = new List<ListViewItem>();
            SelectedIndex = 0;
            _visibleItemsFirstIndex = 0;
            for (int i = 0; i < FSViewer.Items.Count; i++)
            {
                string shortName = FSViewer.Items[i].Name;
                if (shortName.Length > NumericConstants.MaxFileNameLength)
                {
                    shortName = shortName.Substring(0, NumericConstants.MaxFileNameLength - 4)
                        .Insert(NumericConstants.MaxFileNameLength - 4, StringResources.LongFileNameEnd);
                }
                newItems.Add(new ListViewItem(this, FSViewer.Items[i].ToString()));
            }
            newItems[SelectedIndex].Selected = true;
            lock (_locker)
            {
                _items = newItems;
            }
        }
    }
}