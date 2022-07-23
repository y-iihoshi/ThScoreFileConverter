using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th18;

namespace ThScoreFileConverter.Tests.Models.Th18;

[TestClass]
public class AchievementReplacerTests
{
    internal static IStatus Status { get; } = StatusTests.MockStatus().Object;

    [TestMethod]
    public void AchievementReplacerTest()
    {
        var replacer = new AchievementReplacer(Status);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new AchievementReplacer(Status);
        Assert.AreEqual("霊夢でクリア", replacer.Replace("%T18ACHV02"));
    }

    [TestMethod]
    public void ReplaceTestNotCleared()
    {
        var replacer = new AchievementReplacer(Status);
        Assert.AreEqual("??????????", replacer.Replace("%T18ACHV04"));
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var replacer = new AchievementReplacer(Status);
        Assert.AreEqual("%T18ACHV00", replacer.Replace("%T18ACHV00"));
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var replacer = new AchievementReplacer(Status);
        Assert.AreEqual("%T18ACHV41", replacer.Replace("%T18ACHV41"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new AchievementReplacer(Status);
        Assert.AreEqual("%T18XXXX22", replacer.Replace("%T18XXXX22"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new AchievementReplacer(Status);
        Assert.AreEqual("%T18ACHVXX", replacer.Replace("%T18ACHVXX"));
    }
}
