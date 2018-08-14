using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th06CharaLevelPairTests
    {
        internal struct Properties<TChara, TLevel>
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            public TChara chara;
            public TLevel level;
        };

        internal static Properties<TChara, TLevel> GetValidProperties<TChara, TLevel>()
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => new Properties<TChara, TLevel>()
            {
                chara = TestUtils.Cast<TChara>(1),
                level = TestUtils.Cast<TLevel>(2)
            };

        internal static void Validate<TParent, TChara, TLevel>(
            in Th06CharaLevelPairWrapper<TParent, TChara, TLevel> pair, in Properties<TChara, TLevel> properties)
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
        {
            Assert.AreEqual(properties.chara, pair.Chara);
            Assert.AreEqual(properties.level, pair.Level);
        }

        internal static void CharaLevelPairTestHelper<TParent, TChara, TLevel>()
            where TParent : ThConverter
            where TChara : struct, Enum
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TChara, TLevel>();

                var pair = new Th06CharaLevelPairWrapper<TParent, TChara, TLevel>(properties.chara, properties.level);

                Validate(pair, properties);
            });

        [TestMethod]
        public void Th06CharaLevelPairTest()
            => CharaLevelPairTestHelper<Th06Converter, Th06Converter.Chara, ThConverter.Level>();

        [TestMethod]
        public void Th07CharaLevelPairTest()
            => CharaLevelPairTestHelper<Th07Converter, Th07Converter.Chara, Th07Converter.Level>();
    }
}
