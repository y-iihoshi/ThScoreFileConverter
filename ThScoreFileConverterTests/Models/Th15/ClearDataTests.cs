using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Models.Th13;
using ThScoreFileConverterTests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IPractice = ThScoreFileConverter.Models.Th13.IPractice;
using StagePractice = ThScoreFileConverter.Models.Th14.StagePractice;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class ClearDataTests
    {
        internal static Mock<IClearData> MockClearData()
        {
            static IPractice CreatePractice((Level, StagePractice) pair)
            {
                var mock = new Mock<IPractice>();
                _ = mock.SetupGet(p => p.Score).Returns(123456u - (TestUtils.Cast<uint>(pair.Item1) * 10u));
                _ = mock.SetupGet(p => p.ClearFlag).Returns((byte)(TestUtils.Cast<int>(pair.Item2) % 2));
                _ = mock.SetupGet(p => p.EnableFlag).Returns((byte)(TestUtils.Cast<int>(pair.Item1) % 2));
                return mock.Object;
            }

            var mock = new Mock<IClearData>();
            _ = mock.SetupGet(m => m.Signature).Returns("CR");
            _ = mock.SetupGet(m => m.Version).Returns(1);
            _ = mock.SetupGet(m => m.Checksum).Returns(0u);
            _ = mock.SetupGet(m => m.Size).Returns(0xA4A0);
            _ = mock.SetupGet(m => m.Chara).Returns(CharaWithTotal.Marisa);
            _ = mock.SetupGet(m => m.GameModeData).Returns(
                EnumHelper<GameMode>.Enumerable.ToDictionary(
                    mode => mode,
                    _ => ClearDataPerGameModeTests.MockClearDataPerGameMode().Object));
            _ = mock.SetupGet(m => m.Practices).Returns(
                EnumHelper<Level>.Enumerable
                    .SelectMany(level => EnumHelper<StagePractice>.Enumerable.Select(stage => (level, stage)))
                    .ToDictionary(pair => pair, pair => CreatePractice(pair)));
            return mock;
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
            var mock = MockClearData();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            var clearData = new ClearData(chapter);

            Validate(mock.Object, clearData);
            Assert.IsFalse(clearData.IsValid);
        }

        [TestMethod]
        public void ClearDataTestInvalidSignature()
        {
            var mock = MockClearData();
            var signature = mock.Object.Signature;
            _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new ClearData(chapter));
        }

        [TestMethod]
        public void ClearDataTestInvalidVersion()
        {
            var mock = MockClearData();
            var version = mock.Object.Version;
            _ = mock.SetupGet(m => m.Version).Returns(++version);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new ClearData(chapter));
        }

        [TestMethod]
        public void ClearDataTestInvalidSize()
        {
            var mock = MockClearData();
            var size = mock.Object.Size;
            _ = mock.SetupGet(m => m.Size).Returns(--size);

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
            _ = Assert.ThrowsException<InvalidDataException>(() => _ = new ClearData(chapter));
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
