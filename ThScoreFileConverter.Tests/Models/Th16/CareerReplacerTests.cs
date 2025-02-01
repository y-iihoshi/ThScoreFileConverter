using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Models.Th16;
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

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(dictionary, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestStoryClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CS003AY1").ShouldBe("invoked: 15");
    }

    [TestMethod]
    public void ReplaceTestStoryTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CS003AY2").ShouldBe("invoked: 59");
    }

    [TestMethod]
    public void ReplaceTestStoryTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CS000AY1").ShouldBe("invoked: 8568");
    }

    [TestMethod]
    public void ReplaceTestStoryTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CS000AY2").ShouldBe("invoked: 13804");
    }

    [TestMethod]
    public void ReplaceTestPracticeClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CP003AY1").ShouldBe("invoked: 37");
    }

    [TestMethod]
    public void ReplaceTestPracticeTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CP003AY2").ShouldBe("invoked: 81");
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CP000AY1").ShouldBe("invoked: 11186");
    }

    [TestMethod]
    public void ReplaceTestPracticeTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CP000AY2").ShouldBe("invoked: 16422");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var dictionary = ImmutableDictionary<CharaWithTotal, IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(dictionary, formatterMock);
        replacer.Replace("%T16CS003AY1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestEmptyCards()
    {
        var clearData = Substitute.For<IClearData>();
        _ = clearData.Chara.Returns(CharaWithTotal.Aya);
        _ = clearData.Cards.Returns(ImmutableDictionary<int, ISpellCard>.Empty);
        var dictionary = new[] { clearData }.ToDictionary(data => data.Chara);
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CareerReplacer(dictionary, formatterMock);
        replacer.Replace("%T16CS003AY1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16XS003AY1").ShouldBe("%T16XS003AY1");
    }

    [TestMethod]
    public void ReplaceTestInvalidGameMode()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CX003AY1").ShouldBe("%T16CX003AY1");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CS121AY1").ShouldBe("%T16CS121AY1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CS003XX1").ShouldBe("%T16CS003XX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearDataDictionary, formatterMock);
        replacer.Replace("%T16CS003AYX").ShouldBe("%T16CS003AYX");
    }
}
