using System.Collections.Immutable;
using ThScoreFileConverter.Core.Models.Th18;
using ThScoreFileConverter.Models.Th18;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Core.Models.Th18.CharaWithTotal,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Level,
    ThScoreFileConverter.Core.Models.Th14.LevelPracticeWithTotal,
    ThScoreFileConverter.Core.Models.Stage,
    ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th13.StageProgress>>;

namespace ThScoreFileConverter.Tests.Models.Th18;

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
        Assert.AreEqual("invoked: 13", replacer.Replace("%T18CS01MR1"));
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 57", replacer.Replace("%T18CS01MR2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 35", replacer.Replace("%T18CP01MR1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 79", replacer.Replace("%T18CP01MR2"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 2989", replacer.Replace("%T18CS00MR1"));
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 7641", replacer.Replace("%T18CS00MR2"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 5379", replacer.Replace("%T18CP00MR1"));
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 9887", replacer.Replace("%T18CP00MR2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentStoryClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CS01RM1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentStoryTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CS01RM2"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentPracticeClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CP01RM1"));
    }

    [TestMethod]
    public void ReplaceTestNonexistentPracticeTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("invoked: 0", replacer.Replace("%T18CP01RM2"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18XS01MR1", replacer.Replace("%T18XS01MR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18CX01MR1", replacer.Replace("%T18CX01MR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18CS98MR1", replacer.Replace("%T18CS98MR1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18CS01XX1", replacer.Replace("%T18CS01XX1"));
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        Assert.AreEqual("%T18CS01MR3", replacer.Replace("%T18CS01MR3"));
    }
}
