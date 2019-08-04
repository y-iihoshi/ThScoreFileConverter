using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Th07.Wrappers;

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

        internal static void Validate(in VersionInfoWrapper versionInfo, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, versionInfo.Signature);
            Assert.AreEqual(properties.size1, versionInfo.Size1);
            Assert.AreEqual(properties.size2, versionInfo.Size2);
            CollectionAssert.AreEqual(data, versionInfo.Data.ToArray());
            Assert.AreEqual(data[0], versionInfo.FirstByteOfData);
            CollectionAssert.AreEqual(properties.version, versionInfo.Version.ToArray());
        }

        [TestMethod]
        public void Th07VersionInfoTestChapter()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var chapter = ChapterWrapper.Create(MakeByteArray(properties));
                var versionInfo = new VersionInfoWrapper(chapter);

                Validate(versionInfo, properties);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "versionInfo")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07VersionInfoTestNullChapter()
            => TestUtils.Wrap(() =>
            {
                var versionInfo = new VersionInfoWrapper(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "versionInfo")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07VersionInfoTestInvalidSignature()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = properties.signature.ToLowerInvariant();

                var chapter = ChapterWrapper.Create(MakeByteArray(properties));
                var versionInfo = new VersionInfoWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "versionInfo")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07VersionInfoTestInvalidSize1()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size1;

                var chapter = ChapterWrapper.Create(MakeByteArray(properties));
                var versionInfo = new VersionInfoWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });
    }
}
