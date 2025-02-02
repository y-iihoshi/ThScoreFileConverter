using NSubstitute;
using ThScoreFileConverter.Models.Th165;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th165;

internal static class ScoreExtensions
{
    internal static void ShouldBe(this IScore actual, IScore expected)
    {
        actual.Signature.ShouldBe(expected.Signature);
        actual.Version.ShouldBe(expected.Version);
        actual.Checksum.ShouldBe(expected.Checksum);
        actual.Size.ShouldBe(expected.Size);
        actual.Number.ShouldBe(expected.Number);
        actual.ClearCount.ShouldBe(expected.ClearCount);
        actual.ChallengeCount.ShouldBe(expected.ChallengeCount);
        actual.NumPhotos.ShouldBe(expected.NumPhotos);
        actual.HighScore.ShouldBe(expected.HighScore);
    }
}

[TestClass]
public class ScoreTests
{
    internal static IScore MockScore()
    {
        var mock = Substitute.For<IScore>();
        _ = mock.Signature.Returns("SN");
        _ = mock.Version.Returns((ushort)1);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x234);
        _ = mock.Number.Returns(12);
        _ = mock.ClearCount.Returns(34);
        _ = mock.ChallengeCount.Returns(56);
        _ = mock.NumPhotos.Returns(78);
        _ = mock.HighScore.Returns(1234567);
        return mock;
    }

    internal static byte[] MakeByteArray(IScore score)
    {
        return TestUtils.MakeByteArray(
            score.Signature.ToCharArray(),
            score.Version,
            score.Checksum,
            score.Size,
            score.Number,
            score.ClearCount,
            0,
            score.ChallengeCount,
            score.NumPhotos,
            score.HighScore,
            new byte[0x210]);
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

    [DataTestMethod]
    [DataRow("SN", (ushort)1, 0x234, true)]
    [DataRow("sn", (ushort)1, 0x234, false)]
    [DataRow("SN", (ushort)0, 0x234, false)]
    [DataRow("SN", (ushort)1, 0x235, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        Score.CanInitialize(chapter).ShouldBe(expected);
    }
}
