using SharpGoldSrc.Wad.Internal;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace SharpGoldSrc.Wad
{
    public class WadTexture
    {
        public string Name { get; private set; }

        public UInt32 Width { get; private set; }
        public UInt32 Height { get; private set; }

        public byte[][] Mipmaps { get; private set; }
        public Color[] Palette { get; private set; }

        internal bool IsInitialized { get; set; }

        internal WadTexture(string name)
        {
            Name = name;
            IsInitialized = false;
        }

        internal void InitializeFromMipTexture(MipTexture mipTexture)
        {
            Width = mipTexture.Width;
            Height = mipTexture.Height;

            Mipmaps = new byte[mipTexture.MipData.Length][];

            for (int i = 0; i < mipTexture.MipData.Length; i++)
            {
                Mipmaps[i] = new byte[mipTexture.MipData[i].Length];
                mipTexture.MipData[i].CopyTo(Mipmaps[i], 0);
            }

            Palette = new Color[mipTexture.Palette.Length];
            mipTexture.Palette.CopyTo(Palette, 0);

            IsInitialized = true;
        }

        public Bitmap GetMipmapAsBitmap(int mipmapLevel)
        {
            if (mipmapLevel < 0 || mipmapLevel > Mipmaps.Length - 1)
            {
                throw new ArgumentException("mipmapLevel");
            }

            var mipmapWidth = Width >> mipmapLevel;
            var mipmapHeight = Height >> mipmapLevel;

            var bitmap = new Bitmap((int)mipmapWidth, (int)mipmapHeight, PixelFormat.Format32bppArgb);

            for (int y = 0; y < mipmapHeight; y++)
            {
                for (int x = 0; x < mipmapWidth; x++)
                {
                    var pixelColor = Palette[Mipmaps[mipmapLevel][y * mipmapWidth + x]];

                    if (pixelColor == Color.Blue)
                    {
                        pixelColor = Color.Transparent;
                    }

                    bitmap.SetPixel(x, y, pixelColor);
                }
            }
            
            return bitmap;
        }
    }
}
