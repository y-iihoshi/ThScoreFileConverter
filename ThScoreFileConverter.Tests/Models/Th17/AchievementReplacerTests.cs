using NSubstitute;
using ThScoreFileConverter.Models.Th17;

namespace ThScoreFileConverter.Tests.Models.Th17;

[TestClass]
public class AchievementReplacerTests
{
    internal static IAchievementHolder MockAchievementHolder()
    {
        var mock = Substitute.For<IAchievementHolder>();
        _ = mock.Achievements.Returns(Enumerable.Range(0, 40).Select(number => (byte)(number % 3)));
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
        replacer.Replace("%T17ACHV23").ShouldBe("ノーマルクリア");
    }

    [TestMethod]
    public void ReplaceTestNotCleared()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        replacer.Replace("%T17ACHV22").ShouldBe("??????????");
    }

    [TestMethod]
    public void ReplaceTestZeroNumber()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        replacer.Replace("%T17ACHV00").ShouldBe("%T17ACHV00");
    }

    [TestMethod]
    public void ReplaceTestExceededNumber()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        replacer.Replace("%T17ACHV41").ShouldBe("%T17ACHV41");
    }

    [TestMethod]
    public void ReplaceTestInvalidFormat()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        replacer.Replace("%T17XXXX22").ShouldBe("%T17XXXX22");
    }

    [TestMethod]
    public void ReplaceTestInvalidNumber()
    {
        var replacer = new AchievementReplacer(AchievementHolder);
        replacer.Replace("%T17ACHVXX").ShouldBe("%T17ACHVXX");
    }
}
