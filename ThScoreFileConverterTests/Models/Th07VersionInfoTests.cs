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
    public class Th07VersionInfoTests
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

        internal static void Validate<TParent>(in Th07VersionInfoWrapper<TParent> versionInfo, in Properties properties)
            where TParent : ThConverter
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, versionInfo.Signature);
            Assert.AreEqual(properties.size1, versionInfo.Size1);
            Assert.AreEqual(properties.size2, versionInfo.Size2);
            CollectionAssert.AreEqual(data, versionInfo.Data.ToArray());
            Assert.AreEqual(data[0], versionInfo.FirstByteOfData);
            CollectionAssert.AreEqual(properties.version, versionInfo.Version.ToArray());
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void VersionInfoTestChapterHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var versionInfo = new Th07VersionInfoWrapper<TParent>(chapter);

                Validate(versionInfo, properties);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "versionInfo")]
        internal static void VersionInfoTestNullChapterHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var versionInfo = new Th07VersionInfoWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "versionInfo")]
        internal static void VersionInfoTestInvalidSignatureHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = properties.signature.ToLowerInvariant();

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var versionInfo = new Th07VersionInfoWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "versionInfo")]
        internal static void VersionInfoTestInvalidSize1Helper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                ++properties.size1;

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var versionInfo = new Th07VersionInfoWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        #region Th07

        [TestMethod]
        public void Th07VersionInfoTestChapter()
            => VersionInfoTestChapterHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07VersionInfoTestNullChapter()
            => VersionInfoTestNullChapterHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07VersionInfoTestInvalidSignature()
            => VersionInfoTestInvalidSignatureHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07VersionInfoTestInvalidSize1()
            => VersionInfoTestInvalidSize1Helper<Th07Converter>();

        #endregion

        #region Th08

        [TestMethod]
        public void Th08VersionInfoTestChapter()
            => VersionInfoTestChapterHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08VersionInfoTestNullChapter()
            => VersionInfoTestNullChapterHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08VersionInfoTestInvalidSignature()
            => VersionInfoTestInvalidSignatureHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08VersionInfoTestInvalidSize1()
            => VersionInfoTestInvalidSize1Helper<Th08Converter>();

        #endregion

        #region Th09

        [TestMethod]
        public void Th09VersionInfoTestChapter()
            => VersionInfoTestChapterHelper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09VersionInfoTestNullChapter()
            => VersionInfoTestNullChapterHelper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09VersionInfoTestInvalidSignature()
            => VersionInfoTestInvalidSignatureHelper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09VersionInfoTestInvalidSize1()
            => VersionInfoTestInvalidSize1Helper<Th09Converter>();

        #endregion
    }
}
