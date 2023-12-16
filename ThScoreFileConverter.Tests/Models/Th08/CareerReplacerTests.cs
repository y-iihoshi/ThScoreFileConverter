using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class CareerReplacerTests
{
    private static ICardAttack[] CreateCardAttacks()
    {
        var storyCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        var maxBonuses = storyCareerMock.MaxBonuses;
        var trialCounts = storyCareerMock.TrialCounts;
        var clearCounts = storyCareerMock.ClearCounts;
        _ = storyCareerMock.MaxBonuses.Returns(maxBonuses.ToDictionary(pair => pair.Key, pair => pair.Value * 1000));
        _ = storyCareerMock.TrialCounts.Returns(trialCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 3));
        _ = storyCareerMock.ClearCounts.Returns(clearCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 2));

        var practiceCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        _ = practiceCareerMock.MaxBonuses.Returns(maxBonuses.ToDictionary(pair => pair.Key, pair => pair.Value * 2000));
        _ = practiceCareerMock.TrialCounts.Returns(trialCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 4));
        _ = practiceCareerMock.ClearCounts.Returns(clearCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 3));

        var attack1Mock = CardAttackTests.MockCardAttack();
        var cardId = attack1Mock.CardId;

        var attack2Mock = CardAttackTests.MockCardAttack();
        _ = attack2Mock.CardId.Returns(++cardId);
        _ = attack2Mock.StoryCareer.Returns(storyCareerMock);
        CardAttackTests.SetupHasTried(attack2Mock);

        var attack3Mock = CardAttackTests.MockCardAttack();
        _ = attack3Mock.Level.Returns(LevelPracticeWithTotal.LastWord);
        _ = attack3Mock.CardId.Returns((short)222);
        _ = attack3Mock.PracticeCareer.Returns(practiceCareerMock);
        CardAttackTests.SetupHasTried(attack3Mock);

        return [attack1Mock, attack2Mock, attack3Mock];
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(element => (int)element.CardId);

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(cardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestStoryMaxBonus()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CS123MA1"));
    }

    [TestMethod]
    public void ReplaceTestStoryClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 19", replacer.Replace("%T08CS123MA2"));
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T08CS123MA3"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalMaxBonus()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);

        Assert.AreEqual("invoked: 1001", replacer.Replace("%T08CS000MA1"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 57", replacer.Replace("%T08CS000MA2"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 84", replacer.Replace("%T08CS000MA3"));
    }

    [TestMethod]
    public void ReplaceTestStoryLastWord()
    {
        var mock = CardAttackTests.MockCardAttack();
        _ = mock.CardId.Returns((short)206);
        var cardAttacks = new[] { mock, }.ToDictionary(attack => (int)attack.CardId);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CareerReplacer(cardAttacks, formatterMock);
        Assert.AreEqual("%T08CS206MA1", replacer.Replace("%T08CS206MA1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeMaxBonus()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CP123MA1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 19", replacer.Replace("%T08CP123MA2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T08CP123MA3"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalMaxBonus()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);

        Assert.AreEqual("invoked: 2002", replacer.Replace("%T08CP000MA1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 95", replacer.Replace("%T08CP000MA2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 126", replacer.Replace("%T08CP000MA3"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentMaxBonus()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CS001MA1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CS001MA2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CS001MA3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08XS123MA1", replacer.Replace("%T08XS123MA1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CX123MA1", replacer.Replace("%T08CX123MA1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CS223MA1", replacer.Replace("%T08CS223MA1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CS123MA4", replacer.Replace("%T08CS123MA4"));
    }
}
