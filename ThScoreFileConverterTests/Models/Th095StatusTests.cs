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
    public class Th095StatusTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public int size;
            public uint checksum;
            public byte[] lastName;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "ST",
            version = 0,
            size = 0x458,
            checksum = 0u,
            lastName = Encoding.Default.GetBytes("Player1\0\0\0")
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(properties.lastName, new byte[0x442]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.size,
                properties.checksum,
                MakeData(properties));

        internal static void Validate(in Th095StatusWrapper status, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, status.Signature);
            Assert.AreEqual(properties.version, status.Version);
            Assert.AreEqual(properties.size, status.Size);
            Assert.AreEqual(properties.checksum, status.Checksum);
            CollectionAssert.AreEqual(data, status.Data.ToArray());
            CollectionAssert.AreEqual(properties.lastName, status.LastName.ToArray());
        }

        [TestMethod]
        public void Th095StatusTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th095ChapterWrapper<Th095Converter>.Create(MakeByteArray(properties));
            var status = new Th095StatusWrapper(chapter);

            Validate(status, properties);
            Assert.IsFalse(status.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095StatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            var status = new Th095StatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th095StatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = Th095ChapterWrapper<Th095Converter>.Create(MakeByteArray(properties));
            var status = new Th095StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th095StatusTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.version;

            var chapter = Th095ChapterWrapper<Th095Converter>.Create(MakeByteArray(properties));
            var status = new Th095StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th095StatusTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size;

            var chapter = Th095ChapterWrapper<Th095Converter>.Create(MakeByteArray(properties));
            var status = new Th095StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)0, 0x458, true)]
        [DataRow("st", (ushort)0, 0x458, false)]
        [DataRow("ST", (ushort)1, 0x458, false)]
        [DataRow("ST", (ushort)0, 0x459, false)]
        public void Th095StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th095ChapterWrapper<Th095Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

                Assert.AreEqual(expected, Th095StatusWrapper.CanInitialize(chapter));
            });
    }
}
