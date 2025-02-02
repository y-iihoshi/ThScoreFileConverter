using ThScoreFileConverter.Core.Models.Th075;

namespace ThScoreFileConverter.Core.Tests.Models.Th075;

[TestClass]
public class SpellCardInfoTests
{
    [TestMethod]
    public void SpellCardInfoTest()
    {
        var expectedName = "「百万鬼夜行」";
        var expectedEnemy = Chara.Suika;
        var expectedLevel = Level.Normal;

        var actual = new SpellCardInfo(expectedName, expectedEnemy, expectedLevel);

        actual.Name.ShouldBe(expectedName);
        actual.Enemy.ShouldBe(expectedEnemy);
        actual.Level.ShouldBe(expectedLevel);
    }
}
