using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th18;

namespace ThScoreFileConverterTests.Models.Th18;

[TestClass]
public class AbilityCardReplacerTests
{
    internal static IStatus Status { get; } = StatusTests.MockStatus().Object;

    [TestMethod]
    public void AbilityCardReplacerTest()
    {
        var replacer = new AbilityCardReplacer(Status);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new AbilityCardReplacer(Status);
        Assert.AreEqual("太古の勾玉", replacer.Replace("%T18ABIL22"));
    }

    [TestMethod]
    public void ReplaceTestNotCleared()
    {
        var replacer = new AbilityCardReplacer(Status);
        Assert.AreEqual("??????????", replacer.Replace("%T18ABIL05"));
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var replacer = new AbilityCardReplacer(Status);
        Assert.AreEqual("%T18ABIL00", replacer.Replace("%T18ABIL00"));
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var replacer = new AbilityCardReplacer(Status);
        Assert.AreEqual("%T18ABIL57", replacer.Replace("%T18ABIL57"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new AbilityCardReplacer(Status);
        Assert.AreEqual("%T18XXXX22", replacer.Replace("%T18XXXX22"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new AbilityCardReplacer(Status);
        Assert.AreEqual("%T18ABILXX", replacer.Replace("%T18ABILXX"));
    }
}
