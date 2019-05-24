using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace SharpGoldSrc.Sprites
{
    public class SpritePalette
    {
        public UInt16 Size { get; private set; }
        public IReadOnlyList<Color> Entries { get; private set; }

        public SpritePalette()
        {
            this.Entries = new List<Color>();
        }

        internal static SpritePalette ReadFromStream(Stream stream)
        {
            var palette = new SpritePalette();

            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                palette.Size = reader.ReadUInt16();

                var colorEntries = new List<Color>();

                for (int i = 0; i < palette.Size; ++i)
                {
                    var colorEntry = reader.ReadBytes(3);
                    colorEntries.Add(Color.FromArgb(0xFF, colorEntry[0], colorEntry[1], colorEntry[2]));
                }

                palette.Entries = colorEntries;
            }

            return palette;
        }
    }
}
