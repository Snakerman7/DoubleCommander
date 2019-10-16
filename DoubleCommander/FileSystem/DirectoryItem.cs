using DoubleCommander.Resources;
using System.IO;

namespace DoubleCommander.FileSystem
{
    public class DirectoryItem : FileSystemItem
    {
        public override string Name => Path.GetFileName(FullName);

        public DirectoryItem(string fullName) : base(fullName)
        {
        }

        public override string ToString()
        {
            string shortName = Name.Length > 46 ? Name.Substring(0, 42) + StringResources.LongFileNameEnd : Name;
            return $"{shortName,-46}{StringResources.DirectoryMark,-6}";
        }
    }
}
