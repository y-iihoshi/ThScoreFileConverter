using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th075;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class SpellCardInfoTests
    {
        internal struct Properties
        {
            public string name;
            public Th075Converter.Chara enemy;
            public Th075Converter.Level level;
        };

        internal static Properties ValidProperties => new Properties()
        {
            name = "「百万鬼夜行」",
            enemy = Th075Converter.Chara.Suika,
            level = Th075Converter.Level.Normal
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
