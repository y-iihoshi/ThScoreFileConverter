using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

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

        internal static Properties ValidProperties => new Properties()
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

        internal static void Validate(in VersionInfo versionInfo, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, versionInfo.Signature);
            Assert.AreEqual(properties.size1, versionInfo.Size1);
            Assert.AreEqual(properties.size2, versionInfo.Size2);
            Assert.AreEqual(data[0], versionInfo.FirstByteOfData);
            CollectionAssert.AreEqual(properties.version, versionInfo.Version.ToArray());
        }

        [TestMethod]
        public void VersionInfoTestChapter()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var chapter = ChapterWrapper.Create(MakeByteArray(properties));
                var versionInfo = new VersionInfo(chapter.Target as Chapter);

                Validate(versionInfo, properties);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "versionInfo")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void VersionInfoTestNullChapter()
            => TestUtils.Wrap(() =>
            {
                var versionInfo = new VersionInfo(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "versionInfo")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void VersionInfoTestInvalidSignature()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = properties.signature.ToLowerInvariant();

                var chapter = ChapterWrapper.Create(MakeByteArray(properties));
                var versionInfo = new VersionInfo(chapter.Target as Chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "versionInfo")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void VersionInfoTestInvalidSize1()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size1;

                var chapter = ChapterWrapper.Create(MakeByteArray(properties));
                var versionInfo = new VersionInfo(chapter.Target as Chapter);

                Assert.Fail(TestUtils.Unreachable);
            });
    }
}
