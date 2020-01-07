using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th12;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th12
{
    [TestClass]
    public class StatusTests
    {
        internal static StatusStub ValidStub { get; } = Th10.StatusTests.MakeValidStub(0x0002, 17);

        [TestMethod]
        public void StatusTestChapter()
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(stub));
            var status = new Status(chapter);

            Th10.StatusTests.Validate(stub, status);
            Assert.IsFalse(status.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StatusTestNullChapter()
        {
            _ = new Status(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidSignature()
        {
            var stub = new StatusStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidVersion()
        {
            var stub = new StatusStub(ValidStub);
            ++stub.Version;

            var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidSize()
        {
            var stub = new StatusStub(ValidStub);
            ++stub.Size;

            var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [DataTestMethod]
        [DataRow("ST", (ushort)2, 0x448, true)]
        [DataRow("st", (ushort)2, 0x448, false)]
        [DataRow("ST", (ushort)1, 0x448, false)]
        [DataRow("ST", (ushort)2, 0x449, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(expected, Status.CanInitialize(chapter));
        }
    }
}
