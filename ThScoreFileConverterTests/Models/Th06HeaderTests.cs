using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th06HeaderTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public byte[] data;
        };

        internal static Properties GetValidProperties(string signature)
            => new Properties()
            {
                signature = signature,
                size1 = 12,
                size2 = 12,
                data = new byte[] { 0x10, 0x00, 0x00, 0x00 }
            };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(), properties.size1, properties.size2, properties.data);

        internal static void Validate<TParent>(in Th06HeaderWrapper<TParent> header, in Properties properties)
            where TParent : ThConverter
        {
            Assert.AreEqual(properties.signature, header.Signature);
            Assert.AreEqual(properties.size1, header.Size1);
            Assert.AreEqual(properties.size2, header.Size2);
            CollectionAssert.AreEqual(properties.data, header.Data.ToArray());
            Assert.AreEqual(properties.data[0], header.FirstByteOfData);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th06HeaderTestHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var header = new Th06HeaderWrapper<TParent>(chapter);

                Validate(header, properties);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th06HeaderTestNullHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var header = new Th06HeaderWrapper<TParent>(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th06HeaderTestInvalidSignatureHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature.ToLowerInvariant());

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var header = new Th06HeaderWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "header")]
        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void Th06HeaderTestInvalidSize1Helper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                ++properties.size1;
                properties.data = properties.data.Concat(new byte[] { default }).ToArray();

                var chapter = Th06ChapterWrapper<TParent>.Create(MakeByteArray(properties));
                var header = new Th06HeaderWrapper<TParent>(chapter);

                Assert.Fail(TestUtils.Unreachable);
            });

        #region Th06

        private const string th06ValidSignature = "TH6K";

        [TestMethod()]
        public void Th06HeaderTest()
            => Th06HeaderTestHelper<Th06Converter>(th06ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th06HeaderTestNull()
            => Th06HeaderTestNullHelper<Th06Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06HeaderTestInvalidSignature()
            => Th06HeaderTestInvalidSignatureHelper<Th06Converter>(th06ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th06HeaderTestInvalidSize1()
            => Th06HeaderTestInvalidSize1Helper<Th06Converter>(th06ValidSignature);

        #endregion

        #region Th07

        private const string th07ValidSignature = "TH7K";

        [TestMethod()]
        public void Th07HeaderTest()
            => Th06HeaderTestHelper<Th07Converter>(th07ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07HeaderTestNull()
            => Th06HeaderTestNullHelper<Th07Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07HeaderTestInvalidSignature()
            => Th06HeaderTestInvalidSignatureHelper<Th07Converter>(th07ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th07HeaderTestInvalidSize1()
            => Th06HeaderTestInvalidSize1Helper<Th07Converter>(th07ValidSignature);

        #endregion

        #region Th08

        private const string th08ValidSignature = "TH8K";

        [TestMethod()]
        public void Th08HeaderTest()
            => Th06HeaderTestHelper<Th08Converter>(th08ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08HeaderTestNull()
            => Th06HeaderTestNullHelper<Th08Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08HeaderTestInvalidSignature()
            => Th06HeaderTestInvalidSignatureHelper<Th08Converter>(th08ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th08HeaderTestInvalidSize1()
            => Th06HeaderTestInvalidSize1Helper<Th08Converter>(th08ValidSignature);

        #endregion

        #region Th09

        private const string th09ValidSignature = "TH9K";

        [TestMethod()]
        public void Th09HeaderTest()
            => Th06HeaderTestHelper<Th09Converter>(th09ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th09HeaderTestNull()
            => Th06HeaderTestNullHelper<Th09Converter>();

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09HeaderTestInvalidSignature()
            => Th06HeaderTestInvalidSignatureHelper<Th09Converter>(th09ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th09HeaderTestInvalidSize1()
            => Th06HeaderTestInvalidSize1Helper<Th09Converter>(th09ValidSignature);

        #endregion
    }
}
