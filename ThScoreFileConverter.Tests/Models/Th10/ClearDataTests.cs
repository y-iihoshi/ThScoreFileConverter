using NSubstitute;
using ThScoreFileConverter.Core.Extensions;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th10;
using ThScoreFileConverter.Models.Th10;

namespace ThScoreFileConverter.Tests.Models.Th10;

[TestClass]
public class ClearDataTests
{
    internal static IClearData<CharaWithTotal> MockClearData()
    {
        static IScoreData<StageProgress> MockScoreData(int index)
        {
            var mock = Substitute.For<IScoreData<StageProgress>>();
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

        static ISpellCard<Level> MockSpellCard(int index)
        {
            var mock = Substitute.For<ISpellCard<Level>>();
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
            level => Enumerable.Range(0, 10).Select(MockScoreData).ToList() as IReadOnlyList<IScoreData<StageProgress>>);
        var practices = levelsExceptExtra.Cartesian(stagesExceptExtra).ToDictionary(pair => pair, MockPractice);
        var cards = Enumerable.Range(1, 110).ToDictionary(index => index, MockSpellCard);

        var mock = Substitute.For<IClearData<CharaWithTotal>>();
        _ = mock.Signature.Returns("CR");
        _ = mock.Version.Returns((ushort)0x0000);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x437C);
        _ = mock.Chara.Returns(CharaWithTotal.ReimuB);
        _ = mock.Rankings.Returns(rankings);
        _ = mock.TotalPlayCount.Returns(23);
        _ = mock.PlayTime.Returns(4567890);
        _ = mock.ClearCounts.Returns(levels.ToDictionary(level => level, level => 100 - (int)level));
        _ = mock.Practices.Returns(practices);
        _ = mock.Cards.Returns(cards);
        return mock;
    }

    internal static byte[] MakeByteArray<TCharaWithTotal>(
        IClearData<TCharaWithTotal> clearData, int scoreDataUnknownSize)
        where TCharaWithTotal : struct, Enum
    {
        return TestUtils.MakeByteArray(
            clearData.Signature.ToCharArray(),
            clearData.Version,
            clearData.Checksum,
            clearData.Size,
            TestUtils.Cast<int>(clearData.Chara),
            clearData.Rankings.Values.SelectMany(
                ranking => ranking.Select(scoreData => ScoreDataTests.MakeByteArray(scoreData, scoreDataUnknownSize))),
            clearData.TotalPlayCount,
            clearData.PlayTime,
            clearData.ClearCounts.Values,
            clearData.Practices.Values.Select(PracticeTests.MakeByteArray),
            clearData.Cards.Values.Select(SpellCardTests.MakeByteArray));
    }

    internal static byte[] MakeByteArray(IClearData<CharaWithTotal> clearData)
    {
        return MakeByteArray(clearData, 0);
    }

    internal static void Validate<TCharaWithTotal>(
        IClearData<TCharaWithTotal> expected, IClearData<TCharaWithTotal> actual)
        where TCharaWithTotal : struct, Enum
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

        foreach (var pair in expected.Practices)
        {
            PracticeTests.Validate(pair.Value, actual.Practices[pair.Key]);
        }

        foreach (var pair in expected.Cards)
        {
            SpellCardTests.Validate(pair.Value, actual.Cards[pair.Key]);
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
    [DataRow("CR", (ushort)0, 0x437C, true)]
    [DataRow("cr", (ushort)0, 0x437C, false)]
    [DataRow("CR", (ushort)1, 0x437C, false)]
    [DataRow("CR", (ushort)0, 0x437D, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        ClearData.CanInitialize(chapter).ShouldBe(expected);
    }
}
