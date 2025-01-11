using System.Collections.Generic;
using System.IO;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th18;
using ThScoreFileConverter.Models.Th18;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using Definitions = ThScoreFileConverter.Models.Th18.Definitions;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;
using LevelPracticeWithTotal = ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th18;

[TestClass]
public class ClearDataTests
{
    internal static IClearData MockClearData()
    {
        static IScoreData MockScoreData(int index)
        {
            var mock = Substitute.For<IScoreData>();
            _ = mock.Score.Returns(12345670u - ((uint)index * 1000u));
            _ = mock.StageProgress.Returns((StageProgress)index);
            _ = mock.ContinueCount.Returns((byte)index);
            _ = mock.Name.Returns(TestUtils.MakeByteArray($"Player{index}\0\0\0").Skip(1));   // skip length
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

        static ISpellCard MockSpellCard(
            int clear, int practiceClear, int trial, int practiceTrial, int id, Level level)
        {
            var mock = Substitute.For<ISpellCard>();
            _ = mock.Name.Returns(TestUtils.MakeRandomArray(0xC0));
            _ = mock.ClearCount.Returns(clear);
            _ = mock.PracticeClearCount.Returns(practiceClear);
            _ = mock.TrialCount.Returns(trial);
            _ = mock.PracticeTrialCount.Returns(practiceTrial);
            _ = mock.Id.Returns(id);
            _ = mock.Level.Returns(level);
            _ = mock.PracticeScore.Returns(90123);
            return mock;
        }

        var levelsWithTotal = EnumHelper<LevelPracticeWithTotal>.Enumerable;
        var levelsExceptExtra = EnumHelper<Level>.Enumerable.Where(static level => level != Level.Extra);
        var stagesExceptExtra = EnumHelper<Stage>.Enumerable.Where(static stage => stage != Stage.Extra);
        var rankings = levelsWithTotal.ToDictionary(
            level => level,
            level => Enumerable.Range(0, 10).Select(MockScoreData).ToList() as IReadOnlyList<IScoreData>);
        var cards = Definitions.CardTable.ToDictionary(
            pair => pair.Key,
            pair => MockSpellCard(
                (pair.Key % 2 == 0) ? 0 : 12 + pair.Key,
                (pair.Key % 3 == 0) ? 0 : 34 + pair.Key,
                (pair.Key % 4 == 0) ? 0 : 56 + pair.Key,
                (pair.Key % 5 == 0) ? 0 : 78 + pair.Key,
                pair.Value.Id,
                pair.Value.Level));
        var practices = levelsExceptExtra.Cartesian(stagesExceptExtra).ToDictionary(pair => pair, MockPractice);

        var mock = Substitute.For<IClearData>();
        _ = mock.Signature.Returns("CR");
        _ = mock.Version.Returns((ushort)3);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x130F0);
        _ = mock.Chara.Returns(CharaWithTotal.Marisa);
        _ = mock.Rankings.Returns(rankings);
        _ = mock.Cards.Returns(cards);
        _ = mock.TotalPlayCount.Returns(23);
        _ = mock.PlayTime.Returns(4567890);
        _ = mock.ClearCounts.Returns(levelsWithTotal.ToDictionary(level => level, level => 100 - (int)level));
        _ = mock.ClearFlags.Returns(levelsWithTotal.ToDictionary(level => level, level => (int)level % 2));
        _ = mock.Practices.Returns(practices);
        return mock;
    }

    internal static byte[] MakeByteArray(IClearData clearData)
    {
        var spellCardMock = SpellCardTests.MockSpellCard();
        var spellCardBytes = SpellCardTests.MakeByteArray(spellCardMock);

        return TestUtils.MakeByteArray(
            clearData.Signature.ToCharArray(),
            clearData.Version,
            clearData.Checksum,
            clearData.Size,
            (int)clearData.Chara,
            clearData.Rankings.Values.SelectMany(
                ranking => ranking.Select(scoreData => Th17.ScoreDataTests.MakeByteArray(scoreData))),
            clearData.Cards.Values.Select(SpellCardTests.MakeByteArray),
            Enumerable.Range(0, 10).Select(_ => spellCardBytes),
            clearData.TotalPlayCount,
            clearData.PlayTime,
            0u,
            clearData.ClearCounts.Values,
            clearData.ClearFlags.Values,
            new byte[0xCA08],
            clearData.Practices.Values.Select(Th10.PracticeTests.MakeByteArray),
            new byte[0x120]
            );
    }

    internal static void Validate(IClearData expected, IClearData actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Version, actual.Version);
        Assert.AreEqual(expected.Checksum, actual.Checksum);
        Assert.AreEqual(expected.Size, actual.Size);
        Assert.AreEqual(expected.Chara, actual.Chara);

        foreach (var pair in expected.Rankings)
        {
            for (var index = 0; index < pair.Value.Count; ++index)
            {
                Th10.ScoreDataTests.Validate(actual.Rankings[pair.Key][index], pair.Value[index]);
            }
        }

        foreach (var pair in expected.Cards)
        {
            SpellCardTests.Validate(pair.Value, actual.Cards[pair.Key]);
        }

        Assert.AreEqual(expected.TotalPlayCount, actual.TotalPlayCount);
        Assert.AreEqual(expected.PlayTime, actual.PlayTime);
        CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
        CollectionAssert.That.AreEqual(expected.ClearFlags.Values, actual.ClearFlags.Values);

        foreach (var pair in expected.Practices)
        {
            Th10.PracticeTests.Validate(pair.Value, actual.Practices[pair.Key]);
        }
    }

    [TestMethod]
    public void ClearDataTestChapter()
    {
        var mock = MockClearData();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var clearData = new ClearData(chapter);

        Validate(mock, clearData);
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
    [DataRow("CR", (ushort)3, 0x130F0, true)]
    [DataRow("cr", (ushort)3, 0x130F0, false)]
    [DataRow("CR", (ushort)4, 0x130F0, false)]
    [DataRow("CR", (ushort)3, 0x130F1, false)]
    public void ClearDataCanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        Assert.AreEqual(expected, ClearData.CanInitialize(chapter));
    }
}
