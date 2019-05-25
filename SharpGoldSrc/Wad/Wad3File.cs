using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace SharpGoldSrc.Wad
{
    public class Wad3File
    {
        private WadHeader _header;

        public DirectoryEntry[] Entries { get; private set; }
        public MipTexture[] Textures { get; private set; }

        private Wad3File()
        {
            _header = new WadHeader();
        }

        public static Wad3File FromStream(Stream stream)
        {
            using (var streamReader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                var wadFile = new Wad3File();
                wadFile.ReadHeader(streamReader);

                if (!wadFile._header.IsValidHeader)
                {
                    throw new NotSupportedException("Stream is not a valid WAD3 file.");
                }

                wadFile.ReadDirectoryEntries(streamReader);
                wadFile.ReadMipTextures(streamReader);

                return wadFile;
            }

            return null;
        }

        private void ReadHeader(BinaryReader binaryReader)
        {
            _header.MagicNumber = binaryReader.ReadBytes(4);
            _header.DirectoryEntryCount = binaryReader.ReadInt32();
            _header.DirectoryEntryOffset = binaryReader.ReadInt32();
        }

        private void ReadDirectoryEntries(BinaryReader binaryReader)
        {
            binaryReader.BaseStream.Seek(_header.DirectoryEntryOffset, SeekOrigin.Begin);

            var directoryEntries = new DirectoryEntry[_header.DirectoryEntryCount];

            for (int i = 0; i < _header.DirectoryEntryCount; i++)
            {
                var newEntry = new DirectoryEntry();
                newEntry.FileOffset = binaryReader.ReadInt32();
                newEntry.CompressedSize = binaryReader.ReadInt32();
                newEntry.Size = binaryReader.ReadInt32();
                newEntry.Type = binaryReader.ReadByte();
                newEntry.IsCompressed = binaryReader.ReadBoolean();
                newEntry._dummy = binaryReader.ReadInt16();
                newEntry.Name = binaryReader.ReadBytes(DirectoryEntry.TEXTURE_NAME_MAX_LENGTH);

                directoryEntries[i] = newEntry;
            }

            Entries = directoryEntries;
        }

        private void ReadMipTextures(BinaryReader binaryReader)
        {
            var mipTextures = new MipTexture[_header.DirectoryEntryCount];

            for (int i = 0; i < _header.DirectoryEntryCount; i++)
            {
                var currentEntry = Entries[i];

                binaryReader.BaseStream.Seek(currentEntry.FileOffset, SeekOrigin.Begin);

                var newMipTexture = new MipTexture();
                newMipTexture.Name = binaryReader.ReadBytes(MipTexture.TEXTURE_NAME_MAX_LENGTH);
                newMipTexture.Width = binaryReader.ReadUInt32();
                newMipTexture.Height = binaryReader.ReadUInt32();
                newMipTexture.DataOffsets = new UInt32[MipTexture.MIPMAP_LEVEL_COUNT];
                newMipTexture.MipData = new byte[MipTexture.MIPMAP_LEVEL_COUNT][];
                newMipTexture.Palette = new Color[MipTexture.PALETTE_ENTRY_COUNT];

                for (int j = 0; j < MipTexture.MIPMAP_LEVEL_COUNT; j++)
                {
                    newMipTexture.DataOffsets[j] = binaryReader.ReadUInt32();
                }

                var mipDataSize = (int)(newMipTexture.Width * newMipTexture.Height);

                for (int j = 0; j < MipTexture.MIPMAP_LEVEL_COUNT; j++)
                {
                    binaryReader.BaseStream.Seek(currentEntry.FileOffset + newMipTexture.DataOffsets[j], SeekOrigin.Begin);

                    newMipTexture.MipData[j] = binaryReader.ReadBytes(mipDataSize);

                    mipDataSize /= 4;
                }

                newMipTexture._dummy = binaryReader.ReadInt16();

                for (int j = 0; j < MipTexture.PALETTE_ENTRY_COUNT; j++)
                {
                    var colorData = binaryReader.ReadBytes(3);

                    newMipTexture.Palette[j] = Color.FromArgb(0xFF, colorData[0], colorData[1], colorData[2]);
                }

                mipTextures[i] = newMipTexture;
            }

            Textures = mipTextures;
        }
    }
}
