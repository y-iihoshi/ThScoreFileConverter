using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class CardReplacerTests
{
    private static IEnumerable<ICardAttack> CreateCardAttacks()
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

        var attack2Mock = CardAttackTests.MockCardAttack();
        _ = attack2Mock.CardId.Returns(_ => (short)(attack1Mock.CardId + 1));
        _ = attack2Mock.StoryCareer.Returns(storyCareerMock);
        _ = attack2Mock.PracticeCareer.Returns(practiceCareerMock);
        CardAttackTests.SetupHasTried(attack2Mock);

        return new[] { attack1Mock, attack2Mock };
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(element => (int)element.CardId);

    [TestMethod]
    public void CardReplacerTest()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CardReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var replacer = new CardReplacer(cardAttacks, true);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestName()
    {
        var replacer = new CardReplacer(CardAttacks, false);
        Assert.AreEqual("天丸「壺中の天地」", replacer.Replace("%T08CARD123N"));
        Assert.AreEqual("覚神「神代の記憶」", replacer.Replace("%T08CARD124N"));
    }

    [TestMethod]
    public void ReplaceTestRank()
    {
        var replacer = new CardReplacer(CardAttacks, false);
        Assert.AreEqual("Lunatic", replacer.Replace("%T08CARD123R"));
        Assert.AreEqual("Easy", replacer.Replace("%T08CARD124R"));
    }

    [TestMethod]
    public void ReplaceTestRankLastWord()
    {
        var replacer = new CardReplacer(CardAttacks, false);
        Assert.AreEqual("Last Word", replacer.Replace("%T08CARD206R"));
    }

    [TestMethod]
    public void ReplaceTestHiddenName()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("天丸「壺中の天地」", replacer.Replace("%T08CARD123N"));
        Assert.AreEqual("??????????", replacer.Replace("%T08CARD124N"));
    }

    [TestMethod]
    public void ReplaceTestHiddenRank()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("Lunatic", replacer.Replace("%T08CARD123R"));
        Assert.AreEqual("?????", replacer.Replace("%T08CARD124R"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentName()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("??????????", replacer.Replace("%T08CARD125N"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentRank()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("?????", replacer.Replace("%T08CARD125R"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("%T08XXXX123N", replacer.Replace("%T08XXXX123N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("%T08CARD223N", replacer.Replace("%T08CARD223N"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var replacer = new CardReplacer(CardAttacks, true);
        Assert.AreEqual("%T08CARD123X", replacer.Replace("%T08CARD123X"));
    }
}
