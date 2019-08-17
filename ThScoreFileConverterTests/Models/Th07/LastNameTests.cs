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
    public class LastNameTests
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

        internal static void Validate(in LastName lastName, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, lastName.Signature);
            Assert.AreEqual(properties.size1, lastName.Size1);
            Assert.AreEqual(properties.size2, lastName.Size2);
            Assert.AreEqual(data[0], lastName.FirstByteOfData);
            CollectionAssert.AreEqual(properties.name, lastName.Name.ToArray());
        }

        [TestMethod]
        public void Th07LastNameTestChapter()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var chapter = ChapterWrapper.Create(MakeByteArray(properties));
                var lastName = new LastName(chapter.Target as Chapter);

                Validate(lastName, properties);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "lastName")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07LastNameTestNullChapter()
            => TestUtils.Wrap(() =>
            {
                var lastName = new LastName(null);

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

                var chapter = ChapterWrapper.Create(MakeByteArray(properties));
                var lastName = new LastName(chapter.Target as Chapter);

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

                var chapter = ChapterWrapper.Create(MakeByteArray(properties));
                var lastName = new LastName(chapter.Target as Chapter);

                Assert.Fail(TestUtils.Unreachable);
            });
    }
}
