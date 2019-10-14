using NConsoleGraphics;
using System.Linq;

namespace DoubleCommander.Resources
{
    public static class StringResources
    {
        public static readonly string[] FileSizeUnits = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
        public const string FontName = "Consolas";
        public const string BackPath = "..";
        public const string LongFileNameEnd = "[..]";
        public const string DirectoryMark = "<DIR>";

        public static readonly char[] Letters = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (char)i).ToArray();
    }
}
