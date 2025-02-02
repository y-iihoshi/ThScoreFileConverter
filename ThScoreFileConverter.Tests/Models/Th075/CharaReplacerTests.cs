using System.Collections.Immutable;
using ThScoreFileConverter.Core.Helpers;
using ThScoreFileConverter.Core.Models.Th075;
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverter.Tests.Models.Th075;

[TestClass]
public class CharaReplacerTests
{
    internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
        EnumHelper<Level>.Enumerable.ToDictionary(
            level => (CharaWithReserved.Reimu, level),
            level => ClearDataTests.MockClearData());

    [TestMethod]
    public void CharaReplacerTest()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void CharaReplacerTestEmpty()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(clearData, formatterMock);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTestUseCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);

        replacer.Replace("%T75CHRHRM1").ShouldBe("invoked: 1234");
    }

    [TestMethod]
    public void ReplaceTestClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);

        replacer.Replace("%T75CHRHRM2").ShouldBe("invoked: 2345");
    }

    [TestMethod]
    public void ReplaceTestMaxCombo()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);

        replacer.Replace("%T75CHRHRM3").ShouldBe("invoked: 3456");
    }

    [TestMethod]
    public void ReplaceTestMaxDamage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);

        replacer.Replace("%T75CHRHRM4").ShouldBe("invoked: 4567");
    }

    [TestMethod]
    public void ReplaceTestEmpty()
    {
        var clearData = ImmutableDictionary<(CharaWithReserved, Level), IClearData>.Empty;
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(clearData, formatterMock);
        replacer.Replace("%T75CHRHRM1").ShouldBe("invoked: 0");
        replacer.Replace("%T75CHRHRM2").ShouldBe("invoked: 0");
        replacer.Replace("%T75CHRHRM3").ShouldBe("invoked: 0");
        replacer.Replace("%T75CHRHRM4").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentUseCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CHRHMR1").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentClearCount()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CHRHMR2").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentMaxCombo()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CHRHMR3").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestNonexistentMaxDamage()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CHRHMR4").ShouldBe("invoked: 0");
    }

    [TestMethod]
    public void ReplaceTestMeiling()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CHRHML1").ShouldBe("%T75CHRHML1");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);
        replacer.Replace("%T75XXXHRM1").ShouldBe("%T75XXXHRM1");
    }

    [TestMethod]
    public void ReplaceTestInvalidLevel()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CHRXRM1").ShouldBe("%T75CHRXRM1");
    }

    [TestMethod]
    public void ReplaceTestInvalidChara()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CHRHXX1").ShouldBe("%T75CHRHXX1");
    }

    [TestMethod]
    public void ReplaceTestInvalidType()
    {
        var formatterMock = NumberFormatterTests.Mock;
        var replacer = new CharaReplacer(ClearData, formatterMock);
        replacer.Replace("%T75CHRHRMX").ShouldBe("%T75CHRHRMX");
    }
}
