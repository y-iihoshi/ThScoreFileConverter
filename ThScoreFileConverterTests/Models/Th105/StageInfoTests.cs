using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models.Th105;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th105
{
    [TestClass]
    public class StageInfoTests
    {
        internal struct Properties<TChara>
            where TChara : struct, Enum
        {
            public Stage stage;
            public TChara enemy;
            public IEnumerable<int> cardIds;
        }

        internal static Properties<TChara> MakeValidProperties<TChara>()
            where TChara : struct, Enum
            => new Properties<TChara>()
            {
                stage = Stage.Two,
                enemy = TestUtils.Cast<TChara>(2),
                cardIds = new List<int>() { 3, 4, 5 }
            };

        internal static void Validate<TChara>(in Properties<TChara> expected, in StageInfo<TChara> actual)
            where TChara : struct, Enum
        {
            Assert.AreEqual(expected.stage, actual.Stage);
            Assert.AreEqual(expected.enemy, actual.Enemy);
            CollectionAssert.That.AreEqual(expected.cardIds, actual.CardIds);
        }

        internal static void StageInfoTestHelper<TChara>()
            where TChara : struct, Enum
        {
            var properties = MakeValidProperties<TChara>();

            var spellCardInfo = new StageInfo<TChara>(properties.stage, properties.enemy, properties.cardIds);

            Validate(properties, spellCardInfo);
        }

        [TestMethod]
        public void StageInfoTest() => StageInfoTestHelper<Chara>();
    }
}
