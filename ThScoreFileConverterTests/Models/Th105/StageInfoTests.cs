using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ThScoreFileConverter.Models;
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
            public List<int> cardIds;
        };

        internal static Properties<TStage, TChara> GetValidProperties<TStage, TChara>()
            where TStage : struct, Enum
            where TChara : struct, Enum
            => new Properties<TStage, TChara>()
            {
                stage = TestUtils.Cast<TStage>(1),
                enemy = TestUtils.Cast<TChara>(2),
                cardIds = new List<int>() { 3, 4, 5 }
            };

        internal static void Validate<TStage, TChara>(
            in StageInfo<TStage, TChara> spellCardInfo, in Properties<TStage, TChara> properties)
            where TStage : struct, Enum
            where TChara : struct, Enum
        {
            Assert.AreEqual(properties.stage, spellCardInfo.Stage);
            Assert.AreEqual(properties.enemy, spellCardInfo.Enemy);
            CollectionAssert.That.AreEqual(properties.cardIds, spellCardInfo.CardIds);
        }

        internal static void StageInfoTestHelper<TStage, TChara>()
            where TStage : struct, Enum
            where TChara : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TStage, TChara>();

                var spellCardInfo = new StageInfo<TStage, TChara>(
                    properties.stage, properties.enemy, properties.cardIds);

                Validate(spellCardInfo, properties);
            });

        [TestMethod]
        public void Th105StageInfoTest()
            => StageInfoTestHelper<Th105Converter.Stage, Th105Converter.Chara>();

        [TestMethod]
        public void Th123StageInfoTest()
            => StageInfoTestHelper<Th123Converter.Stage, Th123Converter.Chara>();
    }
}
