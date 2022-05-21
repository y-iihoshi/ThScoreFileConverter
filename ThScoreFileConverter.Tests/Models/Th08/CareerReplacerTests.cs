using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class CareerReplacerTests
{
    private static IEnumerable<ICardAttack> CreateCardAttacks()
    {
        var storyCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        var maxBonuses = storyCareerMock.Object.MaxBonuses;
        var trialCounts = storyCareerMock.Object.TrialCounts;
        var clearCounts = storyCareerMock.Object.ClearCounts;
        _ = storyCareerMock.SetupGet(m => m.MaxBonuses).Returns(
            maxBonuses.ToDictionary(pair => pair.Key, pair => pair.Value * 1000));
        _ = storyCareerMock.SetupGet(m => m.TrialCounts).Returns(
            trialCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 3));
        _ = storyCareerMock.SetupGet(m => m.ClearCounts).Returns(
            clearCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 2));

        var practiceCareerMock = CardAttackCareerTests.MockCardAttackCareer();
        _ = practiceCareerMock.SetupGet(m => m.MaxBonuses).Returns(
            maxBonuses.ToDictionary(pair => pair.Key, pair => pair.Value * 2000));
        _ = practiceCareerMock.SetupGet(m => m.TrialCounts).Returns(
            trialCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 4));
        _ = practiceCareerMock.SetupGet(m => m.ClearCounts).Returns(
            clearCounts.ToDictionary(pair => pair.Key, pair => pair.Value * 3));

        var attack1Mock = CardAttackTests.MockCardAttack();

        var attack2Mock = CardAttackTests.MockCardAttack();
        _ = attack2Mock.SetupGet(m => m.CardId).Returns((short)(attack1Mock.Object.CardId + 1));
        _ = attack2Mock.SetupGet(m => m.StoryCareer).Returns(storyCareerMock.Object);
        CardAttackTests.SetupHasTried(attack2Mock);

        var attack3Mock = CardAttackTests.MockCardAttack();
        _ = attack3Mock.SetupGet(m => m.Level).Returns(LevelPracticeWithTotal.LastWord);
        _ = attack3Mock.SetupGet(m => m.CardId).Returns(222);
        _ = attack3Mock.SetupGet(m => m.PracticeCareer).Returns(practiceCareerMock.Object);
        CardAttackTests.SetupHasTried(attack3Mock);

        return new[] { attack1Mock.Object, attack2Mock.Object, attack3Mock.Object };
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(element => (int)element.CardId);

    private static Mock<INumberFormatter> MockNumberFormatter()
    {
        var mock = new Mock<INumberFormatter>();
        _ = mock.Setup(formatter => formatter.FormatNumber(It.IsAny<It.IsValueType>()))
            .Returns((object value) => "invoked: " + value.ToString());
        return mock;
    }

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(cardAttacks, formatterMock.Object);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestStoryMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CS123MA1"));
    }

    [TestMethod]
    public void ReplaceTestStoryClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 19", replacer.Replace("%T08CS123MA2"));
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T08CS123MA3"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);

        Assert.AreEqual("invoked: 1001", replacer.Replace("%T08CS000MA1"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 57", replacer.Replace("%T08CS000MA2"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 84", replacer.Replace("%T08CS000MA3"));
    }

    [TestMethod]
    public void ReplaceTestStoryLastWord()
    {
        var mock = CardAttackTests.MockCardAttack();
        _ = mock.SetupGet(m => m.CardId).Returns(206);
        var cardAttacks = new[] { mock.Object, }.ToDictionary(attack => (int)attack.CardId);
        var formatterMock = MockNumberFormatter();

        var replacer = new CareerReplacer(cardAttacks, formatterMock.Object);
        Assert.AreEqual("%T08CS206MA1", replacer.Replace("%T08CS206MA1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CP123MA1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 19", replacer.Replace("%T08CP123MA2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T08CP123MA3"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);

        Assert.AreEqual("invoked: 2002", replacer.Replace("%T08CP000MA1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 95", replacer.Replace("%T08CP000MA2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 126", replacer.Replace("%T08CP000MA3"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CS001MA1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CS001MA2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CS001MA3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T08XS123MA1", replacer.Replace("%T08XS123MA1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T08CX123MA1", replacer.Replace("%T08CX123MA1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T08CS223MA1", replacer.Replace("%T08CS223MA1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock.Object);
        Assert.AreEqual("%T08CS123MA4", replacer.Replace("%T08CS123MA4"));
    }
}
