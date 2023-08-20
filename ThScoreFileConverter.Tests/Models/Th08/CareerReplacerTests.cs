﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th08;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;

namespace ThScoreFileConverter.Tests.Models.Th08;

[TestClass]
public class CareerReplacerTests
{
    private static IEnumerable<ICardAttack> CreateCardAttacks()
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

        var attack2Mock = CardAttackTests.MockCardAttack();
        _ = attack2Mock.CardId.Returns(_ => (short)(attack1Mock.CardId + 1));
        _ = attack2Mock.StoryCareer.Returns(storyCareerMock);
        CardAttackTests.SetupHasTried(attack2Mock);

        var attack3Mock = CardAttackTests.MockCardAttack();
        _ = attack3Mock.Level.Returns(LevelPracticeWithTotal.LastWord);
        _ = attack3Mock.CardId.Returns((short)222);
        _ = attack3Mock.PracticeCareer.Returns(practiceCareerMock);
        CardAttackTests.SetupHasTried(attack3Mock);

        return new[] { attack1Mock, attack2Mock, attack3Mock };
    }

    internal static IReadOnlyDictionary<int, ICardAttack> CardAttacks { get; } =
        CreateCardAttacks().ToDictionary(element => (int)element.CardId);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        _ = mock.FormatNumber(Arg.Any<long>()).Returns(callInfo => $"invoked: {(long)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var cardAttacks = ImmutableDictionary<int, ICardAttack>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(cardAttacks, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestStoryMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CS123MA1"));
    }

    [TestMethod]
    public void ReplaceTestStoryClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 19", replacer.Replace("%T08CS123MA2"));
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T08CS123MA3"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);

        Assert.AreEqual("invoked: 1001", replacer.Replace("%T08CS000MA1"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 57", replacer.Replace("%T08CS000MA2"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 84", replacer.Replace("%T08CS000MA3"));
    }

    [TestMethod]
    public void ReplaceTestStoryLastWord()
    {
        var mock = CardAttackTests.MockCardAttack();
        _ = mock.CardId.Returns((short)206);
        var cardAttacks = new[] { mock, }.ToDictionary(attack => (int)attack.CardId);
        var formatterMock = MockNumberFormatter();

        var replacer = new CareerReplacer(cardAttacks, formatterMock);
        Assert.AreEqual("%T08CS206MA1", replacer.Replace("%T08CS206MA1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 1", replacer.Replace("%T08CP123MA1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 19", replacer.Replace("%T08CP123MA2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 21", replacer.Replace("%T08CP123MA3"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);

        Assert.AreEqual("invoked: 2002", replacer.Replace("%T08CP000MA1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 95", replacer.Replace("%T08CP000MA2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 126", replacer.Replace("%T08CP000MA3"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentMaxBonus()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CS001MA1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CS001MA2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T08CS001MA3"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08XS123MA1", replacer.Replace("%T08XS123MA1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CX123MA1", replacer.Replace("%T08CX123MA1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CS223MA1", replacer.Replace("%T08CS223MA1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(CardAttacks, formatterMock);
        Assert.AreEqual("%T08CS123MA4", replacer.Replace("%T08CS123MA4"));
    }
}
