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
                UpdateItems();
            }
            else
            {
                string path = Path.Combine(CurrentPath, name);
                CurrentPath = path;
                UpdateItems();
            }
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
    }
}
