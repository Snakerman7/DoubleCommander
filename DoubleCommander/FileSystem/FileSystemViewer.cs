using DoubleCommander.Resources;
using GenericCollections;
using System;
using System.IO;
using System.Linq;
using System.Security.AccessControl;

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
            foreach (var item in GetDirectoryContent())
            {
                Items.Add(item);
            }
        }

        private FileSystemEnumarable GetDirectoryContent()
        {
            if (CurrentPath != string.Empty)
            {
                return new FileSystemItem[] { new FileSystemItem(StringResources.BackPath) }
                .Concat(Directory.GetDirectories(CurrentPath)
                                .Select(path => new DirectoryInfo(path))
                                .Where(dir => CheckFolderPermission(dir.FullName))
                                .Select(dir =>
                                    new DirectoryItem(dir.FullName)))
                .Concat(Directory.GetFiles(CurrentPath)
                                .Select(path => new FileInfo(path))
                                .Select(file =>
                                    new FileItem(file.FullName, file.Length)));
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

        private static bool CheckFolderPermission(string folderPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
            try
            {
                DirectorySecurity dirAC = dirInfo.GetAccessControl(AccessControlSections.Access);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
