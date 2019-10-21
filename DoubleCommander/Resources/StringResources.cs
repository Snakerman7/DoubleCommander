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
        public const string FilePropertiesLabels = 
            "Name:\nParent directory:\nRoot directory:\nIs read only:\nLast read time:\nLast write time:\nSize:";
        public const string DirectoryPropertiesLabels =
            "Name:\nParent directory:\nRoot directory:\nLast read time:\nLast write time:\nSize:\nFiles:\nFolders:";
        public const string NewFolderViewTitle = "New folder";
        public const string RenameViewTitle = "Rename: ";
        public const string HelpText =
            "F1 : Copy | F2 : Cut | F3 : Paste | F4 : List of disks | F5 : Properties | F6 : Rename | F7 : New folder";
        public const string OkButtonText = "Ok";
        public const string CancelButtonText = "Cancel";
        public static readonly char[] Letters = Enumerable.Range('a', 'z' - 'a' + 1).Select(i => (char)i).ToArray();
    }
}
