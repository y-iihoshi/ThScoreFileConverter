using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th16;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th16;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th16.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th16.IScoreData>;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Core.Models.Level>;

namespace ThScoreFileConverter.Tests.Models.Th16;

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
        Assert.AreEqual("invoked: 15", replacer.Replace("%T16CS003AY1"));
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 59", replacer.Replace("%T16CS003AY2"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 8568", replacer.Replace("%T16CS000AY1"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 13804", replacer.Replace("%T16CS000AY2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 37", replacer.Replace("%T16CP003AY1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 81", replacer.Replace("%T16CP003AY2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 11186", replacer.Replace("%T16CP000AY1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 16422", replacer.Replace("%T16CP000AY2"));
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T16CS003AY1"));
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Aya);
        _ = clearData.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = MockNumberFormatter();

        var replacer = new CareerReplacer(dictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T16CS003AY1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T16XS003AY1", replacer.Replace("%T16XS003AY1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T16CX003AY1", replacer.Replace("%T16CX003AY1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T16CS121AY1", replacer.Replace("%T16CS121AY1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T16CS003XX1", replacer.Replace("%T16CS003XX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = MockNumberFormatter();
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T16CS003AYX", replacer.Replace("%T16CS003AYX"));
    }
}
