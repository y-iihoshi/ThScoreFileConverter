using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th14;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th14;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th14.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPractice,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th14;

[TestClass]
public class CareerReplacerTests
{
    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData() }.ToDictionary(clearData => clearData.Chara);

    private static INumberFormatter MockNumberFormatter()
    {
        // NOTE: NSubstitute v5.0.0 has no substitute for Moq's It.IsAny<It.IsValueType>.
        var mock = Substitute.For<INumberFormatter>();
        _ = mock.FormatNumber(Arg.Any<int>()).Returns(callInfo => $"invoked: {(int)callInfo[0]}");
        return mock;
    }

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestStoryClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 15", replacer.Replace("%T14CS003RB1"));
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 59", replacer.Replace("%T14CS003RB2"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 8700", replacer.Replace("%T14CS000RB1"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 13980", replacer.Replace("%T14CS000RB2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 37", replacer.Replace("%T14CP003RB1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 81", replacer.Replace("%T14CP003RB2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 11340", replacer.Replace("%T14CP000RB1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 16620", replacer.Replace("%T14CP000RB2"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T14CS003RB1"));
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.ReimuB);
        _ = clearData.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new CareerReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T14CS003RB1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T14XS003RB1", replacer.Replace("%T14XS003RB1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T14CX003RB1", replacer.Replace("%T14CX003RB1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T14CS121RB1", replacer.Replace("%T14CS121RB1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T14CS003XX1", replacer.Replace("%T14CS003XX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T14CS003RBX", replacer.Replace("%T14CS003RBX"));
    }
}
