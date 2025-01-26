using NSubstitute;
using ThScoreFileConverter.Core.Models.Th095;
using ThScoreFileConverter.Models.Th095;

namespace ThScoreFileConverter.Tests.Models.Th095;

[TestClass]
public class ScoreTests
{
    internal static IScore MockScore()
    {
        var mock = Substitute.For<IScore>();
        _ = mock.Signature.Returns("SC");
        _ = mock.Version.Returns((ushort)1);
        _ = mock.Size.Returns(0x60);
        _ = mock.Checksum.Returns(0u);
        _ = mock.LevelScene.Returns((Level.Nine, 6));
        _ = mock.HighScore.Returns(1234567);
        _ = mock.BestshotScore.Returns(23456);
        _ = mock.DateTime.Returns(34567890u);
        _ = mock.TrialCount.Returns(9876);
        _ = mock.SlowRate1.Returns(1.23f);
        _ = mock.SlowRate2.Returns(2.34f);
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
            0u,
            score.BestshotScore,
            new byte[0x20],
            score.DateTime,
            0u,
            score.TrialCount,
            score.SlowRate1,
            score.SlowRate2,
            new byte[0x10]);
    }

    internal static void Validate(IScore expected, IScore actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Version, actual.Version);
        Assert.AreEqual(expected.Size, actual.Size);
        Assert.AreEqual(expected.Checksum, actual.Checksum);
        Assert.AreEqual(expected.LevelScene, actual.LevelScene);
        Assert.AreEqual(expected.HighScore, actual.HighScore);
        Assert.AreEqual(expected.BestshotScore, actual.BestshotScore);
        Assert.AreEqual(expected.DateTime, actual.DateTime);
        Assert.AreEqual(expected.TrialCount, actual.TrialCount);
        Assert.AreEqual(expected.SlowRate1, actual.SlowRate1);
        Assert.AreEqual(expected.SlowRate2, actual.SlowRate2);
    }

    [TestMethod]
    public void ScoreTestChapter()
    {
        var mock = MockScore();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var score = new Score(chapter);

        Validate(mock, score);
        Assert.IsFalse(score.IsValid);
    }

    [TestMethod]
    public void ScoreTestInvalidSignature()
    {
        var mock = MockScore();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new Score(chapter));
    }

    [TestMethod]
    public void ScoreTestInvalidVersion()
    {
        var mock = MockScore();
        var version = mock.Version;
        _ = mock.Version.Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new Score(chapter));
    }

    [TestMethod]
    public void ScoreTestInvalidSize()
    {
        var mock = MockScore();
        var size = mock.Size;
        _ = mock.Size.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new Score(chapter));
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
        _ = Assert.ThrowsException<InvalidCastException>(() => new Score(chapter));
    }

    [DataTestMethod]
    [DataRow("SC", (ushort)1, 0x60, true)]
    [DataRow("sc", (ushort)1, 0x60, false)]
    [DataRow("SC", (ushort)0, 0x60, false)]
    [DataRow("SC", (ushort)1, 0x61, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

        Assert.AreEqual(expected, Score.CanInitialize(chapter));
    }
}
