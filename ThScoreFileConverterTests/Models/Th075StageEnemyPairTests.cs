using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th075StageEnemyPairTests
    {
        internal struct Properties
        {
            public Th075Converter.Stage stage;
            public Th075Converter.Chara enemy;
        };

        internal static Properties ValidProperties => new Properties()
        {
            stage = Th075Converter.Stage.St7,
            enemy = Th075Converter.Chara.Suika
        };

        internal static void Validate(in Th075StageEnemyPairWrapper pair, in Properties properties)
        {
            Assert.AreEqual(properties.stage, pair.Stage);
            Assert.AreEqual(properties.enemy, pair.Enemy);
        }

        [TestMethod]
        public void Th075StageEnemyPairTest() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var pair = new Th075StageEnemyPairWrapper(properties.stage, properties.enemy);

            Validate(pair, properties);
        });
    }
}
