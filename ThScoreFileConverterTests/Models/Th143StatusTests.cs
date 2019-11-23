using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Th143.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th143StatusTests
    {
        internal static StatusStub ValidStub { get; } = new StatusStub()
        {
            Signature = "ST",
            Version = 1,
            Checksum = 0u,
            Size = 0x224,
            LastName = TestUtils.CP932Encoding.GetBytes("Player1     \0\0"),
            BgmFlags = TestUtils.MakeRandomArray<byte>(9),
            TotalPlayTime = 12345678,
            LastMainItem = ItemWithTotal.Camera,
            LastSubItem = ItemWithTotal.Doll,
            NicknameFlags = TestUtils.MakeRandomArray<byte>(71)
        };

        internal static byte[] MakeData(IStatus status)
            => TestUtils.MakeByteArray(
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

        internal static byte[] MakeByteArray(IStatus status)
            => TestUtils.MakeByteArray(
                status.Signature.ToCharArray(),
                status.Version,
                status.Checksum,
                status.Size,
                MakeData(status));

        internal static void Validate(IStatus expected, in Th143StatusWrapper actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(data, actual.Data);
            CollectionAssert.That.AreEqual(expected.LastName, actual.LastName);
            CollectionAssert.That.AreEqual(expected.BgmFlags, actual.BgmFlags);
            Assert.AreEqual(expected.TotalPlayTime, actual.TotalPlayTime);
            Assert.AreEqual(expected.LastMainItem, actual.LastMainItem);
            Assert.AreEqual(expected.LastSubItem, actual.LastSubItem);
            CollectionAssert.That.AreEqual(expected.NicknameFlags, actual.NicknameFlags);
        }

        [TestMethod]
        public void Th143StatusTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var status = new Th143StatusWrapper(chapter);

            Validate(stub, status);
            Assert.IsFalse(status.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th143StatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th143StatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new StatusStub(ValidStub);
            stub.Signature = stub.Signature.ToLower(CultureInfo.InvariantCulture);

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = new StatusStub(ValidStub);
            stub.Version += 1;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new StatusStub(ValidStub);
            --stub.Size;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th143StatusWrapper(chapter);

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

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th143StatusWrapper.CanInitialize(chapter));
            });

        public static IEnumerable<object[]> InvalidItems
            => TestUtils.GetInvalidEnumerators(typeof(ItemWithTotal));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidItems))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th143StatusTestInvalidLastMainItem(int item) => TestUtils.Wrap(() =>
        {
            var stub = new StatusStub(ValidStub)
            {
                LastMainItem = TestUtils.Cast<ItemWithTotal>(item),
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidItems))]
        [ExpectedException(typeof(InvalidCastException))]
        public void Th143StatusTestInvalidLastSubItem(int item) => TestUtils.Wrap(() =>
        {
            var stub = new StatusStub(ValidStub)
            {
                LastSubItem = TestUtils.Cast<ItemWithTotal>(item),
            };

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
