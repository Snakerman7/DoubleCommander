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
            return $"{shortName,-46}{Extension,-6}{BytesToString(Size),8}";
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
    }
}
