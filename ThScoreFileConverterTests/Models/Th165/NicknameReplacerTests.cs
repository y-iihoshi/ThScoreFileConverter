using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th165;

namespace ThScoreFileConverterTests.Models.Th165;

[TestClass]
public class NicknameReplacerTests
{
    internal static IStatus Status { get; } = StatusTests.MockStatus().Object;

    [TestMethod]
    public void NicknameReplacerTest()
    {
        var replacer = new NicknameReplacer(Status);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new NicknameReplacer(Status);
        Assert.AreEqual("初めての弾幕写真", replacer.Replace("%T165NICK13"));
    }

    [TestMethod]
    public void ReplaceTestNotCleared()
    {
        var replacer = new NicknameReplacer(Status);
        Assert.AreEqual("??????????", replacer.Replace("%T165NICK12"));
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var replacer = new NicknameReplacer(Status);
        Assert.AreEqual("%T165NICK00", replacer.Replace("%T165NICK00"));
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var replacer = new NicknameReplacer(Status);
        Assert.AreEqual("%T165NICK51", replacer.Replace("%T165NICK51"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new NicknameReplacer(Status);
        Assert.AreEqual("%T165XXXX13", replacer.Replace("%T165XXXX13"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new NicknameReplacer(Status);
        Assert.AreEqual("%T165NICKXX", replacer.Replace("%T165NICKXX"));
    }
}
