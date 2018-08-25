using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th095EnemyCardPairTests
    {
        internal struct Properties<TEnemy>
            where TEnemy : struct, Enum
        {
            public TEnemy enemy;
            public string card;
        };

        internal static Properties<TEnemy> GetValidProperties<TEnemy>()
            where TEnemy : struct, Enum
            => new Properties<TEnemy>()
            {
                enemy = TestUtils.Cast<TEnemy>(18),
                card = "新難題「金閣寺の一枚天井」"
            };

        internal static void Validate<TParent, TEnemy>(
            in Th095EnemyCardPairWrapper<TParent, TEnemy> pair, in Properties<TEnemy> properties)
            where TParent : ThConverter
            where TEnemy : struct, Enum
        {
            Assert.AreEqual(properties.enemy, pair.Enemy);
            Assert.AreEqual(properties.card, pair.Card);
        }

        internal static void EnemyCardPairTestHelper<TParent, TEnemy>()
            where TParent : ThConverter
            where TEnemy : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TEnemy>();

                var pair = new Th095EnemyCardPairWrapper<TParent, TEnemy>(properties.enemy, properties.card);

                Validate(pair, properties);
            });

        [TestMethod]
        public void Th095EnemyCardPairTest()
            => EnemyCardPairTestHelper<Th095Converter, Th095Converter.Enemy>();

        [TestMethod]
        public void Th125EnemyCardPairTest()
            => EnemyCardPairTestHelper<Th125Converter, Th125Converter.Enemy>();
    }
}
