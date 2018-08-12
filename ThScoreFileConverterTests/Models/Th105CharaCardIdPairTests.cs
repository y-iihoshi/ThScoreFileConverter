using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th105CharaCardIdPairTests
    {
        internal struct Properties<TChara>
            where TChara : struct, Enum
        {
            public TChara chara;
            public int cardId;
        };

        internal static Properties<TChara> GetValidProperties<TChara>()
            where TChara : struct, Enum
            => new Properties<TChara>()
            {
                chara = TestUtils.Cast<TChara>(1),
                cardId = 2
            };

        internal static void Validate<TParent, TChara>(
            in Th105CharaCardIdPairWrapper<TParent, TChara> pair, in Properties<TChara> properties)
            where TParent : ThConverter
            where TChara : struct, Enum
        {
            Assert.AreEqual(properties.chara, pair.Chara);
            Assert.AreEqual(properties.cardId, pair.CardId);
        }

        internal static void CharaCardIdPairTestHelper<TParent, TChara>()
            where TParent : ThConverter
            where TChara : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TChara>();

                var pair = new Th105CharaCardIdPairWrapper<TParent, TChara>(properties.chara, properties.cardId);

                Validate(pair, properties);
            });

        [TestMethod]
        public void Th105CharaCardIdPairTest()
            => CharaCardIdPairTestHelper<Th105Converter, Th105Converter.Chara>();

        [TestMethod]
        public void Th123CharaCardIdPairTest()
            => CharaCardIdPairTestHelper<Th123Converter, Th123Converter.Chara>();
    }
}
