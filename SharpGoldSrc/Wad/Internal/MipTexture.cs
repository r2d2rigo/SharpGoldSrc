using System;
using System.Drawing;

namespace SharpGoldSrc.Wad.Internal
{
    public struct MipTexture
    {
        internal static readonly int TEXTURE_NAME_MAX_LENGTH = 16;
        internal static readonly int MIPMAP_LEVEL_COUNT = 4;
        internal static readonly int PALETTE_ENTRY_COUNT = 256;

        public byte[] Name;
        public UInt32 Width;
        public UInt32 Height;
        public UInt32[] DataOffsets;
        public byte[][] MipData;
        internal Int16 _dummy;
        public Color[] Palette;
    }
}
