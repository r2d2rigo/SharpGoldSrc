using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharpGoldSrc.Sprites
{
    public class Sprite
    {
        public UInt32 Version { get; private set; }
        public SpriteType Type { get; private set; }
        public SpriteFormat Format { get; private set; }
        public float BoudingRadius { get; private set; }
        public UInt32 MaxWidth { get; private set; }
        public UInt32 MaxHeight { get; private set; }
        public float BeamLength { get; private set; }
        public SpriteSynchronizationType SynchronizationType { get; private set; }
        public SpritePalette Palette { get; private set; }
        public IReadOnlyList<SpriteFrame> Frames { get; private set; }

        public static Sprite FromStream(Stream stream)
        {
            return ReadFromStream(stream);
        }

        internal static Sprite ReadFromStream(Stream stream)
        {
            var sprite = new Sprite();

            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                var magicNumber = reader.ReadBytes(4);
                UInt32 frameCount = 0;

                sprite.Version = reader.ReadUInt32();
                sprite.Type = (SpriteType)reader.ReadUInt32();
                sprite.Format = (SpriteFormat)reader.ReadUInt32();
                sprite.BoudingRadius = reader.ReadSingle();
                sprite.MaxWidth = reader.ReadUInt32();
                sprite.MaxHeight = reader.ReadUInt32();
                frameCount = reader.ReadUInt32();
                sprite.BeamLength = reader.ReadSingle();
                sprite.SynchronizationType = (SpriteSynchronizationType)reader.ReadUInt32();
                sprite.Palette = SpritePalette.ReadFromStream(stream);

                List<SpriteFrame> frames = new List<SpriteFrame>();

                for (int i = 0; i < frameCount; ++i)
                {
                    frames.Add(SpriteFrame.ReadFromStream(stream));
                }

                sprite.Frames = frames;
            }

            return sprite;
        }
    }
}
