using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Th143.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th143ItemStatusTests
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
            FramesOrRanges = 1
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

        internal static void Validate(IItemStatus expected, in Th143ItemStatusWrapper actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(data, actual.Data);
            Assert.AreEqual(expected.Item, actual.Item);
            Assert.AreEqual(expected.UseCount, actual.UseCount);
            Assert.AreEqual(expected.ClearedCount, actual.ClearedCount);
            Assert.AreEqual(expected.ClearedScenes, actual.ClearedScenes);
            Assert.AreEqual(expected.ItemLevel, actual.ItemLevel);
            Assert.AreEqual(expected.AvailableCount, actual.AvailableCount);
            Assert.AreEqual(expected.FramesOrRanges, actual.FramesOrRanges);
        }

        [TestMethod]
        public void Th143ItemStatusTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var itemStatus = new Th143ItemStatusWrapper(chapter);

            Validate(stub, itemStatus);
            Assert.IsFalse(itemStatus.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th143ItemStatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th143ItemStatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143ItemStatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new ItemStatusStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th143ItemStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143ItemStatusTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = new ItemStatusStub(ValidStub);
            ++stub.Version;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th143ItemStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143ItemStatusTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new ItemStatusStub(ValidStub);
            --stub.Size;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th143ItemStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidItems
            => TestUtils.GetInvalidEnumerators(typeof(ItemWithTotal));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidItems))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th143ItemStatusTestInvalidItems(int item) => TestUtils.Wrap(() =>
        {
            var stub = new ItemStatusStub(ValidStub)
            {
                Item = TestUtils.Cast<ItemWithTotal>(item),
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th143ItemStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("TI", (ushort)1, 0x34, true)]
        [DataRow("ti", (ushort)1, 0x34, false)]
        [DataRow("TI", (ushort)0, 0x34, false)]
        [DataRow("TI", (ushort)1, 0x35, false)]
        public void Th143ItemStatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th143ItemStatusWrapper.CanInitialize(chapter));
            });
    }
}
