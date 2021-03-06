﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class StatusTests
    {
        internal static Mock<IStatus> MockStatus()
        {
            var mock = new Mock<IStatus>();
            _ = mock.SetupGet(m => m.Signature).Returns("ST");
            _ = mock.SetupGet(m => m.Version).Returns(1);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0x224);
            _ = mock.SetupGet(m => m.LastName).Returns(TestUtils.CP932Encoding.GetBytes("Player1     \0\0"));
            _ = mock.SetupGet(m => m.BgmFlags).Returns(TestUtils.MakeRandomArray<byte>(9));
            _ = mock.SetupGet(m => m.TotalPlayTime).Returns(12345678);
            _ = mock.SetupGet(m => m.LastMainItem).Returns(ItemWithTotal.Camera);
            _ = mock.SetupGet(m => m.LastSubItem).Returns(ItemWithTotal.Doll);
            _ = mock.SetupGet(m => m.NicknameFlags).Returns(
                Enumerable.Range(0, 71).Select(value => (byte)((value % 3 == 0) ? 0 : 1)).ToArray());
            return mock;
        }

        internal static byte[] MakeByteArray(IStatus status)
        {
            return TestUtils.MakeByteArray(
                status.Signature.ToCharArray(),
                status.Version,
                status.Checksum,
                status.Size,
                status.LastName,
                new byte[0x12],
                status.BgmFlags,
                new byte[0x17],
                status.TotalPlayTime,
                0,
                TestUtils.Cast<int>(status.LastMainItem),
                TestUtils.Cast<int>(status.LastSubItem),
                new byte[0x54],
                status.NicknameFlags,
                new byte[0x12D]);
        }

        internal static void Validate(IStatus expected, IStatus actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(expected.LastName, actual.LastName);
            CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
            Assert.AreEqual(expected.TotalPlayTime, actual.TotalPlayTime);
            Assert.AreEqual(expected.LastMainItem, actual.LastMainItem);
            Assert.AreEqual(expected.LastSubItem, actual.LastSubItem);
            CollectionAssert.That.AreEqual(expected.NicknameFlags, actual.NicknameFlags);
        }

        [TestMethod]
        public void StatusTestChapter()
        {
            var mock = MockStatus();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var status = new Status(chapter);

            Validate(mock.Object, status);
            Assert.IsFalse(status.IsValid);
        }

        [TestMethod]
        public void StatusTestInvalidSignature()
        {
            var mock = MockStatus();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
        }

        [TestMethod]
        public void StatusTestInvalidVersion()
        {
            var mock = MockStatus();
            var version = mock.Object.Version;
            _ = mock.SetupGet(m => m.Version).Returns(++version);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
        }

        [TestMethod]
        public void StatusTestInvalidSize()
        {
            var mock = MockStatus();
            var size = mock.Object.Size;
            _ = mock.SetupGet(m => m.Size).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => new Status(chapter));
        }

        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x224, true)]
        [DataRow("st", (ushort)1, 0x224, false)]
        [DataRow("ST", (ushort)0, 0x224, false)]
        [DataRow("ST", (ushort)1, 0x225, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(expected, Status.CanInitialize(chapter));
        }

        public static IEnumerable<object[]> InvalidItems
            => TestUtils.GetInvalidEnumerators(typeof(ItemWithTotal));

        [DataTestMethod]
        [DynamicData(nameof(InvalidItems))]
        public void StatusTestInvalidLastMainItem(int item)
        {
            var mock = MockStatus();
            _ = mock.SetupGet(m => m.LastMainItem).Returns(TestUtils.Cast<ItemWithTotal>(item));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidCastException>(() => new Status(chapter));
        }

        [DataTestMethod]
        [DynamicData(nameof(InvalidItems))]
        public void StatusTestInvalidLastSubItem(int item)
        {
            var mock = MockStatus();
            _ = mock.SetupGet(m => m.LastSubItem).Returns(TestUtils.Cast<ItemWithTotal>(item));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidCastException>(() => new Status(chapter));
        }
    }
}
