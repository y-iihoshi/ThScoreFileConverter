using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th07.Wrappers;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class FileHeaderTests
    {
        internal struct Properties
        {
            public ushort checksum;
            public short version;
            public int size;
            public int decodedAllSize;
            public int decodedBodySize;
            public int encodedBodySize;
        };

        internal static Properties GetValidProperties(short version, int size) => new Properties()
        {
            checksum = 12,
            version = version,
            size = size,
            decodedAllSize = 78 + size,
            decodedBodySize = 78,
            encodedBodySize = 90
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                (ushort)0,
                properties.checksum,
                properties.version,
                (ushort)0,
                properties.size,
                0u,
                properties.decodedAllSize,
                properties.decodedBodySize,
                properties.encodedBodySize);

        internal static void Validate<TParent>(in FileHeaderWrapper<TParent> header, in Properties properties)
            where TParent : ThConverter
        {
            Assert.AreEqual(properties.checksum, header.Checksum);
            Assert.AreEqual(properties.version, header.Version);
            Assert.AreEqual(properties.size, header.Size);
            Assert.AreEqual(properties.decodedAllSize, header.DecodedAllSize);
            Assert.AreEqual(properties.decodedBodySize, header.DecodedBodySize);
            Assert.AreEqual(properties.encodedBodySize, header.EncodedBodySize);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void FileHeaderTestHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties();

                var header = new FileHeaderWrapper<TParent>();

                Validate(header, properties);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestHelper<TParent>(short version, short size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size);

                var header = FileHeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsTrue(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var header = new FileHeaderWrapper<TParent>();
                header.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedHelper<TParent>(short version, short size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size);
                var array = MakeByteArray(properties);
                array = array.Take(array.Length - 1).ToArray();

                FileHeaderWrapper<TParent>.Create(array);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededHelper<TParent>(short version, short size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size);
                var array = MakeByteArray(properties).Concat(new byte[] { 1 }).ToArray();

                var header = FileHeaderWrapper<TParent>.Create(array);

                Validate(header, properties);
                Assert.IsTrue(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestInvalidVersionHelper<TParent>(short version, int size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size);
                ++properties.version;

                var header = FileHeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestInvalidSizeHelper<TParent>(short version, int size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size);
                ++properties.size;

                var header = FileHeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestInvalidDecodedAllSizeHelper<TParent>(short version, int size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size);
                ++properties.decodedAllSize;

                var header = FileHeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestInvalidDecodedBodySizeHelper<TParent>(short version, int size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties(version, size);
                ++properties.decodedBodySize;

                var header = FileHeaderWrapper<TParent>.Create(MakeByteArray(properties));

                Validate(header, properties);
                Assert.IsFalse(header.IsValid.Value);
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void WriteToTestHelper<TParent>(short version, short size)
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                MemoryStream stream = null;
                try
                {
                    stream = new MemoryStream();
                    using (var writer = new BinaryWriter(stream))
                    {
                        stream = null;

                        var properties = GetValidProperties(version, size);
                        var array = MakeByteArray(properties);

                        var header = FileHeaderWrapper<TParent>.Create(array);
                        header.WriteTo(writer);

                        writer.Flush();
                        writer.Seek(0, SeekOrigin.Begin);

                        var actualArray = new byte[writer.BaseStream.Length];
                        writer.BaseStream.Read(actualArray, 0, actualArray.Length);

                        CollectionAssert.AreEqual(array, actualArray);
                    }
                }
                finally
                {
                    stream?.Dispose();
                }
            });

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void WriteToTestNullHelper<TParent>()
            where TParent : ThConverter
            => TestUtils.Wrap(() =>
            {
                var header = new FileHeaderWrapper<TParent>();
                header.WriteTo(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        #region Th07

        [TestMethod]
        public void Th07FileHeaderTest()
            => FileHeaderTestHelper<Th07Converter>();

        [TestMethod]
        public void Th07FileHeaderReadFromTest()
            => ReadFromTestHelper<Th07Converter>(0x0B, 0x1C);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07FileHeaderReadFromTestNull()
            => ReadFromTestNullHelper<Th07Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th07FileHeaderReadFromTestShortened()
            => ReadFromTestShortenedHelper<Th07Converter>(0x0B, 0x1C);

        [TestMethod]
        public void Th07FileHeaderReadFromTestExceeded()
            => ReadFromTestExceededHelper<Th07Converter>(0x0B, 0x1C);

        [TestMethod]
        public void Th07FileHeaderReadFromTestInvalidVersion()
            => ReadFromTestInvalidVersionHelper<Th07Converter>(0x0B, 0x1C);

        [TestMethod]
        public void Th07FileHeaderReadFromTestInvalidSize()
            => ReadFromTestInvalidSizeHelper<Th07Converter>(0x0B, 0x1C);

        [TestMethod]
        public void Th07FileHeaderReadFromTestInvalidDecodedAllSize()
            => ReadFromTestInvalidDecodedAllSizeHelper<Th07Converter>(0x0B, 0x1C);

        [TestMethod]
        public void Th07FileHeaderReadFromTestInvalidDecodedBodySize()
            => ReadFromTestInvalidDecodedBodySizeHelper<Th07Converter>(0x0B, 0x1C);

        [TestMethod]
        public void Th07FileHeaderWriteToTest()
            => WriteToTestHelper<Th07Converter>(0x0B, 0x1C);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th07FileHeaderWriteToTestNull()
            => WriteToTestNullHelper<Th07Converter>();

        #endregion

        #region Th08

        [TestMethod]
        public void Th08FileHeaderTest()
            => FileHeaderTestHelper<Th08Converter>();

        [TestMethod]
        public void Th08FileHeaderReadFromTest()
            => ReadFromTestHelper<Th08Converter>(0x01, 0x1C);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08FileHeaderReadFromTestNull()
            => ReadFromTestNullHelper<Th08Converter>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void Th08FileHeaderReadFromTestShortened()
            => ReadFromTestShortenedHelper<Th08Converter>(0x01, 0x1C);

        [TestMethod]
        public void Th08FileHeaderReadFromTestExceeded()
            => ReadFromTestExceededHelper<Th08Converter>(0x01, 0x1C);

        [TestMethod]
        public void Th08FileHeaderReadFromTestInvalidVersion()
            => ReadFromTestInvalidVersionHelper<Th08Converter>(0x01, 0x1C);

        [TestMethod]
        public void Th08FileHeaderReadFromTestInvalidSize()
            => ReadFromTestInvalidSizeHelper<Th08Converter>(0x01, 0x1C);

        [TestMethod]
        public void Th08FileHeaderReadFromTestInvalidDecodedAllSize()
            => ReadFromTestInvalidDecodedAllSizeHelper<Th08Converter>(0x01, 0x1C);

        [TestMethod]
        public void Th08FileHeaderReadFromTestInvalidDecodedBodySize()
            => ReadFromTestInvalidDecodedBodySizeHelper<Th08Converter>(0x01, 0x1C);

        [TestMethod]
        public void Th08FileHeaderWriteToTest()
            => WriteToTestHelper<Th08Converter>(0x01, 0x1C);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th08FileHeaderWriteToTestNull()
            => WriteToTestNullHelper<Th08Converter>();

        #endregion
    }
}
