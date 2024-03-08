using System.IO.Compression;

namespace G3Archive
{
    public class G3Pak_FileTableEntry
    {
        public G3Pak_FileTableEntry_Header Header;
        public G3Pak_DirectoryEntry DirectoryEntry = default!;
        public G3Pak_FileEntry FileEntry = default!;

        public G3Pak_FileTableEntry(ReadBinary Read, ref long offset)
        {
            Header = new G3Pak_FileTableEntry_Header(Read, ref offset);
            if (Header.Attributes == 16)
            {
                DirectoryEntry = new G3Pak_DirectoryEntry(Read, ref offset);
            }
            else
            {
                FileEntry = new G3Pak_FileEntry(Read, ref offset);
            }
        }

        public int Extract(ReadBinary Read, string dest, List<String> ignoredExts)
        {
            Directory.CreateDirectory(dest);
            
            for (int i = 0; i < DirectoryEntry.DirCount; i++)
            {
                G3Pak_FileTableEntry _fileTableEntry = DirectoryEntry.DirTable[i];
                string FileName = string.Join("", _fileTableEntry.DirectoryEntry.FileName.Data);
                Directory.CreateDirectory(Path.Combine(dest, FileName));
                DirectoryEntry.DirTable[i].Extract(Read, dest, ignoredExts); // Extract child folders
            }
            for (int i = 0; i < DirectoryEntry.FileCount; i++)
            {
                G3Pak_FileTableEntry _fileTableEntry = DirectoryEntry.FileTable[i];
                string FileName = string.Join("", _fileTableEntry.FileEntry.FileName.Data);

                if(ignoredExts.Contains(Path.GetExtension(FileName))) { continue; }

                int rawData_Bytes = (int)_fileTableEntry.FileEntry.Bytes;
                byte[] rawData = Read.Bytes(Convert.ToInt64(_fileTableEntry.FileEntry.Offset), rawData_Bytes);
                
                using (FileStream _fs = new FileStream(Path.Combine(dest, FileName), FileMode.OpenOrCreate))
                {
                    _fs.Write(rawData);
                    _fs.Flush();
                }
            }
            return 0;
        }
    }
}
