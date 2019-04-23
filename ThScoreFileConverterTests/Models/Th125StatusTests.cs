using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th125StatusTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public int size;
            public uint checksum;
            public byte[] lastName;
            public byte[] bgmFlags;
            public int totalPlayTime;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "ST",
            version = 1,
            size = 0x474,
            checksum = 0u,
            lastName = Encoding.Default.GetBytes("Player1\0\0\0"),
            bgmFlags = TestUtils.MakeRandomArray<byte>(6),
            totalPlayTime = 12345678
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.lastName,
                new byte[0x2],
                properties.bgmFlags,
                new byte[0x2E],
                properties.totalPlayTime,
                new byte[0x424]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.size,
                properties.checksum,
                MakeData(properties));

        internal static void Validate(in Th125StatusWrapper status, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, status.Signature);
            Assert.AreEqual(properties.version, status.Version);
            Assert.AreEqual(properties.size, status.Size);
            Assert.AreEqual(properties.checksum, status.Checksum);
            CollectionAssert.AreEqual(data, status.Data.ToArray());
            CollectionAssert.AreEqual(properties.lastName, status.LastName.ToArray());
            CollectionAssert.AreEqual(properties.bgmFlags, status.BgmFlags.ToArray());
            Assert.AreEqual(properties.totalPlayTime, status.TotalPlayTime);
        }

        [TestMethod]
        public void Th125StatusTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th095ChapterWrapper<Th125Converter>.Create(MakeByteArray(properties));
            var status = new Th125StatusWrapper(chapter);

            Validate(status, properties);
            Assert.IsFalse(status.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th125StatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            var status = new Th125StatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th125StatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th095ChapterWrapper<Th125Converter>.Create(MakeByteArray(properties));
            var status = new Th125StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th125StatusTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.version;

            var chapter = Th095ChapterWrapper<Th125Converter>.Create(MakeByteArray(properties));
            var status = new Th125StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th125StatusTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size;

            var chapter = Th095ChapterWrapper<Th125Converter>.Create(MakeByteArray(properties));
            var status = new Th125StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x474, true)]
        [DataRow("st", (ushort)1, 0x474, false)]
        [DataRow("ST", (ushort)0, 0x474, false)]
        [DataRow("ST", (ushort)1, 0x475, false)]
        public void Th125StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th095ChapterWrapper<Th125Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(expected, Th125StatusWrapper.CanInitialize(chapter));
            });
    }
}
