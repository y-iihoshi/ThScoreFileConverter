using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Models.Th08;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using Stage = ThScoreFileConverter.Core.Models.Th08.Stage;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class PracticeScoreTests
{
    internal static IPracticeScore MockPracticeScore()
    {
        var pairs = EnumHelper.Cartesian<Stage, Level>().ToArray();
        var mock = Substitute.For<IPracticeScore>();
        _ = mock.Signature.Returns("PSCR");
        _ = mock.Size1.Returns((short)0x178);
        _ = mock.Size2.Returns((short)0x178);
        _ = mock.PlayCounts.Returns(pairs.ToDictionary(pair => pair, pair => ((int)pair.First * 10) + (int)pair.Second));
        _ = mock.HighScores.Returns(pairs.ToDictionary(pair => pair, pair => ((int)pair.Second * 10) + (int)pair.First));
        _ = mock.Chara.Returns(Chara.MarisaAlice);
        return mock;
    }

    internal static byte[] MakeByteArray(IPracticeScore score)
    {
        return TestUtils.MakeByteArray(
            score.Signature.ToCharArray(),
            score.Size1,
            score.Size2,
            0u,
            score.PlayCounts.Values,
            score.HighScores.Values,
            (byte)score.Chara,
            new byte[3]);
    }

    internal static void Validate(IPracticeScore expected, IPracticeScore actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Size1, actual.Size1);
        Assert.AreEqual(expected.Size2, actual.Size2);
        Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
        CollectionAssert.That.AreEqual(expected.PlayCounts.Values, actual.PlayCounts.Values);
        CollectionAssert.That.AreEqual(expected.HighScores.Values, actual.HighScores.Values);
        Assert.AreEqual(expected.Chara, actual.Chara);
    }

    [TestMethod]
    public void PracticeScoreTestChapter()
    {
        var mock = MockPracticeScore();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        var score = new PracticeScore(chapter);

        Validate(mock, score);
    }

    [TestMethod]
    public void PracticeScoreTestInvalidSignature()
    {
        var mock = MockPracticeScore();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new PracticeScore(chapter));
    }

    [TestMethod]
    public void PracticeScoreTestInvalidSize1()
    {
        var mock = MockPracticeScore();
        var size = mock.Size1;
        _ = mock.Size1.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new PracticeScore(chapter));
    }

    public static IEnumerable<object[]> InvalidCharacters => TestUtils.GetInvalidEnumerators<Chara>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void PracticeScoreTestInvalidChara(int chara)
    {
        var mock = MockPracticeScore();
        _ = mock.Chara.Returns((Chara)chara);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidCastException>(() => new PracticeScore(chapter));
    }
}
