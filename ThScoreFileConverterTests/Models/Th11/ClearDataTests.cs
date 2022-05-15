using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Helpers;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th11;
using ThScoreFileConverterTests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Models.Th11.CharaWithTotal>;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th10.StageProgress>;
using ISpellCard = ThScoreFileConverter.Models.Th10.ISpellCard<ThScoreFileConverter.Models.Level>;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverterTests.Models.Th11;

[TestClass]
public class ClearDataTests
{
    internal static Mock<IClearData> MockClearData()
    {
        static IScoreData CreateScoreData(int index)
        {
            var mock = new Mock<IScoreData>();
            _ = mock.SetupGet(s => s.Score).Returns(12345670u - ((uint)index * 1000u));
            _ = mock.SetupGet(s => s.StageProgress).Returns(StageProgress.Five);
            _ = mock.SetupGet(s => s.ContinueCount).Returns((byte)index);
            _ = mock.SetupGet(s => s.Name).Returns(TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"));
            _ = mock.SetupGet(s => s.DateTime).Returns(34567890u);
            _ = mock.SetupGet(s => s.SlowRate).Returns(1.2f);
            return mock.Object;
        }

        static IPractice CreatePractice((Level, Stage) pair)
        {
            var mock = new Mock<IPractice>();
            _ = mock.SetupGet(p => p.Score).Returns(123456u - ((uint)pair.Item1 * 10u));
            _ = mock.SetupGet(p => p.Cleared).Returns((byte)((int)pair.Item2 % 2));
            _ = mock.SetupGet(p => p.Unlocked).Returns((byte)((int)pair.Item1 % 2));
            return mock.Object;
        }

        static ISpellCard CreateSpellCard(int index)
        {
            var mock = new Mock<ISpellCard>();
            _ = mock.SetupGet(s => s.Name).Returns(TestUtils.MakeRandomArray<byte>(0x80));
            _ = mock.SetupGet(s => s.ClearCount).Returns(123 + index);
            _ = mock.SetupGet(s => s.TrialCount).Returns(456 + index);
            _ = mock.SetupGet(s => s.Id).Returns(index);
            _ = mock.SetupGet(s => s.Level).Returns(Level.Hard);
            return mock.Object;
        }

        var levels = EnumHelper<Level>.Enumerable;
        var levelsExceptExtra = levels.Where(level => level != Level.Extra);
        var stages = EnumHelper<Stage>.Enumerable;
        var stagesExceptExtra = stages.Where(stage => stage != Stage.Extra);

        var mock = new Mock<IClearData>();
        _ = mock.SetupGet(m => m.Signature).Returns("CR");
        _ = mock.SetupGet(m => m.Version).Returns(0x0000);
        _ = mock.SetupGet(m => m.Checksum).Returns(0u);
        _ = mock.SetupGet(m => m.Size).Returns(0x68D4);
        _ = mock.SetupGet(m => m.Chara).Returns(CharaWithTotal.ReimuSuika);
        _ = mock.SetupGet(m => m.Rankings).Returns(
            levels.ToDictionary(
                level => level,
                level => Enumerable.Range(0, 10).Select(index => CreateScoreData(index)).ToList()
                    as IReadOnlyList<IScoreData>));
        _ = mock.SetupGet(m => m.TotalPlayCount).Returns(23);
        _ = mock.SetupGet(m => m.PlayTime).Returns(4567890);
        _ = mock.SetupGet(m => m.ClearCounts).Returns(
            levels.ToDictionary(level => level, level => 100 - (int)level));
        _ = mock.SetupGet(m => m.Practices).Returns(
            levelsExceptExtra
                .SelectMany(level => stagesExceptExtra.Select(stage => (level, stage)))
                .ToDictionary(pair => pair, pair => CreatePractice(pair)));
        _ = mock.SetupGet(m => m.Cards).Returns(
            Enumerable.Range(1, 175).ToDictionary(index => index, index => CreateSpellCard(index)));
        return mock;
    }

    internal static byte[] MakeByteArray(IClearData clearData)
    {
        return Th10.ClearDataTests.MakeByteArray(clearData, 4);
    }

    [TestMethod]
    public void ClearDataTestChapter()
    {
        var mock = MockClearData();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        var clearData = new ClearData(chapter);

        Th10.ClearDataTests.Validate(mock.Object, clearData);
        Assert.IsFalse(clearData.IsValid);
    }

    [TestMethod]
    public void ClearDataTestInvalidSignature()
    {
        var mock = MockClearData();
        var signature = mock.Object.Signature;
        _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    public void ClearDataTestInvalidVersion()
    {
        var mock = MockClearData();
        var version = mock.Object.Version;
        _ = mock.SetupGet(m => m.Version).Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    public void ClearDataTestInvalidSize()
    {
        var mock = MockClearData();
        var size = mock.Object.Size;
        _ = mock.SetupGet(m => m.Size).Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
    }

    [DataTestMethod]
    [DataRow("CR", (ushort)0, 0x68D4, true)]
    [DataRow("cr", (ushort)0, 0x68D4, false)]
    [DataRow("CR", (ushort)1, 0x68D4, false)]
    [DataRow("CR", (ushort)0, 0x68D5, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        Assert.AreEqual(expected, ClearData.CanInitialize(chapter));
    }
}
