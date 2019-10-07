using DoubleCommander.Resources;
using GenericCollections;
using System;
using System.IO;
using System.Linq;

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
            UpdateItems();
        }

        public void UpdateItems()
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
                return new FileSystemItem[] { new FileSystemItem(StringResources.BackPath, FileSystemItemType.Directory) }
                .Concat(Directory.GetDirectories(CurrentPath)
                                .Select(path => new DirectoryInfo(path))
                                .Select(dir => new FileSystemItem(dir.Name, FileSystemItemType.Directory,string.Empty, StringResources.DirectoryMark)))
                .Concat(Directory.GetFiles(CurrentPath)
                                .Select(path => new FileInfo(path))
                                .Select(file => new FileSystemItem(Path.GetFileNameWithoutExtension(file.Name), FileSystemItemType.File, BytesToString(file.Length), file.Extension.TrimStart('.'))));
            }
            else
            {
                return DriveInfo.GetDrives().Where(d => d.IsReady).Select(d => new FileSystemItem(d.Name, FileSystemItemType.Drive));
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
                UpdateItems();
            }
            else
            {
                string path = Path.Combine(CurrentPath, name);
                var access = Directory.GetAccessControl(path);
                if (!access.AreAuditRulesProtected)
                {
                    CurrentPath = path;
                    UpdateItems();
                }
            }
        }

        private static string BytesToString(long byteCount)
        {
            if (byteCount == 0)
                return "0" + StringResources.FileSizeUnits[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + StringResources.FileSizeUnits[place];
        }

        public static string CheckFile(string filePath)
        {
            string name = Path.GetFileName(filePath);
            string path = Path.GetDirectoryName(filePath);
            int i = 1;
            while (File.Exists(filePath))
            {
                filePath = Path.Combine(path, name.Insert(name.LastIndexOf('.'), $"(Copy{i++})"));
            }
            return filePath;
        }

        public static void MoveFile(string sourcePath, string destPath, string fileName)
        {
            string sourceFileName = Path.Combine(sourcePath, fileName);
            string destFileName = Path.Combine(destPath, fileName);
            int i = 1;
            while (File.Exists(destFileName))
            {
                destFileName = Path.Combine(destPath, fileName.Insert(fileName.IndexOf('.'), $"(Copy{i++})"));
            }
            File.Move(sourceFileName, destFileName);
        }
    }
}
