using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th08StageLevelPairTests
    {
        internal struct Properties
        {
            public Th08Converter.Stage stage;
            public ThConverter.Level level;
        };

        internal static Properties ValidProperties => new Properties()
        {
            stage = Th08Converter.Stage.St6B,
            level = ThConverter.Level.Lunatic
        };

        internal static void Validate(in Th08StageLevelPairWrapper pair, in Properties properties)
        {
            Assert.AreEqual(properties.stage, pair.Stage);
            Assert.AreEqual(properties.level, pair.Level);
        }

        [TestMethod]
        public void Th08StageLevelPairTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var pair = new Th08StageLevelPairWrapper(properties.stage, properties.level);

            Validate(pair, properties);
        });
    }
}
