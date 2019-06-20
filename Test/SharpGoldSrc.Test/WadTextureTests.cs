using NUnit.Framework;
using SharpGoldSrc.Wad;
using System;
using System.IO;
using System.Linq;

namespace SharpGoldSrc.Tests
{
    [TestFixture]
    public class WadTextureTests
    {
        [Test]
        public void GetMipmapAsBitmap_WadTexture_GeneratesCorrectBitmaps()
        {
            using (var wad3FileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources/wad3.wad")))
            {
                var wadFile = Wad3File.FromStream(wad3FileStream);

                var baboonTexture = wadFile["baboon"];
                var baboonMipmap0 = baboonTexture.GetMipmapAsBitmap(0);
                Assert.AreEqual(512, baboonMipmap0.Width);
                Assert.AreEqual(512, baboonMipmap0.Height);

                var baboonMipmap1 = baboonTexture.GetMipmapAsBitmap(1);
                Assert.AreEqual(256, baboonMipmap1.Width);
                Assert.AreEqual(256, baboonMipmap1.Height);

                var baboonMipmap2 = baboonTexture.GetMipmapAsBitmap(2);
                Assert.AreEqual(128, baboonMipmap2.Width);
                Assert.AreEqual(128, baboonMipmap2.Height);

                var baboonMipmap3 = baboonTexture.GetMipmapAsBitmap(3);
                Assert.AreEqual(64, baboonMipmap3.Width);
                Assert.AreEqual(64, baboonMipmap3.Height);
            }
        }
    }
}
