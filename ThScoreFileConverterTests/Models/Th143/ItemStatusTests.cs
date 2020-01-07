using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.Models.Th143.Stubs;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class ItemStatusTests
    {
        internal static ItemStatusStub ValidStub { get; } = new ItemStatusStub()
        {
            Signature = "TI",
            Version = 1,
            Checksum = 0u,
            Size = 0x34,
            Item = ItemWithTotal.NoItem,
            UseCount = 98,
            ClearedCount = 76,
            ClearedScenes = 54,
            ItemLevel = 3,
            AvailableCount = 2,
            FramesOrRanges = 1,
        };

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
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var itemStatus = new ItemStatus(chapter);

            Validate(stub, itemStatus);
            Assert.IsFalse(itemStatus.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ItemStatusTestNullChapter()
        {
            _ = new ItemStatus(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ItemStatusTestInvalidSignature()
        {
            var stub = new ItemStatusStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ItemStatus(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ItemStatusTestInvalidVersion()
        {
            var stub = new ItemStatusStub(ValidStub);
            ++stub.Version;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ItemStatus(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ItemStatusTestInvalidSize()
        {
            var stub = new ItemStatusStub(ValidStub);
            --stub.Size;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ItemStatus(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        public static IEnumerable<object[]> InvalidItems
            => TestUtils.GetInvalidEnumerators(typeof(ItemWithTotal));

        [DataTestMethod]
        [DynamicData(nameof(InvalidItems))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ItemStatusTestInvalidItems(int item)
        {
            var stub = new ItemStatusStub(ValidStub)
            {
                Item = TestUtils.Cast<ItemWithTotal>(item),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ItemStatus(chapter);

            Assert.Fail(TestUtils.Unreachable);
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
