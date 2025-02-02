using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverter.Tests.Models.Th165;

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
        replacer.Replace("%T165NICK13").ShouldBe("初めての弾幕写真");
    }

    [TestMethod]
    public void ReplaceTestNotCleared()
    {
        var replacer = new NicknameReplacer(Status);
        replacer.Replace("%T165NICK12").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var replacer = new NicknameReplacer(Status);
        replacer.Replace("%T165NICK00").ShouldBe("%T165NICK00");
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var replacer = new NicknameReplacer(Status);
        replacer.Replace("%T165NICK51").ShouldBe("%T165NICK51");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new NicknameReplacer(Status);
        replacer.Replace("%T165XXXX13").ShouldBe("%T165XXXX13");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new NicknameReplacer(Status);
        replacer.Replace("%T165NICKXX").ShouldBe("%T165NICKXX");
    }
}
