using NUnit.Framework;
using SharpGoldSrc.Wad;
using System;
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
        public void FromStream_Wad3File_HasCorrectNumberOfDirectoryEntries()
        {
            using (var wad3FileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources/wad3.wad")))
            {
                var wadFile = Wad3File.FromStream(wad3FileStream);

                Assert.AreEqual(2, wadFile.Entries.Length);
            }
        }

        [Test]
        public void FromStream_Wad3File_HasCorrectDirectoryEntries()
        {
            using (var wad3FileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources/wad3.wad")))
            {
                var wadFile = Wad3File.FromStream(wad3FileStream);

                Assert.NotZero(wadFile.Entries[0].FileOffset);
                Assert.NotZero(wadFile.Entries[0].Size);
                Assert.NotZero(wadFile.Entries[0].CompressedSize);
                Assert.NotZero(wadFile.Entries[0].Type);
                Assert.False(wadFile.Entries[0].IsCompressed);
                Assert.True(wadFile.Entries[0].Name.SequenceEqual(new[]
                {
                    (byte)'b',
                    (byte)'a',
                    (byte)'b',
                    (byte)'o',
                    (byte)'o',
                    (byte)'n',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                }));

                Assert.NotZero(wadFile.Entries[1].FileOffset);
                Assert.NotZero(wadFile.Entries[1].Size);
                Assert.NotZero(wadFile.Entries[1].CompressedSize);
                Assert.NotZero(wadFile.Entries[1].Type);
                Assert.False(wadFile.Entries[1].IsCompressed);
                Assert.True(wadFile.Entries[1].Name.SequenceEqual(new[]
                {
                    (byte)'P',
                    (byte)'E',
                    (byte)'P',
                    (byte)'P',
                    (byte)'E',
                    (byte)'R',
                    (byte)'S',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                    (byte)'\0',
                }));
            }
        }


        [Test]
        public void FromStream_Wad3File_HasCorrectMipTextures()
        {
            using (var wad3FileStream = File.OpenRead(Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources/wad3.wad")))
            {
                var wadFile = Wad3File.FromStream(wad3FileStream);

                Assert.AreEqual(512, wadFile.Textures[0].Width);
                Assert.AreEqual(512, wadFile.Textures[0].Height);
                Assert.AreEqual(4, wadFile.Textures[0].DataOffsets.Length);
                Assert.AreEqual(4, wadFile.Textures[0].MipData.Length);
                Assert.AreEqual(262144, wadFile.Textures[0].MipData[0].Length);
                Assert.AreEqual(65536, wadFile.Textures[0].MipData[1].Length);
                Assert.AreEqual(16384, wadFile.Textures[0].MipData[2].Length);
                Assert.AreEqual(4096, wadFile.Textures[0].MipData[3].Length);
                Assert.AreEqual(256, wadFile.Textures[0].Palette.Length);
                // TODO: original string is not set to zero, fix this later
                //Assert.True(wadFile.Textures[0].Name.SequenceEqual(new[]
                //{
                //    (byte)'b',
                //    (byte)'a',
                //    (byte)'b',
                //    (byte)'o',
                //    (byte)'o',
                //    (byte)'n',
                //    (byte)'\0',
                //    (byte)'\0',
                //    (byte)'\0',
                //    (byte)'\0',
                //    (byte)'\0',
                //    (byte)'\0',
                //    (byte)'\0',
                //    (byte)'\0',
                //    (byte)'\0',
                //    (byte)'\0',
                //}));
            }
        }
    }
}
