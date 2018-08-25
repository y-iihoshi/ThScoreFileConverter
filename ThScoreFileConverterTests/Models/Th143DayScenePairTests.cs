using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th143DayScenePairTests
    {
        internal struct Properties
        {
            public Th143Converter.Day day;
            public int scene;
        };

        internal static Properties ValidProperties => new Properties()
        {
            day = Th143Converter.Day.Last,
            scene = 4
        };

        internal static void Validate(in Th143DayScenePairWrapper pair, in Properties properties)
        {
            Assert.AreEqual(properties.day, pair.Day);
            Assert.AreEqual(properties.scene, pair.Scene);
        }

        [TestMethod]
        public void DayScenePairTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var pair = new Th143DayScenePairWrapper(properties.day, properties.scene);

            Validate(pair, properties);
        });
    }
}
