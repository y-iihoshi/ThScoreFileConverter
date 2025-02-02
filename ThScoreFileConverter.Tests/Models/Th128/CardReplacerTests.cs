using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models;
using ThScoreFileConverter.Models.Th128;

namespace ThScoreFileConverter.Tests.Models.Th128;

[TestClass]
public class CardReplacerTests
{
    private static ISpellCard MockSpellCard(int id, int trialCount, Level level, bool hasTried)
    {
        var mock = Substitute.For<ISpellCard>();
        _ = mock.Id.Returns(id);
        _ = mock.TrialCount.Returns(trialCount);
        _ = mock.Level.Returns(level);
        _ = mock.HasTried.Returns(hasTried);
        return mock;
    }

    internal static IReadOnlyDictionary<int, ISpellCard> SpellCards { get; } = new[]
    {
        MockSpellCard(3, 1, Level.Hard, true),
        MockSpellCard(4, 0, Level.Lunatic, false),
    }.ToDictionary(card => card.Id);

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(SpellCards, false);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var cards = ImmutableDictionary<int, ISpellCard>.Empty;
        var replacer = new CardReplacer(cards, false);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(SpellCards, false);
        replacer.Replace("%T128CARD003N").ShouldBe("月符「ルナティックレイン」");
        replacer.Replace("%T128CARD004N").ShouldBe("月符「ルナティックレイン」");
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(SpellCards, false);
        replacer.Replace("%T128CARD003R").ShouldBe("Hard");
        replacer.Replace("%T128CARD004R").ShouldBe("Lunatic");
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(SpellCards, true);
        replacer.Replace("%T128CARD003N").ShouldBe("月符「ルナティックレイン」");
        replacer.Replace("%T128CARD004N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(SpellCards, true);
        replacer.Replace("%T128CARD003R").ShouldBe("Hard");
        replacer.Replace("%T128CARD004R").ShouldBe("Lunatic");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<int, ISpellCard>.Empty;

        var replacer = new CardReplacer(dictionary, true);
        replacer.Replace("%T128CARD003N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(SpellCards, false);
        replacer.Replace("%T128XXXX003N").ShouldBe("%T128XXXX003N");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(SpellCards, false);
        replacer.Replace("%T128CARD251N").ShouldBe("%T128CARD251N");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(SpellCards, false);
        replacer.Replace("%T128CARD003X").ShouldBe("%T128CARD003X");
    }
}
