using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DoubleCommander.Resources;

namespace DoubleCommander.FileSystem
{
    public class FileItem : FileSystemItem
    {
        public override string Name => Path.GetFileNameWithoutExtension(FullName);
        public string NameWithExtension => Path.GetFileName(FullName);
        public long Size { get; }
        public string Extension { get => Path.GetExtension(FullName).TrimStart('.'); }

        public FileItem(string fullName, long size)
            :base(fullName)
        {
            Size = size;
        }

        public override string ToString()
        {
            string shortName = Name.Length > 46 ? Name.Substring(0, 42) + StringResources.LongFileNameEnd : Name;
            return $"{shortName,-46}{Extension,-6}{FileSystemViewer.BytesToString(Size),8}";
        }
    }
}
