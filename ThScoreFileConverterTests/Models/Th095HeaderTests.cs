using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th095HeaderTests
    {
        internal struct Properties
        {
            public string signature;
            public int encodedAllSize;
            public int encodedBodySize;
            public int decodedBodySize;
        };

        internal static Properties DefaultProperties
            => new Properties()
            {
                signature = string.Empty,
                encodedAllSize = default,
                encodedBodySize = default,
                decodedBodySize = default
            };

        internal static Properties GetValidProperties(string signature)
            => new Properties()
            {
                signature = signature,
                encodedAllSize = 36,
                encodedBodySize = 12,
                decodedBodySize = 56
            };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.encodedAllSize,
                0u,
                0u,
                properties.encodedBodySize,
                properties.decodedBodySize);

        internal static void Validate<TParent>(in Th095HeaderWrapper<TParent> header, in Properties properties)
            where TParent : ThConverter
        {
            Assert.AreEqual(properties.signature, header.Signature);
            Assert.AreEqual(properties.encodedAllSize, header.EncodedAllSize);
            Assert.AreEqual(properties.encodedBodySize, header.EncodedBodySize);
            Assert.AreEqual(properties.decodedBodySize, header.DecodedBodySize);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void HeaderTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var header = new Th095HeaderWrapper<TParent>();

                Validate(header, DefaultProperties);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);

                var header = Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsTrue(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var header = new Th095HeaderWrapper<TParent>();
                header.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestEmptySignatureHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(string.Empty);

                // <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >
                // __ __ __ __ 24 00 00 00 00 00 00 00 00 00 00 00 0c 00 00 00 38 00 00 00
                //             <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody >

                // The actual value of the DecodedBodySize property can not be read.
                // so EndOfStreamException will be thrown.
                Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedSignatureHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature.Substring(0, signature.Length - 1));

                // <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >
                // __ xx xx xx 24 00 00 00 00 00 00 00 00 00 00 00 0c 00 00 00 38 00 00 00
                //    <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >

                // The actual value of the DecodedBodySize property can not be read.
                // so EndOfStreamException will be thrown.
                Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededSignatureHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature + "E");

                // <--- sig ----> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >
                // xx xx xx xx 45 24 00 00 00 00 00 00 00 00 00 00 00 0c 00 00 00 38 00 00 00
                // <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >

                var header = Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.AreEqual(signature, header.Signature);
                Assert.AreNotEqual(properties.encodedAllSize, header.EncodedAllSize);
                Assert.AreNotEqual(properties.encodedBodySize, header.EncodedBodySize);
                Assert.AreNotEqual(properties.decodedBodySize, header.DecodedBodySize);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNegativeEncodedAllSizeHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                properties.encodedAllSize = -1;

                Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestZeroEncodedAllSizeHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                properties.encodedAllSize = 0;

                var header = Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedEncodedAllSizeHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                --properties.encodedAllSize;

                var header = Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededEncodedAllSizeHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                ++properties.encodedAllSize;

                var header = Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNegativeEncodedBodySizeHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                properties.encodedBodySize = -1;

                Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestZeroEncodedBodySizeHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                properties.encodedBodySize = 0;

                var header = Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedEncodedBodySizeHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                --properties.encodedBodySize;

                var header = Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededEncodedBodySizeHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                ++properties.encodedBodySize;

                var header = Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNegativeDecodedBodySizeHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                properties.decodedBodySize = -1;

                Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestZeroDecodedBodySizeHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                properties.decodedBodySize = 0;

                var header = Th095HeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsTrue(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void WriteToTestHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                var byteArray = MakeByteArray(properties);

                var header = Th095HeaderWrapper<TParent>.Create(byteArray);

                MemoryStream stream = null;
                try
                {
                    stream = new MemoryStream();
                    using (var writer = new BinaryWriter(stream))
                    {
                        stream = null;
                        header.WriteTo(writer);

                        var wroteByteArray = new byte[writer.BaseStream.Length];
                        writer.BaseStream.Position = 0;
                        writer.BaseStream.Read(wroteByteArray, 0, wroteByteArray.Length);

                        CollectionAssert.AreEqual(byteArray, wroteByteArray);
                    }
                }
                finally
                {
                    stream?.Dispose();
                }
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void WriteToTestNullHelper<TParent>(string signature)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(signature);
                var byteArray = MakeByteArray(properties);

                var header = Th095HeaderWrapper<TParent>.Create(byteArray);
                header.WriteTo(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        #region Th095

        private const string th095ValidSignature = "TH95";

        [TestMethod()]
        public void Th095HeaderTest()
            => HeaderTestHelper<Th095Converter>();

        [TestMethod()]
        public void Th095ReadFromTest()
            => ReadFromTestHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095ReadFromTestNull()
            => ReadFromTestNullHelper<Th095Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th095ReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th095Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th095ReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        public void Th095ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        public void Th095ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        public void Th095ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        public void Th095ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        public void Th095ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        public void Th095ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        public void Th095ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        public void Th095ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        public void Th095WriteToTest()
            => WriteToTestHelper<Th095Converter>(th095ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095WriteToTestNull()
            => WriteToTestNullHelper<Th095Converter>(th095ValidSignature);

        #endregion

        #region Th10

        private const string th10ValidSignature = "TH10";

        [TestMethod()]
        public void Th10HeaderTest()
            => HeaderTestHelper<Th10Converter>();

        [TestMethod()]
        public void Th10ReadFromTest()
            => ReadFromTestHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10ReadFromTestNull()
            => ReadFromTestNullHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th10Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th10ReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        public void Th10ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th10ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        public void Th10ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        public void Th10ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        public void Th10ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th10ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        public void Th10ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        public void Th10ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        public void Th10ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th10ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        public void Th10ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        public void Th10WriteToTest()
            => WriteToTestHelper<Th10Converter>(th10ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10WriteToTestNull()
            => WriteToTestNullHelper<Th10Converter>(th10ValidSignature);

        #endregion

        #region Th11

        private const string th11ValidSignature = "TH11";

        [TestMethod()]
        public void Th11HeaderTest()
            => HeaderTestHelper<Th11Converter>();

        [TestMethod()]
        public void Th11ReadFromTest()
            => ReadFromTestHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11ReadFromTestNull()
            => ReadFromTestNullHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th11ReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th11Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th11ReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        public void Th11ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th11ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        public void Th11ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        public void Th11ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        public void Th11ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th11ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        public void Th11ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        public void Th11ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        public void Th11ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th11ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        public void Th11ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        public void Th11WriteToTest()
            => WriteToTestHelper<Th11Converter>(th11ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11WriteToTestNull()
            => WriteToTestNullHelper<Th11Converter>(th11ValidSignature);

        #endregion

        #region Th12

        private const string th12ValidSignature = "TH21";

        [TestMethod()]
        public void Th12HeaderTest()
            => HeaderTestHelper<Th12Converter>();

        [TestMethod()]
        public void Th12ReadFromTest()
            => ReadFromTestHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12ReadFromTestNull()
            => ReadFromTestNullHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th12ReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th12Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th12ReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        public void Th12ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th12ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        public void Th12ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        public void Th12ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        public void Th12ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th12ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        public void Th12ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        public void Th12ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        public void Th12ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th12ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        public void Th12ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        public void Th12WriteToTest()
            => WriteToTestHelper<Th12Converter>(th12ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12WriteToTestNull()
            => WriteToTestNullHelper<Th12Converter>(th12ValidSignature);

        #endregion

        #region Th125

        private const string th125ValidSignature = "T125";

        [TestMethod()]
        public void Th125HeaderTest()
            => HeaderTestHelper<Th125Converter>();

        [TestMethod()]
        public void Th125ReadFromTest()
            => ReadFromTestHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th125ReadFromTestNull()
            => ReadFromTestNullHelper<Th125Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th125ReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th125Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th125ReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        public void Th125ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        public void Th125ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        public void Th125ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        public void Th125ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        public void Th125ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        public void Th125ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        public void Th125ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        public void Th125ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        public void Th125WriteToTest()
            => WriteToTestHelper<Th125Converter>(th125ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th125WriteToTestNull()
            => WriteToTestNullHelper<Th125Converter>(th125ValidSignature);

        #endregion

        #region Th128

        private const string th128ValidSignature = "T821";

        [TestMethod()]
        public void Th128HeaderTest()
            => HeaderTestHelper<Th128Converter>();

        [TestMethod()]
        public void Th128ReadFromTest()
            => ReadFromTestHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128ReadFromTestNull()
            => ReadFromTestNullHelper<Th128Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th128ReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th128Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th128ReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        public void Th128ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th128ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        public void Th128ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        public void Th128ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        public void Th128ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th128ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        public void Th128ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        public void Th128ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        public void Th128ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th128ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        public void Th128ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        public void Th128WriteToTest()
            => WriteToTestHelper<Th128Converter>(th128ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128WriteToTestNull()
            => WriteToTestNullHelper<Th128Converter>(th128ValidSignature);

        #endregion

        #region Th13

        private const string th13ValidSignature = "TH31";

        [TestMethod()]
        public void Th13HeaderTest()
            => HeaderTestHelper<Th13Converter>();

        [TestMethod()]
        public void Th13ReadFromTest()
            => ReadFromTestHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13ReadFromTestNull()
            => ReadFromTestNullHelper<Th13Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th13ReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th13Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th13ReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        public void Th13ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th13ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        public void Th13ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        public void Th13ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        public void Th13ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th13ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        public void Th13ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        public void Th13ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        public void Th13ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th13ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        public void Th13ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        public void Th13WriteToTest()
            => WriteToTestHelper<Th13Converter>(th13ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13WriteToTestNull()
            => WriteToTestNullHelper<Th13Converter>(th13ValidSignature);

        #endregion

        #region Th14

        private const string th14ValidSignature = "TH41";

        [TestMethod()]
        public void Th14HeaderTest()
            => HeaderTestHelper<Th14Converter>();

        [TestMethod()]
        public void Th14ReadFromTest()
            => ReadFromTestHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14ReadFromTestNull()
            => ReadFromTestNullHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th14ReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th14Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th14ReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        public void Th14ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th14ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        public void Th14ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        public void Th14ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        public void Th14ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th14ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        public void Th14ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        public void Th14ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        public void Th14ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th14ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        public void Th14ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        public void Th14WriteToTest()
            => WriteToTestHelper<Th14Converter>(th14ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14WriteToTestNull()
            => WriteToTestNullHelper<Th14Converter>(th14ValidSignature);

        #endregion

        #region Th143

        private const string th143ValidSignature = "T341";

        [TestMethod()]
        public void Th143HeaderTest()
            => HeaderTestHelper<Th143Converter>();

        [TestMethod()]
        public void Th143ReadFromTest()
            => ReadFromTestHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th143ReadFromTestNull()
            => ReadFromTestNullHelper<Th143Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th143ReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th143Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th143ReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        public void Th143ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th143ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        public void Th143ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        public void Th143ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        public void Th143ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th143ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        public void Th143ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        public void Th143ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        public void Th143ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th143ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        public void Th143ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        public void Th143WriteToTest()
            => WriteToTestHelper<Th143Converter>(th143ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th143WriteToTestNull()
            => WriteToTestNullHelper<Th143Converter>(th143ValidSignature);

        #endregion

        #region Th15

        private const string th15ValidSignature = "TH51";

        [TestMethod()]
        public void Th15HeaderTest()
            => HeaderTestHelper<Th15Converter>();

        [TestMethod()]
        public void Th15ReadFromTest()
            => ReadFromTestHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15ReadFromTestNull()
            => ReadFromTestNullHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th15ReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th15Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th15ReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        public void Th15ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th15ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        public void Th15ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        public void Th15ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        public void Th15ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th15ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        public void Th15ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        public void Th15ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        public void Th15ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th15ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        public void Th15ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        public void Th15WriteToTest()
            => WriteToTestHelper<Th15Converter>(th15ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15WriteToTestNull()
            => WriteToTestNullHelper<Th15Converter>(th15ValidSignature);

        #endregion

        #region Th16

        private const string th16ValidSignature = "TH61";

        [TestMethod()]
        public void Th16HeaderTest()
            => HeaderTestHelper<Th16Converter>();

        [TestMethod()]
        public void Th16ReadFromTest()
            => ReadFromTestHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16ReadFromTestNull()
            => ReadFromTestNullHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th16ReadFromTestEmptySignature()
            => ReadFromTestEmptySignatureHelper<Th16Converter>();

        [TestMethod()]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th16ReadFromTestShortenedSignature()
            => ReadFromTestShortenedSignatureHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        public void Th16ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th16ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        public void Th16ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        public void Th16ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        public void Th16ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th16ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        public void Th16ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        public void Th16ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        public void Th16ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th16ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        public void Th16ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        public void Th16WriteToTest()
            => WriteToTestHelper<Th16Converter>(th16ValidSignature);

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16WriteToTestNull()
            => WriteToTestNullHelper<Th16Converter>(th16ValidSignature);

        #endregion
    }
}
