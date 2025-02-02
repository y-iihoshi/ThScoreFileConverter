using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverter.Tests.Models.Th143;

[TestClass]
public class NicknameReplacerTests
{
    internal static IStatus Status { get; } = StatusTests.MockStatus();

    [TestMethod]
    public void NicknameReplacerTest()
    {
        var replacer = new NicknameReplacer(Status);
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new NicknameReplacer(Status);
        replacer.Replace("%T143NICK70").ShouldBe("究極反則生命体");
    }

    [TestMethod]
    public void ReplaceTestNotCleared()
    {
        var replacer = new NicknameReplacer(Status);
        replacer.Replace("%T143NICK69").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var replacer = new NicknameReplacer(Status);
        replacer.Replace("%T143NICK00").ShouldBe("%T143NICK00");
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var replacer = new NicknameReplacer(Status);
        replacer.Replace("%T143NICK71").ShouldBe("%T143NICK71");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new NicknameReplacer(Status);
        replacer.Replace("%T143XXXX70").ShouldBe("%T143XXXX70");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new NicknameReplacer(Status);
        replacer.Replace("%T143NICKXX").ShouldBe("%T143NICKXX");
    }
}
