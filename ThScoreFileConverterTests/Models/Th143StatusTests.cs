using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th143StatusTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public uint checksum;
            public int size;
            public byte[] lastName;
            public byte[] bgmFlags;
            public int totalPlayTime;
            public Th143Converter.ItemWithTotal lastMainItem;
            public Th143Converter.ItemWithTotal lastSubItem;
            public byte[] nicknameFlags;
        };

        internal static Properties ValidProperties => new Properties()
        {
            signature = "ST",
            version = 1,
            checksum = 0u,
            size = 0x224,
            lastName = Encoding.Default.GetBytes("Player1     \0\0"),
            bgmFlags = TestUtils.MakeRandomArray<byte>(9),
            totalPlayTime = 12345678,
            lastMainItem = Th143Converter.ItemWithTotal.Camera,
            lastSubItem = Th143Converter.ItemWithTotal.Doll,
            nicknameFlags = TestUtils.MakeRandomArray<byte>(71)
        };

        internal static byte[] MakeData(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.lastName,
                new byte[0x12],
                properties.bgmFlags,
                new byte[0x17],
                properties.totalPlayTime,
                0,
                TestUtils.Cast<int>(properties.lastMainItem),
                TestUtils.Cast<int>(properties.lastSubItem),
                new byte[0x54],
                properties.nicknameFlags,
                new byte[0x12D]);

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                MakeData(properties));

        internal static void Validate(in Th143StatusWrapper status, in Properties properties)
        {
            var data = MakeData(properties);

            Assert.AreEqual(properties.signature, status.Signature);
            Assert.AreEqual(properties.version, status.Version);
            Assert.AreEqual(properties.checksum, status.Checksum);
            Assert.AreEqual(properties.size, status.Size);
            CollectionAssert.AreEqual(data, status.Data.ToArray());
            CollectionAssert.AreEqual(properties.lastName, status.LastName?.ToArray());
            CollectionAssert.AreEqual(properties.bgmFlags, status.BgmFlags?.ToArray());
            Assert.AreEqual(properties.totalPlayTime, status.TotalPlayTime);
            Assert.AreEqual(properties.lastMainItem, status.LastMainItem);
            Assert.AreEqual(properties.lastSubItem, status.LastSubItem);
            CollectionAssert.AreEqual(properties.nicknameFlags, status.NicknameFlags?.ToArray());
        }

        [TestMethod]
        public void Th143StatusTestChapter() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var chapter = Th10ChapterWrapper<Th143Converter>.Create(MakeByteArray(properties));
            var status = new Th143StatusWrapper(chapter);

            Validate(status, properties);
            Assert.IsFalse(status.IsValid.Value);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th143StatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            var status = new Th143StatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.ToLower(CultureInfo.InvariantCulture);

            var chapter = Th10ChapterWrapper<Th143Converter>.Create(MakeByteArray(properties));
            var status = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.version += 1;

            var chapter = Th10ChapterWrapper<Th143Converter>.Create(MakeByteArray(properties));
            var status = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.size += 1;

            var chapter = Th10ChapterWrapper<Th143Converter>.Create(MakeByteArray(properties));
            var status = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x224, true)]
        [DataRow("st", (ushort)1, 0x224, false)]
        [DataRow("ST", (ushort)0, 0x224, false)]
        [DataRow("ST", (ushort)1, 0x225, false)]
        public void Th143StatusCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th10ChapterWrapper<Th143Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th143StatusWrapper.CanInitialize(chapter));
            });

        public static IEnumerable<object[]> InvalidItems
            => TestUtils.GetInvalidEnumerators(typeof(Th143Converter.ItemWithTotal));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidItems))]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidLastMainItem(int item) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.lastMainItem = TestUtils.Cast<Th143Converter.ItemWithTotal>(item);

            var chapter = Th10ChapterWrapper<Th143Converter>.Create(MakeByteArray(properties));
            var status = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidItems))]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidLastSubItem(int item) => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.lastSubItem = TestUtils.Cast<Th143Converter.ItemWithTotal>(item);

            var chapter = Th10ChapterWrapper<Th143Converter>.Create(MakeByteArray(properties));
            var status = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
