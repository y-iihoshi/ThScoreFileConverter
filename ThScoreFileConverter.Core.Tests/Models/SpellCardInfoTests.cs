using System;
using System.Collections.Generic;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Core.Tests.UnitTesting;

namespace ThScoreFileConverter.Core.Tests.Models;

using CardInfo = SpellCardInfo<Stage, Level>;

[TestClass]
public class SpellCardInfoTests
{
    [TestMethod]
    public void SpellCardInfoTest()
    {
        var info = new CardInfo(1, "月符「ムーンライトレイ」", Stage.One, Level.Hard, Level.Lunatic);

        Assert.AreEqual(1, info.Id);
        Assert.AreEqual("月符「ムーンライトレイ」", info.Name);
        Assert.AreEqual(Stage.One, info.Stage);
        Assert.AreEqual(Level.Hard, info.Level);
        CollectionAssert.That.AreEqual(new[] { Level.Hard, Level.Lunatic }, info.Levels);
    }

    [TestMethod]
    public void SpellCardInfoTestNegativeId()
    {
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new CardInfo(-1, "月符「ムーンライトレイ」", Stage.One, Level.Hard, Level.Lunatic));
    }

    [TestMethod]
    public void SpellCardInfoTestZeroId()
    {
        _ = Assert.ThrowsException<ArgumentOutOfRangeException>(
            () => new CardInfo(0, "月符「ムーンライトレイ」", Stage.One, Level.Hard, Level.Lunatic));
    }

    [TestMethod]
    public void SpellCardInfoTestEmptyName()
    {
        _ = Assert.ThrowsException<ArgumentException>(
            () => new CardInfo(1, string.Empty, Stage.One, Level.Hard, Level.Lunatic));
    }

    public static IEnumerable<object[]> InvalidStages
        => TestHelper.GetInvalidEnumerators(typeof(Stage));

    [DataTestMethod]
    [DynamicData(nameof(InvalidStages))]
    public void SpellCardInfoTestInvalidStage(int stage)
    {
        var invalid = TestHelper.Cast<Stage>(stage);
        _ = Assert.ThrowsException<ArgumentException>(
            () => new CardInfo(1, "月符「ムーンライトレイ」", invalid, Level.Hard, Level.Lunatic));
    }

    public static IEnumerable<object[]> InvalidLevels
        => TestHelper.GetInvalidEnumerators(typeof(Level));

    [DataTestMethod]
    [DynamicData(nameof(InvalidLevels))]
    public void SpellCardInfoTestInvalidLevel(int level)
    {
        var invalid = TestHelper.Cast<Level>(level);
        _ = Assert.ThrowsException<ArgumentException>(
            () => new CardInfo(1, "月符「ムーンライトレイ」", Stage.One, Level.Hard, invalid));
    }

    [TestMethod]
    public void SpellCardInfoTestOneLevel()
    {
        var info = new CardInfo(1, "霜符「フロストコラムス」", Stage.One, Level.Hard);

        Assert.AreEqual(1, info.Id);
        Assert.AreEqual("霜符「フロストコラムス」", info.Name);
        Assert.AreEqual(Stage.One, info.Stage);
        Assert.AreEqual(Level.Hard, info.Level);
        CollectionAssert.That.AreEqual(new[] { Level.Hard }, info.Levels);
    }

    [TestMethod]
    public void SpellCardInfoTestZeroLevels()
    {
        _ = Assert.ThrowsException<ArgumentException>(
            () => new CardInfo(1, "霜符「フロストコラムス」", Stage.One));
    }
}
