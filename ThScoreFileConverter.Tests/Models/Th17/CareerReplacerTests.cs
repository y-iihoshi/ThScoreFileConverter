using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th17;
using ThScoreFileConverter.Models.Th17;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th17.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Th14.StagePractice,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Tests.Models.Th17;

[TestClass]
public class CareerReplacerTests
{
    internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
        new[] { ClearDataTests.MockClearData() }.ToDictionary(clearData => clearData.Chara);

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(dictionary, formatterMock);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTestStoryClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 13", replacer.Replace("%T17CS001MB1"));
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 57", replacer.Replace("%T17CS001MB2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T17CP001MB1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 79", replacer.Replace("%T17CP001MB2"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 3213", replacer.Replace("%T17CS000MB1"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 8107", replacer.Replace("%T17CS000MB2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 5780", replacer.Replace("%T17CP000MB1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 10419", replacer.Replace("%T17CP000MB2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentStoryClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T17CS001RA1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentStoryTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T17CS001RA2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentPracticeClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T17CP001RA1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentPracticeTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T17CP001RA2"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T17XS001MB1", replacer.Replace("%T17XS001MB1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T17CX001MB1", replacer.Replace("%T17CX001MB1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T17CS102MB1", replacer.Replace("%T17CS102MB1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T17CS001XX1", replacer.Replace("%T17CS001XX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T17CS001MB3", replacer.Replace("%T17CS001MB3"));
    }
}
