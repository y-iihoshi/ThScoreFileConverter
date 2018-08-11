using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th095LevelScenePairTests
    {
        internal struct Properties
        {
            public Th095Converter.Level level;
            public int scene;
        };

        internal static Properties ValidProperties => new Properties()
        {
            level = Th095Converter.Level.Lv9,
            scene = 6
        };

        internal static void Validate(in Th095LevelScenePairWrapper pair, in Properties properties)
        {
            Assert.AreEqual(properties.level, pair.Level);
            Assert.AreEqual(properties.scene, pair.Scene);
        }

        [TestMethod]
        public void Th095LevelScenePairTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var pair = new Th095LevelScenePairWrapper(properties.level, properties.scene);

            Validate(pair, properties);
        });
    }
}
