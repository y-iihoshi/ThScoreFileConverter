﻿using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Models.Th08;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class HighScoreTests
{
    internal static IHighScore MockHighScore()
    {
        var mock = Substitute.For<IHighScore>();
        _ = mock.Signature.Returns("HSCR");
        _ = mock.Size1.Returns((short)0x0168);
        _ = mock.Size2.Returns((short)0x0168);
        _ = mock.Score.Returns(1234567u);
        _ = mock.SlowRate.Returns(9.87f);
        _ = mock.Chara.Returns(Chara.MarisaAlice);
        _ = mock.Level.Returns(Level.Hard);
        _ = mock.StageProgress.Returns(StageProgress.Three);
        _ = mock.Name.Returns(TestUtils.CP932Encoding.GetBytes("Player1\0\0"));
        _ = mock.Date.Returns(TestUtils.CP932Encoding.GetBytes("01/23\0"));
        _ = mock.ContinueCount.Returns((ushort)2);
        _ = mock.PlayerNum.Returns((byte)5);
        _ = mock.PlayTime.Returns(987654u);
        _ = mock.PointItem.Returns(1234);
        _ = mock.MissCount.Returns(9);
        _ = mock.BombCount.Returns(6);
        _ = mock.LastSpellCount.Returns(12);
        _ = mock.PauseCount.Returns(3);
        _ = mock.TimePoint.Returns(65432);
        _ = mock.HumanRate.Returns(7890);
        _ = mock.CardFlags.Returns(Enumerable.Range(1, 222).ToDictionary(id => id, id => (byte)(id is 3 or 7 ? id : 0)));
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
            highScore.ContinueCount,
            new byte[0x1C],
            highScore.PlayerNum,
            new byte[0x1F],
            highScore.PlayTime,
            highScore.PointItem,
            0u,
            highScore.MissCount,
            highScore.BombCount,
            highScore.LastSpellCount,
            highScore.PauseCount,
            highScore.TimePoint,
            highScore.HumanRate,
            highScore.CardFlags.Values,
            new byte[2]);
    }

    internal static void Validate(IHighScore expected, IHighScore actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Size1, actual.Size1);
        Assert.AreEqual(expected.Size2, actual.Size2);
        Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
        Assert.AreEqual(expected.Score, actual.Score);
        Assert.AreEqual(expected.SlowRate, actual.SlowRate);
        Assert.AreEqual(expected.Chara, actual.Chara);
        Assert.AreEqual(expected.Level, actual.Level);
        Assert.AreEqual(expected.StageProgress, actual.StageProgress);
        CollectionAssert.That.AreEqual(expected.Name, actual.Name);
        CollectionAssert.That.AreEqual(expected.Date, actual.Date);
        Assert.AreEqual(expected.ContinueCount, actual.ContinueCount);
        Assert.AreEqual(expected.PlayerNum, actual.PlayerNum);
        Assert.AreEqual(expected.PlayTime, actual.PlayTime);
        Assert.AreEqual(expected.PointItem, actual.PointItem);
        Assert.AreEqual(expected.MissCount, actual.MissCount);
        Assert.AreEqual(expected.BombCount, actual.BombCount);
        Assert.AreEqual(expected.LastSpellCount, actual.LastSpellCount);
        Assert.AreEqual(expected.PauseCount, actual.PauseCount);
        Assert.AreEqual(expected.TimePoint, actual.TimePoint);
        Assert.AreEqual(expected.HumanRate, actual.HumanRate);
        CollectionAssert.That.AreEqual(expected.CardFlags.Values, actual.CardFlags.Values);
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
        var cardFlags = Enumerable.Empty<byte>();

        var highScore = new HighScore(score);

        Assert.AreEqual(score, highScore.Score);
        CollectionAssert.That.AreEqual(TestUtils.CP932Encoding.GetBytes(name), highScore.Name);
        CollectionAssert.That.AreEqual(TestUtils.CP932Encoding.GetBytes(date), highScore.Date);
        CollectionAssert.That.AreEqual(cardFlags, highScore.CardFlags.Values);
    }

    [TestMethod]
    public void HighScoreTestZeroScore()
    {
        var score = 0u;
        var name = "--------\0";
        var date = "--/--\0";
        var cardFlags = Enumerable.Empty<byte>();

        var highScore = new HighScore(score);

        Assert.AreEqual(score, highScore.Score);
        CollectionAssert.That.AreEqual(TestUtils.CP932Encoding.GetBytes(name), highScore.Name);
        CollectionAssert.That.AreEqual(TestUtils.CP932Encoding.GetBytes(date), highScore.Date);
        CollectionAssert.That.AreEqual(cardFlags, highScore.CardFlags.Values);
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
    public void HighScoreTestInvalidSize1()
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

    public static IEnumerable<object[]> InvalidStageProgresses => TestUtils.GetInvalidEnumerators<StageProgress>();

    [DataTestMethod]
    [DynamicData(nameof(InvalidStageProgresses))]
    public void HighScoreTestInvalidStageProgress(int stageProgress)
    {
        var mock = MockHighScore();
        _ = mock.StageProgress.Returns((StageProgress)stageProgress);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock));
        _ = Assert.ThrowsException<InvalidCastException>(() => new HighScore(chapter));
    }
}
