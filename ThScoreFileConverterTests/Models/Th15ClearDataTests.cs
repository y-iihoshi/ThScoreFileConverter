using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using ThScoreFileConverterTests.Models.Th13;
using ThScoreFileConverterTests.Models.Th13.Stubs;
using ThScoreFileConverterTests.Models.Th15.Stubs;
using ThScoreFileConverterTests.Models.Wrappers;
using IPractice = ThScoreFileConverter.Models.Th13.IPractice;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th15ClearDataTests
    {
        internal static ClearDataStub GetValidStub()
        {
            var modes = Utils.GetEnumerator<GameMode>();
            var levels = Utils.GetEnumerator<Level>();
            var stages = Utils.GetEnumerator<StagePractice>();

            return new ClearDataStub()
            {
                Signature = "CR",
                Version = 1,
                Checksum = 0u,
                Size = 0xA4A0,
                Chara = CharaWithTotal.Reimu,
                GameModeData = modes.ToDictionary(
                    mode => mode,
                    _ => Th15ClearDataPerGameModeTests.GetValidStub() as IClearDataPerGameMode),
                Practices = levels
                    .SelectMany(level => stages.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => new PracticeStub()
                        {
                            Score = 123456u - TestUtils.Cast<uint>(pair.level) * 10u,
                            ClearFlag = (byte)(TestUtils.Cast<int>(pair.stage) % 2),
                            EnableFlag = (byte)(TestUtils.Cast<int>(pair.level) % 2)
                        } as IPractice)
            };
        }

        internal static byte[] MakeData(IClearData clearData)
            => TestUtils.MakeByteArray(
                (int)clearData.Chara,
                clearData.GameModeData.Values.SelectMany(
                    data => Th15ClearDataPerGameModeTests.MakeByteArray(data)).ToArray(),
                clearData.Practices.Values.SelectMany(
                    practice => PracticeTests.MakeByteArray(practice)).ToArray(),
                new byte[0x40]);

        internal static byte[] MakeByteArray(IClearData clearData)
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Version,
                clearData.Checksum,
                clearData.Size,
                MakeData(clearData));

        internal static void Validate(IClearData expected, in Th15ClearDataWrapper actual)
        {
            var data = MakeData(expected);

            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            CollectionAssert.That.AreEqual(data, actual.Data);
            Assert.AreEqual(expected.Chara, actual.Chara);

            foreach (var pair in expected.GameModeData)
            {
                Th15ClearDataPerGameModeTests.Validate(pair.Value, actual.GameModeDataItem(pair.Key));
            }

            foreach (var pair in expected.Practices)
            {
                PracticeTests.Validate(pair.Value, actual.Practices[pair.Key]);
            }
        }


        [TestMethod]
        public void Th15ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var stub = GetValidStub();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            var clearData = new Th15ClearDataWrapper(chapter);

            Validate(stub, clearData);
            Assert.IsFalse(clearData.IsValid.Value);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Th15ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new Th15ClearDataWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = GetValidStub();
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th15ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15ClearDataTestInvalidVersion() => TestUtils.Wrap(() =>
        {
            var stub = GetValidStub();
            ++stub.Version;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th15ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Th15ClearDataTestInvalidSize() => TestUtils.Wrap(() =>
        {
            var stub = GetValidStub();
            --stub.Size;

            var chapter = ChapterWrapper.Create(MakeByteArray(stub));
            _ = new Th15ClearDataWrapper(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow("CR", (ushort)1, 0xA4A0, true)]
        [DataRow("cr", (ushort)1, 0xA4A0, false)]
        [DataRow("CR", (ushort)0, 0xA4A0, false)]
        [DataRow("CR", (ushort)1, 0xA4A1, false)]
        public void Th15ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
            => TestUtils.Wrap(() =>
            {
                var checksum = 0u;
                var data = new byte[size];

                var chapter = ChapterWrapper.Create(
                    TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

                Assert.AreEqual(expected, Th15ClearDataWrapper.CanInitialize(chapter));
            });
    }
}
