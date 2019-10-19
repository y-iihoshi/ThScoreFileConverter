using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverterTests.Models.Th095
{
    [TestClass]
    public class HeaderBaseTests
    {
        internal struct Properties
        {
            public string signature;
            public int encodedAllSize;
            public int encodedBodySize;
            public int decodedBodySize;
        };

        internal static Properties DefaultProperties { get; } = new Properties()
        {
            signature = string.Empty,
            encodedAllSize = default,
            encodedBodySize = default,
            decodedBodySize = default
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "abcd",
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

        internal static void Validate(in Properties expected, in HeaderBase actual)
        {
            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.encodedAllSize, actual.EncodedAllSize);
            Assert.AreEqual(expected.encodedBodySize, actual.EncodedBodySize);
            Assert.AreEqual(expected.decodedBodySize, actual.DecodedBodySize);
        }

        [TestMethod]
        public void HeaderTest() => TestUtils.Wrap(() =>
        {
            var header = new HeaderBase();

            Validate(DefaultProperties, header);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Validate(properties, header);
            Assert.IsTrue(header.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var header = new HeaderBase();
            header.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestEmptySignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = string.Empty;

            // <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >
            // __ __ __ __ 24 00 00 00 00 00 00 00 00 00 00 00 0c 00 00 00 38 00 00 00
            //             <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody >

            // The actual value of the DecodedBodySize property can not be read.
            // so EndOfStreamException will be thrown.
            _ = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

            // <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >
            // __ xx xx xx 24 00 00 00 00 00 00 00 00 00 00 00 0c 00 00 00 38 00 00 00
            //    <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >

            // The actual value of the DecodedBodySize property can not be read.
            // so EndOfStreamException will be thrown.
            _ = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature += "e";

            // <--- sig ----> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >
            // xx xx xx xx 45 24 00 00 00 00 00 00 00 00 00 00 00 0c 00 00 00 38 00 00 00
            // <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >

            var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Assert.AreEqual(ValidProperties.signature, header.Signature);
            Assert.AreNotEqual(properties.encodedAllSize, header.EncodedAllSize);
            Assert.AreNotEqual(properties.encodedBodySize, header.EncodedBodySize);
            Assert.AreNotEqual(properties.decodedBodySize, header.DecodedBodySize);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestNegativeEncodedAllSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.encodedAllSize = -1;

            _ = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestZeroEncodedAllSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.encodedAllSize = 0;

            var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Validate(properties, header);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        public void ReadFromTestShortenedEncodedAllSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.encodedAllSize;

            var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Validate(properties, header);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        public void ReadFromTestExceededEncodedAllSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.encodedAllSize;

            var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Validate(properties, header);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestNegativeEncodedBodySize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.encodedBodySize = -1;

            _ = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestZeroEncodedBodySize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.encodedBodySize = 0;

            var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Validate(properties, header);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        public void ReadFromTestShortenedEncodedBodySize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.encodedBodySize;

            var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Validate(properties, header);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        public void ReadFromTestExceededEncodedBodySize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.encodedBodySize;

            var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Validate(properties, header);
            Assert.IsFalse(header.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ReadFromTestNegativeDecodedBodySize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.decodedBodySize = -1;

            _ = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestZeroDecodedBodySize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.decodedBodySize = 0;

            var header = TestUtils.Create<HeaderBase>(MakeByteArray(properties));

            Validate(properties, header);
            Assert.IsTrue(header.IsValid);
        });

        [TestMethod]
        public void WriteToTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            var byteArray = MakeByteArray(properties);

            var header = TestUtils.Create<HeaderBase>(byteArray);

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteToTestNull() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            var byteArray = MakeByteArray(properties);

            var header = TestUtils.Create<HeaderBase>(byteArray);
            header.WriteTo(null);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
