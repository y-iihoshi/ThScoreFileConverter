using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th095LevelScenePairTests
    {
        internal struct Properties<TLevel>
            where TLevel : struct, Enum
        {
            public TLevel level;
            public int scene;
        };

        internal static Properties<TLevel> GetValidProperties<TLevel>()
            where TLevel : struct, Enum
            => new Properties<TLevel>()
            {
                level = TestUtils.Cast<TLevel>(8),
                scene = 6
            };

        internal static void Validate<TParent, TLevel>(
            in Th095LevelScenePairWrapper<TParent, TLevel> pair, in Properties<TLevel> properties)
            where TParent : ThConverter
            where TLevel : struct, Enum
        {
            Assert.AreEqual(properties.level, pair.Level);
            Assert.AreEqual(properties.scene, pair.Scene);
        }

        internal static void LevelScenePairTestHelper<TParent, TLevel>()
            where TParent : ThConverter
            where TLevel : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var properties = GetValidProperties<TLevel>();

                var pair = new Th095LevelScenePairWrapper<TParent, TLevel>(properties.level, properties.scene);

                Validate(pair, properties);
            });

        [TestMethod]
        public void Th095LevelScenePairTest()
            => LevelScenePairTestHelper<Th095Converter, Th095Converter.Level>();

        [TestMethod]
        public void Th125LevelScenePairTest()
            => LevelScenePairTestHelper<Th125Converter, Th125Converter.Level>();
    }
}
