﻿using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th11;
using ThScoreFileConverterTests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IStatus = ThScoreFileConverter.Models.Th10.IStatus;

namespace ThScoreFileConverterTests.Models.Th11
{
    [TestClass]
    public class StatusTests
    {
        internal static Mock<IStatus> MockStatus()
        {
            return Th10.StatusTests.MockStatus(0x0000, 17);
        }

        [TestMethod]
        public void StatusTestChapter()
        {
            var mock = MockStatus();

            var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(mock.Object));
            var status = new Status(chapter);

            Th10.StatusTests.Validate(mock.Object, status);
            Assert.IsFalse(status.IsValid);
        }

        [TestMethod]
        public void StatusTestInvalidSignature()
        {
            var mock = MockStatus();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
        }

        [TestMethod]
        public void StatusTestInvalidVersion()
        {
            var mock = MockStatus();
            var version = mock.Object.Version;
            _ = mock.SetupGet(m => m.Version).Returns(++version);

            var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
        }

        [TestMethod]
        public void StatusTestInvalidSize()
        {
            var mock = MockStatus();
            var size = mock.Object.Size;
            _ = mock.SetupGet(m => m.Size).Returns(++size);

            var chapter = TestUtils.Create<Chapter>(Th10.StatusTests.MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
        }

        [DataTestMethod]
        [DataRow("ST", (ushort)0, 0x448, true)]
        [DataRow("st", (ushort)0, 0x448, false)]
        [DataRow("ST", (ushort)1, 0x448, false)]
        [DataRow("ST", (ushort)0, 0x449, false)]
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
