using NUnit.Framework;
using SharpGoldSrc.Sprites;
using System.IO;

namespace SharpGoldSrc.Tests
{
    [TestFixture]
    public class SpriteUnitTests
    {
        [Test]
        public void TestSingleSprite()
        {
            var spriteFile = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "crosshairs.spr"));

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

        [Test]
        public void TestAnimatedSprite()
        {
            var spriteFile = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "animglow01.spr"));

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
