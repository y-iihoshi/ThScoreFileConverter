using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th16;
using ThScoreFileConverter.Models.Th16;
using static ThScoreFileConverter.Tests.Models.Th10.PracticeExtensions;
using static ThScoreFileConverter.Tests.Models.Th13.SpellCardExtensions;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;
using IPractice = ThScoreFileConverter.Models.Th10.IPractice;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;
using LevelPracticeWithTotal = ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal;
using StagePractice = ThScoreFileConverter.Core.Models.Th14.StagePractice;
using StageProgress = ThScoreFileConverter.Models.Th13.StageProgress;

namespace ThScoreFileConverter.Tests.Models.Th16;

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
            _ = mock.Season.Returns(Season.Autumn);
            return mock;
        }

        static IPractice MockPractice((Level, StagePractice) pair)
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
            _ = mock.ClearCount.Returns(12 + index);
            _ = mock.PracticeClearCount.Returns(34 + index);
            _ = mock.TrialCount.Returns(56 + index);
            _ = mock.PracticeTrialCount.Returns(78 + index);
            _ = mock.Id.Returns(index);
            _ = mock.Level.Returns(Level.Hard);
            _ = mock.PracticeScore.Returns(90123);
            return mock;
        }

        var levelsWithTotal = EnumHelper<LevelPracticeWithTotal>.Enumerable;
        var rankings = levelsWithTotal.ToDictionary(
            level => level,
            level => Enumerable.Range(0, 10).Select(MockScoreData).ToList() as IReadOnlyList<IScoreData>);
        var practices = EnumHelper.Cartesian<Level, StagePractice>().ToDictionary(pair => pair, MockPractice);
        var cards = Enumerable.Range(1, 119).ToDictionary(index => index, MockSpellCard);

        var mock = Substitute.For<IClearData>();
        _ = mock.Signature.Returns("CR");
        _ = mock.Version.Returns((ushort)1);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x5318);
        _ = mock.Chara.Returns(CharaWithTotal.Aya);
        _ = mock.Rankings.Returns(rankings);
        _ = mock.TotalPlayCount.Returns(23);
        _ = mock.PlayTime.Returns(4567890);
        _ = mock.ClearCounts.Returns(levelsWithTotal.ToDictionary(level => level, level => 100 - (int)level));
        _ = mock.ClearFlags.Returns(levelsWithTotal.ToDictionary(level => level, level => (int)level % 2));
        _ = mock.Practices.Returns(practices);
        _ = mock.Cards.Returns(cards);
        return mock;
    }

    internal static byte[] MakeByteArray(IClearData clearData)
    {
        return TestUtils.MakeByteArray(
            clearData.Signature.ToCharArray(),
            clearData.Version,
            clearData.Checksum,
            clearData.Size,
            (int)clearData.Chara,
            clearData.Rankings.Values.SelectMany(ranking => ranking.Select(ScoreDataTests.MakeByteArray)),
            clearData.Cards.Values.Select(Th13.SpellCardTests.MakeByteArray),
            clearData.TotalPlayCount,
            clearData.PlayTime,
            0u,
            clearData.ClearCounts.Values,
            clearData.ClearFlags.Values,
            clearData.Practices.Values.Select(Th10.PracticeTests.MakeByteArray),
            new byte[0x40]);
    }

    internal static void Validate(IClearData expected, IClearData actual)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Version.ShouldBe(expected.Version);
        actual.Checksum.ShouldBe(expected.Checksum);
        actual.Size.ShouldBe(expected.Size);
        actual.Chara.ShouldBe(expected.Chara);

        foreach (var pair in expected.Rankings)
        {
            for (var index = 0; index < pair.Value.Count; ++index)
            {
                ScoreDataTests.Validate(pair.Value[index], actual.Rankings[pair.Key][index]);
            }
        }

        actual.TotalPlayCount.ShouldBe(expected.TotalPlayCount);
        actual.PlayTime.ShouldBe(expected.PlayTime);
        actual.ClearCounts.Values.ShouldBe(expected.ClearCounts.Values);
        actual.ClearFlags.Values.ShouldBe(expected.ClearFlags.Values);

        foreach (var pair in expected.Practices)
        {
            actual.Practices[pair.Key].ShouldBe(pair.Value);
        }

        foreach (var pair in expected.Cards)
        {
            actual.Cards[pair.Key].ShouldBe(pair.Value);
        }
    }

    [TestMethod]
    public void ClearDataTestChapter()
    {
        var mock = MockClearData();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var clearData = new ClearData(chapter);

        Validate(mock, clearData);
        clearData.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void ClearDataTestInvalidSignature()
    {
        var mock = MockClearData();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    public void ClearDataTestInvalidVersion()
    {
        var mock = MockClearData();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new ClearData(chapter));
    }

    [TestMethod]
    public void ClearDataTestInvalidSize()
    {
        var mock = MockClearData();
        var size = mock.Size;
        _ = mock.Size.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new ClearData(chapter));
    }

    [DataTestMethod]
    [DataRow("CR", (ushort)1, 0x5318, true)]
    [DataRow("cr", (ushort)1, 0x5318, false)]
    [DataRow("CR", (ushort)0, 0x5318, false)]
    [DataRow("CR", (ushort)1, 0x5319, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        ClearData.CanInitialize(chapter).ShouldBe(expected);
    }
}
