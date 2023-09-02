using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models.Th128;
using Definitions = ThScoreFileConverter.Models.Th128.Definitions;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class CollectRateReplacerTests
{
    private static ISpellCard MockSpellCard(int noIce, int noMiss, int trial, int id, Level level)
    {
        var mock = Substitute.For<ISpellCard>();
        _ = mock.NoIceCount.Returns(noIce);
        _ = mock.NoMissCount.Returns(noMiss);
        _ = mock.TrialCount.Returns(trial);
        _ = mock.Id.Returns(id);
        _ = mock.Level.Returns(level);
        return mock;
    }

    internal static IReadOnlyDictionary<int, ISpellCard> SpellCards { get; } =
        Definitions.CardTable.ToDictionary(
            pair => pair.Key,
            pair => MockSpellCard(pair.Key % 3, pair.Key % 5, pair.Key % 7, pair.Value.Id, pair.Value.Level));

    [TestMethod]
    public void CollectRateReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var cards = ImmutableDictionary<int, ISpellCard>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cards, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestNoIceCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T128CRGHA231"));
    }

    [TestMethod]
    public void ReplaceTestNoMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 5", replacer.Replace("%T128CRGHA232"));
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 5", replacer.Replace("%T128CRGHA233"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtraNoIceCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 7", replacer.Replace("%T128CRGXA231"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtraNoMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 8", replacer.Replace("%T128CRGXA232"));
    }

    [TestMethod]
    public void ReplaceTestLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 9", replacer.Replace("%T128CRGXA233"));
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("%T128CRGHEXT1", replacer.Replace("%T128CRGHEXT1"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalNoIceCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 16", replacer.Replace("%T128CRGTA231"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalNoMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 19", replacer.Replace("%T128CRGTA232"));
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T128CRGTA233"));
    }

    [TestMethod]
    public void ReplaceTestStageTotalNoIceCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 40", replacer.Replace("%T128CRGHTTL1"));
    }

    [TestMethod]
    public void ReplaceTestStageTotalNoMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 48", replacer.Replace("%T128CRGHTTL2"));
    }

    [TestMethod]
    public void ReplaceTestStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 51", replacer.Replace("%T128CRGHTTL3"));
    }

    [TestMethod]
    public void ReplaceTestTotalNoIceCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 167", replacer.Replace("%T128CRGTTTL1"));
    }

    [TestMethod]
    public void ReplaceTestTotalNoMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 200", replacer.Replace("%T128CRGTTTL2"));
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("invoked: 215", replacer.Replace("%T128CRGTTTL3"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var cards = ImmutableDictionary<int, ISpellCard>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cards, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T128CRGHA231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("%T128XXXHA231", replacer.Replace("%T128XXXHA231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("%T128CRGYA231", replacer.Replace("%T128CRGYA231"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("%T128CRGHXXX1", replacer.Replace("%T128CRGHXXX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        Assert.AreEqual("%T128CRGHA23X", replacer.Replace("%T128CRGHA23X"));
    }
}
