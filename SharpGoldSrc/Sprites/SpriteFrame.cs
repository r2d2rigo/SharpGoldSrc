using System;
using System.IO;
using System.Text;

namespace SharpGoldSrc.Sprites
{
    public class SpriteFrame
    {
        public UInt32 Group { get; private set; }
        public Int32 OriginX { get; private set; }
        public Int32 OriginY { get; private set; }
        public UInt32 Width { get; private set; }
        public UInt32 Height { get; private set; }
        public Byte[] Data { get; private set; }

        public SpriteFrame()
        {
            this.Data = new Byte[0];
        }

        internal static SpriteFrame ReadFromStream(Stream stream)
        {
            var frame = new SpriteFrame();

            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                frame.Group = reader.ReadUInt32();
                frame.OriginX = reader.ReadInt32();
                frame.OriginY = reader.ReadInt32();
                frame.Width = reader.ReadUInt32();
                frame.Height = reader.ReadUInt32();

                var dataSize = frame.Width * frame.Height;
                frame.Data = reader.ReadBytes((int)dataSize);
            }

            return frame;
        }
    }
}
