namespace DoubleCommander
{
    public enum FileSystemItemType
    {
        Drive,
        Directory,
        File
    }

    public class FileSystemItem
    {
        public string Name { get; }
        public string Size { get; }
        public string Extension { get; }
        public FileSystemItemType Type { get; }

        public FileSystemItem(string name, FileSystemItemType type, string size = "", string extension = "")
        {
            Name = name;
            Type = type;
            Size = size;
            Extension = extension;
        }
    }
}
