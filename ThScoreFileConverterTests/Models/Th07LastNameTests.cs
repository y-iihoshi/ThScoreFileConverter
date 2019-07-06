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

        internal static void Validate(in Th07LastNameWrapper lastName, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, lastName.Signature);
            Assert.AreEqual(properties.size1, lastName.Size1);
            Assert.AreEqual(properties.size2, lastName.Size2);
            CollectionAssert.AreEqual(data, lastName.Data.ToArray());
            Assert.AreEqual(data[0], lastName.FirstByteOfData);
            CollectionAssert.AreEqual(properties.name, lastName.Name.ToArray());
        }

        [TestMethod]
        public void Th07LastNameTestChapter()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));
                var lastName = new Th07LastNameWrapper(chapter);

                Validate(lastName, properties);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastName")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07LastNameTestNullChapter()
            => TestUtils.Wrap(() =>
            {
                var lastName = new Th07LastNameWrapper(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastName")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07LastNameTestInvalidSignature()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.signature = properties.signature.ToLowerInvariant();

                var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));
                var lastName = new Th07LastNameWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastName")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07LastNameTestInvalidSize1()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                --properties.size1;

                var chapter = Th06ChapterWrapper.Create(MakeByteArray(properties));
                var lastName = new Th07LastNameWrapper(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });
    }
}
