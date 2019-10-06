using System;
using System.Threading;
using GenericCollections;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class ListView : View
    {
        private readonly List<ListViewItem> _items = new List<ListViewItem>();
        private int SelectedIndex = 0;
        private readonly object _locker = new object();
        private readonly FileSystemViewer _provider;


        public ListView(FileSystemViewer provider)
        {
            _provider = provider;
            var names = _provider.GetDirectoryContent();
            for (int i = 0; i < names.Length; i++)
            {
                _items.Add(new ListViewItem(this, names[i]));
            }
            _items[SelectedIndex].Selected = true;
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            g.FillRectangle(0xff0000aa, Position.X, Position.Y, Size.Width, Size.Height);
            ListViewItem[] items = null;
            lock (_locker)
            {
                items = _items.ToArray();
            }
            for (int i = 0; i < items.Length; i++)
            {
                items[i].Draw(g, new Point(Position.X + 5, Position.Y + i * 15 + 5), new Size(15, Size.Width - 10));
            }
        }

        public override void OnKeyDown(Keys key)
        {
            if (Enabled)
            {
                if (key == Keys.DOWN && SelectedIndex < _items.Count - 1)
                {
                    _items[SelectedIndex].Selected = false;
                    SelectedIndex++;
                    _items[SelectedIndex].Selected = true;
                }
                else if (key == Keys.UP && SelectedIndex > 0)
                {
                    _items[SelectedIndex].Selected = false;
                    SelectedIndex--;
                    _items[SelectedIndex].Selected = true;
                }
                if (key == Keys.RETURN)
                {
                    lock (_locker)
                    {
                        string name = _items[SelectedIndex].Text;
                        _provider.GoToFolder(name);
                        _items.Clear();
                        SelectedIndex = 0;
                        var names = _provider.GetDirectoryContent();
                        for (int i = 0; i < names.Length; i++)
                        {
                            _items.Add(new ListViewItem(this, names[i]));
                        }
                        _items[SelectedIndex].Selected = true;
                    }
                }
                if(key == Keys.BACK)
                {
                    if (_provider.CurrentPath != string.Empty)
                    {
                        lock (_locker)
                        {
                            string name = "..";
                            _provider.GoToFolder(name);
                            _items.Clear();
                            SelectedIndex = 0;
                            var names = _provider.GetDirectoryContent();
                            for (int i = 0; i < names.Length; i++)
                            {
                                _items.Add(new ListViewItem(this, names[i]));
                            }
                            _items[SelectedIndex].Selected = true;
                        }
                    }
                }
            }
        }
    }
}
