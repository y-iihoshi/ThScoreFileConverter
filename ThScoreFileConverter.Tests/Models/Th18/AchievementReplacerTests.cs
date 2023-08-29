using System.Linq;
using NSubstitute;
using ThScoreFileConverter.Models.Th18;
using IAchievementHolder = ThScoreFileConverter.Models.Th17.IAchievementHolder;

namespace ThScoreFileConverter.Tests.Models.Th18;

[TestClass]
public class AchievementReplacerTests
{
    internal static IAchievementHolder MockAchievementHolder()
    {
        var mock = Substitute.For<IAchievementHolder>();
        _ = mock.Achievements.Returns(Enumerable.Range(0, 30).Select(number => (byte)(number % 3)));
        return mock;
    }

    internal static IAchievementHolder AchievementHolder { get; } = MockAchievementHolder();

    [TestMethod]
    public void AchievementReplacerTest()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        Assert.IsNotNull(replacer);
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        Assert.AreEqual("霊夢でクリア", replacer.Replace("%T18ACHV02"));
    }

    [TestMethod]
    public void ReplaceTestNotCleared()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        Assert.AreEqual("??????????", replacer.Replace("%T18ACHV04"));
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        Assert.AreEqual("%T18ACHV00", replacer.Replace("%T18ACHV00"));
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        Assert.AreEqual("%T18ACHV31", replacer.Replace("%T18ACHV31"));
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        Assert.AreEqual("%T18XXXX22", replacer.Replace("%T18XXXX22"));
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        Assert.AreEqual("%T18ACHVXX", replacer.Replace("%T18ACHVXX"));
    }
}
