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
        internal struct Properties<TStage, TChara>
            where TStage : struct, Enum
            where TChara : struct, Enum
        {
            public TStage stage;
            public TChara enemy;
            public IEnumerable<int> cardIds;
        };

        internal static Properties<TStage, TChara> MakeValidProperties<TStage, TChara>()
            where TStage : struct, Enum
            where TChara : struct, Enum
            => new Properties<TStage, TChara>()
            {
                stage = TestUtils.Cast<TStage>(1),
                enemy = TestUtils.Cast<TChara>(2),
                cardIds = new List<int>() { 3, 4, 5 }
            };

        internal static void Validate<TStage, TChara>(
            in Properties<TStage, TChara> expected, in StageInfo<TStage, TChara> actual)
            where TStage : struct, Enum
            where TChara : struct, Enum
        {
            Assert.AreEqual(expected.stage, actual.Stage);
            Assert.AreEqual(expected.enemy, actual.Enemy);
            CollectionAssert.That.AreEqual(expected.cardIds, actual.CardIds);
        }

        internal static void StageInfoTestHelper<TStage, TChara>()
            where TStage : struct, Enum
            where TChara : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = MakeValidProperties<TStage, TChara>();

                var spellCardInfo = new StageInfo<TStage, TChara>(
                    properties.stage, properties.enemy, properties.cardIds);

                Validate(properties, spellCardInfo);
            });

        [TestMethod]
        public void StageInfoTest() => StageInfoTestHelper<Stage, Chara>();
    }
}
