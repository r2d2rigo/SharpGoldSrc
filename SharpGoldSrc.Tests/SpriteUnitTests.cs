using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpGoldSrc.Sprites;
using System.IO;

namespace SharpGoldSrc.Tests
{
    [TestClass]
    public class SpriteUnitTests
    {
        [TestMethod]
        public void TestSingleSprite()
        {
            var spriteFile = File.OpenRead("crosshairs.spr");

            var sprite = Sprite.FromStream(spriteFile);

            Assert.AreEqual(SpriteType.VpParallel, sprite.Type);
            Assert.AreEqual(SpriteFormat.AlphaTest, sprite.Format);

            Assert.IsTrue(128 == sprite.MaxWidth);
            Assert.IsTrue(128 == sprite.MaxHeight);

            Assert.IsTrue(1 == sprite.Frames.Count);

            var firstFrame = sprite.Frames[0];
            Assert.IsTrue(-64 == firstFrame.OriginX);
            Assert.IsTrue(64 == firstFrame.OriginY);
        }

        [TestMethod]
        public void TestAnimatedSprite()
        {
            var spriteFile = File.OpenRead("animglow01.spr");

            var sprite = Sprite.FromStream(spriteFile);

            Assert.AreEqual(SpriteType.VpParallel, sprite.Type);
            Assert.AreEqual(SpriteFormat.Additive, sprite.Format);

            Assert.IsTrue(96 == sprite.MaxWidth);
            Assert.IsTrue(96 == sprite.MaxHeight);

            Assert.IsTrue(7 == sprite.Frames.Count);

            var firstFrame = sprite.Frames[0];
            Assert.IsTrue(-48 == firstFrame.OriginX);
            Assert.IsTrue(48 == firstFrame.OriginY);
        }
    }
}
