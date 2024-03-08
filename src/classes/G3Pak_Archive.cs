using System.Text;

namespace G3Archive
{
    public class G3Pak_Archive
    {
        public FileInfo? File;
        private long currentOffset = 0;

        private G3Pak_ArchiveHeader Header = default!;
        private ReadBinary Read = default!;

        public void ReadArchive(FileInfo file)
        {
            File = file;
            Read = new ReadBinary(new FileStream(file.FullName, FileMode.Open, FileAccess.Read));
            Header = new G3Pak_ArchiveHeader(Read, ref currentOffset);
        }

        public int Extract(string dest, List<string> ignoredExts)
        {
            currentOffset = Convert.ToInt64(Header.OffsetToFiles);
            G3Pak_FileTableEntry RootEntry = new G3Pak_FileTableEntry(Read, ref currentOffset);
            int result = RootEntry.Extract(Read, dest, ignoredExts);
            return result;
        }
    }
}
