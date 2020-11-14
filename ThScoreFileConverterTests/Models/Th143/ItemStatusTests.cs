using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models.Th143;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class ItemStatusTests
    {
        internal static Mock<IItemStatus> MockItemStatus()
        {
            var mock = new Mock<IItemStatus>();
            _ = mock.SetupGet(m => m.Signature).Returns("TI");
            _ = mock.SetupGet(m => m.Version).Returns(1);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0x34);
            _ = mock.SetupGet(m => m.Item).Returns(ItemWithTotal.NoItem);
            _ = mock.SetupGet(m => m.UseCount).Returns(98);
            _ = mock.SetupGet(m => m.ClearedCount).Returns(76);
            _ = mock.SetupGet(m => m.ClearedScenes).Returns(54);
            _ = mock.SetupGet(m => m.ItemLevel).Returns(3);
            _ = mock.SetupGet(m => m.AvailableCount).Returns(2);
            _ = mock.SetupGet(m => m.FramesOrRanges).Returns(1);
            return mock;
        }

        internal static byte[] MakeData(IItemStatus itemStatus)
            => TestUtils.MakeByteArray(
                (int)itemStatus.Item,
                itemStatus.UseCount,
                itemStatus.ClearedCount,
                itemStatus.ClearedScenes,
                itemStatus.ItemLevel,
                0,
                itemStatus.AvailableCount,
                itemStatus.FramesOrRanges,
                new int[2]);

        internal static byte[] MakeByteArray(IItemStatus itemStatus)
            => TestUtils.MakeByteArray(
                itemStatus.Signature.ToCharArray(),
                itemStatus.Version,
                itemStatus.Checksum,
                itemStatus.Size,
                MakeData(itemStatus));

        internal static void Validate(IItemStatus expected, IItemStatus actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.Item, actual.Item);
            Assert.AreEqual(expected.UseCount, actual.UseCount);
            Assert.AreEqual(expected.ClearedCount, actual.ClearedCount);
            Assert.AreEqual(expected.ClearedScenes, actual.ClearedScenes);
            Assert.AreEqual(expected.ItemLevel, actual.ItemLevel);
            Assert.AreEqual(expected.AvailableCount, actual.AvailableCount);
            Assert.AreEqual(expected.FramesOrRanges, actual.FramesOrRanges);
        }

        [TestMethod]
        public void ItemStatusTestChapter()
        {
            var mock = MockItemStatus();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var itemStatus = new ItemStatus(chapter);

            Validate(mock.Object, itemStatus);
            Assert.IsFalse(itemStatus.IsValid);
        }

        [TestMethod]
        public void ItemStatusTestNullChapter()
            => _ = Assert.ThrowsException<ArgumentNullException>(() => _ = new ItemStatus(null!));

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        public void ItemStatusTestInvalidSignature()
        {
            var mock = MockItemStatus();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new ItemStatus(chapter));
        }

        [TestMethod]
        public void ItemStatusTestInvalidVersion()
        {
            var mock = MockItemStatus();
            var version = mock.Object.Version;
            _ = mock.SetupGet(m => m.Version).Returns(++version);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new ItemStatus(chapter));
        }

        [TestMethod]
        public void ItemStatusTestInvalidSize()
        {
            var mock = MockItemStatus();
            var size = mock.Object.Size;
            _ = mock.SetupGet(m => m.Size).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new ItemStatus(chapter));
        }

        public static IEnumerable<object[]> InvalidItems
            => TestUtils.GetInvalidEnumerators(typeof(ItemWithTotal));

        [DataTestMethod]
        [DynamicData(nameof(InvalidItems))]
        public void ItemStatusTestInvalidItems(int item)
        {
            var mock = MockItemStatus();
            _ = mock.SetupGet(m => m.Item).Returns(TestUtils.Cast<ItemWithTotal>(item));

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidCastException>(() => _ = new ItemStatus(chapter));
        }

        [DataTestMethod]
        [DataRow("TI", (ushort)1, 0x34, true)]
        [DataRow("ti", (ushort)1, 0x34, false)]
        [DataRow("TI", (ushort)0, 0x34, false)]
        [DataRow("TI", (ushort)1, 0x35, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(expected, ItemStatus.CanInitialize(chapter));
        }
    }
}
