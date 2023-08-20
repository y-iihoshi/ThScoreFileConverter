using System;
using System.Collections.Generic;
using System.IO;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th06;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Tests.UnitTesting;

namespace ThScoreFileConverter.Tests.Models.Th06;

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

    internal static void Validate(IPracticeScore expected, IPracticeScore actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Size1, actual.Size1);
        Assert.AreEqual(expected.Size2, actual.Size2);
        Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
        Assert.AreEqual(expected.HighScore, actual.HighScore);
        Assert.AreEqual(expected.Chara, actual.Chara);
        Assert.AreEqual(expected.Level, actual.Level);
        Assert.AreEqual(expected.Stage, actual.Stage);
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

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<Level>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void PracticeScoreTestInvalidLevel(int level)
    {
        var mock = MockPracticeScore();
        _ = mock.Level.Returns((Level)level);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidCastException>(() => new PracticeScore(chapter));
    }

    public static IEnumerable<object[]> InvalidStages => TestUtils.GetInvalidEnumerators<Stage>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStages))]
    public void PracticeScoreTestInvalidStage(int stage)
    {
        var mock = MockPracticeScore();
        _ = mock.Stage.Returns((Stage)stage);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidCastException>(() => new PracticeScore(chapter));
    }
}
