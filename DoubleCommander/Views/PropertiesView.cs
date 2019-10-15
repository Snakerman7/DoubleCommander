using DoubleCommander.Common;
using DoubleCommander.Core;
using DoubleCommander.FileSystem;
using NConsoleGraphics;

namespace DoubleCommander.Views
{
    public class PropertiesView : View
    {
        private readonly string _text;
        private readonly FileSystemItem _fsItem;

        public PropertiesView(FileSystemItem fsItem, Point position, Size size, View parent = null) 
            : base(position, size, parent)
        {
            _fsItem = fsItem;
        }

        public override void OnKeyDown(KeyEventArgs e)
        {
            
        }

        public override void OnPaint(ConsoleGraphics g)
        {
            
        }
    }
}
