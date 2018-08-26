using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th075SpellCardInfoTests
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

        internal static void Validate(in Th075SpellCardInfoWrapper spellCardInfo, in Properties properties)
        {
            Assert.AreEqual(properties.name, spellCardInfo.Name);
            Assert.AreEqual(properties.enemy, spellCardInfo.Enemy);
            Assert.AreEqual(properties.level, spellCardInfo.Level);
        }

        [TestMethod]
        public void Th075SpellCardInfoTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var spellCardInfo = new Th075SpellCardInfoWrapper(properties.name, properties.enemy, properties.level);

            Validate(spellCardInfo, properties);
        });
    }
}
