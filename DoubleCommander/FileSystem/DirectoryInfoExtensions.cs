using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DoubleCommander.FileSystem
{
    public static class DirectoryInfoExtensions
    {
        private static long _totalDirSize = 0;
        private static long _completeSize = 0;
        private static long _currentFileSize = 0;

        public static void Rename(this DirectoryInfo di, string newName)
        {
            di.MoveTo(Path.Combine(di.Parent.FullName, newName));
        }

        private static void CopyTo(this DirectoryInfo source, DirectoryInfo target,
            Action<int> progressCallback, bool overwiteFiles = true)
        {
            target.Create();
            Parallel.ForEach(source.GetDirectories(), (sourceChildDirectory) =>
                CopyTo(sourceChildDirectory, new DirectoryInfo(Path.Combine(target.FullName, sourceChildDirectory.Name)), progressCallback));
            foreach (var sourceFile in source.GetFiles())
            {
                _currentFileSize = sourceFile.Length;
                sourceFile.CopyTo(Path.Combine(target.FullName, sourceFile.Name), UpdateProgress);
                _completeSize += _currentFileSize;
            }

            void UpdateProgress(int i)
            {
                int progress = (int)(((_completeSize + (_currentFileSize * (i / 100.0))) / _totalDirSize) * 100);
                progressCallback(progress);
            }
        }

        public static void CopyTo(this DirectoryInfo source, string target, Action<int> progressCallback, bool overwiteFiles = true)
        {
            _totalDirSize = source.GetDirectorySize();
            CopyTo(source, new DirectoryInfo(target), progressCallback, overwiteFiles);
        }

        public static long GetDirectorySize(this DirectoryInfo directoryInfo, bool recursive = true)
        {
            var startDirectorySize = default(long);
            if (directoryInfo == null || !directoryInfo.Exists)
                return startDirectorySize;

            foreach (var fileInfo in directoryInfo.GetFiles())
                Interlocked.Add(ref startDirectorySize, fileInfo.Length);

            if (recursive)
                Parallel.ForEach(directoryInfo.GetDirectories(), (subDirectory) =>
            Interlocked.Add(ref startDirectorySize, GetDirectorySize(subDirectory, recursive)));

            return startDirectorySize;
        }

        public static (int filesCount, int dirsCount) GetContentCount(this DirectoryInfo directoryInfo, bool recursive = true)
        {
            var filesCount = default(int);
            var dirsCount = default(int);
            if (directoryInfo == null || !directoryInfo.Exists)
                return (filesCount, dirsCount);

            filesCount += directoryInfo.GetFiles().Length;
            dirsCount += directoryInfo.GetDirectories().Length;

            if (recursive)
            {
                Parallel.ForEach(directoryInfo.GetDirectories(), (subDirectory) =>
                {
                    var (innerFilesCount, innerDirsCount) = subDirectory.GetContentCount();
                    Interlocked.Add(ref filesCount, innerFilesCount);
                    Interlocked.Add(ref dirsCount, innerDirsCount);
                });
            }

            return (filesCount, dirsCount);
        }
    }
}
