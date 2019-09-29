using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
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
            CollectionAssert.That.AreEqual(data, status.Data);
            CollectionAssert.That.AreEqual(properties.lastName, status.LastName);
            CollectionAssert.That.AreEqual(properties.bgmFlags, status.BgmFlags);
            Assert.AreEqual(properties.totalPlayTime, status.TotalPlayTime);
            CollectionAssert.That.AreEqual(properties.nicknameFlags, status.NicknameFlags);
        }

        [TestMethod]
        public void Th165StatusTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
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

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
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

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
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

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
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

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th165StatusWrapper.CanInitialize(chapter));
            });
    }
}
