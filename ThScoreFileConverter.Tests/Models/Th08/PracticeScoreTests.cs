using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Core.Tests.UnitTesting;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using Stage = ThScoreFileConverter.Core.Models.Th08.Stage;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class PracticeScoreTests
{
    internal static Mock<IPracticeScore> MockPracticeScore()
    {
        var pairs = EnumHelper.Cartesian<Stage, Level>();
        var mock = new Mock<IPracticeScore>();
        _ = mock.SetupGet(m => m.Signature).Returns("PSCR");
        _ = mock.SetupGet(m => m.Size1).Returns(0x178);
        _ = mock.SetupGet(m => m.Size2).Returns(0x178);
        _ = mock.SetupGet(m => m.PlayCounts).Returns(
            pairs.ToDictionary(pair => pair, pair => ((int)pair.First * 10) + (int)pair.Second));
        _ = mock.SetupGet(m => m.HighScores).Returns(
            pairs.ToDictionary(pair => pair, pair => ((int)pair.Second * 10) + (int)pair.First));
        _ = mock.SetupGet(m => m.Chara).Returns(Chara.MarisaAlice);
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

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        var score = new PracticeScore(chapter);

        Validate(mock.Object, score);
    }

    [TestMethod]
    public void PracticeScoreTestInvalidSignature()
    {
        var mock = MockPracticeScore();
        var signature = mock.Object.Signature;
        _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidDataException>(() => new PracticeScore(chapter));
    }

    [TestMethod]
    public void PracticeScoreTestInvalidSize1()
    {
        var mock = MockPracticeScore();
        var size = mock.Object.Size1;
        _ = mock.SetupGet(m => m.Size1).Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidDataException>(() => new PracticeScore(chapter));
    }

    public static IEnumerable<object[]> InvalidCharacters
        => TestUtils.GetInvalidEnumerators(typeof(Chara));

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void PracticeScoreTestInvalidChara(int chara)
    {
        var mock = MockPracticeScore();
        _ = mock.SetupGet(m => m.Chara).Returns(TestUtils.Cast<Chara>(chara));

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidCastException>(() => new PracticeScore(chapter));
    }
}
