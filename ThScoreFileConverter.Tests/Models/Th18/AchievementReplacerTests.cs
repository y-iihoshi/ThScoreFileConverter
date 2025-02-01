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
        _ = replacer.ShouldNotBeNull();
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        replacer.Replace("%T18ACHV02").ShouldBe("霊夢でクリア");
    }

    [TestMethod]
    public void ReplaceTestNotCleared()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        replacer.Replace("%T18ACHV04").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        replacer.Replace("%T18ACHV00").ShouldBe("%T18ACHV00");
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        replacer.Replace("%T18ACHV31").ShouldBe("%T18ACHV31");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        replacer.Replace("%T18XXXX22").ShouldBe("%T18XXXX22");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        replacer.Replace("%T18ACHVXX").ShouldBe("%T18ACHVXX");
    }
}
