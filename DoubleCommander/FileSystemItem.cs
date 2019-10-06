using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCommander
{
    public class FileSystemItem
    {
        public string Name { get; }
        public string Size { get; }
        public string Extension { get; }

        public FileSystemItem(string name, string size, string extension)
        {
            Name = name;
            Size = size;
            Extension = extension;
        }
    }
}
