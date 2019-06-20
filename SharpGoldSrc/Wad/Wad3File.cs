using SharpGoldSrc.Wad.Internal;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpGoldSrc.Wad
{
    public class Wad3File : IDisposable
    {
        private WadHeader _header;
        private DirectoryEntry[] _entries;
        private BinaryReader _binaryReader;
        private WadTexture[] _textures;

        public int TextureCount
        {
            get
            {
                return _textures.Length;
            }
        }

        public WadTexture this[string textureName]
        {
            get
            {
                var existingTexture = _textures.SingleOrDefault(t => t.Name == textureName);

                if (existingTexture != null)
                {
                    if (!existingTexture.IsInitialized)
                    {
                        ReadMipTexture(_entries.SingleOrDefault(e => Encoding.ASCII.GetString(e.Name).TrimEnd('\0') == textureName));
                    }
                }

                return existingTexture;
            }
        }

        private Wad3File()
        {
            _header = new WadHeader();
        }

        public static Wad3File FromStream(Stream stream, bool loadOnDemand = false)
        {
            var wadFile = new Wad3File();
            wadFile._binaryReader = new BinaryReader(stream, Encoding.ASCII, true);
            wadFile.ReadHeader();

            if (!wadFile._header.IsValidHeader)
            {
                throw new NotSupportedException("Stream is not a valid WAD3 file.");
            }

            wadFile.ReadDirectoryEntries();

            if (!loadOnDemand)
            {
                wadFile.ReadMipTextures();
                wadFile._binaryReader.Dispose();
                wadFile._binaryReader = null;
            }

            return wadFile;
        }

        private void ReadHeader()
        {
            _header.MagicNumber = _binaryReader.ReadBytes(4);
            _header.DirectoryEntryCount = _binaryReader.ReadInt32();
            _header.DirectoryEntryOffset = _binaryReader.ReadInt32();
        }

        private void ReadDirectoryEntries()
        {
            _binaryReader.BaseStream.Seek(_header.DirectoryEntryOffset, SeekOrigin.Begin);

            var directoryEntries = new DirectoryEntry[_header.DirectoryEntryCount];

            for (int i = 0; i < _header.DirectoryEntryCount; i++)
            {
                var newEntry = new DirectoryEntry();
                newEntry.FileOffset = _binaryReader.ReadInt32();
                newEntry.CompressedSize = _binaryReader.ReadInt32();
                newEntry.Size = _binaryReader.ReadInt32();
                newEntry.Type = _binaryReader.ReadByte();
                newEntry.IsCompressed = _binaryReader.ReadBoolean();
                newEntry._dummy = _binaryReader.ReadInt16();
                newEntry.Name = _binaryReader.ReadBytes(DirectoryEntry.TEXTURE_NAME_MAX_LENGTH);

                directoryEntries[i] = newEntry;
            }

            _entries = directoryEntries;
            _textures = _entries.Select(e => new WadTexture(Encoding.ASCII.GetString(e.Name).TrimEnd('\0'))).ToArray();
        }

        private void ReadMipTextures()
        {
            for (int i = 0; i < _header.DirectoryEntryCount; i++)
            {
                var currentEntry = _entries[i];

                ReadMipTexture(currentEntry);
            }
        }

        private void ReadMipTexture(DirectoryEntry directoryEntry)
        {
            var wadTexture = _textures.SingleOrDefault(t => t.Name == Encoding.ASCII.GetString(directoryEntry.Name).TrimEnd('\0'));

            if (wadTexture == null)
            {
                return;
            }

            _binaryReader.BaseStream.Seek(directoryEntry.FileOffset, SeekOrigin.Begin);

            var newMipTexture = new MipTexture();
            newMipTexture.Name = _binaryReader.ReadBytes(MipTexture.TEXTURE_NAME_MAX_LENGTH);
            newMipTexture.Width = _binaryReader.ReadUInt32();
            newMipTexture.Height = _binaryReader.ReadUInt32();
            newMipTexture.DataOffsets = new UInt32[MipTexture.MIPMAP_LEVEL_COUNT];
            newMipTexture.MipData = new byte[MipTexture.MIPMAP_LEVEL_COUNT][];
            newMipTexture.Palette = new Color[MipTexture.PALETTE_ENTRY_COUNT];

            for (int j = 0; j < MipTexture.MIPMAP_LEVEL_COUNT; j++)
            {
                newMipTexture.DataOffsets[j] = _binaryReader.ReadUInt32();
            }

            var mipDataSize = (int)(newMipTexture.Width * newMipTexture.Height);

            for (int j = 0; j < MipTexture.MIPMAP_LEVEL_COUNT; j++)
            {
                _binaryReader.BaseStream.Seek(directoryEntry.FileOffset + newMipTexture.DataOffsets[j], SeekOrigin.Begin);

                newMipTexture.MipData[j] = _binaryReader.ReadBytes(mipDataSize);

                mipDataSize /= 4;
            }

            newMipTexture._dummy = _binaryReader.ReadInt16();

            for (int j = 0; j < MipTexture.PALETTE_ENTRY_COUNT; j++)
            {
                var colorData = _binaryReader.ReadBytes(3);

                newMipTexture.Palette[j] = Color.FromArgb(0xFF, colorData[0], colorData[1], colorData[2]);
            }

            wadTexture.InitializeFromMipTexture(newMipTexture);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_binaryReader != null)
                    {
                        _binaryReader.Dispose();
                        _binaryReader = null;
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
