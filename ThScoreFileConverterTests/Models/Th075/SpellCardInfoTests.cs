using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th075;
using Level = ThScoreFileConverter.Models.Th075.Level;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class SpellCardInfoTests
    {
        internal struct Properties
        {
            public string name;
            public Chara enemy;
            public Level level;
        };

        internal static Properties ValidProperties => new Properties()
        {
            name = "「百万鬼夜行」",
            enemy = Chara.Suika,
            level = Level.Normal
        };

        internal static void Validate(in Properties properties, in SpellCardInfo spellCardInfo)
        {
            Assert.AreEqual(properties.name, spellCardInfo.Name);
            Assert.AreEqual(properties.enemy, spellCardInfo.Enemy);
            Assert.AreEqual(properties.level, spellCardInfo.Level);
        }

        [TestMethod]
        public void SpellCardInfoTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var spellCardInfo = new SpellCardInfo(properties.name, properties.enemy, properties.level);

            Validate(properties, spellCardInfo);
        });
    }
}
