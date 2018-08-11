using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th10StatusTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public uint checksum;
            public int size;
            public byte[] lastName;
            public byte[] bgmFlags;
        };

        internal static Properties GetValidProperties(ushort version, int size, int numBgms) => new Properties()
        {
            signature = "ST",
            version = version,
            checksum = 0u,
            size = size,
            lastName = Encoding.Default.GetBytes("Player1\0\0\0"),
            bgmFlags = TestUtils.MakeRandomArray<byte>(numBgms)
        };

        internal static byte[] MakeData(in Properties properties)
        {
            // NOTE: header == (signature, version, checksum, size)
            var headerSize =
                Encoding.Default.GetByteCount(properties.signature) + sizeof(ushort) + sizeof(uint) + sizeof(int);
            // NOTE: data == (lastName, gap1, bgms, gap2)
            var dataSize = properties.size - headerSize;
            var gap1Size = 0x10;
            var gap2Size = dataSize - properties.lastName.Length - gap1Size - properties.bgmFlags.Length;

            return TestUtils.MakeByteArray(
                properties.lastName, new byte[gap1Size], properties.bgmFlags, new byte[gap2Size]);
        }

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                MakeData(properties));

        internal static void Validate<TParent>(in Th10StatusWrapper<TParent> status, in Properties properties)
            where TParent : ThConverter
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, status.Signature);
            Assert.AreEqual(properties.version, status.Version);
            Assert.AreEqual(properties.checksum, status.Checksum);
            Assert.AreEqual(properties.size, status.Size);
            CollectionAssert.AreEqual(data, status.Data.ToArray());
            CollectionAssert.AreEqual(properties.lastName, status.LastName.ToArray());
            CollectionAssert.AreEqual(properties.bgmFlags, status.BgmFlags.ToArray());
        }

        internal static void StatusTestChapterHelper<TParent>(ushort version, int size, int numBgms)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size, numBgms);

                var chapter = Th10ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var status = new Th10StatusWrapper<TParent>(chapter);

                Validate(status, properties);
                Assert.IsFalse(status.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestNullChapterHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var status = new Th10StatusWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidSignatureHelper<TParent>(ushort version, int size, int numBgms)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size, numBgms);
                properties.signature = properties.signature.ToLowerInvariant(); 

                var chapter = Th10ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var status = new Th10StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidVersionHelper<TParent>(ushort version, int size, int numBgms)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size, numBgms);
                ++properties.version;

                var chapter = Th10ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var status = new Th10StatusWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        internal static void StatusTestInvalidSizeHelper<TParent>(ushort version, int size, int numBgms)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size, numBgms);
                ++properties.size;

                var chapter = Th10ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var status = new Th10StatusWrapper<TParent>(chapter);

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

                Assert.AreEqual(expected, Th10StatusWrapper<TParent>.CanInitialize(chapter));
            });

        #region Th10

        [TestMethod]
        public void Th10StatusTestChapter()
            => StatusTestChapterHelper<Th10Converter>(0, 0x448, 18);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th10Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th10StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th10Converter>(0, 0x448, 18);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th10StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th10Converter>(0, 0x448, 18);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th10StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th10Converter>(0, 0x448, 18);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)0, 0x448, true)]
        [DataRow("st", (ushort)0, 0x448, false)]
        [DataRow("ST", (ushort)1, 0x448, false)]
        [DataRow("ST", (ushort)0, 0x449, false)]
        public void Th10StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th10Converter>(signature, version, size, expected);

        #endregion

        #region Th11

        [TestMethod]
        public void Th11StatusTestChapter()
            => StatusTestChapterHelper<Th11Converter>(0, 0x448, 17);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th11Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th11StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th11Converter>(0, 0x448, 17);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th11StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th11Converter>(0, 0x448, 17);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th11StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th11Converter>(0, 0x448, 17);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)0, 0x448, true)]
        [DataRow("st", (ushort)0, 0x448, false)]
        [DataRow("ST", (ushort)1, 0x448, false)]
        [DataRow("ST", (ushort)0, 0x449, false)]
        public void Th11StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th11Converter>(signature, version, size, expected);

        #endregion

        #region Th12

        [TestMethod]
        public void Th12StatusTestChapter()
            => StatusTestChapterHelper<Th12Converter>(2, 0x448, 17);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12StatusTestNullChapter()
            => StatusTestNullChapterHelper<Th12Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th12StatusTestInvalidSignature()
            => StatusTestInvalidSignatureHelper<Th12Converter>(2, 0x448, 17);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th12StatusTestInvalidVersion()
            => StatusTestInvalidVersionHelper<Th12Converter>(2, 0x448, 17);

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th12StatusTestInvalidSize()
            => StatusTestInvalidSizeHelper<Th12Converter>(2, 0x448, 17);

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)2, 0x448, true)]
        [DataRow("st", (ushort)2, 0x448, false)]
        [DataRow("ST", (ushort)1, 0x448, false)]
        [DataRow("ST", (ushort)2, 0x449, false)]
        public void Th12StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => CanInitializeTestHelper<Th12Converter>(signature, version, size, expected);

        #endregion
    }
}
