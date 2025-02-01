using System.Collections.Immutable;
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
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var cards = ImmutableDictionary<int, ISpellCard>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cards, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestNoIceCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGHA231").ShouldBe("invoked: 4");
    }

    [TestMethod]
    public void ReplaceTestNoMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGHA232").ShouldBe("invoked: 5");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGHA233").ShouldBe("invoked: 5");
    }

    [TestMethod]
    public void ReplaceTestLevelExtraNoIceCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGXA231").ShouldBe("invoked: 7");
    }

    [TestMethod]
    public void ReplaceTestLevelExtraNoMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGXA232").ShouldBe("invoked: 8");
    }

    [TestMethod]
    public void ReplaceTestLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGXA233").ShouldBe("invoked: 9");
    }

    [TestMethod]
    public void ReplaceTestStageExtra()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGHEXT1").ShouldBe("%T128CRGHEXT1");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalNoIceCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGTA231").ShouldBe("invoked: 16");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalNoMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGTA232").ShouldBe("invoked: 19");
    }

    [TestMethod]
    public void ReplaceTestLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGTA233").ShouldBe("invoked: 21");
    }

    [TestMethod]
    public void ReplaceTestStageTotalNoIceCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGHTTL1").ShouldBe("invoked: 40");
    }

    [TestMethod]
    public void ReplaceTestStageTotalNoMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGHTTL2").ShouldBe("invoked: 48");
    }

    [TestMethod]
    public void ReplaceTestStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGHTTL3").ShouldBe("invoked: 51");
    }

    [TestMethod]
    public void ReplaceTestTotalNoIceCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGTTTL1").ShouldBe("invoked: 167");
    }

    [TestMethod]
    public void ReplaceTestTotalNoMissCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGTTTL2").ShouldBe("invoked: 200");
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGTTTL3").ShouldBe("invoked: 215");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var cards = ImmutableDictionary<int, ISpellCard>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cards, formatterMock);
        replacer.Replace("%T128CRGHA231").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128XXXHA231").ShouldBe("%T128XXXHA231");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGYA231").ShouldBe("%T128CRGYA231");
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGHXXX1").ShouldBe("%T128CRGHXXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(SpellCards, formatterMock);
        replacer.Replace("%T128CRGHA23X").ShouldBe("%T128CRGHA23X");
    }
}
