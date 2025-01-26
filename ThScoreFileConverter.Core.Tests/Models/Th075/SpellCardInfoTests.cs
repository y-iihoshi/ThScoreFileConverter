using ThScoreFileConverter.Core.Models.Th075;

namespace ThScoreFileConverter.Core.Tests.Models.Th075;

[TestClass]
public class SpellCardInfoTests
{
    internal struct Properties
    {
        public string name;
        public Chara enemy;
        public Level level;
    }

    internal static Properties ValidProperties { get; } = new Properties()
    {
        name = "「百万鬼夜行」",
        enemy = Chara.Suika,
        level = Level.Normal,
    };

    internal static void Validate(in Properties properties, in SpellCardInfo spellCardInfo)
    {
        spellCardInfo.Name.ShouldBe(properties.name);
        spellCardInfo.Enemy.ShouldBe(properties.enemy);
        spellCardInfo.Level.ShouldBe(properties.level);
    }

    [TestMethod]
    public void SpellCardInfoTest()
    {
        var properties = ValidProperties;

        var spellCardInfo = new SpellCardInfo(properties.name, properties.enemy, properties.level);

        Validate(properties, spellCardInfo);
    }
}
