using System;
using System.Collections.Generic;
using System.Text;

namespace SharpGoldSrc.Wad
{
    public struct DirectoryEntry
    {
        internal static readonly int TEXTURE_NAME_MAX_LENGTH = 16;

        public Int32 FileOffset;
        public Int32 CompressedSize;
        public Int32 Size;
        public byte Type;
        public bool IsCompressed;
        internal Int16 _dummy;
        public byte[] Name;
    }
}
