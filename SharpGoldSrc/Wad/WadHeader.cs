using System;
using System.Linq;

namespace SharpGoldSrc.Wad
{
    internal struct WadHeader
    {
        private static readonly byte[] WAD3_HEADER =
        {
            (byte)'W',
            (byte)'A',
            (byte)'D',
            (byte)'3',
        };

        public byte[] MagicNumber;
        public Int32 DirectoryEntryCount;
        public Int32 DirectoryEntryOffset;

        public bool IsValidHeader
        {
            get => MagicNumber.SequenceEqual(WAD3_HEADER);
        }
    }
}
