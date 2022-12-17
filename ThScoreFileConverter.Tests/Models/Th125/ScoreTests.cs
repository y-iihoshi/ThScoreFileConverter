using System;
using System.Collections.Generic;
using System.IO;
using Moq;
using ThScoreFileConverter.Core.Models.Th125;
using ThScoreFileConverter.Models.Th125;
using ThScoreFileConverter.Tests.UnitTesting;
using Chapter = ThScoreFileConverter.Models.Th095.Chapter;

namespace ThScoreFileConverter.Tests.Models.Th125;

[TestClass]
public class ScoreTests
{
    internal static Mock<IScore> MockScore()
    {
        var mock = new Mock<IScore>();
        _ = mock.SetupGet(m => m.Signature).Returns("SC");
        _ = mock.SetupGet(m => m.Version).Returns(0);
        _ = mock.SetupGet(m => m.Size).Returns(0x48);
        _ = mock.SetupGet(m => m.Checksum).Returns(0u);
        _ = mock.SetupGet(m => m.LevelScene).Returns((Level.Nine, 7));
        _ = mock.SetupGet(m => m.HighScore).Returns(1234567);
        _ = mock.SetupGet(m => m.Chara).Returns(Chara.Hatate);
        _ = mock.SetupGet(m => m.TrialCount).Returns(9876);
        _ = mock.SetupGet(m => m.FirstSuccess).Returns(5432);
        _ = mock.SetupGet(m => m.DateTime).Returns(34567890);
        _ = mock.SetupGet(m => m.BestshotScore).Returns(23456);
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
            new byte[4],
            (int)score.Chara,
            new byte[4],
            score.TrialCount,
            score.FirstSuccess,
            0u,
            score.DateTime,
            new uint[3],
            score.BestshotScore,
            new byte[8]);
    }

    internal static void Validate(IScore expected, IScore actual)
    {
        Assert.AreEqual(expected.Signature, actual.Signature);
        Assert.AreEqual(expected.Version, actual.Version);
        Assert.AreEqual(expected.Size, actual.Size);
        Assert.AreEqual(expected.Checksum, actual.Checksum);
        Assert.AreEqual(expected.LevelScene, actual.LevelScene);
        Assert.AreEqual(expected.HighScore, actual.HighScore);
        Assert.AreEqual(expected.Chara, actual.Chara);
        Assert.AreEqual(expected.TrialCount, actual.TrialCount);
        Assert.AreEqual(expected.FirstSuccess, actual.FirstSuccess);
        Assert.AreEqual(expected.DateTime, actual.DateTime);
        Assert.AreEqual(expected.BestshotScore, actual.BestshotScore);
    }

    [TestMethod]
    public void ScoreTestChapter()
    {
        var mock = MockScore();

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        var score = new Score(chapter);

        Validate(mock.Object, score);
        Assert.IsFalse(score.IsValid);
    }

    [TestMethod]
    public void ScoreTestInvalidSignature()
    {
        var mock = MockScore();
        var signature = mock.Object.Signature;
        _ = mock.SetupGet(m => m.Signature).Returns(signature.ToLowerInvariant());

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidDataException>(() => new Score(chapter));
    }

    [TestMethod]
    public void ScoreTestInvalidVersion()
    {
        var mock = MockScore();
        var version = mock.Object.Version;
        _ = mock.SetupGet(m => m.Version).Returns(++version);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidDataException>(() => new Score(chapter));
    }

    [TestMethod]
    public void ScoreTestInvalidSize()
    {
        var mock = MockScore();
        var size = mock.Object.Size;
        _ = mock.SetupGet(m => m.Size).Returns(--size);

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidDataException>(() => new Score(chapter));
    }

    public static IEnumerable<object[]> InvalidLevels
        => TestUtils.GetInvalidEnumerators(typeof(Level));

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void ScoreTestInvalidLevel(int level)
    {
        var mock = MockScore();
        var levelScene = mock.Object.LevelScene;
        _ = mock.SetupGet(m => m.LevelScene).Returns((TestUtils.Cast<Level>(level), levelScene.Scene));

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidCastException>(() => new Score(chapter));
    }

    public static IEnumerable<object[]> InvalidCharacters
        => TestUtils.GetInvalidEnumerators(typeof(Chara));

    [DataTestMethod]
    [DynamicData(nameof(InvalidCharacters))]
    public void ScoreTestInvalidChara(int chara)
    {
        var mock = MockScore();
        _ = mock.SetupGet(m => m.Chara).Returns(TestUtils.Cast<Chara>(chara));

        var chapter = TestUtils.Create<Chapter>(MakeByteArray(mock.Object));
        _ = Assert.ThrowsException<InvalidCastException>(() => new Score(chapter));
    }

    [DataTestMethod]
    [DataRow("SC", (ushort)0, 0x48, true)]
    [DataRow("sc", (ushort)0, 0x48, false)]
    [DataRow("SC", (ushort)1, 0x48, false)]
    [DataRow("SC", (ushort)0, 0x49, false)]
    public void CanInitializeTest(string signature, ushort version, int size, bool expected)
    {
        var checksum = 0u;
        var data = new byte[size];

        var chapter = TestUtils.Create<Chapter>(
            TestUtils.MakeByteArray(signature.ToCharArray(), version, size, checksum, data));

        Assert.AreEqual(expected, Score.CanInitialize(chapter));
    }
}
