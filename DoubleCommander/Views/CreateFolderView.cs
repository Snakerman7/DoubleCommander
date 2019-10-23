using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.Resources;
using NConsoleGraphics;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DoubleCommander.Views
{
    public class CreateFolderView : EditTextView
    {
        private readonly string _path;

        public CreateFolderView(string title, string path, Point position, View parent = null)
            : base(title, position, parent)
        {
            _path = path;
        }

        protected override void Action()
        {

            if (_okButton.Selected)
            {
                try
                {
                    string fullName = Path.Combine(_path, _textBox.Text.ToString());
                    Directory.CreateDirectory(fullName);
                    Parent?.Update();
                }
                catch (IOException ex)
                {
                    HandleException(ex);
                }
                catch (UnauthorizedAccessException ex)
                {
                    HandleException(ex);
                }
            }

            Close();
        }

        private void HandleException(Exception ex)
        {
            _ = new MessageView(ex.Message, Parent);
            Parent = null;
        }
    }
}
