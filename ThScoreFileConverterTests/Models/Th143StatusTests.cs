using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass()]
    public class Th143StatusTests
    {
        [TestMethod()]
        public void Th143StatusTestChapter() => TestUtils.Wrap(() =>
        {
            var signature = "ST";
            var version = (ushort)1;
            var size = 0x224;
            var checksum = 0u;
            var lastName = "Player1     \0\0";
            var unknown1 = TestUtils.MakeRandomArray<byte>(0x12);
            var bgmFlags = TestUtils.MakeRandomArray<byte>(9);
            var unknown2 = TestUtils.MakeRandomArray<byte>(0x17);
            var totalPlayTime = 12345678;
            var unknown3 = 0;
            var lastMainItem = Th143Converter.ItemWithTotal.Camera;
            var lastSubItem = Th143Converter.ItemWithTotal.Doll;
            var unknown4 = TestUtils.MakeRandomArray<byte>(0x54);
            var nicknameFlags = TestUtils.MakeRandomArray<byte>(71);
            var unknown5 = TestUtils.MakeRandomArray<byte>(0x12D);
            var data = TestUtils.MakeByteArray(
                lastName.ToCharArray(),
                unknown1,
                bgmFlags,
                unknown2,
                totalPlayTime,
                unknown3,
                (int)lastMainItem,
                (int)lastSubItem,
                unknown4,
                nicknameFlags,
                unknown5);

            var chapter = Th095ChapterWrapper<Th143Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
            var status = new Th143StatusWrapper(chapter);

            Assert.AreEqual(signature, status.Signature);
            Assert.AreEqual(version, status.Version);
            Assert.AreEqual(size, status.Size);
            Assert.AreEqual(checksum, status.Checksum);
            Assert.IsFalse(status.IsValid.Value);
            CollectionAssert.AreEqual(data, status.Data.ToArray());
            CollectionAssert.AreEqual(Encoding.Default.GetBytes(lastName), status.LastName.ToArray());
            CollectionAssert.AreEqual(bgmFlags, status.BgmFlags.ToArray());
            Assert.AreEqual(totalPlayTime, status.TotalPlayTime);
            Assert.AreEqual(lastMainItem, status.LastMainItem);
            Assert.AreEqual(lastSubItem, status.LastSubItem);
            CollectionAssert.AreEqual(nicknameFlags, status.NicknameFlags.ToArray());
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th143StatusTestNullChapter() => TestUtils.Wrap(() =>
        {
            var status = new Th143StatusWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var signature = "st";
            var version = (ushort)1;
            var size = 0x224;
            var checksum = 0u;
            var lastName = "Player1     \0\0";
            var unknown1 = TestUtils.MakeRandomArray<byte>(0x12);
            var bgmFlags = TestUtils.MakeRandomArray<byte>(9);
            var unknown2 = TestUtils.MakeRandomArray<byte>(0x17);
            var totalPlayTime = 12345678;
            var unknown3 = 0;
            var lastMainItem = Th143Converter.ItemWithTotal.Camera;
            var lastSubItem = Th143Converter.ItemWithTotal.Doll;
            var unknown4 = TestUtils.MakeRandomArray<byte>(0x54);
            var nicknameFlags = TestUtils.MakeRandomArray<byte>(71);
            var unknown5 = TestUtils.MakeRandomArray<byte>(0x12D);
            var data = TestUtils.MakeByteArray(
                lastName.ToCharArray(),
                unknown1,
                bgmFlags,
                unknown2,
                totalPlayTime,
                unknown3,
                (int)lastMainItem,
                (int)lastSubItem,
                unknown4,
                nicknameFlags,
                unknown5);

            var chapter = Th095ChapterWrapper<Th143Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
            var status = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var signature = "ST";
            var version = (ushort)0;
            var size = 0x224;
            var checksum = 0u;
            var lastName = "Player1     \0\0";
            var unknown1 = TestUtils.MakeRandomArray<byte>(0x12);
            var bgmFlags = TestUtils.MakeRandomArray<byte>(9);
            var unknown2 = TestUtils.MakeRandomArray<byte>(0x17);
            var totalPlayTime = 12345678;
            var unknown3 = 0;
            var lastMainItem = Th143Converter.ItemWithTotal.Camera;
            var lastSubItem = Th143Converter.ItemWithTotal.Doll;
            var unknown4 = TestUtils.MakeRandomArray<byte>(0x54);
            var nicknameFlags = TestUtils.MakeRandomArray<byte>(71);
            var unknown5 = TestUtils.MakeRandomArray<byte>(0x12D);
            var data = TestUtils.MakeByteArray(
                lastName.ToCharArray(),
                unknown1,
                bgmFlags,
                unknown2,
                totalPlayTime,
                unknown3,
                (int)lastMainItem,
                (int)lastSubItem,
                unknown4,
                nicknameFlags,
                unknown5);

            var chapter = Th095ChapterWrapper<Th143Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
            var status = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var signature = "ST";
            var version = (ushort)1;
            var size = 0x225;
            var checksum = 0u;
            var lastName = "Player1     \0\0";
            var unknown1 = TestUtils.MakeRandomArray<byte>(0x12);
            var bgmFlags = TestUtils.MakeRandomArray<byte>(9);
            var unknown2 = TestUtils.MakeRandomArray<byte>(0x17);
            var totalPlayTime = 12345678;
            var unknown3 = 0;
            var lastMainItem = Th143Converter.ItemWithTotal.Camera;
            var lastSubItem = Th143Converter.ItemWithTotal.Doll;
            var unknown4 = TestUtils.MakeRandomArray<byte>(0x54);
            var nicknameFlags = TestUtils.MakeRandomArray<byte>(71);
            var unknown5 = TestUtils.MakeRandomArray<byte>(0x12D);
            var data = TestUtils.MakeByteArray(
                lastName.ToCharArray(),
                unknown1,
                bgmFlags,
                unknown2,
                totalPlayTime,
                unknown3,
                (int)lastMainItem,
                (int)lastSubItem,
                unknown4,
                nicknameFlags,
                unknown5);

            var chapter = Th095ChapterWrapper<Th143Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
            var status = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        internal static void CanInitializeTestHelper(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = Th095ChapterWrapper<Th143Converter>.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th143StatusWrapper.CanInitialize(chapter));
            });

        [TestMethod()]
        public void Th143StatusCanInitializeTest()
            => CanInitializeTestHelper("ST", 1, 0x224, true);

        [TestMethod()]
        public void Th143StatusCanInitializeTestInvalidSignature()
            => CanInitializeTestHelper("st", 1, 0x224, false);

        [TestMethod()]
        public void Th143StatusCanInitializeTestInvalidVersion()
            => CanInitializeTestHelper("ST", 0, 0x224, false);

        [TestMethod()]
        public void Th143StatusCanInitializeTestInvalidSize()
            => CanInitializeTestHelper("ST", 1, 0x225, false);

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidLastMainItem() => TestUtils.Wrap(() =>
        {
            var signature = "ST";
            var version = (ushort)1;
            var size = 0x224;
            var checksum = 0u;
            var lastName = "Player1     \0\0";
            var unknown1 = TestUtils.MakeRandomArray<byte>(0x12);
            var bgmFlags = TestUtils.MakeRandomArray<byte>(9);
            var unknown2 = TestUtils.MakeRandomArray<byte>(0x17);
            var totalPlayTime = 12345678;
            var unknown3 = 0;
            var lastMainItem = (Th143Converter.ItemWithTotal)(-1);
            var lastSubItem = Th143Converter.ItemWithTotal.Doll;
            var unknown4 = TestUtils.MakeRandomArray<byte>(0x54);
            var nicknameFlags = TestUtils.MakeRandomArray<byte>(71);
            var unknown5 = TestUtils.MakeRandomArray<byte>(0x12D);
            var data = TestUtils.MakeByteArray(
                lastName.ToCharArray(),
                unknown1,
                bgmFlags,
                unknown2,
                totalPlayTime,
                unknown3,
                (int)lastMainItem,
                (int)lastSubItem,
                unknown4,
                nicknameFlags,
                unknown5);

            var chapter = Th095ChapterWrapper<Th143Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
            var status = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "status")]
        [TestMethod()]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th143StatusTestInvalidLastSubItem() => TestUtils.Wrap(() =>
        {
            var signature = "ST";
            var version = (ushort)1;
            var size = 0x224;
            var checksum = 0u;
            var lastName = "Player1     \0\0";
            var unknown1 = TestUtils.MakeRandomArray<byte>(0x12);
            var bgmFlags = TestUtils.MakeRandomArray<byte>(9);
            var unknown2 = TestUtils.MakeRandomArray<byte>(0x17);
            var totalPlayTime = 12345678;
            var unknown3 = 0;
            var lastMainItem = Th143Converter.ItemWithTotal.Camera;
            var lastSubItem = (Th143Converter.ItemWithTotal)(-1);
            var unknown4 = TestUtils.MakeRandomArray<byte>(0x54);
            var nicknameFlags = TestUtils.MakeRandomArray<byte>(71);
            var unknown5 = TestUtils.MakeRandomArray<byte>(0x12D);
            var data = TestUtils.MakeByteArray(
                lastName.ToCharArray(),
                unknown1,
                bgmFlags,
                unknown2,
                totalPlayTime,
                unknown3,
                (int)lastMainItem,
                (int)lastSubItem,
                unknown4,
                nicknameFlags,
                unknown5);

            var chapter = Th095ChapterWrapper<Th143Converter>.Create(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));
            var status = new Th143StatusWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
