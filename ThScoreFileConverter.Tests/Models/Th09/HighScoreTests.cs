using System;
using System.Collections.Generic;
using System.IO;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th09;
using ThScoreFileConverter.Models.Th09;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th09;

[TestClass]
public class HighScoreTests
{
    internal static IHighScore MockHighScore()
    {
        var mock = Substitute.For<IHighScore>();
        _ = mock.Signature.Returns("HSCR");
        _ = mock.Size1.Returns((short)0x2C);
        _ = mock.Size2.Returns((short)0x2C);
        _ = mock.Score.Returns(1234567u);
        _ = mock.Chara.Returns(Chara.Marisa);
        _ = mock.Level.Returns(Level.Hard);
        _ = mock.Rank.Returns((short)987);
        _ = mock.Name.Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0"));
        _ = mock.Date.Returns(TestUtils.CP932Encoding.GetBytes("06/01/23\0"));
        _ = mock.ContinueCount.Returns((byte)2);
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
            0u,
            (byte)highScore.Chara,
            (byte)highScore.Level,
            highScore.Rank,
            highScore.Name,
            highScore.Date,
            (byte)0,
            highScore.ContinueCount);
    }

    internal static void Validate(IHighScore expected, IHighScore actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Size1, actual.Size1);
        Assert.AreEqual(expected.Size2, actual.Size2);
        Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
        Assert.AreEqual(expected.Score, actual.Score);
        Assert.AreEqual(expected.Chara, actual.Chara);
        Assert.AreEqual(expected.Level, actual.Level);
        Assert.AreEqual(expected.Rank, actual.Rank);
        CollectionAssert.That.AreEqual(expected.Name, actual.Name);
        CollectionAssert.That.AreEqual(expected.Date, actual.Date);
        Assert.AreEqual(expected.ContinueCount, actual.ContinueCount);
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
    public void HighScoreTestInvalidSignature()
    {
        var mock = MockHighScore();
        var signature = mock.Signature;
        _ = mock.Signature.Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new HighScore(chapter));
    }

    [TestMethod]
    public void HighScoreTestInvalidSize()
    {
        var mock = MockHighScore();
        var size = mock.Size1;
        _ = mock.Size1.Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidDataException>(() => new HighScore(chapter));
    }

    public static IEnumerable<object[]> InvalidCharacters => TestUtils.GetInvalidEnumerators<Chara>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void HighScoreTestInvalidChara(int chara)
    {
        var mock = MockHighScore();
        _ = mock.Chara.Returns((Chara)chara);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidCastException>(() => new HighScore(chapter));
    }

    public static IEnumerable<object[]> InvalidLevels => TestUtils.GetInvalidEnumerators<Level>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void HighScoreTestInvalidLevel(int level)
    {
        var mock = MockHighScore();
        _ = mock.Level.Returns((Level)level);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidCastException>(() => new HighScore(chapter));
    }
}
