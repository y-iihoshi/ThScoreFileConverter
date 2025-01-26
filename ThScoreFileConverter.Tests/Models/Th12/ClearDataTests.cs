using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th12;
using ThScoreFileConverter.Models.Th12;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<ThScoreFileConverter.Core.Models.Th12.CharaWithTotal>;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th10.StageProgress>;
using ISpellCard = ThScoreFileConverter.Models.Th10.ISpellCard<ThScoreFileConverter.Core.Models.Level>;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th12;

[TestClass]
public class ClearDataTests
{
    internal static IClearData MockClearData()
    {
        static IScoreData MockScoreData(int index)
        {
            var mock = Substitute.For<IScoreData>();
            _ = mock.Score.Returns(12345670u - ((uint)index * 1000u));
            _ = mock.StageProgress.Returns(StageProgress.Five);
            _ = mock.ContinueCount.Returns((byte)index);
            _ = mock.Name.Returns(TestUtils.CP932Encoding.GetBytes($"Player{index}\0\0\0"));
            _ = mock.DateTime.Returns(34567890u);
            _ = mock.SlowRate.Returns(1.2f);
            return mock;
        }

        static IPractice MockPractice((Level, Stage) pair)
        {
            var mock = Substitute.For<IPractice>();
            _ = mock.Score.Returns(123456u - ((uint)pair.Item1 * 10u));
            _ = mock.Cleared.Returns((byte)((int)pair.Item2 % 2));
            _ = mock.Unlocked.Returns((byte)((int)pair.Item1 % 2));
            return mock;
        }

        static ISpellCard MockSpellCard(int index)
        {
            var mock = Substitute.For<ISpellCard>();
            _ = mock.Name.Returns(TestUtils.MakeRandomArray(0x80));
            _ = mock.ClearCount.Returns(123 + index);
            _ = mock.TrialCount.Returns(456 + index);
            _ = mock.Id.Returns(index);
            _ = mock.Level.Returns(Level.Hard);
            return mock;
        }

        var levels = EnumHelper<Level>.Enumerable;
        var levelsExceptExtra = levels.Where(level => level != Level.Extra);
        var stages = EnumHelper<Stage>.Enumerable;
        var stagesExceptExtra = stages.Where(stage => stage != Stage.Extra);

        var rankings = levels.ToDictionary(
            level => level,
            level => Enumerable.Range(0, 10).Select(MockScoreData).ToList() as IReadOnlyList<IScoreData>);
        var practices = levelsExceptExtra.Cartesian(stagesExceptExtra).ToDictionary(pair => pair, MockPractice);
        var cards = Enumerable.Range(1, 113).ToDictionary(index => index, MockSpellCard);

        var mock = Substitute.For<IClearData>();
        _ = mock.Signature.Returns("CR");
        _ = mock.Version.Returns((ushort)0x0002);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x45F4);
        _ = mock.Chara.Returns(CharaWithTotal.ReimuB);
        _ = mock.Rankings.Returns(rankings);
        _ = mock.TotalPlayCount.Returns(23);
        _ = mock.PlayTime.Returns(4567890);
        _ = mock.ClearCounts.Returns(levels.ToDictionary(level => level, level => 100 - (int)level));
        _ = mock.Practices.Returns(practices);
        _ = mock.Cards.Returns(cards);
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

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var clearData = new ClearData(chapter);

        Th10.ClearDataTests.Validate(mock, clearData);
        Assert.IsFalse(clearData.IsValid);
    }

    [TestMethod]
    public void ClearDataTestInvalidSignature()
    {
        var mock = MockClearData();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    public void ClearDataTestInvalidVersion()
    {
        var mock = MockClearData();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    public void ClearDataTestInvalidSize()
    {
        var mock = MockClearData();
        var size = mock.Size;
        _ = mock.Size.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new ClearData(chapter));
    }

    [DataTestMethod]
    [DataRow("CR", (ushort)2, 0x45F4, true)]
    [DataRow("cr", (ushort)2, 0x45F4, false)]
    [DataRow("CR", (ushort)1, 0x45F4, false)]
    [DataRow("CR", (ushort)2, 0x45F5, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        Assert.AreEqual(expected, ClearData.CanInitialize(chapter));
    }
}
