using System.IO;

namespace DoubleCommander.FileSystem
{
    public class FileSystemItem
    {
        public string FullName { get; }

        public virtual string Name { get => FullName; }

        public FileSystemItem(string fullName)
        {
            FullName = fullName;
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}
