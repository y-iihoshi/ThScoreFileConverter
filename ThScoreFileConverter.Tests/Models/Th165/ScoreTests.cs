using System.IO;
using NSubstitute;
using ThScoreFileConverter.Models.Th165;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th165;

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

    internal static void Validate(IScore expected, IScore actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Version, actual.Version);
        Assert.AreEqual(expected.Checksum, actual.Checksum);
        Assert.AreEqual(expected.Size, actual.Size);
        Assert.AreEqual(expected.Number, actual.Number);
        Assert.AreEqual(expected.ClearCount, actual.ClearCount);
        Assert.AreEqual(expected.ChallengeCount, actual.ChallengeCount);
        Assert.AreEqual(expected.NumPhotos, actual.NumPhotos);
        Assert.AreEqual(expected.HighScore, actual.HighScore);
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

        Assert.AreEqual(expected, Score.CanInitialize(chapter));
    }
}
