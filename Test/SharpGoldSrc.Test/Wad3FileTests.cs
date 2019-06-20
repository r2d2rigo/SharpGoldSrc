using NUnit.Framework;
using SharpGoldSrc.Wad;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpGoldSrc.Tests
{
    [TestFixture]
    public class Wad3FileTests
    {
        [Test]
        public void FromStream_Wad2File_ShouldFail()
        {
            using (var wad2FileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources/wad2.wad")))
            {
                Assert.Catch<NotSupportedException>(() => Wad3File.FromStream(wad2FileStream));
            }
        }

        [Test]
        public void FromStream_Wad3File_ShouldLoad()
        {
            using (var wad3FileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources/wad3.wad")))
            {
                var wadFile = Wad3File.FromStream(wad3FileStream);

                Assert.NotNull(wadFile);
            }
        }

        [Test]
        public void FromStream_Wad3File_HasCorrectNumberOfTextures()
        {
            using (var wad3FileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources/wad3.wad")))
            {
                var wadFile = Wad3File.FromStream(wad3FileStream);

                Assert.AreEqual(2, wadFile.TextureCount);
            }
        }

        [Test]
        public void FromStream_Wad3File_HasCorrectTextures()
        {
            using (var wad3FileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources/wad3.wad")))
            {
                var wadFile = Wad3File.FromStream(wad3FileStream);
                var baboonTexture = wadFile["baboon"];

                Assert.AreEqual("baboon", baboonTexture.Name);
                Assert.AreEqual(512, baboonTexture.Width);
                Assert.AreEqual(512, baboonTexture.Height);
                Assert.AreEqual(4, baboonTexture.Mipmaps.Length);
                Assert.AreEqual(262144, baboonTexture.Mipmaps[0].Length);
                Assert.AreEqual(65536, baboonTexture.Mipmaps[1].Length);
                Assert.AreEqual(16384, baboonTexture.Mipmaps[2].Length);
                Assert.AreEqual(4096, baboonTexture.Mipmaps[3].Length);
                Assert.AreEqual(256, baboonTexture.Palette.Length);

                var peppersTexture = wadFile["PEPPERS"];

                Assert.AreEqual("PEPPERS", peppersTexture.Name);
                Assert.AreEqual(256, peppersTexture.Width);
                Assert.AreEqual(256, peppersTexture.Height);
                Assert.AreEqual(4, peppersTexture.Mipmaps.Length);
                Assert.AreEqual(65536, peppersTexture.Mipmaps[0].Length);
                Assert.AreEqual(16384, peppersTexture.Mipmaps[1].Length);
                Assert.AreEqual(4096, peppersTexture.Mipmaps[2].Length);
                Assert.AreEqual(1024, peppersTexture.Mipmaps[3].Length);
                Assert.AreEqual(256, peppersTexture.Palette.Length);
            }
        }
    }
}
