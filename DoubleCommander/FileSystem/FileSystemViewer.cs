using DoubleCommander.Resources;
using GenericCollections;
using System;
using System.IO;
using System.Linq;

namespace DoubleCommander.FileSystem
{
    using FileSystemEnumarable = System.Collections.Generic.IEnumerable<FileSystemItem>;

    public class FileSystemViewer
    {
        public string CurrentPath { get; private set; }
        public List<FileSystemItem> Items { get; } = new List<FileSystemItem>();

        public FileSystemViewer()
        {
            CurrentPath = string.Empty;
            UpdateItems();
        }

        public void UpdateItems()
        {
            Items.Clear();
            if (CurrentPath != string.Empty)
                Items.Add(new FileSystemItem(StringResources.BackPath));
            try
            {
                foreach (var item in GetDirectoryContent())
                {
                    Items.Add(item);
                }
            }
            catch (IOException) { }
            catch (System.Security.SecurityException) { }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
        }

        private FileSystemEnumarable GetDirectoryContent()
        {
            if (CurrentPath != string.Empty)
            {
                return Directory.EnumerateDirectories(CurrentPath).Select<string, FileSystemItem>(dirPath => new DirectoryItem(dirPath))
                    .Concat(Directory.EnumerateFiles(CurrentPath).Select(filePath => new FileItem(filePath, new FileInfo(filePath).Length)));
            }
            else
            {
                return DriveInfo.GetDrives().Where(d => d.IsReady).Select(d => new FileSystemItem(d.Name));
            }
        }

        public void GoToFolder(string name)
        {
            if (name == StringResources.BackPath)
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
            else if (name == string.Empty)
            {
                CurrentPath = name;
            }
            else
            {
                string path = Path.Combine(CurrentPath, name);
                CurrentPath = path;
            }
            UpdateItems();
        }

        public static string BytesToString(long byteCount)
        {
            if (byteCount == 0)
                return "0" + StringResources.FileSizeUnits[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + StringResources.FileSizeUnits[place];
        }

    }
}
