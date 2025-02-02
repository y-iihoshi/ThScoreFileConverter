using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class CardReplacerTests
{
    private static ICardAttack[] CreateCardAttacks()
    {
        var storyCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        var trialCounts = storyCareerMock.TrialCounts;
        var clearCounts = storyCareerMock.ClearCounts;
        var noTrialCounts = trialCounts.ToDictionary(pair => pair.Key, pair => 0);
        var noClearCounts = clearCounts.ToDictionary(pair => pair.Key, pair => 0);
        _ = storyCareerMock.TrialCounts.Returns(noTrialCounts);
        _ = storyCareerMock.ClearCounts.Returns(noClearCounts);

        var practiceCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        _ = practiceCareerMock.TrialCounts.Returns(noTrialCounts);
        _ = practiceCareerMock.ClearCounts.Returns(noClearCounts);

        var attack1Mock = CardAttackTests.MockCardAttack();
        var cardId = attack1Mock.CardId;

        var attack2Mock = CardAttackTests.MockCardAttack();
        _ = attack2Mock.CardId.Returns(++cardId);
        _ = attack2Mock.StoryCareer.Returns(storyCareerMock);
        _ = attack2Mock.PracticeCareer.Returns(practiceCareerMock);
        CardAttackTests.SetupHasTried(attack2Mock);

        return [attack1Mock, attack2Mock];
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(element => (int)element.CardId);

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var replacer = new CardReplacer(cardAttacks, true);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(CardAttacks, false);
        replacer.Replace("%T08CARD123N").ShouldBe("天丸「壺中の天地」");
        replacer.Replace("%T08CARD124N").ShouldBe("覚神「神代の記憶」");
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(CardAttacks, false);
        replacer.Replace("%T08CARD123R").ShouldBe("Lunatic");
        replacer.Replace("%T08CARD124R").ShouldBe("Easy");
    }

    [TestMethod]
    public void ReplaceTestRankLastWord()
    {
        var replacer = new CardReplacer(CardAttacks, false);
        replacer.Replace("%T08CARD206R").ShouldBe("Last Word");
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T08CARD123N").ShouldBe("天丸「壺中の天地」");
        replacer.Replace("%T08CARD124N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T08CARD123R").ShouldBe("Lunatic");
        replacer.Replace("%T08CARD124R").ShouldBe("?????");
    }

    [TestMethod]
    public void ReplaceTestNonexistentName()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T08CARD125N").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T08CARD125R").ShouldBe("?????");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T08XXXX123N").ShouldBe("%T08XXXX123N");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T08CARD223N").ShouldBe("%T08CARD223N");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        replacer.Replace("%T08CARD123X").ShouldBe("%T08CARD123X");
    }
}
