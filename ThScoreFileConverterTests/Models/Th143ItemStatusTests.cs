using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th143ItemStatusTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public uint checksum;
            public int size;
            public Th143Converter.ItemWithTotal item;
            public int useCount;
            public int clearedCount;
            public int clearedScenes;
            public int itemLevel;
            public int availableCount;
            public int framesOrRanges;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "TI",
            version = 1,
            checksum = 0u,
            size = 0x34,
            item = Th143Converter.ItemWithTotal.NoItem,
            useCount = 98,
            clearedCount = 76,
            clearedScenes = 54,
            itemLevel = 3,
            availableCount = 2,
            framesOrRanges = 1
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                (int)properties.item,
                properties.useCount,
                properties.clearedCount,
                properties.clearedScenes,
                properties.itemLevel,
                0,
                properties.availableCount,
                properties.framesOrRanges,
                new int[2]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                MakeData(properties));

        internal static void Validate(in Th143ItemStatusWrapper itemStatus, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, itemStatus.Signature);
            Assert.AreEqual(properties.version, itemStatus.Version);
            Assert.AreEqual(properties.checksum, itemStatus.Checksum);
            Assert.AreEqual(properties.size, itemStatus.Size);
            CollectionAssert.AreEqual(data, itemStatus.Data.ToArray());
            Assert.AreEqual(properties.item, itemStatus.Item);
            Assert.AreEqual(properties.useCount, itemStatus.UseCount);
            Assert.AreEqual(properties.clearedCount, itemStatus.ClearedCount);
            Assert.AreEqual(properties.clearedScenes, itemStatus.ClearedScenes);
            Assert.AreEqual(properties.itemLevel, itemStatus.ItemLevel);
            Assert.AreEqual(properties.availableCount, itemStatus.AvailableCount);
            Assert.AreEqual(properties.framesOrRanges, itemStatus.FramesOrRanges);
        }

        [TestMethod]
        public void Th143ItemStatusTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var itemStatus = new Th143ItemStatusWrapper(chapter);

            Validate(itemStatus, properties);
            Assert.IsFalse(itemStatus.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "itemStatus")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th143ItemStatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            var itemStatus = new Th143ItemStatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "itemStatus")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143ItemStatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var itemStatus = new Th143ItemStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "itemStatus")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143ItemStatusTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.version;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var itemStatus = new Th143ItemStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "itemStatus")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143ItemStatusTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var itemStatus = new Th143ItemStatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidItems
            => TestUtils.GetInvalidEnumerators(typeof(Th143Converter.ItemWithTotal));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "itemStatus")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidItems))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th143ItemStatusTestInvalidItems(int item) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.item = TestUtils.Cast<Th143Converter.ItemWithTotal>(item);

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));
            var itemStatus = new Th143ItemStatusWrapper(chapter);

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
