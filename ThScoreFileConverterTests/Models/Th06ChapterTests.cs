using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th06ChapterTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public byte[] data;
        };

        internal static Properties DefaultProperties => new Properties()
        {
            signature = string.Empty,
            size1 = default,
            size2 = default,
            data = new byte[] { }
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "ABCD",
            size1 = 12,
            size2 = 34,
            data = new byte[] { 0x56, 0x78, 0x9A, 0xBC }
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.size1,
                properties.size2,
                properties.data);

        internal static void Validate(in Th06ChapterWrapper chapter, in Properties properties)
        {
            Assert.AreEqual(properties.signature, chapter.Signature);
            Assert.AreEqual(properties.size1, chapter.Size1);
            Assert.AreEqual(properties.size2, chapter.Size2);
            CollectionAssert.AreEqual(properties.data, chapter.Data.ToArray());
            Assert.AreEqual((properties.data?.Length > 0 ? properties.data[0] : default), chapter.FirstByteOfData);
        }

        [TestMethod]
        public void Th06ChapterTest()
            => TestUtils.Wrap(() =>
            {
                var chapter = new Th06ChapterWrapper();

                Validate(chapter, DefaultProperties);
            });

        [TestMethod]
        public void Th06ChapterTestCopy()
            => TestUtils.Wrap(() =>
            {
                var chapter1 = new Th06ChapterWrapper();
                var chapter2 = new Th06ChapterWrapper(chapter1);

                Validate(chapter2, DefaultProperties);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "chapter")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06ChapterTestNull()
            => TestUtils.Wrap(() =>
            {
                var chapter = new Th06ChapterWrapper(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void Th06ChapterReadFromTest()
            => TestUtils.Wrap(() =>
            {
                var chapter = Th06ChapterWrapper.Create(MakeByteArray(ValidProperties));

                Validate(chapter, ValidProperties);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06ChapterReadFromTestNull()
            => TestUtils.Wrap(() =>
            {
                var chapter = new Th06ChapterWrapper();
                chapter.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestEmptySignature()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = string.Empty;

                // <-- sig --> size1 size2 <- data -->
                // __ __ __ __ 0c 00 22 00 56 78 9a bc
                //             <-- sig --> size1 size2 <dat>

                // The actual value of the Size1 property becomes too large and
                // the Data property becomes empty,
                // so EndOfStreamException will be thrown.
                Th06ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestShortenedSignature()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

                // <-- sig --> size1 size2 <- data -->
                // __ 41 42 43 0c 00 22 00 56 78 9a bc
                //    <-- sig --> size1 size2 < dat ->

                // The actual value of the Size1 property becomes too large,
                // so EndOfStreamException will be thrown.
                Th06ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestExceededSignature()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature += "E";

                // <--- sig ----> size1 size2 <- data -->
                // 41 42 43 44 45 0c 00 22 00 56 78 9a bc
                // <-- sig --> size1 size2 <---- data ---->

                // The actual value of the Size1 property becomes too large,
                // so EndOfStreamException will be thrown.
                Th06ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th06ChapterReadFromTestNegativeSize1()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size1 = -1;

                Th06ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th06ChapterReadFromTestZeroSize1()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size1 = 0;

                Th06ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void Th06ChapterReadFromTestShortenedSize1()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size1;

                var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));

                Assert.AreEqual(properties.signature, chapter.Signature);
                Assert.AreEqual(properties.size1, chapter.Size1);
                Assert.AreEqual(properties.size2, chapter.Size2);
                CollectionAssert.AreNotEqual(properties.data, chapter.Data.ToArray());
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestExceededSize1()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                ++properties.size1;

                Th06ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void Th06ChapterReadFromTestNegativeSize2()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size2 = -1;

                var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));

                Validate(chapter, properties);
            });

        [TestMethod]
        public void Th06ChapterReadFromTestZeroSize2()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.size2 = 0;

                var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));

                Validate(chapter, properties);
            });

        [TestMethod]
        public void Th06ChapterReadFromTestShortenedSize2()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size2;

                var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));

                Validate(chapter, properties);
            });

        [TestMethod]
        public void Th06ChapterReadFromTestExceededSize2()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                ++properties.size2;

                var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));

                Validate(chapter, properties);
            });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th06ChapterReadFromTestEmptyData()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.data = new byte[] { };

                Th06ChapterWrapper.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [TestMethod]
        public void Th06ChapterReadFromTestMisalignedData()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size1;
                properties.data = properties.data.Take(properties.data.Length - 1).ToArray();

                var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));

                Validate(chapter, properties);
            });
    }
}
