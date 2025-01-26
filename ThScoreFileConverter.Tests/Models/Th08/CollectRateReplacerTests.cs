using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class CollectRateReplacerTests
{
    private static ICardAttack[] CreateCardAttacks()
    {
        var id2StoryCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        var trialCounts = id2StoryCareerMock.TrialCounts;
        var clearCounts = id2StoryCareerMock.ClearCounts;
        var id2TrialCounts = trialCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 4);
        var id2ClearCounts = clearCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 3);
        _ = id2StoryCareerMock.TrialCounts.Returns(id2TrialCounts);
        _ = id2StoryCareerMock.ClearCounts.Returns(id2ClearCounts);

        var id2PracticeCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        _ = id2PracticeCareerMock.TrialCounts.Returns(id2TrialCounts);
        _ = id2PracticeCareerMock.ClearCounts.Returns(id2ClearCounts);

        var id6StoryCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        var id6TrialCounts = trialCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 5);
        var id6ClearCounts = clearCounts.ToDictionary(pair => pair.Key, pair => 0);
        _ = id6StoryCareerMock.TrialCounts.Returns(id6TrialCounts);
        _ = id6StoryCareerMock.ClearCounts.Returns(id6ClearCounts);

        var id6PracticeCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        _ = id6PracticeCareerMock.TrialCounts.Returns(id6TrialCounts);
        _ = id6PracticeCareerMock.ClearCounts.Returns(id6ClearCounts);

        var id192StoryCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        var id192TrialCounts = trialCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 2);
        var id192ClearCounts = clearCounts.ToDictionary(pair => pair.Key, pair => 0);
        _ = id192StoryCareerMock.TrialCounts.Returns(id192TrialCounts);
        _ = id192StoryCareerMock.ClearCounts.Returns(id192ClearCounts);

        var id192PracticeCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        _ = id192PracticeCareerMock.TrialCounts.Returns(id192TrialCounts);
        _ = id192PracticeCareerMock.ClearCounts.Returns(id192ClearCounts);

        var id222StoryCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        var id222TrialCounts = trialCounts.ToDictionary(pair => pair.Key, pair => 0);
        var id222ClearCounts = clearCounts.ToDictionary(pair => pair.Key, pair => 0);
        _ = id222StoryCareerMock.TrialCounts.Returns(id222TrialCounts);
        _ = id222StoryCareerMock.ClearCounts.Returns(id222ClearCounts);

        var id222PracticeCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        _ = id222PracticeCareerMock.TrialCounts.Returns(id222TrialCounts);
        _ = id222PracticeCareerMock.ClearCounts.Returns(id222ClearCounts);

        var attack1Mock = CardAttackTests.MockCardAttack();

        var attack2Mock = CardAttackTests.MockCardAttack();
        _ = attack2Mock.CardId.Returns((short)2);
        _ = attack2Mock.StoryCareer.Returns(id2StoryCareerMock);
        _ = attack2Mock.PracticeCareer.Returns(id2PracticeCareerMock);
        CardAttackTests.SetupHasTried(attack2Mock);

        var attack3Mock = CardAttackTests.MockCardAttack();
        _ = attack3Mock.CardId.Returns((short)6);
        _ = attack3Mock.StoryCareer.Returns(id6StoryCareerMock);
        _ = attack3Mock.PracticeCareer.Returns(id6PracticeCareerMock);
        CardAttackTests.SetupHasTried(attack3Mock);

        var attack4Mock = CardAttackTests.MockCardAttack();
        _ = attack4Mock.CardId.Returns((short)192);
        _ = attack4Mock.StoryCareer.Returns(id192StoryCareerMock);
        _ = attack4Mock.PracticeCareer.Returns(id192PracticeCareerMock);
        CardAttackTests.SetupHasTried(attack4Mock);

        var attack5Mock = CardAttackTests.MockCardAttack();
        _ = attack5Mock.CardId.Returns((short)222);
        _ = attack5Mock.StoryCareer.Returns(id222StoryCareerMock);
        _ = attack5Mock.PracticeCareer.Returns(id222PracticeCareerMock);
        CardAttackTests.SetupHasTried(attack5Mock);

        return [attack1Mock, attack2Mock, attack3Mock, attack4Mock, attack5Mock];
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(element => (int)element.CardId);

    [TestMethod]
    public void CollectRateReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CollectRateReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestStoryClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGSLMA1A1"));
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGSLMA1A2"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelExtraClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGSXMA1A1"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGSXMA1A2"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelLastWordClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CRGSWMA1A1", replacer.Replace("%T08CRGSWMA1A1"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelLastWordTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CRGSWMA1A2", replacer.Replace("%T08CRGSWMA1A2"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGSTMA1A1"));
    }

    [TestMethod]
    public void ReplaceTestStoryLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGSTMA1A2"));
    }

    [TestMethod]
    public void ReplaceTestStoryCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGSLTL1A1"));
    }

    [TestMethod]
    public void ReplaceTestStoryCharaTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGSLTL1A2"));
    }

    [TestMethod]
    public void ReplaceTestStoryStageTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGSLMA001"));
    }

    [TestMethod]
    public void ReplaceTestStoryStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T08CRGSLMA002"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGSTTL001"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T08CRGSTTL002"));
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGPLMA1A1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGPLMA1A2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelExtraClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGPXMA1A1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelExtraTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGPXMA1A2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelLastWordClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGPWMA1A1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelLastWordTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGPWMA1A2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGPTMA1A1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeLevelTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGPTMA1A2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCharaTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CRGPLTL1A1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeCharaTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGPLTL1A2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeStageTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGPLMA001"));
    }

    [TestMethod]
    public void ReplaceTestPracticeStageTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T08CRGPLMA002"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 2", replacer.Replace("%T08CRGPTTL001"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 4", replacer.Replace("%T08CRGPTTL002"));
    }

    [TestMethod]
    public void ReplaceTestEmptyClearCount()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGSLMA1A1"));
    }

    [TestMethod]
    public void ReplaceTestEmptyTrialCount()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGSLMA1A2"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08XXXSLMA1A1", replacer.Replace("%T08XXXSLMA1A1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CRGXLMA1A1", replacer.Replace("%T08CRGXLMA1A1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CRGSYMA1A1", replacer.Replace("%T08CRGSYMA1A1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CRGSLXX1A1", replacer.Replace("%T08CRGSLXX1A1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidStage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CRGSLMAXX1", replacer.Replace("%T08CRGSLMAXX1"));
        Assert.AreEqual("%T08CRGSLMAEX1", replacer.Replace("%T08CRGSLMAEX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CRGSLMA1AX", replacer.Replace("%T08CRGSLMA1AX"));
    }

    [TestMethod]
    public void ReplaceTestInvalidCardId()
    {
        var mock = CardAttackTests.MockCardAttack();
        _ = mock.CardId.Returns((short)223);
        var cardAttacks = new[] { mock }.ToDictionary(attack => (int)attack.CardId);

        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CollectRateReplacer(cardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGSLMA1A1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGSXMA1A1"));
        Assert.AreEqual("%T08CRGSWMA1A1", replacer.Replace("%T08CRGSWMA1A1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGPLMA1A1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGPXMA1A1"));
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CRGPWMA1A1"));
    }
}
