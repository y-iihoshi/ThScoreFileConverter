﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using ThScoreFileConverter.Models.Th125;

namespace ThScoreFileConverter.Tests.Models.Th125;

[TestClass]
public class CardReplacerTests
{
    internal static IReadOnlyList<IScore> Scores { get; } = [ScoreTests.MockScore()];

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var replacer = new CardReplacer(scores, true);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestEnemy()
    {
        using var backup = TestHelper.BackupCultureInfo();
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");

        var replacer = new CardReplacer(Scores, false);
        Assert.AreEqual("古明地 こいし", replacer.Replace("%T125CARD961"));
        Assert.AreEqual("古明地 さとり", replacer.Replace("%T125CARD971"));
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(Scores, false);
        Assert.AreEqual("「胎児の夢」", replacer.Replace("%T125CARD962"));
        Assert.AreEqual("想起「うろおぼえの金閣寺」", replacer.Replace("%T125CARD972"));
    }

    [TestMethod]
    public void ReplaceTestHiddenEnemy()
    {
        using var backup = TestHelper.BackupCultureInfo();
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ja-JP");

        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("??????????", replacer.Replace("%T125CARD961"));
        Assert.AreEqual("古明地 さとり", replacer.Replace("%T125CARD971"));
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("??????????", replacer.Replace("%T125CARD962"));
        Assert.AreEqual("想起「うろおぼえの金閣寺」", replacer.Replace("%T125CARD972"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var scores = ImmutableList<IScore>.Empty;
        var replacer = new CardReplacer(scores, true);
        Assert.AreEqual("??????????", replacer.Replace("%T125CARD961"));
        Assert.AreEqual("??????????", replacer.Replace("%T125CARD962"));
    }

    [TestMethod]
    public void ReplaceTestNullScore()
    {
        var scores = new List<IScore> { null! };
        var replacer = new CardReplacer(scores, true);
        Assert.AreEqual("??????????", replacer.Replace("%T125CARD961"));
        Assert.AreEqual("??????????", replacer.Replace("%T125CARD962"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentSpellCard()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("%T125CARD991", replacer.Replace("%T125CARD991"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("%T125XXXX961", replacer.Replace("%T125XXXX961"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("%T125CARDY61", replacer.Replace("%T125CARDY61"));
    }

    [TestMethod]
    public void ReplaceTestInvalidScene()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("%T125CARD9X1", replacer.Replace("%T125CARD9X1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(Scores, true);
        Assert.AreEqual("%T125CARD96X", replacer.Replace("%T125CARD96X"));
    }
}
