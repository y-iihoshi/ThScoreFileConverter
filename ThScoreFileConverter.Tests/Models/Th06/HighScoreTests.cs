using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;
using ThScoreFileConverter.Models.Th06;
using IHighScore = ThScoreFileConverter.Models.Th06.IHighScore<
    ThScoreFileConverter.Core.Models.Th06.Chara,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Models.Th06.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th06;

internal static class HighScoreExtensions
{
    internal static void ShouldBe(this IHighScore actual, IHighScore expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Size1.ShouldBe(expected.Size1);
        actual.Size2.ShouldBe(expected.Size2);
        actual.FirstByteOfData.ShouldBe(expected.FirstByteOfData);
        actual.Score.ShouldBe(expected.Score);
        actual.Chara.ShouldBe(expected.Chara);
        actual.Level.ShouldBe(expected.Level);
        actual.StageProgress.ShouldBe(expected.StageProgress);
        actual.Name.ShouldBe(expected.Name);
    }
}

[TestClass]
public class HighScoreTests
{
    internal static IHighScore MockHighScore()
    {
        var mock = Substitute.For<IHighScore>();
        _ = mock.Signature.Returns("HSCR");
        _ = mock.Size1.Returns((short)0x1C);
        _ = mock.Size2.Returns((short)0x1C);
        _ = mock.Score.Returns(1234567u);
        _ = mock.Chara.Returns(Chara.ReimuB);
        _ = mock.Level.Returns(Level.Hard);
        _ = mock.StageProgress.Returns(StageProgress.Three);
        _ = mock.Name.Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0"));
        return mock;
    }

    internal static byte[] MakeByteArray(IHighScore highScore)
    {
        return TestUtils.MakeByteArray(
            highScore.Signature.ToCharArray(),
            highScore.Size1,
            highScore.Size2,
            0u,
            highScore.Score,
            (byte)highScore.Chara,
            (byte)highScore.Level,
            (byte)highScore.StageProgress,
            highScore.Name);
    }

    [TestMethod]
    public void HighScoreTestChapter()
    {
        var mock = MockHighScore();
        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var highScore = new HighScore(chapter);

        highScore.ShouldBe(mock);
    }

    [TestMethod]
    public void HighScoreTestScore()
    {
        var score = 1234567u;
        var name = "Nanashi\0\0";

        var highScore = new HighScore(score);

        highScore.Score.ShouldBe(score);
        highScore.Name.ShouldBe(TestUtils.CP932Encoding.GetBytes(name));
    }

    [TestMethod]
    public void HighScoreTestZeroScore()
    {
        var score = 0u;
        var name = "Nanashi\0\0";

        var highScore = new HighScore(score);

        highScore.Score.ShouldBe(score);
        highScore.Name.ShouldBe(TestUtils.CP932Encoding.GetBytes(name));
    }

    [TestMethod]
    public void HighScoreTestInvalidSignature()
    {
        var mock = MockHighScore();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new HighScore(chapter));
    }

    [TestMethod]
    public void HighScoreTestInvalidSize1()
    {
        var mock = MockHighScore();
        var size = mock.Size1;
        _ = mock.Size1.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new HighScore(chapter));
    }

    public static IEnumerable<object[]> InvalidCharacters => TestUtils.GetInvalidEnumerators<Chara>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void HighScoreTestInvalidChara(int chara)
    {
        var mock = MockHighScore();
        _ = mock.Chara.Returns((Chara)chara);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new HighScore(chapter));
    }

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<Level>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void HighScoreTestInvalidLevel(int level)
    {
        var mock = MockHighScore();
        _ = mock.Level.Returns((Level)level);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new HighScore(chapter));
    }

    public static IEnumerable<object[]> InvalidStageProgresses => TestUtils.GetInvalidEnumerators<StageProgress>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStageProgresses))]
    public void HighScoreTestInvalidStageProgress(int stageProgress)
    {
        var mock = MockHighScore();
        _ = mock.StageProgress.Returns((StageProgress)stageProgress);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new HighScore(chapter));
    }
}
