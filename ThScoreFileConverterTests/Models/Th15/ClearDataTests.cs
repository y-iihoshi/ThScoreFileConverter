using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Models.Th13;
using ThScoreFileConverterTests.Models.Th13.Stubs;
using ThScoreFileConverterTests.Models.Th15.Stubs;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IPractice = ThScoreFileConverter.Models.Th13.IPractice;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class ClearDataTests
    {
        internal static ClearDataStub MakeValidStub()
        {
            var modes = Utils.GetEnumerator<GameMode>();
            var levels = Utils.GetEnumerator<Level>();
            var stages = Utils.GetEnumerator<StagePractice>();

            return new ClearDataStub
            {
                Signature = "CR",
                Version = 1,
                Checksum = 0u,
                Size = 0xA4A0,
                Chara = CharaWithTotal.Marisa,
                GameModeData = modes.ToDictionary(
                    mode => mode,
                    _ => ClearDataPerGameModeTests.MakeValidStub() as IClearDataPerGameMode),
                Practices = levels
                    .SelectMany(level => stages.Select(stage => (level, stage)))
                    .ToDictionary(
                        pair => pair,
                        pair => new PracticeStub
                        {
                            Score = 123456u - TestUtils.Cast<uint>(pair.level) * 10u,
                            ClearFlag = (byte)(TestUtils.Cast<int>(pair.stage) % 2),
                            EnableFlag = (byte)(TestUtils.Cast<int>(pair.level) % 2),
                        } as IPractice),
            };
        }

        internal static byte[] MakeData(IClearData clearData)
            => TestUtils.MakeByteArray(
                (int)clearData.Chara,
                clearData.GameModeData.Values.SelectMany(
                    data => ClearDataPerGameModeTests.MakeByteArray(data)).ToArray(),
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

        internal static void Validate(IClearData expected, IClearData actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Version, actual.Version);
            Assert.AreEqual(expected.Checksum, actual.Checksum);
            Assert.AreEqual(expected.Size, actual.Size);
            Assert.AreEqual(expected.Chara, actual.Chara);

            foreach (var pair in expected.GameModeData)
            {
                ClearDataPerGameModeTests.Validate(pair.Value, actual.GameModeData[pair.Key]);
            }

            foreach (var pair in expected.Practices)
            {
                PracticeTests.Validate(pair.Value, actual.Practices[pair.Key]);
            }
        }


        [TestMethod]
        public void ClearDataTestChapter()
        {
            var stub = MakeValidStub();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            var clearData = new ClearData(chapter);

            Validate(stub, clearData);
            Assert.IsFalse(clearData.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearDataTestNullChapter()
        {
            _ = new ClearData(null!);

            Assert.Fail(TestUtils.Unreachable);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSignature()
        {
            var stub = MakeValidStub();
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidVersion()
        {
            var stub = MakeValidStub();
            ++stub.Version;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSize()
        {
            var stub = MakeValidStub();
            --stub.Size;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        }

        [DataTestMethod]
        [DataRow("CR", (ushort)1, 0xA4A0, true)]
        [DataRow("cr", (ushort)1, 0xA4A0, false)]
        [DataRow("CR", (ushort)0, 0xA4A0, false)]
        [DataRow("CR", (ushort)1, 0xA4A1, false)]
        public void CanInitializeTest(string signature, ushort version, int size, bool expected)
        {
            var checksum = 0u;
            var data = new byte[size];

            var chapter = TestUtils.Create<Chapter>(
                TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

            Assert.AreEqual(expected, ClearData.CanInitialize(chapter));
        }
    }
}
