using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th06CharaLevelPairTests
    {
        internal struct Properties
        {
            public Th06Converter.Chara chara;
            public ThConverter.Level level;
        };

        internal static Properties ValidProperties => new Properties()
        {
            chara = Th06Converter.Chara.ReimuB,
            level = ThConverter.Level.Lunatic
        };

        internal static void Validate(in Th06CharaLevelPairWrapper pair, in Properties properties)
        {
            Assert.AreEqual(properties.chara, pair.Chara);
            Assert.AreEqual(properties.level, pair.Level);
        }

        [TestMethod]
        public void Th06CharaLevelPairTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var pair = new Th06CharaLevelPairWrapper(properties.chara, properties.level);

            Validate(pair, properties);
        });
    }
}
