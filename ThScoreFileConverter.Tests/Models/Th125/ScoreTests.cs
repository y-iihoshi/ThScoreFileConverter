using NSubstitute;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Models.Th125;
using Chapter = ThScoreFileConverter.Models.Th095.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th125;

internal static class ScoreExtensions
{
    internal static void ShouldBe(this IScore actual, IScore expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Version.ShouldBe(expected.Version);
        actual.Size.ShouldBe(expected.Size);
        actual.Checksum.ShouldBe(expected.Checksum);
        actual.LevelScene.ShouldBe(expected.LevelScene);
        actual.HighScore.ShouldBe(expected.HighScore);
        actual.Chara.ShouldBe(expected.Chara);
        actual.TrialCount.ShouldBe(expected.TrialCount);
        actual.FirstSuccess.ShouldBe(expected.FirstSuccess);
        actual.DateTime.ShouldBe(expected.DateTime);
        actual.BestshotScore.ShouldBe(expected.BestshotScore);
    }
}

[TestClass]
public class ScoreTests
{
    internal static IScore MockScore()
    {
        var mock = Substitute.For<IScore>();
        _ = mock.Signature.Returns("SC");
        _ = mock.Version.Returns((ushort)0);
        _ = mock.Size.Returns(0x48);
        _ = mock.Checksum.Returns(0u);
        _ = mock.LevelScene.Returns((Level.Nine, 7));
        _ = mock.HighScore.Returns(1234567);
        _ = mock.Chara.Returns(Chara.Hatate);
        _ = mock.TrialCount.Returns(9876);
        _ = mock.FirstSuccess.Returns(5432);
        _ = mock.DateTime.Returns(34567890u);
        _ = mock.BestshotScore.Returns(23456);
        return mock;
    }

    internal static byte[] MakeByteArray(IScore score)
    {
        return TestUtils.MakeByteArray(
            score.Signature.ToCharArray(),
            score.Version,
            score.Size,
            score.Checksum,
            ((int)score.LevelScene.Level * 10) + score.LevelScene.Scene - 1,
            score.HighScore,
            new byte[4],
            (int)score.Chara,
            new byte[4],
            score.TrialCount,
            score.FirstSuccess,
            0u,
            score.DateTime,
            new uint[3],
            score.BestshotScore,
            new byte[8]);
    }

    [TestMethod]
    public void ScoreTestChapter()
    {
        var mock = MockScore();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var score = new Score(chapter);

        score.ShouldBe(mock);
        score.IsValid.ShouldBeFalse();
    }

    [TestMethod]
    public void ScoreTestInvalidSignature()
    {
        var mock = MockScore();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new Score(chapter));
    }

    [TestMethod]
    public void ScoreTestInvalidVersion()
    {
        var mock = MockScore();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new Score(chapter));
    }

    [TestMethod]
    public void ScoreTestInvalidSize()
    {
        var mock = MockScore();
        var size = mock.Size;
        _ = mock.Size.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new Score(chapter));
    }

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<Level>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void ScoreTestInvalidLevel(int level)
    {
        var mock = MockScore();
        var levelScene = mock.LevelScene;
        _ = mock.LevelScene.Returns(((Level)level, levelScene.Scene));

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new Score(chapter));
    }

    public static IEnumerable<object[]> InvalidCharacters => TestUtils.GetInvalidEnumerators<Chara>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void ScoreTestInvalidChara(int chara)
    {
        var mock = MockScore();
        _ = mock.Chara.Returns((Chara)chara);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new Score(chapter));
    }

    [DataTestMethod]
    [DataRow("SC", (ushort)0, 0x48, true)]
    [DataRow("sc", (ushort)0, 0x48, false)]
    [DataRow("SC", (ushort)1, 0x48, false)]
    [DataRow("SC", (ushort)0, 0x49, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

        Score.CanInitialize(chapter).ShouldBe(expected);
    }
}
