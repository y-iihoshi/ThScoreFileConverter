using System.Collections.Immutable;
using NSubstitute;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverter.Tests.Models.Th075;

[TestClass]
public class CareerReplacerTests
{
    internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            level => (CharaWithReserved.Reimu, level),
            level => ClearDataTests.MockClearData());

    [TestMethod]
    public void CareerReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CareerReplacerTestEmpty()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(clearData, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestMaxBonus()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C001RM1").ShouldBe("invoked: 36");
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C001RM2").ShouldBe("invoked: 32");
    }

    [TestMethod]
    public void ReplaceTestTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C001RM3").ShouldBe("invoked: 28");
    }

    [TestMethod]
    public void ReplaceTestStar()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C001RM4").ShouldBe("★");
    }

    [TestMethod]
    public void ReplaceTestNotStar()
    {
        static IClearData MockClearData()
        {
            var mock = ClearDataTests.MockClearData();
            var cardTrulyGot = mock.CardTrulyGot;
            _ = mock.CardTrulyGot.Returns(cardTrulyGot.Select((got, index) => index == 0 ? (byte)0 : got).ToList());
            return mock;
        }

        var clearData = EnumHelper<Level>.Enumerable.ToDictionary(
            level => (CharaWithReserved.Reimu, level),
            level => MockClearData());
        var formatterMock = NumberFormatterTests.Mock;

        var replacer = new CareerReplacer(clearData, formatterMock);
        replacer.Replace("%T75C001RM4").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestTotalMaxBonus()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C000RM1").ShouldBe("invoked: 23400");
    }

    [TestMethod]
    public void ReplaceTestTotalClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C000RM2").ShouldBe("invoked: 23000");
    }

    [TestMethod]
    public void ReplaceTestTotalTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C000RM3").ShouldBe("invoked: 22600");
    }

    [TestMethod]
    public void ReplaceTestTotalStar()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C000RM4").ShouldBe("%T75C000RM4");
    }

    [TestMethod]
    public void ReplaceTestMeiling()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C001ML1").ShouldBe("%T75C001ML1");
    }

    [TestMethod]
    public void ReplaceTestNonexistentMaxBonus()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C001MR1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C001MR2").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentTrialCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C001MR3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentStar()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C001MR4").ShouldBeEmpty();
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75X001RM1").ShouldBe("%T75X001RM1");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C101RM1").ShouldBe("%T75C101RM1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C001XX1").ShouldBe("%T75C001XX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CareerReplacer(ClearData, formatterMock);
        replacer.Replace("%T75C001RMX").ShouldBe("%T75C001RMX");
    }
}
