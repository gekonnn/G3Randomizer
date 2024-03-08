namespace G3Archive
{
    public class G3Pak_ArchiveHeader
    {
        private readonly int magic = 0x30563347;
        public UInt32 Version;
        public UInt32 Product;
        public UInt32 Revision;
        public UInt32 Encryption;
        public UInt32 Compression;
        public UInt32 Reserved;
        public UInt64 OffsetToFiles;
        public UInt64 OffsetToFolders;
        public UInt64 OffsetToVolume;

        public long Pos_OffsetToFiles;
        public long Pos_OffsetToFolders;
        public long Pos_OffsetToVolume;

        public G3Pak_ArchiveHeader(ReadBinary Read, ref long offset)
        {
            Version = Read.UInt32(ref offset);
            Product = Read.UInt32(ref offset);

            // Check if file is a valid G3Pak archive (G3V0) 
            if (Product != magic) { throw new Exception("Specified file is not an G3Pak archive."); }

            Revision = Read.UInt32(ref offset);
            Encryption = Read.UInt32(ref offset);
            Compression = Read.UInt32(ref offset);
            Reserved = Read.UInt32(ref offset);

            Pos_OffsetToFiles = offset;
            OffsetToFiles = Read.UInt64(ref offset);
            Pos_OffsetToFolders = offset;
            OffsetToFolders = Read.UInt64(ref offset);
            Pos_OffsetToVolume = offset;
            OffsetToVolume = Read.UInt64(ref offset);
        }
    }
}
