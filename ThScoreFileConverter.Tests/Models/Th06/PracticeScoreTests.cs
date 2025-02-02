using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;
using ThScoreFileConverter.Models.Th06;

namespace ThScoreFileConverter.Tests.Models.Th06;

internal static class PracticeScoreExtensions
{
    internal static void ShouldBe(this IPracticeScore actual, IPracticeScore expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Size1.ShouldBe(expected.Size1);
        actual.Size2.ShouldBe(expected.Size2);
        actual.FirstByteOfData.ShouldBe(expected.FirstByteOfData);
        actual.HighScore.ShouldBe(expected.HighScore);
        actual.Chara.ShouldBe(expected.Chara);
        actual.Level.ShouldBe(expected.Level);
        actual.Stage.ShouldBe(expected.Stage);
    }
}

[TestClass]
public class PracticeScoreTests
{
    internal static IPracticeScore MockPracticeScore()
    {
        var mock = Substitute.For<IPracticeScore>();
        _ = mock.Signature.Returns("PSCR");
        _ = mock.Size1.Returns((short)0x14);
        _ = mock.Size2.Returns((short)0x14);
        _ = mock.HighScore.Returns(123456);
        _ = mock.Chara.Returns(Chara.ReimuB);
        _ = mock.Level.Returns(Level.Hard);
        _ = mock.Stage.Returns(Stage.Six);
        return mock;
    }

    internal static byte[] MakeByteArray(IPracticeScore score)
    {
        return TestUtils.MakeByteArray(
            score.Signature.ToCharArray(),
            score.Size1,
            score.Size2,
            0u,
            score.HighScore,
            (byte)score.Chara,
            (byte)score.Level,
            (byte)score.Stage,
            (byte)0);
    }

    [TestMethod]
    public void PracticeScoreTestChapter()
    {
        var mock = MockPracticeScore();
        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var score = new PracticeScore(chapter);

        score.ShouldBe(mock);
    }

    [TestMethod]
    public void PracticeScoreTestInvalidSignature()
    {
        var mock = MockPracticeScore();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new PracticeScore(chapter));
    }

    [TestMethod]
    public void PracticeScoreTestInvalidSize1()
    {
        var mock = MockPracticeScore();
        var size = mock.Size1;
        _ = mock.Size1.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidDataException>(() => new PracticeScore(chapter));
    }

    public static IEnumerable<object[]> InvalidCharacters => TestUtils.GetInvalidEnumerators<Chara>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void PracticeScoreTestInvalidChara(int chara)
    {
        var mock = MockPracticeScore();
        _ = mock.Chara.Returns((Chara)chara);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new PracticeScore(chapter));
    }

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<Level>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void PracticeScoreTestInvalidLevel(int level)
    {
        var mock = MockPracticeScore();
        _ = mock.Level.Returns((Level)level);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new PracticeScore(chapter));
    }

    public static IEnumerable<object[]> InvalidStages => TestUtils.GetInvalidEnumerators<Stage>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStages))]
    public void PracticeScoreTestInvalidStage(int stage)
    {
        var mock = MockPracticeScore();
        _ = mock.Stage.Returns((Stage)stage);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Should.Throw<InvalidCastException>(() => new PracticeScore(chapter));
    }
}
