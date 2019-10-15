using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleCommander.FileSystem
{
    public static class DirectoryInfoExtensions
    {
        public static void Rename(this DirectoryInfo di, string newName)
        {
            di.MoveTo(Path.Combine(di.Parent.FullName, newName));
        }

        public static void CopyTo(this DirectoryInfo source, DirectoryInfo target, bool overwiteFiles = true)
        {
            Parallel.ForEach(source.GetDirectories(), (sourceChildDirectory) =>
                CopyTo(sourceChildDirectory, new DirectoryInfo(Path.Combine(target.FullName, sourceChildDirectory.Name))));
            foreach (var sourceFile in source.GetFiles())
                sourceFile.CopyTo(Path.Combine(target.FullName, sourceFile.Name), overwiteFiles);
        }

        public static void CopyTo(this DirectoryInfo source, string target, bool overwiteFiles = true)
        {
            CopyTo(source, new DirectoryInfo(target), overwiteFiles);
        }
    }
}
