using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ThScoreFileConverter.Models.Tests
{
    [TestClass()]
    public class Th095HeaderTests
    {
        internal static void Validate<TParent>(
            Th095HeaderWrapper<TParent> header,
            string signature,
            int encodedAllSize,
            int encodedBodySize,
            int decodedBodySize,
            bool isValid)
            where TParent : ThConverter
        {
            if (header == null)
                throw new ArgumentNullException(nameof(header));

            Assert.AreEqual(signature, header.Signature);
            Assert.AreEqual(encodedAllSize, header.EncodedAllSize);
            Assert.AreEqual(encodedBodySize, header.EncodedBodySize);
            Assert.AreEqual(decodedBodySize, header.DecodedBodySize);
            Assert.AreEqual(isValid, header.IsValid);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void HeaderTestHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var header = new Th095HeaderWrapper<TParent>();
                Validate(header, string.Empty, 0, 0, 0, false);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = 36;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 12;
                var decodedBodySize = 56;

                var header = Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        signature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Validate(header, signature, encodedAllSize, encodedBodySize, decodedBodySize, true);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNullHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var header = new Th095HeaderWrapper<TParent>();
                header.ReadFrom(null);
                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestEmptySignatureHelper<TParent>()
            where TParent : ThConverter
        {
            try
            {
                var signature = string.Empty;
                var encodedAllSize = 36;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 12;
                var decodedBodySize = 56;

                // <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >
                // __ __ __ __ 24 00 00 00 00 00 00 00 00 00 00 00 0c 00 00 00 38 00 00 00
                //             <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody >

                // The actual value of the DecodedBodySize property can not be read.
                // so EndOfStreamException will be thrown.
                Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        signature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedSignatureHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var shortenedSignature = signature.Substring(0, 3);
                var encodedAllSize = 36;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 12;
                var decodedBodySize = 56;

                // <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >
                // __ xx xx xx 24 00 00 00 00 00 00 00 00 00 00 00 0c 00 00 00 38 00 00 00
                //    <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >

                // The actual value of the DecodedBodySize property can not be read.
                // so EndOfStreamException will be thrown.
                Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        shortenedSignature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededSignatureHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var exceededSignature = signature + "E";
                var encodedAllSize = 36;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 12;
                var decodedBodySize = 56;

                // <--- sig ----> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >
                // xx xx xx xx 45 24 00 00 00 00 00 00 00 00 00 00 00 0c 00 00 00 38 00 00 00
                // <-- sig --> < encAll -> <- unk1 --> <- unk2 --> < encBody > < decBody >

                var header = Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        exceededSignature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Assert.AreEqual(signature, header.Signature);
                Assert.AreNotEqual(encodedAllSize, header.EncodedAllSize);
                Assert.AreNotEqual(encodedBodySize, header.EncodedBodySize);
                Assert.AreNotEqual(decodedBodySize, header.DecodedBodySize);
                Assert.IsFalse(header.IsValid.Value);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNegativeEncodedAllSizeHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = -1;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 12;
                var decodedBodySize = 56;

                Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        signature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestZeroEncodedAllSizeHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = 0;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 12;
                var decodedBodySize = 56;

                var header = Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        signature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Validate(header, signature, encodedAllSize, encodedBodySize, decodedBodySize, false);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedEncodedAllSizeHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = 35;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 12;
                var decodedBodySize = 56;

                var header = Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        signature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Validate(header, signature, encodedAllSize, encodedBodySize, decodedBodySize, false);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededEncodedAllSizeHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = 37;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 12;
                var decodedBodySize = 56;

                var header = Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        signature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Validate(header, signature, encodedAllSize, encodedBodySize, decodedBodySize, false);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNegativeEncodedBodySizeHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = 36;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = -1;
                var decodedBodySize = 56;

                Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        signature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestZeroEncodedBodySizeHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = 36;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 0;
                var decodedBodySize = 56;

                var header = Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        signature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Validate(header, signature, encodedAllSize, encodedBodySize, decodedBodySize, false);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestShortenedEncodedBodySizeHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = 36;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 11;
                var decodedBodySize = 56;

                var header = Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        signature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Validate(header, signature, encodedAllSize, encodedBodySize, decodedBodySize, false);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestExceededEncodedBodySizeHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = 36;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 13;
                var decodedBodySize = 56;

                var header = Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        signature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Validate(header, signature, encodedAllSize, encodedBodySize, decodedBodySize, false);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestNegativeDecodedBodySizeHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = 36;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 12;
                var decodedBodySize = -1;

                Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        signature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Assert.Fail(TestUtils.Unreachable);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void ReadFromTestZeroDecodedBodySizeHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = 36;
                var unknown1 = 0u;
                var unknown2 = 0u;
                var encodedBodySize = 12;
                var decodedBodySize = 0;

                var header = Th095HeaderWrapper<TParent>.Create(
                    TestUtils.MakeByteArray(
                        signature.ToCharArray(),
                        encodedAllSize,
                        unknown1,
                        unknown2,
                        encodedBodySize,
                        decodedBodySize));

                Validate(header, signature, encodedAllSize, encodedBodySize, decodedBodySize, true);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void WriteToTestHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = 36;
                var unknown1 = 78u;
                var unknown2 = 90u;
                var encodedBodySize = 12;
                var decodedBodySize = 56;
                var byteArray = TestUtils.MakeByteArray(
                    signature.ToCharArray(),
                    encodedAllSize,
                    unknown1,
                    unknown2,
                    encodedBodySize,
                    decodedBodySize);

                var header = Th095HeaderWrapper<TParent>.Create(TestUtils.MakeByteArray(byteArray));

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
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        internal static void WriteToTestNullHelper<TParent>(string signature)
            where TParent : ThConverter
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length != 4)
                throw new ArgumentException("invalid length", nameof(signature));

            try
            {
                var encodedAllSize = 36;
                var unknown1 = 78u;
                var unknown2 = 90u;
                var encodedBodySize = 12;
                var decodedBodySize = 56;
                var byteArray = TestUtils.MakeByteArray(
                    signature.ToCharArray(),
                    encodedAllSize,
                    unknown1,
                    unknown2,
                    encodedBodySize,
                    decodedBodySize);

                var header = Th095HeaderWrapper<TParent>.Create(TestUtils.MakeByteArray(byteArray));
                header.WriteTo(null);
                Assert.Fail(TestUtils.Unreachable);

            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                throw;
            }
        }

        #region Th095

        [TestMethod()]
        public void Th095HeaderTest()
            => HeaderTestHelper<Th095Converter>();

        [TestMethod()]
        public void Th095ReadFromTest()
            => ReadFromTestHelper<Th095Converter>("TH95");

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
            => ReadFromTestShortenedSignatureHelper<Th095Converter>("TH95");

        [TestMethod()]
        public void Th095ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th095Converter>("TH95");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th095Converter>("TH95");

        [TestMethod()]
        public void Th095ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th095Converter>("TH95");

        [TestMethod()]
        public void Th095ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th095Converter>("TH95");

        [TestMethod()]
        public void Th095ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th095Converter>("TH95");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th095Converter>("TH95");

        [TestMethod()]
        public void Th095ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th095Converter>("TH95");

        [TestMethod()]
        public void Th095ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th095Converter>("TH95");

        [TestMethod()]
        public void Th095ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th095Converter>("TH95");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th095ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th095Converter>("TH95");

        [TestMethod()]
        public void Th095ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th095Converter>("TH95");

        [TestMethod()]
        public void Th095WriteToTest()
            => WriteToTestHelper<Th095Converter>("TH95");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th095WriteToTestNull()
            => WriteToTestNullHelper<Th095Converter>("TH95");

        #endregion

        #region Th10

        [TestMethod()]
        public void Th10HeaderTest()
            => HeaderTestHelper<Th10Converter>();

        [TestMethod()]
        public void Th10ReadFromTest()
            => ReadFromTestHelper<Th10Converter>("TH10");

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
            => ReadFromTestShortenedSignatureHelper<Th10Converter>("TH10");

        [TestMethod()]
        public void Th10ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th10Converter>("TH10");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th10ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th10Converter>("TH10");

        [TestMethod()]
        public void Th10ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th10Converter>("TH10");

        [TestMethod()]
        public void Th10ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th10Converter>("TH10");

        [TestMethod()]
        public void Th10ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th10Converter>("TH10");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th10ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th10Converter>("TH10");

        [TestMethod()]
        public void Th10ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th10Converter>("TH10");

        [TestMethod()]
        public void Th10ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th10Converter>("TH10");

        [TestMethod()]
        public void Th10ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th10Converter>("TH10");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th10ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th10Converter>("TH10");

        [TestMethod()]
        public void Th10ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th10Converter>("TH10");

        [TestMethod()]
        public void Th10WriteToTest()
            => WriteToTestHelper<Th10Converter>("TH10");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th10WriteToTestNull()
            => WriteToTestNullHelper<Th10Converter>("TH10");

        #endregion

        #region Th11

        [TestMethod()]
        public void Th11HeaderTest()
            => HeaderTestHelper<Th11Converter>();

        [TestMethod()]
        public void Th11ReadFromTest()
            => ReadFromTestHelper<Th11Converter>("TH11");

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
            => ReadFromTestShortenedSignatureHelper<Th11Converter>("TH11");

        [TestMethod()]
        public void Th11ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th11Converter>("TH11");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th11ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th11Converter>("TH11");

        [TestMethod()]
        public void Th11ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th11Converter>("TH11");

        [TestMethod()]
        public void Th11ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th11Converter>("TH11");

        [TestMethod()]
        public void Th11ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th11Converter>("TH11");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th11ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th11Converter>("TH11");

        [TestMethod()]
        public void Th11ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th11Converter>("TH11");

        [TestMethod()]
        public void Th11ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th11Converter>("TH11");

        [TestMethod()]
        public void Th11ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th11Converter>("TH11");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th11ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th11Converter>("TH11");

        [TestMethod()]
        public void Th11ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th11Converter>("TH11");

        [TestMethod()]
        public void Th11WriteToTest()
            => WriteToTestHelper<Th11Converter>("TH11");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th11WriteToTestNull()
            => WriteToTestNullHelper<Th11Converter>("TH11");

        #endregion

        #region Th12

        [TestMethod()]
        public void Th12HeaderTest()
            => HeaderTestHelper<Th12Converter>();

        [TestMethod()]
        public void Th12ReadFromTest()
            => ReadFromTestHelper<Th12Converter>("TH21");

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
            => ReadFromTestShortenedSignatureHelper<Th12Converter>("TH21");

        [TestMethod()]
        public void Th12ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th12Converter>("TH21");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th12ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th12Converter>("TH21");

        [TestMethod()]
        public void Th12ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th12Converter>("TH21");

        [TestMethod()]
        public void Th12ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th12Converter>("TH21");

        [TestMethod()]
        public void Th12ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th12Converter>("TH21");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th12ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th12Converter>("TH21");

        [TestMethod()]
        public void Th12ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th12Converter>("TH21");

        [TestMethod()]
        public void Th12ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th12Converter>("TH21");

        [TestMethod()]
        public void Th12ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th12Converter>("TH21");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th12ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th12Converter>("TH21");

        [TestMethod()]
        public void Th12ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th12Converter>("TH21");

        [TestMethod()]
        public void Th12WriteToTest()
            => WriteToTestHelper<Th12Converter>("TH21");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th12WriteToTestNull()
            => WriteToTestNullHelper<Th12Converter>("TH21");

        #endregion

        #region Th125

        [TestMethod()]
        public void Th125HeaderTest()
            => HeaderTestHelper<Th125Converter>();

        [TestMethod()]
        public void Th125ReadFromTest()
            => ReadFromTestHelper<Th125Converter>("T125");

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
            => ReadFromTestShortenedSignatureHelper<Th125Converter>("T125");

        [TestMethod()]
        public void Th125ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th125Converter>("T125");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th125Converter>("T125");

        [TestMethod()]
        public void Th125ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th125Converter>("T125");

        [TestMethod()]
        public void Th125ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th125Converter>("T125");

        [TestMethod()]
        public void Th125ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th125Converter>("T125");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th125Converter>("T125");

        [TestMethod()]
        public void Th125ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th125Converter>("T125");

        [TestMethod()]
        public void Th125ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th125Converter>("T125");

        [TestMethod()]
        public void Th125ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th125Converter>("T125");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th125ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th125Converter>("T125");

        [TestMethod()]
        public void Th125ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th125Converter>("T125");

        [TestMethod()]
        public void Th125WriteToTest()
            => WriteToTestHelper<Th125Converter>("T125");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th125WriteToTestNull()
            => WriteToTestNullHelper<Th125Converter>("T125");

        #endregion

        #region Th128

        [TestMethod()]
        public void Th128HeaderTest()
            => HeaderTestHelper<Th128Converter>();

        [TestMethod()]
        public void Th128ReadFromTest()
            => ReadFromTestHelper<Th128Converter>("T821");

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
            => ReadFromTestShortenedSignatureHelper<Th128Converter>("T821");

        [TestMethod()]
        public void Th128ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th128Converter>("T821");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th128ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th128Converter>("T821");

        [TestMethod()]
        public void Th128ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th128Converter>("T821");

        [TestMethod()]
        public void Th128ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th128Converter>("T821");

        [TestMethod()]
        public void Th128ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th128Converter>("T821");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th128ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th128Converter>("T821");

        [TestMethod()]
        public void Th128ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th128Converter>("T821");

        [TestMethod()]
        public void Th128ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th128Converter>("T821");

        [TestMethod()]
        public void Th128ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th128Converter>("T821");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th128ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th128Converter>("T821");

        [TestMethod()]
        public void Th128ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th128Converter>("T821");

        [TestMethod()]
        public void Th128WriteToTest()
            => WriteToTestHelper<Th128Converter>("T821");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th128WriteToTestNull()
            => WriteToTestNullHelper<Th128Converter>("T821");

        #endregion

        #region Th13

        [TestMethod()]
        public void Th13HeaderTest()
            => HeaderTestHelper<Th13Converter>();

        [TestMethod()]
        public void Th13ReadFromTest()
            => ReadFromTestHelper<Th13Converter>("TH31");

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
            => ReadFromTestShortenedSignatureHelper<Th13Converter>("TH31");

        [TestMethod()]
        public void Th13ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th13Converter>("TH31");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th13ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th13Converter>("TH31");

        [TestMethod()]
        public void Th13ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th13Converter>("TH31");

        [TestMethod()]
        public void Th13ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th13Converter>("TH31");

        [TestMethod()]
        public void Th13ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th13Converter>("TH31");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th13ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th13Converter>("TH31");

        [TestMethod()]
        public void Th13ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th13Converter>("TH31");

        [TestMethod()]
        public void Th13ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th13Converter>("TH31");

        [TestMethod()]
        public void Th13ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th13Converter>("TH31");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th13ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th13Converter>("TH31");

        [TestMethod()]
        public void Th13ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th13Converter>("TH31");

        [TestMethod()]
        public void Th13WriteToTest()
            => WriteToTestHelper<Th13Converter>("TH31");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th13WriteToTestNull()
            => WriteToTestNullHelper<Th13Converter>("TH31");

        #endregion

        #region Th14

        [TestMethod()]
        public void Th14HeaderTest()
            => HeaderTestHelper<Th14Converter>();

        [TestMethod()]
        public void Th14ReadFromTest()
            => ReadFromTestHelper<Th14Converter>("TH41");

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
            => ReadFromTestShortenedSignatureHelper<Th14Converter>("TH41");

        [TestMethod()]
        public void Th14ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th14Converter>("TH41");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th14ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th14Converter>("TH41");

        [TestMethod()]
        public void Th14ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th14Converter>("TH41");

        [TestMethod()]
        public void Th14ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th14Converter>("TH41");

        [TestMethod()]
        public void Th14ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th14Converter>("TH41");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th14ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th14Converter>("TH41");

        [TestMethod()]
        public void Th14ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th14Converter>("TH41");

        [TestMethod()]
        public void Th14ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th14Converter>("TH41");

        [TestMethod()]
        public void Th14ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th14Converter>("TH41");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th14ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th14Converter>("TH41");

        [TestMethod()]
        public void Th14ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th14Converter>("TH41");

        [TestMethod()]
        public void Th14WriteToTest()
            => WriteToTestHelper<Th14Converter>("TH41");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th14WriteToTestNull()
            => WriteToTestNullHelper<Th14Converter>("TH41");

        #endregion

        #region Th143

        [TestMethod()]
        public void Th143HeaderTest()
            => HeaderTestHelper<Th143Converter>();

        [TestMethod()]
        public void Th143ReadFromTest()
            => ReadFromTestHelper<Th143Converter>("T341");

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
            => ReadFromTestShortenedSignatureHelper<Th143Converter>("T341");

        [TestMethod()]
        public void Th143ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th143Converter>("T341");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th143ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th143Converter>("T341");

        [TestMethod()]
        public void Th143ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th143Converter>("T341");

        [TestMethod()]
        public void Th143ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th143Converter>("T341");

        [TestMethod()]
        public void Th143ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th143Converter>("T341");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th143ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th143Converter>("T341");

        [TestMethod()]
        public void Th143ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th143Converter>("T341");

        [TestMethod()]
        public void Th143ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th143Converter>("T341");

        [TestMethod()]
        public void Th143ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th143Converter>("T341");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th143ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th143Converter>("T341");

        [TestMethod()]
        public void Th143ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th143Converter>("T341");

        [TestMethod()]
        public void Th143WriteToTest()
            => WriteToTestHelper<Th143Converter>("T341");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th143WriteToTestNull()
            => WriteToTestNullHelper<Th143Converter>("T341");

        #endregion

        #region Th15

        [TestMethod()]
        public void Th15HeaderTest()
            => HeaderTestHelper<Th15Converter>();

        [TestMethod()]
        public void Th15ReadFromTest()
            => ReadFromTestHelper<Th15Converter>("TH51");

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
            => ReadFromTestShortenedSignatureHelper<Th15Converter>("TH51");

        [TestMethod()]
        public void Th15ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th15Converter>("TH51");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th15ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th15Converter>("TH51");

        [TestMethod()]
        public void Th15ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th15Converter>("TH51");

        [TestMethod()]
        public void Th15ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th15Converter>("TH51");

        [TestMethod()]
        public void Th15ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th15Converter>("TH51");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th15ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th15Converter>("TH51");

        [TestMethod()]
        public void Th15ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th15Converter>("TH51");

        [TestMethod()]
        public void Th15ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th15Converter>("TH51");

        [TestMethod()]
        public void Th15ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th15Converter>("TH51");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th15ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th15Converter>("TH51");

        [TestMethod()]
        public void Th15ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th15Converter>("TH51");

        [TestMethod()]
        public void Th15WriteToTest()
            => WriteToTestHelper<Th15Converter>("TH51");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15WriteToTestNull()
            => WriteToTestNullHelper<Th15Converter>("TH51");

        #endregion

        #region Th16

        [TestMethod()]
        public void Th16HeaderTest()
            => HeaderTestHelper<Th16Converter>();

        [TestMethod()]
        public void Th16ReadFromTest()
            => ReadFromTestHelper<Th16Converter>("TH61");

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
            => ReadFromTestShortenedSignatureHelper<Th16Converter>("TH61");

        [TestMethod()]
        public void Th16ReadFromTestExceededSignature()
            => ReadFromTestExceededSignatureHelper<Th16Converter>("TH61");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th16ReadFromTestNegativeEncodedAllSize()
            => ReadFromTestNegativeEncodedAllSizeHelper<Th16Converter>("TH61");

        [TestMethod()]
        public void Th16ReadFromTestZeroEncodedAllSize()
            => ReadFromTestZeroEncodedAllSizeHelper<Th16Converter>("TH61");

        [TestMethod()]
        public void Th16ReadFromTestShortenedEncodedAllSize()
            => ReadFromTestShortenedEncodedAllSizeHelper<Th16Converter>("TH61");

        [TestMethod()]
        public void Th16ReadFromTestExceededEncodedAllSize()
            => ReadFromTestExceededEncodedAllSizeHelper<Th16Converter>("TH61");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th16ReadFromTestNegativeEncodedBodySize()
            => ReadFromTestNegativeEncodedBodySizeHelper<Th16Converter>("TH61");

        [TestMethod()]
        public void Th16ReadFromTestZeroEncodedBodySize()
            => ReadFromTestZeroEncodedBodySizeHelper<Th16Converter>("TH61");

        [TestMethod()]
        public void Th16ReadFromTestShortenedEncodedBodySize()
            => ReadFromTestShortenedEncodedBodySizeHelper<Th16Converter>("TH61");

        [TestMethod()]
        public void Th16ReadFromTestExceededEncodedBodySize()
            => ReadFromTestExceededEncodedBodySizeHelper<Th16Converter>("TH61");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Th16ReadFromTestNegativeDecodedBodySize()
            => ReadFromTestNegativeDecodedBodySizeHelper<Th16Converter>("TH61");

        [TestMethod()]
        public void Th16ReadFromTestZeroDecodedBodySize()
            => ReadFromTestZeroDecodedBodySizeHelper<Th16Converter>("TH61");

        [TestMethod()]
        public void Th16WriteToTest()
            => WriteToTestHelper<Th16Converter>("TH61");

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th16WriteToTestNull()
            => WriteToTestNullHelper<Th16Converter>("TH61");

        #endregion
    }
}
