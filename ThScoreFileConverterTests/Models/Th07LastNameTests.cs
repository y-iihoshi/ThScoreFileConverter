using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th07LastNameTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public byte[] name;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "LSNM",
            size1 = 0x18,
            size2 = 0x18,
            name = TestUtils.MakeRandomArray<byte>(12)
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(0u, properties.name);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, MakeData(properties));

        internal static void Validate<TParent>(
            in Th07LastNameWrapper<TParent> lastName, in Properties properties)
            where TParent : ThConverter
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, lastName.Signature);
            Assert.AreEqual(properties.size1, lastName.Size1);
            Assert.AreEqual(properties.size2, lastName.Size2);
            CollectionAssert.AreEqual(data, lastName.Data.ToArray());
            Assert.AreEqual(data[0], lastName.FirstByteOfData);
            CollectionAssert.AreEqual(properties.name, lastName.Name.ToArray());
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void LastNameTestChapterHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var lastName = new Th07LastNameWrapper<TParent>(chapter);

                Validate(lastName, properties);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastName")]
        internal static void LastNameTestNullChapterHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var lastName = new Th07LastNameWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastName")]
        internal static void LastNameTestInvalidSignatureHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = properties.signature.ToLowerInvariant();

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var lastName = new Th07LastNameWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastName")]
        internal static void LastNameTestInvalidSize1Helper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                ++properties.size1;

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var lastName = new Th07LastNameWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        #region Th07

        [TestMethod]
        public void Th07LastNameTestChapter()
            => LastNameTestChapterHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07LastNameTestNullChapter()
            => LastNameTestNullChapterHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07LastNameTestInvalidSignature()
            => LastNameTestInvalidSignatureHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07LastNameTestInvalidSize1()
            => LastNameTestInvalidSize1Helper<Th07Converter>();

        #endregion

        #region Th08

        [TestMethod]
        public void Th08LastNameTestChapter()
            => LastNameTestChapterHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08LastNameTestNullChapter()
            => LastNameTestNullChapterHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08LastNameTestInvalidSignature()
            => LastNameTestInvalidSignatureHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08LastNameTestInvalidSize1()
            => LastNameTestInvalidSize1Helper<Th08Converter>();

        #endregion

        #region Th09

        [TestMethod]
        public void Th09LastNameTestChapter()
            => LastNameTestChapterHelper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09LastNameTestNullChapter()
            => LastNameTestNullChapterHelper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09LastNameTestInvalidSignature()
            => LastNameTestInvalidSignatureHelper<Th09Converter>();

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09LastNameTestInvalidSize1()
            => LastNameTestInvalidSize1Helper<Th09Converter>();

        #endregion
    }
}
