using System.Linq;
using System.IO;
using GenericCollections;

namespace DoubleCommander
{
    using FileSystemEnumarable = System.Collections.Generic.IEnumerable<FileSystemItem>;

    public class FileSystemViewer
    {
        public string CurrentPath { get; private set; }
        public List<FileSystemItem> Items { get; } = new List<FileSystemItem>();

        public FileSystemViewer()
        {
            CurrentPath = string.Empty;
        }

        private void UpdateItems()
        {
            Items.Clear();
            foreach(var item in GetDirectoryContent())
            {
                Items.Add(item);
            }
        }

        private FileSystemEnumarable GetDirectoryContent()
        {
            if (CurrentPath != string.Empty)
            {
                return new FileSystemItem[] { new FileSystemItem("..", "","") }
                .Concat(Directory.GetDirectories(CurrentPath)
                                .Select(path => new DirectoryInfo(path))
                                .Select(dir => new FileSystemItem(dir.Name, "", "<DIR>")))
                .Concat(Directory.GetFiles(CurrentPath)
                                .Select(path => new FileInfo(path))
                                .Select(file => new FileSystemItem(file.Name, file.Length.ToString("N"), file.Extension)));
            }
            else
            {
                return DriveInfo.GetDrives().Where(d => d.IsReady).Select(d => new FileSystemItem(d.Name, "", ""));
            }
        }

        public void GoToFolder(string name)
        {
            if (name == "..")
            {
                if (Path.GetPathRoot(CurrentPath) != CurrentPath)
                {
                    CurrentPath = Path.GetDirectoryName(CurrentPath);
                }
                else
                {
                    CurrentPath = string.Empty;
                }
            }
            else
            {
                CurrentPath = Path.Combine(CurrentPath, name);
            }
            UpdateItems();
        }
    }
}
