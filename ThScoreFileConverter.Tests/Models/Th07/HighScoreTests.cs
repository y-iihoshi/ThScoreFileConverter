using NSubstitute;
using ThScoreFileConverter.Core.Models.Th07;
using ThScoreFileConverter.Models.Th07;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using IHighScore = ThScoreFileConverter.Models.Th07.IHighScore<
    ThScoreFileConverter.Core.Models.Th07.Chara,
    ThScoreFileConverter.Core.Models.Th07.Level,
    ThScoreFileConverter.Models.Th07.StageProgress>;

namespace ThScoreFileConverter.Tests.Models.Th07;

[TestClass]
public class HighScoreTests
{
    internal static IHighScore MockHighScore()
    {
        var mock = Substitute.For<IHighScore>();
        _ = mock.Signature.Returns("HSCR");
        _ = mock.Size1.Returns((short)0x28);
        _ = mock.Size2.Returns((short)0x28);
        _ = mock.Score.Returns(1234567u);
        _ = mock.SlowRate.Returns(9.87f);
        _ = mock.Chara.Returns(Chara.ReimuB);
        _ = mock.Level.Returns(Level.Hard);
        _ = mock.StageProgress.Returns(StageProgress.Three);
        _ = mock.Name.Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0"));
        _ = mock.Date.Returns(TestUtils.CP932Encoding.GetBytes("01/23\0"));
        _ = mock.ContinueCount.Returns((ushort)2);
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
            highScore.SlowRate,
            (byte)highScore.Chara,
            (byte)highScore.Level,
            (byte)highScore.StageProgress,
            highScore.Name,
            highScore.Date,
            highScore.ContinueCount);
    }

    internal static void Validate(IHighScore expected, IHighScore actual)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Size1.ShouldBe(expected.Size1);
        actual.Size2.ShouldBe(expected.Size2);
        actual.FirstByteOfData.ShouldBe(expected.FirstByteOfData);
        actual.Score.ShouldBe(expected.Score);
        actual.SlowRate.ShouldBe(expected.SlowRate);
        actual.Chara.ShouldBe(expected.Chara);
        actual.Level.ShouldBe(expected.Level);
        actual.StageProgress.ShouldBe(expected.StageProgress);
        actual.Name.ShouldBe(expected.Name);
        actual.Date.ShouldBe(expected.Date);
        actual.ContinueCount.ShouldBe(expected.ContinueCount);
    }

    [TestMethod]
    public void HighScoreTestChapter()
    {
        var mock = MockHighScore();
        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var highScore = new HighScore(chapter);

        Validate(mock, highScore);
    }

    [TestMethod]
    public void HighScoreTestScore()
    {
        var score = 1234567u;
        var name = "--------\0";
        var date = "--/--\0";

        var highScore = new HighScore(score);

        highScore.Score.ShouldBe(score);
        highScore.Name.ShouldBe(TestUtils.CP932Encoding.GetBytes(name));
        highScore.Date.ShouldBe(TestUtils.CP932Encoding.GetBytes(date));
    }

    [TestMethod]
    public void HighScoreTestZeroScore()
    {
        var score = 0u;
        var name = "--------\0";
        var date = "--/--\0";

        var highScore = new HighScore(score);

        highScore.Score.ShouldBe(score);
        highScore.Name.ShouldBe(TestUtils.CP932Encoding.GetBytes(name));
        highScore.Date.ShouldBe(TestUtils.CP932Encoding.GetBytes(date));
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
