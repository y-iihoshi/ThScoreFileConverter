using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th143;
using ThScoreFileConverter.Models.Th143;
using Chapter = ThScoreFileConverter.Models.Th10.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th143;

[TestClass]
public class ScoreTests
{
    internal static IScore MockScore()
    {
        var items = EnumHelper<ItemWithTotal>.Enumerable;
        var mock = Substitute.For<IScore>();
        _ = mock.Signature.Returns("SN");
        _ = mock.Version.Returns((ushort)1);
        _ = mock.Checksum.Returns(0u);
        _ = mock.Size.Returns(0x314);
        _ = mock.Number.Returns(69);
        _ = mock.ClearCounts.Returns(items.ToDictionary(item => item, item => (int)item * 10));
        _ = mock.ChallengeCounts.Returns(items.ToDictionary(item => item, item => (int)item * 100));
        _ = mock.HighScore.Returns(456789);
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
            score.ClearCounts.Values,
            score.ChallengeCounts.Values,
            score.HighScore,
            new byte[0x2A8]);
    }

    internal static void Validate(IScore expected, IScore actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Version, actual.Version);
        Assert.AreEqual(expected.Checksum, actual.Checksum);
        Assert.AreEqual(expected.Size, actual.Size);
        Assert.AreEqual(expected.Number, actual.Number);
        CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
        CollectionAssert.That.AreEqual(expected.ChallengeCounts.Values, actual.ChallengeCounts.Values);
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
    [DataRow("SN", (ushort)1, 0x314, true)]
    [DataRow("sn", (ushort)1, 0x314, false)]
    [DataRow("SN", (ushort)0, 0x314, false)]
    [DataRow("SN", (ushort)1, 0x315, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, checksum, size, data));

        Assert.AreEqual(expected, Score.CanInitialize(chapter));
    }
}
