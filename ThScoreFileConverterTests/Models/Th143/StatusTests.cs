using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th143;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th143.Stubs;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class StatusTests
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
            NicknameFlags = Enumerable.Range(0, 71).Select(value => (byte)((value % 3 == 0) ? 0 : 1)).ToArray()
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
        public void StatusTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var status = new Status(chapter);

            Validate(stub, status);
            Assert.IsFalse(status.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Status(null!);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new StatusStub(ValidStub);
            stub.Signature = stub.Signature.ToLower(CultureInfo.InvariantCulture);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = new StatusStub(ValidStub);
            stub.Version += 1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void StatusTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = new StatusStub(ValidStub);
            --stub.Size;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [DataTestMethod]
        [DataRow("ST", (ushort)1, 0x224, true)]
        [DataRow("st", (ushort)1, 0x224, false)]
        [DataRow("ST", (ushort)0, 0x224, false)]
        [DataRow("ST", (ushort)1, 0x225, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = TestUtils.Create<Chapter>(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Status.CanInitialize(chapter));
            });

        public static IEnumerable<object[]> InvalidItems
            => TestUtils.GetInvalidEnumerators(typeof(ItemWithTotal));

        [DataTestMethod]
        [DynamicData(nameof(InvalidItems))]
        [ExpectedException(typeof(InvalidCastException))]
        public void StatusTestInvalidLastMainItem(int item) => TestUtils.Wrap(() =>
        {
            var stub = new StatusStub(ValidStub)
            {
                LastMainItem = TestUtils.Cast<ItemWithTotal>(item),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [DataTestMethod]
        [DynamicData(nameof(InvalidItems))]
        [ExpectedException(typeof(InvalidCastException))]
        public void StatusTestInvalidLastSubItem(int item) => TestUtils.Wrap(() =>
        {
            var stub = new StatusStub(ValidStub)
            {
                LastSubItem = TestUtils.Cast<ItemWithTotal>(item),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new Status(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
