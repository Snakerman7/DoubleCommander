using System;
using System.IO;
using System.Threading.Tasks;

namespace DoubleCommander.FileSystem
{
    public static class FileInfoExtensions
    {
        public static void Rename(this FileInfo fileInfo, string newName)
        {
            fileInfo.MoveTo(Path.Combine(fileInfo.DirectoryName, newName));
        }

        public static void CopyTo(this FileInfo source, string target, Action<int> progressCallback)
        {
            CopyTo(source, new FileInfo(target), progressCallback);
        }

        public static void CopyTo(this FileInfo file, FileInfo destination, Action<int> progressCallback)
        {
            if (destination.Exists)
            {
                throw new IOException($"File \"{destination.FullName}\" already exists");
            }
            const int bufferSize = 1024 * 1024;  //1MB
            byte[] buffer = new byte[bufferSize], buffer2 = new byte[bufferSize];
            bool swap = false;
            int reportedProgress = 0;
            long len = file.Length;
            float flen = len;
            Task writer = null;

            using (var source = file.OpenRead())
            using (var dest = destination.OpenWrite())
            {
                try
                {
                    dest.SetLength(source.Length);
                }
                catch (IOException)
                {
                    dest.Close();
                    destination.Delete();
                    throw;
                }
                int read;
                try
                {
                    for (long size = 0; size < len; size += read)
                    {
                        int progress;
                        if ((progress = ((int)((size / flen) * 100))) != reportedProgress)
                            progressCallback(reportedProgress = progress);
                        read = source.Read(swap ? buffer : buffer2, 0, bufferSize);
                        writer?.Wait();
                        writer = dest.WriteAsync(swap ? buffer : buffer2, 0, read);
                        swap = !swap;
                    }
                    writer?.Wait();
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerException is IOException ioEx)
                    {
                        throw ioEx;
                    }
                }
            }
        }
    }
}
