using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Extensions;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class VersionInfoTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public byte[] version;
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "VRSM",
            size1 = 0x1C,
            size2 = 0x1C,
            version = TestUtils.MakeRandomArray<byte>(6)
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(0u, properties.version, new byte[3], new byte[3], 0u);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate(in Properties expected, in VersionInfo actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.size1, actual.Size1);
            Assert.AreEqual(expected.size2, actual.Size2);
            Assert.AreEqual(data[0], actual.FirstByteOfData);
            CollectionAssert.That.AreEqual(expected.version, actual.Version);
        }

        [TestMethod]
        public void VersionInfoTestChapter()
        {
            var properties = ValidProperties;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            var versionInfo = new VersionInfo(chapter);

            Validate(properties, versionInfo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void VersionInfoTestNullChapter()
        {
            _ = new VersionInfo(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void VersionInfoTestInvalidSignature()
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            _ = new VersionInfo(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void VersionInfoTestInvalidSize1()
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(properties));
            _ = new VersionInfo(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
