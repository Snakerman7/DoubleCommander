using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.FileSystem;
using DoubleCommander.Resources;
using NConsoleGraphics;
using System.Linq;
using System.IO;
using System;

namespace DoubleCommander.Views
{
    public class RenameView : EditTextView
    {
        private readonly FileSystemItem _fsItem;

        public RenameView(string title, FileSystemItem fsItem, Point position, View parent = null)
            : base(title + fsItem.Name, position, parent)
        {
            _fsItem = fsItem;
            _textBox.Text = _fsItem.Name;
        }

        protected override void Action()
        {
            if (_okButton.Selected)
            {
                try
                {
                    switch (_fsItem)
                    {
                        case FileItem file:
                            FileInfo fileInfo = new FileInfo(file.FullName);
                            fileInfo.Rename(_textBox.Text + "." + file.Extension);
                            break;
                        case DirectoryItem dir:
                            DirectoryInfo dirInfo = new DirectoryInfo(dir.FullName);
                            dirInfo.Rename(_textBox.Text);
                            break;
                    }
                }
                catch(Exception ex)
                {
                    _ = new MessageView(ex.Message, Parent);
                    Parent = null;
                }
                Parent?.Update();
            }
            Close();
        }
    }
}
