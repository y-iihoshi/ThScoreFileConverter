using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th143EnemiesCardPairTests
    {
        internal struct Properties
        {
            public Th143Converter.Enemy[] enemies;
            public string card;
        };

        internal static Properties ValidProperties => new Properties()
        {
            enemies = new Th143Converter.Enemy[]{ Th143Converter.Enemy.Seiga, Th143Converter.Enemy.Yoshika },
            card = "入魔「過剰ゾウフォルゥモォ」"
        };

        internal static void Validate(in Th143EnemiesCardPairWrapper pair, in Properties properties)
        {
            Assert.AreEqual(properties.enemies[0], pair.Enemy);
            CollectionAssert.AreEqual(properties.enemies, pair.Enemies.ToArray());
            Assert.AreEqual(properties.card, pair.Card);
        }

        [TestMethod]
        public void Th143EnemiesCardPairTestOneEnemy() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.enemies = properties.enemies.Take(1).ToArray();

            var pair = new Th143EnemiesCardPairWrapper(properties.enemies[0], properties.card);

            Validate(pair, properties);
        });

        [TestMethod]
        public void Th143EnemiesCardPairTestTwoEnemies() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;

            var pair = new Th143EnemiesCardPairWrapper(properties.enemies[0], properties.enemies[1], properties.card);

            Validate(pair, properties);
        });
    }
}
