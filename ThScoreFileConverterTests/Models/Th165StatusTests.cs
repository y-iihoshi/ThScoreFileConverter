using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th165StatusTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public uint checksum;
            public int size;
            public byte[] lastName;
            public byte[] bgmFlags;
            public int totalPlayTime;
            public byte[] nicknameFlags;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "ST",
            version = 2,
            checksum = 0u,
            size = 0x224,
            lastName = TestUtils.CP932Encoding.GetBytes("Player1\0\0\0\0\0\0\0"),
            bgmFlags = TestUtils.MakeRandomArray<byte>(8),
            totalPlayTime = 12345678,
            nicknameFlags = TestUtils.MakeRandomArray<byte>(51)
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.lastName,
                new byte[0x12],
                properties.bgmFlags,
                new byte[0x18],
                properties.totalPlayTime,
                new byte[0x4C],
                properties.nicknameFlags,
                new byte[0x155]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                MakeData(properties));

        internal static void Validate(in Th165StatusWrapper status, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, status.Signature);
            Assert.AreEqual(properties.version, status.Version);
            Assert.AreEqual(properties.checksum, status.Checksum);
            Assert.AreEqual(properties.size, status.Size);
            CollectionAssert.AreEqual(data, status.Data.ToArray());
            CollectionAssert.AreEqual(properties.lastName, status.LastName.ToArray());
            CollectionAssert.AreEqual(properties.bgmFlags, status.BgmFlags.ToArray());
            Assert.AreEqual(properties.totalPlayTime, status.TotalPlayTime);
            CollectionAssert.AreEqual(properties.nicknameFlags, status.NicknameFlags.ToArray());
        }

        [TestMethod]
        public void Th165StatusTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th10ChapterWrapper.Create(MakeByteArray(properties));
            var status = new Th165StatusWrapper(chapter);

            Validate(status, properties);
            Assert.IsFalse(status.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th165StatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            var status = new Th165StatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th165StatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th10ChapterWrapper.Create(MakeByteArray(properties));
            var status = new Th165StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th165StatusTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.version;

            var chapter = Th10ChapterWrapper.Create(MakeByteArray(properties));
            var status = new Th165StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th165StatusTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size;

            var chapter = Th10ChapterWrapper.Create(MakeByteArray(properties));
            var status = new Th165StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)2, 0x224, true)]
        [DataRow("st", (ushort)2, 0x224, false)]
        [DataRow("ST", (ushort)1, 0x224, false)]
        [DataRow("ST", (ushort)2, 0x225, false)]
        public void Th165StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th10ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th165StatusWrapper.CanInitialize(chapter));
            });
    }
}
