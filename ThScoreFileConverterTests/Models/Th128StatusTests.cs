using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th128StatusTests
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
        };

        internal static Properties GetValidProperties(ushort version, int size, int numBgms) => new Properties()
        {
            signature = "ST",
            version = version,
            checksum = 0u,
            size = size,
            lastName = TestUtils.CP932Encoding.GetBytes("Player1\0\0\0"),
            bgmFlags = TestUtils.MakeRandomArray<byte>(numBgms),
            totalPlayTime = 12345678
        };

        internal static byte[] MakeData(in Properties properties, int gap1Size, int gap2Size)
        {
            // NOTE: header == (signature, version, size, checksum)
            var headerSize =
                TestUtils.CP932Encoding.GetByteCount(properties.signature) + sizeof(ushort) + sizeof(uint) + sizeof(int);
            // NOTE: data == (lastName, gap1, bgms, gap2, totalPlayTime, gap3)
            var dataSize = properties.size - headerSize;
            var gap3Size =
                dataSize - properties.lastName.Length - gap1Size - properties.bgmFlags.Length - gap2Size - sizeof(int);

            return TestUtils.MakeByteArray(
                properties.lastName,
                new byte[gap1Size],
                properties.bgmFlags,
                new byte[gap2Size],
                properties.totalPlayTime,
                new byte[gap3Size]);
        }

        internal static byte[] MakeByteArray(in Properties properties, int gap1Size, int gap2Size)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                MakeData(properties, gap1Size, gap2Size));

        internal static void Validate<TParent>(
            in Th128StatusWrapper<TParent> status, in Properties properties, int gap1Size, int gap2Size)
            where TParent : ThConverter
        {
            var data = MakeData(properties, gap1Size, gap2Size);

            Assert.AreEqual(properties.signature, status.Signature);
            Assert.AreEqual(properties.checksum, status.Checksum);
            Assert.AreEqual(properties.version, status.Version);
            Assert.AreEqual(properties.size, status.Size);
            CollectionAssert.AreEqual(data, status.Data.ToArray());
            CollectionAssert.AreEqual(properties.lastName, status.LastName.ToArray());
            CollectionAssert.AreEqual(properties.bgmFlags, status.BgmFlags.ToArray());
            Assert.AreEqual(properties.totalPlayTime, status.TotalPlayTime);
        }

        internal static void StatusTestChapterHelper<TParent>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size, numBgms);

                var chapter = Th10ChapterWrapper<TParent>.Create(MakeByteArray(properties, gap1Size, gap2Size));
                var status = new Th128StatusWrapper<TParent>(chapter);

                Validate(status, properties, gap1Size, gap2Size);
                Assert.IsFalse(status.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestNullChapterHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var status = new Th128StatusWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidSignatureHelper<TParent>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size, numBgms);
                properties.signature = properties.signature.ToLowerInvariant();

                var chapter = Th10ChapterWrapper<TParent>.Create(MakeByteArray(properties, gap1Size, gap2Size));
                var status = new Th128StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidVersionHelper<TParent>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size, numBgms);
                ++properties.version;

                var chapter = Th10ChapterWrapper<TParent>.Create(MakeByteArray(properties, gap1Size, gap2Size));
                var status = new Th128StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidSizeHelper<TParent>(
            ushort version, int size, int numBgms, int gap1Size, int gap2Size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size, numBgms);
                ++properties.size;

                var chapter = Th10ChapterWrapper<TParent>.Create(MakeByteArray(properties, gap1Size, gap2Size));
                var status = new Th128StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        internal static void CanInitializeTestHelper<TParent>(string signature, ushort version, int size, bool expected)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th10ChapterWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th128StatusWrapper<TParent>.CanInitialize(chapter));
            });

        #region Th128

        [TestMethod]
        public void Th128StatusTestChapter()
            => StatusTestChapterHelper<Th128Converter>(2, 0x42C, 10, 0x10, 0x18);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th128Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th128Converter>(2, 0x42C, 10, 0x10, 0x18);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th128Converter>(2, 0x42C, 10, 0x10, 0x18);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th128StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th128Converter>(2, 0x42C, 10, 0x10, 0x18);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)2, 0x42C, true)]
        [DataRow("st", (ushort)2, 0x42C, false)]
        [DataRow("ST", (ushort)1, 0x42C, false)]
        [DataRow("ST", (ushort)2, 0x42D, false)]
        public void Th128StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th128Converter>(signature, version, size, expected);

        #endregion

        #region Th13

        [TestMethod]
        public void Th13StatusTestChapter()
            => StatusTestChapterHelper<Th13Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th13Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th13Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th13Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th13StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th13Converter>(1, 0x42C, 17, 0x10, 0x11);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x42C, true)]
        [DataRow("st", (ushort)1, 0x42C, false)]
        [DataRow("ST", (ushort)0, 0x42C, false)]
        [DataRow("ST", (ushort)1, 0x42D, false)]
        public void Th13StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th13Converter>(signature, version, size, expected);

        #endregion

        #region Th14

        [TestMethod]
        public void Th14StatusTestChapter()
            => StatusTestChapterHelper<Th14Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th14Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th14Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th14Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th14StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th14Converter>(1, 0x42C, 17, 0x10, 0x11);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x42C, true)]
        [DataRow("st", (ushort)1, 0x42C, false)]
        [DataRow("ST", (ushort)0, 0x42C, false)]
        [DataRow("ST", (ushort)1, 0x42D, false)]
        public void Th14StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th14Converter>(signature, version, size, expected);

        #endregion

        #region Th15

        [TestMethod]
        public void Th15StatusTestChapter()
            => StatusTestChapterHelper<Th15Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th15Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th15Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th15Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th15Converter>(1, 0x42C, 17, 0x10, 0x11);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x42C, true)]
        [DataRow("st", (ushort)1, 0x42C, false)]
        [DataRow("ST", (ushort)0, 0x42C, false)]
        [DataRow("ST", (ushort)1, 0x42D, false)]
        public void Th15StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th15Converter>(signature, version, size, expected);

        #endregion

        #region Th16

        [TestMethod]
        public void Th16StatusTestChapter()
            => StatusTestChapterHelper<Th16Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th16Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th16StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th16Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th16StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th16Converter>(1, 0x42C, 17, 0x10, 0x11);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th16StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th16Converter>(1, 0x42C, 17, 0x10, 0x11);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x42C, true)]
        [DataRow("st", (ushort)1, 0x42C, false)]
        [DataRow("ST", (ushort)0, 0x42C, false)]
        [DataRow("ST", (ushort)1, 0x42D, false)]
        public void Th16StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th16Converter>(signature, version, size, expected);

        #endregion
    }
}
